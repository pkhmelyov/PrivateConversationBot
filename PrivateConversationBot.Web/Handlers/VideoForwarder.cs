using System.Threading;
using System.Threading.Tasks;
using PrivateConversationBot.Web.DataAccess;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Types.InputFiles;

namespace PrivateConversationBot.Web.Handlers
{
    public class VideoForwarder : HandlerBase
    {
        public VideoForwarder(PrivateConversationBotDbContext dbContext) : base(dbContext) { }

        public override async Task HandleAsync(IUpdateContext context, UpdateDelegate next, CancellationToken cancellationToken)
        {
            if (AdminUser != null)
            {
                await RegisterMessage(
                    context,
                    (replyToMessageId, client, message) => client.SendVideoAsync(
                        AdminUser.LatestChatId,
                        new InputOnlineFile(message.Video.FileId),
                        message.Video.Duration,
                        message.Video.Width,
                        message.Video.Height,
                        message.Caption,
                        replyToMessageId: replyToMessageId,
                        disableNotification: true,
                        cancellationToken: cancellationToken),
                    cancellationToken);
            }
        }
    }
}
