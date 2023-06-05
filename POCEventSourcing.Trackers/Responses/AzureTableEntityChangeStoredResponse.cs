using POCEventSourcing.Interfaces.Trackers;

namespace POCEventSourcing.Trackers.Responses
{
    public class AzureTableEntityChangeStoredResponse : IEntityChangeStoredResponse
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public string RequestId { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public DateTime? EventDate { get; set; }
    }
}
