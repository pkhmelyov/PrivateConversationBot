using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PrivateConversationBot.Web.DataAccess;
using PrivateConversationBot.Web.Extensions;
using PrivateConversationBot.Web.Handlers;
using PrivateConversationBot.Web.Handlers.Commands;
using PrivateConversationBot.Web.Options;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Types;

namespace PrivateConversationBot.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureApplicationOptions(Configuration);

            services.AddControllersWithViews();

            services.AddTransient<ConversationBot>()
                .Configure<BotOptions<ConversationBot>>(Configuration.GetSection("ConversationBot"))
                .Configure<CustomBotOptions<ConversationBot>>(Configuration.GetSection("ConversationBot"))
                .AddScoped<TextEchoer>()
                .AddScoped<PingCommand>()
                .AddScoped<StartCommand>()
                .AddScoped<WebhookLogger>()
                .AddScoped<StickerHandler>()
                .AddScoped<ExceptionHandler>()
                .AddScoped<UpdateMembersList>()
                .AddScoped<CallbackQueryHandler>()
                .AddScoped<Authenticator>()
                .AddScoped<TextMessageForwarder>()
                .AddScoped<AdminTextMessageForwarder>()
                .AddScoped<StickerForwarder>()
                .AddScoped<AdminStickerForwarder>()
                .AddScoped<ImageHandler>();

            var appOptions = Configuration.Get<ApplicationOptions>();

            services.AddDbContext<PrivateConversationBotDbContext>(options =>
                options.UseNpgsql(appOptions.PostgreSqlConnectionString));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, PrivateConversationBotDbContext dbContext)
        {
            dbContext.Database.Migrate();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseTelegramBotLongPolling<ConversationBot>(ConfigureBot(), startAfter: TimeSpan.FromSeconds(2));
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
                app.UseTelegramBotWebhook<ConversationBot>(ConfigureBot());
                app.EnsureWebhookSet<ConversationBot>();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private IBotBuilder ConfigureBot()
        {
            return new BotBuilder()
                .Use<ExceptionHandler>()
                .UseWhen<WebhookLogger>(When.Webhook)
                .MapWhen(When.NewMessage, msgBranch => msgBranch
                    .Use<Authenticator>()
                    .MapWhen(When.NewCommand, cmdBranch => cmdBranch.UseCommand<StartCommand>("start"))
                    .MapWhen(When.Anonymous, anonymousBranch => { })
                    .MapWhen(When.Authenticated, authenticatedBranch => authenticatedBranch
                        .MapWhen(When.NewTextMessage, txtBranch => txtBranch
                            .UseWhen<TextMessageForwarder>(When.IsNotAdmin)
                            .UseWhen<AdminTextMessageForwarder>(When.IsAdmin)
                        )
                        .MapWhen(When.StickerMessage, stickerBranch => stickerBranch
                            .UseWhen<StickerForwarder>(When.IsNotAdmin)
                            .UseWhen<AdminStickerForwarder>(When.IsAdmin)
                        )
                        .MapWhen(When.NewImage, imgBranch => imgBranch
                            .Use<ImageHandler>()
                        )
                    )
                );
        }
    }public static class IServiceCollectionExtensions {
        public static IServiceCollection ConfigureApplicationOptions(this IServiceCollection services, IConfiguration configurationRoot) {
            return services
                .Configure<ApplicationOptions>(configurationRoot);
        }
    }
}
