using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrivateConversationBot.Web.DataAccess.Entities
{
    public class User
    {
        public int Id { get; set; }
        public bool IsBot { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string LanguageCode { get; set; }
        public long LatestChatId { get; set; }
        public bool IsAdmin { get; set; }
    }
}
