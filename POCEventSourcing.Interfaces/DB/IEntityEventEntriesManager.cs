using POCEventSourcing.Core;

namespace POCEventSourcing.Interfaces.DB
{
    public interface IEntityEventEntriesManager
    {
        IReadOnlyList<EntityEventEntry<TEntity>> GetEntityEventEntries<TEntity>() where TEntity : Entity;

        void ClearEntityEventEntries();
    }
}
