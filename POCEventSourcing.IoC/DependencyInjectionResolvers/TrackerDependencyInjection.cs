using Microsoft.Extensions.DependencyInjection;
using POCEventSourcing.Interfaces.Trackers;
using POCEventSourcing.Trackers;

namespace POCEventSourcing.IoC
{
    internal static class TrackerDependencyInjection
    {
        internal static void Configure(IServiceCollection services)
        {
            services.AddScoped<IEntityChangeTrackerEventStorage, AzureTableEntityChangesTrackingStorage>();
            services.AddScoped<IPostStoredEntityChangeTracking, AzureServiceBusPostStoredEntityChangeTracking>();
        }
    }
}