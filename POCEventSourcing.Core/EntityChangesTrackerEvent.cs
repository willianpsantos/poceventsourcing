namespace POCEventSourcing.Core
{
    public class EntityChangesTrackerEvent
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public IEnumerable<EntityChangesTrackerEventEntry> Entries { get; set; }
    }
}