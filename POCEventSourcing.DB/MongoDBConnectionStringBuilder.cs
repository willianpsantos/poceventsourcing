using POCEventSourcing.Options;
using System.Text;

namespace POCEventSourcing.DB
{
    internal class MongoDBConnectionStringBuilder
    {
        public static string BuildConnectionString(ReadableDatabaseOptions options)
        {
            var builder = new StringBuilder();

            builder.Append("mongodb://");
            builder.Append(options.Host);

            if(options.Port > 0)
            {
                builder.Append(":" + options.Port);
            }

            if(!string.IsNullOrEmpty(options.Username) && !string.IsNullOrWhiteSpace(options.Username))
            {
                builder.Append("Username=" + options.Username);
            }

            if (!string.IsNullOrEmpty(options.Password) && !string.IsNullOrWhiteSpace(options.Password))
            {
                builder.Append("Password=" + options.Password);
            }

            return builder.ToString();
        }
    }
}
