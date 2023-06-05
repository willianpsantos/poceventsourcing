using POCEventSourcing.Core;

namespace POCEventSourcing.ReplicationJob.Interfaces
{
    internal interface IEntityEventTrackerProcessor
    {
        Task ProcessEntryAsync(EntityChangesTrackerEventEntry entry);
    }
}
