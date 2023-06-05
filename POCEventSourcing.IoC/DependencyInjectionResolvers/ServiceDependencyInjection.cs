using Microsoft.Extensions.DependencyInjection;
using POCEventSourcing.Interfaces.Services;
using POCEventSourcing.Services;

namespace POCEventSourcing.IoC
{
    internal static class ServiceDependencyInjection
    {
        internal static void Configure(IServiceCollection services)
        {
            services.AddScoped<IPersonService, PersonService>();
        }
    }
}