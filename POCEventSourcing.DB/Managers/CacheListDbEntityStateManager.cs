using POCEventSourcing.Interfaces.DB;
using POCEventSourcing.Interfaces.Cache;
using POCEventSourcing.Core;

namespace POCEventSourcing.DB.Managers
{
    public class CacheListDbEntityStateManager : ICacheDbEntityStateManager
    {
        private readonly ICacheListManager _cacheManager;

        public CacheListDbEntityStateManager(ICacheListManager cacheManager)
        {
            _cacheManager = cacheManager;
        }

        public async Task DeleteAsync<TEntity>(TEntity entity) where TEntity : Entity
        {
            var task = Task.Factory.StartNew(() => _cacheManager.Remove(entity));

            await task;
        }

        public async Task<long> InsertAsync<TEntity>(TEntity entity) where TEntity : Entity
        {            
            var task = Task.Factory.StartNew(() => _cacheManager.Add<TEntity>(entity));

            await task;

            return entity.Id;
        }

        public async Task UpdateAsync<TEntity>(TEntity entity) where TEntity : Entity
        {            
            var task = Task.Factory.StartNew(() => _cacheManager.Update<TEntity>(entity));

            await task;
        }
    }
}
