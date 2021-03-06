﻿using System.Threading;
using System.Threading.Tasks;
using PrivateConversationBot.Web.DataAccess;
using Telegram.Bot.Framework.Abstractions;

namespace PrivateConversationBot.Web.Handlers
{
    public class StickerForwarder : HandlerBase
    {
        public StickerForwarder(PrivateConversationBotDbContext context) : base(context) { }

        public override async Task HandleAsync(IUpdateContext context, UpdateDelegate next, CancellationToken cancellationToken)
        {
            await RegisterMessage(
                context,
                (replyToMessageId, client, message) => client.SendStickerAsync(
                    AdminUser.LatestChatId,
                    message.Sticker.FileId,
                    replyToMessageId: replyToMessageId,
                    disableNotification: true,
                    cancellationToken: cancellationToken),
                cancellationToken);
        }
    }
}
