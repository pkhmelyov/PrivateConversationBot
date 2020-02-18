﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstractions;

namespace PrivateConversationBot.Web.Middlewares
{
    public class UseWhenMiddleware : IUpdateHandler
    {
        private readonly Predicate<IUpdateContext> _predicate;

        private readonly UpdateDelegate _branch;

        public UseWhenMiddleware(Predicate<IUpdateContext> predicate, UpdateDelegate branch)
        {
            _predicate = predicate;
            _branch = branch;
        }

        public async Task HandleAsync(IUpdateContext context, UpdateDelegate next, CancellationToken cancellationToken)
        {
            if (_predicate(context))
            {
                await _branch(context, cancellationToken).ConfigureAwait(false);
            }

            await next(context, cancellationToken).ConfigureAwait(false);
        }
    }
}
