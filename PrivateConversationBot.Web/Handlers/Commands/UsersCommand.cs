using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstractions;

namespace PrivateConversationBot.Web.Handlers.Commands
{
    public class UsersCommand : CommandBase
    {
        public override Task HandleAsync(IUpdateContext context, UpdateDelegate next, string[] args, CancellationToken cancellationToken)
        {
            context.Bot.Client.SendTextMessageAsync(context.Update.Message.Chat, "asd").Wait();
            return Task.CompletedTask;
        }
    }
}