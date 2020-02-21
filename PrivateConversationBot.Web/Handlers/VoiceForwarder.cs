using System.Threading;
using System.Threading.Tasks;
using PrivateConversationBot.Web.DataAccess;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Types.InputFiles;

namespace PrivateConversationBot.Web.Handlers
{
    public class VoiceForwarder : HandlerBase
    {
        public VoiceForwarder(PrivateConversationBotDbContext dbContext) : base(dbContext) { }

        public override async Task HandleAsync(IUpdateContext context, UpdateDelegate next, CancellationToken cancellationToken)
        {
            await RegisterMessage(
                context,
                (replyToMessageId, client, message) => client.SendVoiceAsync(
                    AdminUser.LatestChatId,
                    new InputOnlineFile(message.Voice.FileId),
                    message.Caption,
                    duration: message.Voice.Duration,
                    disableNotification: true,
                    replyToMessageId: replyToMessageId,
                    cancellationToken: cancellationToken),
                cancellationToken);
        }
    }
}
