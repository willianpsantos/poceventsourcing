namespace POCEventSourcing.Interfaces.Trackers
{
    public interface IEntityChangeStoredMessage
    {
        public string PartitionKey { get; set; }
        public IEnumerable<IEntityChangeStoredResponse> Responses { get; set; }
    }
}
