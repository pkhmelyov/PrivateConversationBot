using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PrivateConversationBot.Web.DataAccess;
using PrivateConversationBot.Web.DataAccess.Entities;
using PrivateConversationBot.Web.Options;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Types.Enums;

namespace PrivateConversationBot.Web.Handlers.Commands
{
    public class StartCommand : CommandBase
    {
        private readonly PrivateConversationBotDbContext _dbContext;
        private readonly CustomBotOptions<ConversationBot> _options;

        public StartCommand(PrivateConversationBotDbContext dbContext, IOptions<CustomBotOptions<ConversationBot>> options)
        {
            _dbContext = dbContext;
            _options = options.Value;
        }

        public override async Task HandleAsync(IUpdateContext context, UpdateDelegate next, string[] args, CancellationToken cancellationToken)
        {
            var message = context.Update.Message;
            var chat = message.Chat;
            var fromUser = message.From;

            string replyText = $"Hello again, {fromUser.FirstName}! I missed you!";

            if (!context.Items.TryGetValue(Constants.UpdateContextItemKeys.CurrentUser, out var temp) || temp == null)
            {
                var currentUser = new User
                {
                    Id = fromUser.Id,
                    IsBot = fromUser.IsBot,
                    FirstName = fromUser.FirstName,
                    LastName = fromUser.LastName,
                    Username = fromUser.Username,
                    LanguageCode = fromUser.LanguageCode,
                    LatestChatId = chat.Id,
                    IsAdmin = !string.IsNullOrEmpty(_options.Admin) && fromUser.Username == _options.Admin
                };
                await _dbContext.Users.AddAsync(currentUser, cancellationToken);

                await _dbContext.SaveChangesAsync(cancellationToken);

                replyText = $"Hi, {fromUser.FirstName}!\nPlease, don't hesitate to chat with me!";
            }

            await context.Bot.Client.SendTextMessageAsync(chat,
                replyText,
                cancellationToken: cancellationToken);

            var adminUser = await _dbContext.Users.FirstOrDefaultAsync(x => x.IsAdmin);

            if (adminUser != null)
            {
                var adminMessage = await context.Bot.Client.SendTextMessageAsync(adminUser.LatestChatId, $"[{fromUser.FirstName}](tg://user?id={fromUser.Id}) joined.", ParseMode.Markdown, cancellationToken: cancellationToken);
                var dbMessage = new Message{
                    Id = adminMessage.MessageId,
                    UserId = fromUser.Id
                };
                await _dbContext.Messages.AddAsync(dbMessage, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }

            await next(context, cancellationToken);
        }
    }
}