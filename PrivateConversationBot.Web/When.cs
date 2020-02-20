using System.Linq;
using Microsoft.AspNetCore.Http;
using PrivateConversationBot.Web.DataAccess.Entities;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Types.Enums;

namespace PrivateConversationBot.Web
{
    public class When
    {
        public static bool Webhook(IUpdateContext context) => context.Items.ContainsKey(nameof(HttpContext));

        public static bool NewMessage(IUpdateContext context) => context.Update.Message != null;

        public static bool NewTextMessage(IUpdateContext context) => context.Update.Message?.Text != null;

        public static bool NewCommand(IUpdateContext context) =>
            context.Update.Message?.Entities?.FirstOrDefault()?.Type == MessageEntityType.BotCommand;

        public static bool MembersChanged(IUpdateContext context) =>
            context.Update.Message?.NewChatMembers != null
            || context.Update.Message?.LeftChatMember != null
            || context.Update.ChannelPost?.NewChatMembers != null
            || context.Update.ChannelPost?.LeftChatMember != null;

        public static bool LocationMessage(IUpdateContext context) =>
            context.Update.Message?.Location != null;

        public static bool StickerMessage(IUpdateContext context) =>
            context.Update.Message?.Sticker != null;

        public static bool CallbackQuery(IUpdateContext context) =>
            context.Update.CallbackQuery != null;

        public static bool Authenticated(IUpdateContext context) =>
            context.Items.TryGetValue(Constants.UpdateContextItemKeys.CurrentUser, out var item) && item is User;

        public static bool Anonymous(IUpdateContext context) => !Authenticated(context);

        public static bool IsAdmin(IUpdateContext context) =>
            Authenticated(context) && ((User) context.Items[Constants.UpdateContextItemKeys.CurrentUser]).IsAdmin;

        public static bool IsNotAdmin(IUpdateContext context) => !IsAdmin(context);

        public static bool NewImage(IUpdateContext context) => context.Update.Message?.Photo != null;

        public static bool NewVideo(IUpdateContext context) => context.Update.Message?.Video != null;
    }
}
