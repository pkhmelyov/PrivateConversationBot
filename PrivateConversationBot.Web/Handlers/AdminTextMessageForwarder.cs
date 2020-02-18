using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PrivateConversationBot.Web.DataAccess;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Types.Enums;

namespace PrivateConversationBot.Web.Handlers
{
    public class AdminTextMessageForwarder : HandlerBase
    {
        public AdminTextMessageForwarder(PrivateConversationBotDbContext dbContext) : base(dbContext) { }

        public override async Task HandleAsync(IUpdateContext context, UpdateDelegate next, CancellationToken cancellationToken)
        {
            await ReplyToAppropriateUser(
                context,
                context.Update.Message.ReplyToMessage.MessageId,
                chatId => context.Bot.Client.SendTextMessageAsync(
                    chatId,
                    context.Update.Message.Text,
                    cancellationToken: cancellationToken),
                cancellationToken);

            await next(context, cancellationToken);
        }
    }
}
