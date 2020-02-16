using Microsoft.Extensions.Options;
using Telegram.Bot.Framework;

namespace PrivateConversationBot.Web
{
    public class ConversationBot : BotBase
    {
        public ConversationBot(IOptions<BotOptions<ConversationBot>> options) : base(options.Value) { }
    }
}