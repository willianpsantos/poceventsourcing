using Microsoft.Extensions.Options;
using POCEventSourcing.Interfaces.Cache;
using POCEventSourcing.Options;
using ServiceStack.Redis;

namespace POCEventSourcing.Cache
{
    public class RedisCacheManager : RedisCacheManagerBase, ICacheManager
    {
        public RedisCacheManager(IOptions<CacheOptions> options) : base(options)
        {

        }

        public T Get<T>(string key)
        {
            using var redisClient = GetRedisClient();

            T value = redisClient.Get<T>(key);

            return value;
        }

        public bool Store<T>(string key, T value, TimeSpan? expire = null)
        {
            using var redisClient = GetRedisClient();
            var success = true;

            if (expire.HasValue)
            {
                success = redisClient.Set<T>(key, value, expire.Value);
            }
            else
            {
                success = redisClient.Set<T>(key, value);
            }

            return success;
        }

        public bool Remove(string key)
        {
            using var client = GetRedisClient();

            var success =  client.Remove(key);

            return success;
        }
    }
}