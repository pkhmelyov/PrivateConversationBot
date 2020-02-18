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

        public override Task HandleAsync(IUpdateContext context, UpdateDelegate next, CancellationToken cancellationToken)
        {
            var currentUser = (User)context.Items[Constants.UpdateContextItemKeys.CurrentUser];
            RegisterMessage(currentUser, () => context.Bot.Client.SendPhotoAsync(AdminUser.LatestChatId, context.Update.Message.Photo[0].))
            _logger.LogInformation("ku");
            return next(context, cancellationToken);
        }
    }
}
