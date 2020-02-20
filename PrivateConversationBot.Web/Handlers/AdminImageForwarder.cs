using System;
using System.Threading;
using System.Threading.Tasks;
using PrivateConversationBot.Web.DataAccess;
using Telegram.Bot.Framework.Abstractions;

namespace PrivateConversationBot.Web.Handlers
{
    public class AdminImageForwarder : HandlerBase
    {
        public AdminImageForwarder(PrivateConversationBotDbContext dbContext) : base(dbContext) { }

        public override async Task HandleAsync(IUpdateContext context, UpdateDelegate next, CancellationToken cancellationToken)
        {
            var photos = context.Update.Message.Photo;

            if (photos.Length > 0)
            {
                var index = 0;
                for (var i = 1; i < photos.Length; i++)
                {
                    if (photos[i].Width > photos[index].Width)
                    {
                        index = i;
                    }
                }
                var photoToSend = photos[index];

                await ReplyToAppropriateUser(
                    context,
                    context.Update.Message?.ReplyToMessage?.MessageId ?? 0,
                    chatId => context.Bot.Client.SendPhotoAsync(
                        chatId,
                        photoToSend.FileId,
                        context.Update.Message.Caption,
                        cancellationToken: cancellationToken),
                    cancellationToken);
            }

            await next(context, cancellationToken);
        }
    }
}
