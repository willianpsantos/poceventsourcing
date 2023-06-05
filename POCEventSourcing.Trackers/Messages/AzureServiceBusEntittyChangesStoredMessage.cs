using POCEventSourcing.Interfaces.Trackers;

namespace POCEventSourcing.Trackers.Messages
{
    public class AzureServiceBusEntittyChangesStoredMessage : IEntityChangeStoredMessage
    {
        public string PartitionKey { get; set; }
        public IEnumerable<IEntityChangeStoredResponse> Responses { get; set; }
    }
}
