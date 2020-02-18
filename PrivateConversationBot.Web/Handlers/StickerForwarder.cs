using System.Threading;
using System.Threading.Tasks;
using PrivateConversationBot.Web.DataAccess;
using PrivateConversationBot.Web.DataAccess.Entities;
using Telegram.Bot.Framework.Abstractions;

namespace PrivateConversationBot.Web.Handlers
{
    public class StickerForwarder : HandlerBase
    {
        public StickerForwarder(PrivateConversationBotDbContext context) : base(context) { }

        public override async Task HandleAsync(IUpdateContext context, UpdateDelegate next, CancellationToken cancellationToken)
        {
            var currentUser = (User) context.Items[Constants.UpdateContextItemKeys.CurrentUser];

            if (AdminUser != null)
            {
                await RegisterMessage(
                    currentUser,
                    () => context.Bot.Client.SendStickerAsync(
                        AdminUser.LatestChatId,
                        context.Update.Message.Sticker.FileId,
                        cancellationToken: cancellationToken),
                    cancellationToken);
            }

            await next(context, cancellationToken);
        }
    }
}
