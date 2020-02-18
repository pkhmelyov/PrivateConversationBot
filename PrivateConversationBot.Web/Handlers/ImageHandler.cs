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
    public class ImageHandler : HandlerBase
    {
        private readonly ILogger<ImageHandler> _logger;
        public ImageHandler(PrivateConversationBotDbContext dbContext, ILogger<ImageHandler> logger) : base(dbContext)
        {
            _logger = logger;
        }

        public override async Task HandleAsync(IUpdateContext context, UpdateDelegate next, CancellationToken cancellationToken)
        {
            var photos = context.Update.Message.Photo;
            _logger.LogInformation($"Photos count: {photos.Length}");

            foreach (var item in photos)
            {
                _logger.LogInformation($"Photo: {item.FileId}, {item.FileSize}, {item.Width}, {item.Height}");
            }

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
                        cancellationToken: cancellationToken),
                    cancellationToken);
            }

            await next(context, cancellationToken);
        }
    }
}
