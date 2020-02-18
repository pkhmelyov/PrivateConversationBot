using System.Threading;
using System.Threading.Tasks;
using PrivateConversationBot.Web.DataAccess;
using Telegram.Bot.Framework.Abstractions;

namespace PrivateConversationBot.Web.Handlers
{
    public class Authenticator : IUpdateHandler
    {
        private readonly PrivateConversationBotDbContext _dbContext;

        public Authenticator(PrivateConversationBotDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task HandleAsync(IUpdateContext context, UpdateDelegate next, CancellationToken cancellationToken)
        {
            var fromUser = context.Update.Message.From;
            var dbUser = await _dbContext.Users.FindAsync(new object[] {fromUser.Id}, cancellationToken);
            if (dbUser != null)
            {
                dbUser.LatestChatId = context.Update.Message.Chat.Id;
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            context.Items[Constants.UpdateContextItemKeys.CurrentUser] = dbUser;
            await next(context, cancellationToken);
        }
    }
}
