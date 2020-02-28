using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PrivateConversationBot.Web.DataAccess;
using Telegram.Bot.Framework.Abstractions;

namespace PrivateConversationBot.Web.Handlers
{
    public class ChannelDocumentsHandler : HandlerBase
    {
        private readonly ILogger<ChannelDocumentsHandler> _logger;

        public ChannelDocumentsHandler(PrivateConversationBotDbContext dbContext,
            ILogger<ChannelDocumentsHandler> logger) : base(dbContext)
        {
            _logger = logger;
        }

        public override async Task HandleAsync(IUpdateContext context, UpdateDelegate next, CancellationToken cancellationToken)
        {
            var document = context.Update.ChannelPost.Document;
            _logger.LogCritical($"Size: {document.FileSize}");
            var file = await context.Bot.Client.GetFileAsync(document.FileId, cancellationToken);

            await context.Bot.Client.DownloadFileAsync(file.FilePath, File.Create("temp/test"), cancellationToken);

            await next(context, cancellationToken);
        }
    }
}
