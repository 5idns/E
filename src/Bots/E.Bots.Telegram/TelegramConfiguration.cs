using E.Bots.Configuration;

namespace E.Bots.Telegram
{
    public class TelegramConfiguration: IBotConfiguration
    {
        public string TenantId { get; set; }
        public string BotToken { get; set; }
    }
}
