using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace POCEventSourcing.IoC
{
    public static class ServiceCollectionExtensions
    {
        public static void AddAllDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            var envBuilder = new TenantOptionsBuilder(configuration);
            var env = envBuilder.Build();

            MassTransitDependencyInjection.Configure(services, configuration, env);
            OptionsDependencyInjection.Configure(services, configuration);
            DbContextDependencyInjection.Configure(services, configuration, env);           
            TrackerDependencyInjection.Configure(services);
            CacheManagerDependencyInjection.Configure(services);
            RepositoryDependencyInjection.Configure(services);
            ServiceDependencyInjection.Configure(services);
        }
    }
}
