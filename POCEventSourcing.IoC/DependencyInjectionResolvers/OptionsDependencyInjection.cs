using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using POCEventSourcing.Options;

namespace POCEventSourcing.IoC
{
    internal static class OptionsDependencyInjection
    {
        internal static void Configure(IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();

            var currentEnv = configuration.GetCurrentTenantEnvironment();

            var configBuilder = new ConfigurationBuilder();

            var tenantConfig = 
                configBuilder
                    .AddConfiguration(configuration)
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile($"{currentEnv}.tenant.json")
                    .Build();

            services.Configure<TenantOptions>(tenantConfig);
            services.Configure<CacheOptions>(tenantConfig.GetSection(nameof(CacheOptions)));
            services.Configure<ReadableDatabaseOptions>(tenantConfig.GetSection(nameof(ReadableDatabaseOptions)));
            services.Configure<AuditLogTableStorageOptions>(tenantConfig.GetSection(nameof(AuditLogTableStorageOptions)));
            services.Configure<WritableDatabaseOptions>(tenantConfig.GetSection(nameof(WritableDatabaseOptions)));
        }
    }
}
