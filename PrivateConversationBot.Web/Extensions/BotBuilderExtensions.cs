using System;
using PrivateConversationBot.Web.Middlewares;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Abstractions;

namespace PrivateConversationBot.Web.Extensions
{
    public static class BotBuilderExtensions
    {

        public static IBotBuilder MapWhen(
             this IBotBuilder builder,
             Predicate<IUpdateContext> predicate,
             Action<IBotBuilder> configure
         )
        {
            var mapBuilder = new BotBuilder();
            configure(mapBuilder);
            UpdateDelegate mapDelegate = mapBuilder.Build();

            builder.Use(new MapWhenMiddleware(predicate, mapDelegate));

            return builder;
        }

        public static IBotBuilder MapWhen<THandler>(
            this IBotBuilder builder,
            Predicate<IUpdateContext> predicate
        )
            where THandler : IUpdateHandler
        {
            var branchDelegate = new BotBuilder().Use<THandler>().Build();
            builder.Use(new MapWhenMiddleware(predicate, branchDelegate));
            return builder;
        }
    }
}
