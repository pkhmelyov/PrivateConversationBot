using PrivateConversationBot.Web;

Host.CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(webBuilder =>
    {
	    webBuilder.UseStartup<Startup>();
    }).Build().Run();