using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PrivateConversationBot.Web.DataAccess;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Types.Enums;

namespace PrivateConversationBot.Web.Handlers.Commands
{
    public class GetUsersCommand : CommandBase
    {
        private readonly PrivateConversationBotDbContext _dbContext;

        public GetUsersCommand(PrivateConversationBotDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public override async Task HandleAsync(IUpdateContext context, UpdateDelegate next, string[] args, CancellationToken cancellationToken)
        {
            var admin = await _dbContext.Users.FirstOrDefaultAsync(x => x.IsAdmin, cancellationToken);

            if (admin != null)
            {
                foreach (var user in _dbContext.Users)
                {
                    var role = user.IsAdmin ? ", admin" : "";
                    await context.Bot.Client.SendTextMessageAsync(
                        admin.LatestChatId,
                        $"[{user.FirstName} {user.LastName}{role}](tg://user?id={user.Id}) @{user.Username}",
                        ParseMode.Markdown,
                        cancellationToken: cancellationToken);
                }
            }
        }
    }
}
