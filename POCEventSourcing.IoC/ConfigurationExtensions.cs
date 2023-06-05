using Microsoft.Extensions.Configuration;

namespace POCEventSourcing.IoC
{
    internal static class ConfigurationExtensions
    {
        internal static string GetCurrentTenantEnvironment(this IConfiguration configuration)
        {
            var envName =
                    configuration
                        .GetSection("TenantEnvironment")
                        .Value;

            return envName;
        }
    }
}
