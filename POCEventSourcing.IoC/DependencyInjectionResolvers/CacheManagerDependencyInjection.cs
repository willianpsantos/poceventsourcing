using Microsoft.Extensions.DependencyInjection;
using POCEventSourcing.Cache;
using POCEventSourcing.Interfaces.Cache;

namespace POCEventSourcing.IoC
{
    internal static class CacheManagerDependencyInjection
    {
        internal static void Configure(IServiceCollection services)
        {
            services.AddScoped<ICacheManager, RedisCacheManager>();
            services.AddScoped<ICacheListManager, RedisCacheListManager>();
        }
    }
}