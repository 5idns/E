using System;
using System.Collections.Concurrent;
using Telegram.Bot;

namespace E.Bots.Telegram
{
    public class BotClientFactory
    {
        private readonly ConcurrentDictionary<string, ITelegramBotClient> _botClients;
        private readonly BotConfigurationFactory _configuration;


        //private readonly IConfigurationSection _botConfigurations;

        public BotClientFactory(BotConfigurationFactory configuration)
        {
            _botClients = new ConcurrentDictionary<string, ITelegramBotClient>();
            _configuration = configuration;
        }


        public ITelegramBotClient GetBotClient(string tenantId)
        {
            var client = _botClients.GetOrAdd(tenantId, key =>
            {
                var botConfig = _configuration.Get<TelegramConfiguration>(tenantId);
                if (botConfig == null)
                {
                    Console.WriteLine($"获取租户 {tenantId} 配置为空");
                }
                //var botConfig = _configurations($"{tenantId}").Get<BotConfiguration>();
                var botClient = new TelegramBotClient(botConfig?.BotToken?? "1748784321:AAEebjgAxILSNEuzvSDJhLjFbTt5DBtmy6U");
                return botClient;
            });
            if (client == null && _botClients.ContainsKey(tenantId))
            {
                _botClients.AddOrUpdate(tenantId, key =>
                {
                    var botConfig = _configuration.Get<TelegramConfiguration>(tenantId);
                    if (botConfig == null)
                    {
                        Console.WriteLine($"获取租户 {tenantId} 配置为空");
                    }
                    //var botConfig = _configurations($"{tenantId}").Get<BotConfiguration>();
                    var botClient = new TelegramBotClient(botConfig?.BotToken ?? "1748784321:AAEebjgAxILSNEuzvSDJhLjFbTt5DBtmy6U");
                    return botClient;
                }, (key, value) =>
                {
                    var botConfig = _configuration.Get<TelegramConfiguration>(tenantId);
                    if (botConfig == null)
                    {
                        Console.WriteLine($"获取租户 {tenantId} 配置为空");
                    }
                    //var botConfig = _configurations($"{tenantId}").Get<BotConfiguration>();
                    var botClient = new TelegramBotClient(botConfig?.BotToken ?? "1748784321:AAEebjgAxILSNEuzvSDJhLjFbTt5DBtmy6U");
                    return botClient;
                });
            }
            
            return client;
        }
    }
}
