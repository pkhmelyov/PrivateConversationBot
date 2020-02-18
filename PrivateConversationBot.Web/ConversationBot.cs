using Microsoft.Extensions.Options;
using MihaZupan;
using PrivateConversationBot.Web.Options;
using Telegram.Bot;
using Telegram.Bot.Framework;

namespace PrivateConversationBot.Web
{
    public class ConversationBot : BotBase
    {
        public ConversationBot(IOptions<CustomBotOptions<ConversationBot>> options) : base(options.Value.Username, CreateClient(options.Value)) { }

        private static ITelegramBotClient CreateClient(CustomBotOptions<ConversationBot> options)
        {
            //if (!options.UseTorProxy)
            //{
            //    return new TelegramBotClient(options.ApiToken);
            //}
            //else
            //{
            //    return new TelegramBotClient(options.ApiToken, new HttpToSocks5Proxy("127.0.0.1", 9150));
            //}
            return new TelegramBotClient(options.ApiToken, new HttpToSocks5Proxy("127.0.0.1", 9150));
        }
    }
}