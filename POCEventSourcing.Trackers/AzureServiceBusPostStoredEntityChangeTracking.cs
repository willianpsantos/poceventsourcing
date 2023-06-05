using MassTransit;
using POCEventSourcing.Interfaces.Trackers;

namespace POCEventSourcing.Trackers
{
    public class AzureServiceBusPostStoredEntityChangeTracking : IPostStoredEntityChangeTracking
    {
        private readonly IPublishEndpoint _publisher;

        public AzureServiceBusPostStoredEntityChangeTracking(IPublishEndpoint publisher)
        {
            _publisher = publisher;
        }

        public async Task SendResultsAsync(IEntityChangeStoredMessage message)
        {
            await _publisher.Publish(message);
        }
    }
}
