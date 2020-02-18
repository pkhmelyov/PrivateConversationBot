using System.Threading;
using System.Threading.Tasks;
using PrivateConversationBot.Web.DataAccess;
using Telegram.Bot.Framework.Abstractions;

namespace PrivateConversationBot.Web.Handlers
{
    public class AdminStickerForwarder : HandlerBase
    {
        public AdminStickerForwarder(PrivateConversationBotDbContext dbContext) : base(dbContext) { }

        public override async Task HandleAsync(IUpdateContext context, UpdateDelegate next, CancellationToken cancellationToken)
        {
            await ReplyToAppropriateUser(
                context,
                context.Update.Message.ReplyToMessage.MessageId,
                chatId => context.Bot.Client.SendStickerAsync(
                    chatId,
                    context.Update.Message.Sticker.FileId,
                    cancellationToken: cancellationToken),
                cancellationToken);
            await next(context, cancellationToken);
        }
    }
}
