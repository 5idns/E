using System.Threading.Tasks;
using E.Common.Serialization;
using E.Tenant;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace E.Bots.Telegram
{
    /// <summary>
    /// Telegram
    /// </summary>
    [Route("{Tenant}/Telegram/[controller]")]
    public class WebhookController : ControllerBase
    {
        private readonly ITenantAccessor _tenantAccessor;
        private readonly HandleUpdateService _handleUpdateService;
        private readonly BotClientFactory _botClientFactory;

        public WebhookController(ITenantAccessor tenantAccessor, BotClientFactory botClientFactory, HandleUpdateService handleUpdateService)
        {
            _tenantAccessor = tenantAccessor;
            _botClientFactory = botClientFactory;
            _handleUpdateService = handleUpdateService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Update update)
        {
            if (update == null)
            {
                var requestString = Request.GetRequestString();
                update = requestString.Deserialize<Update>();
            }
            var tenant = _tenantAccessor.Context;
            var botClient = _botClientFactory.GetBotClient(tenant.TenantId);
            await _handleUpdateService.EchoAsync(botClient, update);
            return Ok();
        }
    }
}
