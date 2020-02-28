using System.Threading;
using System.Threading.Tasks;
using PrivateConversationBot.Web.DataAccess;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Types.Enums;

namespace PrivateConversationBot.Web.Handlers
{
    public class TextMessageForwarder : HandlerBase
    {
        public TextMessageForwarder(PrivateConversationBotDbContext dbContext) : base(dbContext) { }

        public override async Task HandleAsync(IUpdateContext context, UpdateDelegate next, CancellationToken cancellationToken)
        {
            await RegisterMessage(
                context,
                (replyToMessageId, client, message) => client.SendTextMessageAsync(
                    AdminUser.LatestChatId,
                    message.Text,
                    replyToMessageId: replyToMessageId,
                    disableNotification: true,
                    cancellationToken: cancellationToken),
                cancellationToken);
        }
    }
}
