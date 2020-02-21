using System.Threading;
using System.Threading.Tasks;
using PrivateConversationBot.Web.DataAccess;
using Telegram.Bot.Framework.Abstractions;

namespace PrivateConversationBot.Web.Handlers
{
    public class VideoNoteForwarder : HandlerBase
    {
        public VideoNoteForwarder(PrivateConversationBotDbContext dbContext) : base(dbContext) { }

        public override async Task HandleAsync(IUpdateContext context, UpdateDelegate next, CancellationToken cancellationToken)
        {
            await RegisterMessage(
                context,
                (replyToMessageId, client, message) => client.SendVideoNoteAsync(
                    AdminUser.LatestChatId,
                    message.VideoNote.FileId,
                    message.VideoNote.Duration,
                    message.VideoNote.Length,
                    true,
                    replyToMessageId,
                    cancellationToken: cancellationToken),
                cancellationToken);
        }
    }
}
