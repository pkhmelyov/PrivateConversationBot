using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PrivateConversationBot.Web.DataAccess;
using PrivateConversationBot.Web.DataAccess.Entities;
using Telegram.Bot.Framework.Abstractions;

namespace PrivateConversationBot.Web.Handlers
{
    public class ImageForwarder : HandlerBase
    {
        public ImageForwarder(PrivateConversationBotDbContext dbContext) : base(dbContext) { }

        public override async Task HandleAsync(IUpdateContext context, UpdateDelegate next, CancellationToken cancellationToken)
        {
            var photos = context.Update.Message.Photo;

            var currentUser = (User)context.Items[Constants.UpdateContextItemKeys.CurrentUser];

            if (photos.Length > 0)
            {
                var index = 0;
                for(var i = 1; i < photos.Length; i++) {
                    if(photos[i].Width > photos[index].Width){
                        index = i;
                    }
                }
                var photoToSend = photos[index];

                await RegisterMessage(
                    context,
                    currentUser,
                    replyToMessageId => context.Bot.Client.SendPhotoAsync(
                        AdminUser.LatestChatId,
                        photoToSend.FileId,
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
