using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using POCEventSourcing.Interfaces.Repositories;
using POCEventSourcing.Options;
using POCEventSourcing.Repositories;

namespace POCEventSourcing.IoC
{
    internal static class RepositoryDependencyInjection
    {
        internal static void Configure(IServiceCollection services)
        {
            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<IPersonAddressRepository, PersonAddressRepository>();
        }
    }
}