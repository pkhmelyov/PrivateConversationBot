using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PrivateConversationBot.Web.DataAccess;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Types;
using Message = PrivateConversationBot.Web.DataAccess.Entities.Message;
using User = PrivateConversationBot.Web.DataAccess.Entities.User;

namespace PrivateConversationBot.Web.Handlers
{
    public class TextMessageForwarder : HandlerBase
    {
        public TextMessageForwarder(PrivateConversationBotDbContext dbContext) : base(dbContext) { }

        public override async Task HandleAsync(IUpdateContext context, UpdateDelegate next, CancellationToken cancellationToken)
        {
            var currentUser = (User) context.Items[Constants.UpdateContextItemKeys.CurrentUser];

            if (AdminUser != null)
            {
                await RegisterMessage(
                    currentUser,
                    () => context.Bot.Client.SendTextMessageAsync(
                        AdminUser.LatestChatId,
                        context.Update.Message.Text,
                        cancellationToken: cancellationToken),
                    cancellationToken);
            }

            await next(context, cancellationToken);
        }
    }
}
