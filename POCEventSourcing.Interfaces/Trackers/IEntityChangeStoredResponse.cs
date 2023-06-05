namespace POCEventSourcing.Interfaces.Trackers
{
    public interface IEntityChangeStoredResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTime? EventDate { get; set; }
    }
}
