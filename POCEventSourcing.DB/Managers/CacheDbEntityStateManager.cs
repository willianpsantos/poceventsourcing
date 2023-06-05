using POCEventSourcing.Interfaces.DB;
using POCEventSourcing.Interfaces.Cache;
using POCEventSourcing.Core;

namespace POCEventSourcing.DB.Managers
{
    public class CacheDbEntityStateManager : ICacheDbEntityStateManager
    {
        private readonly ICacheManager _cacheManager;

        public CacheDbEntityStateManager(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }

        public async Task DeleteAsync<TEntity>(TEntity entity) where TEntity : Entity
        {
            var cacheKey = entity?.GetCacheKey();

            if (string.IsNullOrEmpty(cacheKey) || string.IsNullOrWhiteSpace(cacheKey))
            {
                return;
            }

            var task = Task.Factory.StartNew(() => _cacheManager.Remove(cacheKey));

            await task;
        }

        public async Task<long> InsertAsync<TEntity>(TEntity entity) where TEntity : Entity
        {
            var cacheKey = entity?.GetCacheKey();

            if (string.IsNullOrEmpty(cacheKey) || string.IsNullOrWhiteSpace(cacheKey))
            {
                return 0;
            }

            var task = Task.Factory.StartNew(() => _cacheManager.Store<TEntity>(cacheKey, entity));

            await task;

            return entity.Id;
        }

        public async Task UpdateAsync<TEntity>(TEntity entity) where TEntity : Entity
        {
            var cacheKey = entity?.GetCacheKey();

            if (string.IsNullOrEmpty(cacheKey) || string.IsNullOrWhiteSpace(cacheKey))
            {
                return;
            }

            var task = Task.Factory.StartNew(() => _cacheManager.Store<TEntity>(cacheKey, entity));

            await task;
        }
    }
}
