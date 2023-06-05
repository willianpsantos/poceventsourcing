using POCEventSourcing.Core;
using POCEventSourcing.ReplicationJob.DB;
using POCEventSourcing.ReplicationJob.Entities;

namespace POCEventSourcing.ReplicationJob.Processors
{
    internal abstract class BaseEntityEventTrackerProcessor
    {
        protected readonly POCReplicationDbContext _context;
        
        protected BaseEntityEventTrackerProcessor(POCReplicationDbContext context)
        {
            _context = context;
        }

        protected virtual void SetReplicationProperties(EntityChangesTrackerEventEntry entry, ref ReplicationEntity data)
        {
            data.ReplicatedAt = DateTime.UtcNow;
            data.LastRowKeyApplied = entry.RowKey;
            data.LastPartitionKeyApplied = entry.PartitionKey;
        }

        protected virtual void SetReplicationProperties<TEntity>(EntityChangesTrackerEventEntry entry, ref TEntity data) where TEntity : ReplicationEntity
        {
            data.ReplicatedAt = DateTime.UtcNow;
            data.LastRowKeyApplied = entry.RowKey;
            data.LastPartitionKeyApplied = entry.PartitionKey;
        }
    }
}
