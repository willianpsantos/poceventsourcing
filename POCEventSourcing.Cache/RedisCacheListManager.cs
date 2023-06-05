using Microsoft.Extensions.Options;
using POCEventSourcing.Core;
using POCEventSourcing.Interfaces.Cache;
using POCEventSourcing.Options;

namespace POCEventSourcing.Cache
{
    public class RedisCacheListManager : RedisCacheManagerBase, ICacheListManager 
    {
        public RedisCacheListManager(IOptions<CacheOptions> options) : base(options)
        {
            
        }

        private string GetListName<TEntity>() => "List_of_" + typeof(TEntity).Name;

        public void Add<TEntity>(TEntity entity) where TEntity : Entity
        {
            using var redisCliente = GetRedisClient();
            var redisLists = redisCliente.As<TEntity>();
            var listName = GetListName<TEntity>();
            var list = redisLists.Lists[listName];

            list.Add(entity);
        }

        public IEnumerable<TEntity> Find<TEntity>(Func<TEntity, bool> expression) where TEntity : Entity
        {
            using var redisCliente = GetRedisClient();
            var redisLists = redisCliente.As<TEntity>();
            var listName = GetListName<TEntity>();
            var list = redisLists.Lists[listName];
            var result = list.Where(expression).ToArray();

            return result;
        }

        public bool Remove<TEntity>(TEntity entity) where TEntity : Entity
        {
            using var redisCliente = GetRedisClient();
            var redisLists = redisCliente.As<TEntity>();
            var listName = GetListName<TEntity>();
            var list = redisLists.Lists[listName];
            
            var success = list.Remove(entity);

            return success;
        }

        public void Update<TEntity>(TEntity entity) where TEntity : Entity
        {
            using var redisCliente = GetRedisClient();
            var redisLists = redisCliente.As<TEntity>();
            var listName = GetListName<TEntity>();
            var list = redisLists.Lists[listName];
            var item = list.Where(x => x.Id == entity.Id).FirstOrDefault();

            if(item == null)
            {
                return;
            }

            list.Remove(item);
            list.Add(entity);
        }

        public IEnumerable<TEntity> GetAll<TEntity>() where TEntity : Entity
        {
            using var redisCliente = GetRedisClient();
            var redisLists = redisCliente.As<TEntity>();
            var listName = GetListName<TEntity>();
            var list = redisLists.Lists[listName];

            return list.ToArray();
        }
    }
}
