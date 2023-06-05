using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using POCEventSourcing.Options;

namespace POCEventSourcing.IoC
{
    internal static class MassTransitDependencyInjection
    {
        public static void Configure(IServiceCollection services, IConfiguration configuration, TenantOptions tenantOptions)
        {
            var serviceBusConnectionString = tenantOptions.ServiceBusOptions.ConnectionString;

            services.AddMassTransit(massTransitCfg =>
            {
                massTransitCfg.UsingAzureServiceBus((serviceBusCtx, serviceBusCfg) =>
                {
                    serviceBusCfg.Host(serviceBusConnectionString);

                    serviceBusCfg.ReceiveEndpoint("entityEventChangesQueue", queueCfg =>
                    {
                        queueCfg.MaxConcurrentCalls = 100;
                        queueCfg.MaxAutoRenewDuration = TimeSpan.FromMinutes(30);
                    });

                    serviceBusCfg.ConfigureEndpoints(serviceBusCtx);
                });
            });

            //services.AddMassTransitHostedService();
        }
    }
}
