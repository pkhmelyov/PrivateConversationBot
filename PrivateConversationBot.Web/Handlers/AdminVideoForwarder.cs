using System.Threading;
using System.Threading.Tasks;
using PrivateConversationBot.Web.DataAccess;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Types.InputFiles;

namespace PrivateConversationBot.Web.Handlers
{
    public class AdminVideoForwarder : HandlerBase
    {
        public AdminVideoForwarder(PrivateConversationBotDbContext dbContext) : base(dbContext) { }

        public override async Task HandleAsync(IUpdateContext context, UpdateDelegate next, CancellationToken cancellationToken)
        {
            var video = context.Update.Message.Video;

            await ReplyToAppropriateUser(
                context,
                context.Update.Message.ReplyToMessage?.MessageId ?? 0,
                chatId => context.Bot.Client.SendVideoAsync(
                    chatId,
                    new InputOnlineFile(video.FileId),
                    video.Duration,
                    video.Width,
                    video.Height,
                    context.Update.Message.Caption,
                    cancellationToken: cancellationToken),
                cancellationToken);
            await next(context, cancellationToken);
        }
    }
}
