using System;
using System.Threading;
using Microsoft.AspNetCore.Builder;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Abstractions;

namespace PrivateConversationBot.Web.Extensions
{
    public static class AppStartupExtensions
    {
        public static IApplicationBuilder UseTelegramBotLongPolling<TBot>(
            this IApplicationBuilder app,
            IBotBuilder botBuilder,
            TimeSpan startAfter = default,
            CancellationToken cancellationToken = default
        )
            where TBot : BotBase
        {
            return app;
        }
    }
}