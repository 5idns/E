using System;
using System.Threading;
using System.Threading.Tasks;
using E.Bots.Configuration;
using E.Tenant;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types.Enums;

namespace E.Bots.Telegram
{
    public class ConfigureWebhook
    {
        private readonly ILogger<ConfigureWebhook> _logger;
        private readonly SystemConfiguration _systemConfig;
        private readonly BotClientFactory _botClientFactory;

        public ConfigureWebhook(ILogger<ConfigureWebhook> logger,
            IConfiguration configuration,
            BotClientFactory botClientFactory)
        {
            _logger = logger;
            _botClientFactory = botClientFactory;
            _systemConfig = new SystemConfiguration();
            configuration.Bind("System", _systemConfig);
        }

        public async Task SetAsync(ITenantContext tenant, CancellationToken cancellationToken)
        {
            var botClient = _botClientFactory.GetBotClient(tenant.TenantId);

            // Configure custom endpoint per Telegram API recommendations:
            // https://core.telegram.org/bots/api#setwebhook
            // If you'd like to make sure that the Webhook request comes from Telegram, we recommend
            // using a secret path in the URL, e.g. https://www.example.com/<token>.
            // Since nobody else knows your bot's token, you can be pretty sure it's us.
            var webhookAddress = $"{_systemConfig.HostAddress}/{tenant.Identity}/Telegram/Webhook";
            _logger.LogInformation("Setting webhook: {0}", webhookAddress);
            await botClient.SetWebhookAsync(
                url: webhookAddress,
                allowedUpdates: Array.Empty<UpdateType>(),
                cancellationToken: cancellationToken);
        }

        public async Task DeleteAsync(ITenantContext tenant, CancellationToken cancellationToken)
        {

            var botClient = _botClientFactory.GetBotClient(tenant.TenantId);

            // Remove webhook upon app shutdown
            _logger.LogInformation("Removing webhook");
            await botClient.DeleteWebhookAsync(cancellationToken: cancellationToken);
        }
    }
}
