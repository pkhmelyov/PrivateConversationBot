﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstractions;

namespace PrivateConversationBot.Web.Handlers
{
    public class UpdateMembersList : IUpdateHandler
    {
        public Task HandleAsync(IUpdateContext context, UpdateDelegate next, CancellationToken cancellationToken)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Updating chat members list...");
            Console.ResetColor();
            return next(context, cancellationToken);
        }
    }
}
