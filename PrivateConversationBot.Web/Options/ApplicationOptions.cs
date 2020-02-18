using System;
using Npgsql;

namespace PrivateConversationBot.Web.Options
{
    public class ApplicationOptions
    {
        public string DATABASE_URL {get;set;}
        public string PostgreSqlConnectionString {
            get {
                Uri databaseUri = new Uri(DATABASE_URL);
                NpgsqlConnectionStringBuilder builder = new NpgsqlConnectionStringBuilder();
                builder.Username = databaseUri.UserInfo.Split(":")[0];
                builder.Password = databaseUri.UserInfo.Split(":")[1];
                builder.Host = databaseUri.Host;
                builder.Port = databaseUri.Port;
                builder.Database = databaseUri.AbsolutePath.Trim('/');
                builder.SslMode = SslMode.Prefer;
                builder.TrustServerCertificate = true;
                return builder.ConnectionString;
            }
        }
    }
}