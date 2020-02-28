using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using PrivateConversationBot.Web.DataAccess;
using PrivateConversationBot.Web.Models;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;

namespace PrivateConversationBot.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ConversationBot _bot;
        private readonly PrivateConversationBotDbContext _dbContext;

        public HomeController(ILogger<HomeController> logger, ConversationBot bot, PrivateConversationBotDbContext dbContext)
        {
            _logger = logger;
            _bot = bot;
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile myZip)
        {
            var tempFilePath = Path.GetTempFileName();
            using (var stream = System.IO.File.Create(tempFilePath))
            {
                myZip.CopyTo(stream);
            }

            try
            {
                var admin = _dbContext.Users.FirstOrDefault(x => x.IsAdmin);
                if (admin == null) return View();
                using (var ar = ZipFile.OpenRead(tempFilePath))
                {
                    var pageSize = 10;
                    var orderedEntries = ar.Entries.OrderBy(x => x.Name).ToArray();
                    var pageNumber = 0;
                    while (true)
                    {
                        var page = orderedEntries.Skip(pageNumber * pageSize).Take(pageSize).ToArray();
                        if (!page.Any()) break;
                        await _bot.Client.SendMediaGroupAsync(
                            page.Select(x => new InputMediaPhoto(new InputMedia(x.Open(), x.Name))),
                            admin.LatestChatId,
                            cancellationToken: CancellationToken.None).ConfigureAwait(false);
                        pageNumber++;
                    }
                }
            }
            finally
            {
                System.IO.File.Delete(tempFilePath);
            }
            return RedirectToAction("Index");
        }
    }
}
