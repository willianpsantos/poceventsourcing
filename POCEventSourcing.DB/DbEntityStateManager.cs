using POCEventSourcing.Core;
using POCEventSourcing.Interfaces.DB;

namespace POCEventSourcing.DB
{
    public class DbEntityStateManager : IFullDbEntityStateManager
    {
        private readonly IReadableDbEntityStateManager _readableDbEntityManager;
        private readonly IWritableDbEntityStateManager _writableDbEntityManager;
        private readonly ICacheDbEntityStateManager _cacheDbEntityStateManager;

        private bool _writeOnReadableDb = true;
        private bool _writeOnCacheDb = true;       

        public DbEntityStateManager
        (
            IWritableDbEntityStateManager writableDb, 
            IReadableDbEntityStateManager readableDb, 
            ICacheDbEntityStateManager cacheDb
        )
        {
            _writableDbEntityManager = writableDb;
            _readableDbEntityManager = readableDb;
            _cacheDbEntityStateManager = cacheDb;
        }

        public IReadOnlyList<EntityEventEntry<TEntity>> GetEntityEventEntries<TEntity>() where TEntity : Entity
        {
            var parsed = _writableDbEntityManager.GetEntityEventEntries<TEntity>();

            return parsed;
        }

        public void ClearEntityEventEntries()
        {
            _writableDbEntityManager.ClearEntityEventEntries();
        }

        public IFullDbEntityStateManager WriteOnReadableDatabase(bool write)
        {
            _writeOnReadableDb = write;
            return this;
        }

        public IFullDbEntityStateManager WriteOnCacheDatabase(bool write)
        {
            _writeOnCacheDb = write;
            return this;
        }

        public async Task DeleteAsync<TEntity>(TEntity entity) where TEntity : Entity
        {
            if (_writeOnCacheDb)
            {
                await _cacheDbEntityStateManager.DeleteAsync(entity);
            }

            if (_writeOnReadableDb)
            {
                await _readableDbEntityManager.DeleteAsync(entity);
            }

            await _writableDbEntityManager.DeleteAsync(entity);
        }

        public async Task<long> InsertAsync<TEntity>(TEntity entity) where TEntity : Entity
        {
            var id = await _writableDbEntityManager.InsertAsync(entity);

            entity.Id = id;

            if (_writeOnReadableDb)
            {
                await _readableDbEntityManager.InsertAsync(entity);
            }

            if (_writeOnCacheDb)
            {
                await _cacheDbEntityStateManager.InsertAsync(entity);
            }

            return id;
        }

        public async Task UpdateAsync<TEntity>(TEntity entity) where TEntity : Entity
        {
            await _writableDbEntityManager.UpdateAsync(entity);

            if (_writeOnReadableDb)
            {
                await _readableDbEntityManager.UpdateAsync(entity);
            }

            if (_writeOnCacheDb)
            {
                await _cacheDbEntityStateManager.UpdateAsync(entity);
            }
        }        
    }
}
