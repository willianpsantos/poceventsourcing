using POCEventSourcing.Enums;

namespace POCEventSourcing.Core
{
    public class EntityChangesTrackerEventEntry
    {
        [TrackerIgnore]
        public string? PartitionKey { get; set; }

        [TrackerIgnore]
        public string RowKey { get; set; }
        public long EntityId { get; set; }
        public long? ParentEntityId { get; set; }
        public string? EntityName { get; set; }
        public EntityEventState State { get; set; }
        public string? Original { get; set; }        
        public string? Updated { get; set; }
        public DateTime EventDate { get; set; }
    }
}