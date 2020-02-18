using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PrivateConversationBot.Web.DataAccess;
using PrivateConversationBot.Web.DataAccess.Entities;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Types.Enums;

namespace PrivateConversationBot.Web.Handlers
{
    public abstract class HandlerBase : IUpdateHandler
    {
        protected PrivateConversationBotDbContext DbContext { get; }

        protected HandlerBase(PrivateConversationBotDbContext dbContext)
        {
            DbContext = dbContext;
        }

        private User _adminUser;
        protected User AdminUser { get { return _adminUser ?? (_adminUser = DbContext.Users.FirstOrDefault(x => x.IsAdmin)); } }

        public abstract Task HandleAsync(IUpdateContext context, UpdateDelegate next, CancellationToken cancellationToken);

        protected async Task RegisterMessage(User currentUser, Func<Task<Telegram.Bot.Types.Message>> messageSender, CancellationToken cancellationToken)
        {
            var sentMessage = await messageSender();
            var dbMessage = new Message
            {
                Id = sentMessage.MessageId,
                UserId = currentUser.Id
            };
            await DbContext.Messages.AddAsync(dbMessage, cancellationToken);
            await DbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task ReplyToAppropriateUser(IUpdateContext context, int replyMessageId, Func<long, Task> messageSender,
            CancellationToken cancellationToken)
        {
            var user = await DbContext.Messages.Where(x => x.Id == replyMessageId)
                .Join(DbContext.Users, m => m.UserId, u => u.Id, (_, u) => u).FirstOrDefaultAsync(cancellationToken);

            if (user != null)
            {
                await messageSender(user.LatestChatId);
            }
            else
            {
                await context.Bot.Client.SendTextMessageAsync(context.Update.Message.Chat, "*USER NOT FOUND*",
                    ParseMode.Markdown, replyToMessageId: context.Update.Message.MessageId,
                    cancellationToken: cancellationToken);
            }
        }
    }
}
