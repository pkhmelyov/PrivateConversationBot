using System.Threading;
using System.Threading.Tasks;
using PrivateConversationBot.Web.DataAccess;
using PrivateConversationBot.Web.DataAccess.Entities;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Types.InputFiles;

namespace PrivateConversationBot.Web.Handlers
{
    public class VideoForwarder : HandlerBase
    {
        public VideoForwarder(PrivateConversationBotDbContext dbContext) : base(dbContext) { }

        public override async Task HandleAsync(IUpdateContext context, UpdateDelegate next, CancellationToken cancellationToken)
        {
            var video = context.Update.Message.Video;

            var currentUser = (User)context.Items[Constants.UpdateContextItemKeys.CurrentUser];

            if (AdminUser != null)
            {
                await RegisterMessage(
                    context,
                    currentUser,
                    replyToMessageId => context.Bot.Client.SendVideoAsync(
                        AdminUser.LatestChatId,
                        new InputOnlineFile(video.FileId),
                        video.Duration,
                        video.Width,
                        video.Height,
                        context.Update.Message.Caption,
                        replyToMessageId: replyToMessageId,
                        disableNotification: true,
                        cancellationToken: cancellationToken),
                    cancellationToken);
            }

            await next(context, cancellationToken);
        }
    }
}
