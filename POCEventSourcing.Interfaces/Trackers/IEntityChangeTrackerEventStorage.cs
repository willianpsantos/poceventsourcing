using POCEventSourcing.Core;

namespace POCEventSourcing.Interfaces.Trackers
{
    public interface IEntityChangeTrackerEventStorage
    {
        Task<IEntityChangeStoredMessage> StoreChangesAsync<TEntity>(IEnumerable<EntityEventEntry<TEntity>> entries) where TEntity : Entity;
    }
}
