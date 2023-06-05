using POCEventSourcing.Core;
using POCEventSourcing.Interfaces.DB;

namespace POCEventSourcing.Repositories
{
    public abstract class Repository : IEntityEventEntriesManager
    {
        protected readonly IFullDbEntityStateManager _dbManager;

        protected Repository(IFullDbEntityStateManager dbManager)
        {
            _dbManager = dbManager;
        }

        public virtual void ClearEntityEventEntries()
        {
            _dbManager.ClearEntityEventEntries();
        }

        public virtual IReadOnlyList<EntityEventEntry<TEntity>> GetEntityEventEntries<TEntity>() where TEntity : Entity
        {
            var entries = _dbManager.GetEntityEventEntries<TEntity>();

            return entries;
        }
    }
}
