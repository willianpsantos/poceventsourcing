using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using POCEventSourcing.DB;
using POCEventSourcing.Options;
using POCEventSourcing.DB.Managers;
using POCEventSourcing.Interfaces.DB;

namespace POCEventSourcing.IoC
{
    internal static class DbContextDependencyInjection
    {
        internal static void Configure(IServiceCollection services, IConfiguration configuration, TenantOptions tenantOptions)
        {
            var connectionString = tenantOptions.WritableDatabaseOptions.ConnectionString;

            services.AddDbContext<POCEventSourcingDbContext>(options =>
            {
                options.UseSqlServer(connectionString, sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly("POCEventSourcing.DB");
                });
            });

            services.AddScoped<DbContext, POCEventSourcingDbContext>();
            services.AddScoped<IWritableDbEntityStateManager, WritableDbEntityStateManager>();
            services.AddScoped<IReadableDbEntityStateManager, ReadableDbEntityStateManager>();
            //services.AddScoped<ICacheDbEntityStateManager, CacheDbEntityStateManager>();
            services.AddScoped<ICacheDbEntityStateManager, CacheListDbEntityStateManager>();
            services.AddScoped<IFullDbEntityStateManager, DbEntityStateManager>();
        }
    }
}