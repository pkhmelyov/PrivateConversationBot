using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Abstractions;

namespace PrivateConversationBot.Web.Options
{
    public class CustomBotOptions<TBot> : BotOptions<TBot> where TBot : IBot
    {
        public string WebhookDomain { get; set; }
        public string Admin { get; set; }
        public bool UseTorProxy { get; set; }
    }
}
