using System.Threading;
using System.Threading.Tasks;
using PrivateConversationBot.Web.DataAccess;
using Telegram.Bot.Framework.Abstractions;

namespace PrivateConversationBot.Web.Handlers
{
    public class ImageForwarder : HandlerBase
    {
        public ImageForwarder(PrivateConversationBotDbContext dbContext) : base(dbContext) { }

        public override async Task HandleAsync(IUpdateContext context, UpdateDelegate next, CancellationToken cancellationToken)
        {
            await RegisterMessage(
                    context,
                    (replyToMessageId, client, message) =>
                    {
                        var photos = message.Photo;
                        var index = 0;
                        for (var i = 1; i < photos.Length; i++)
                        {
                            if (photos[i].Width > photos[index].Width)
                            {
                                index = i;
                            }
                        }
                        var photoToSend = photos[index];
                        return client.SendPhotoAsync(
                            AdminUser.LatestChatId,
                            photoToSend.FileId,
                            message.Caption,
                            replyToMessageId: replyToMessageId,
                            disableNotification: true,
                            cancellationToken: cancellationToken);
                    },
                    cancellationToken);
        }
    }
}
