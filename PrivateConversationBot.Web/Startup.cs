using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PrivateConversationBot.Web.Extensions;
using PrivateConversationBot.Web.Handlers;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Abstractions;

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
            services.AddControllersWithViews();

            services.AddTransient<ConversationBot>()
                .Configure<BotOptions<ConversationBot>>(Configuration.GetSection("ConversationBot"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseTelegramBotLongPolling<ConversationBot>(ConfigureBot());
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
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
                .UseWhen<UpdateMembersList>(When.MembersChanged)
                .MapWhen(When.NewMessage, msgBranch => msgBranch
                    .MapWhen(When.NewTextMessage, txtBranch => txtBranch
                        .Use<TextEchoer>()
                        .MapWhen(When.NewCommand, cmdBranch => cmdBranch
                            .UseCommand<PingCommand>()
                            .UseCommand<StartCommand>()
                        )
                    )
                    .MapWhen<StickerHandler>(When.StickerMessage)
                );
        }
    }
}
