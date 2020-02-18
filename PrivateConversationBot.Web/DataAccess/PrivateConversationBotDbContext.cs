using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PrivateConversationBot.Web.DataAccess.Entities;

namespace PrivateConversationBot.Web.DataAccess
{
    public class PrivateConversationBotDbContext : DbContext
    {
        public PrivateConversationBotDbContext(DbContextOptions<PrivateConversationBotDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }
    }
}
