using Microsoft.Extensions.Options;
using POCEventSourcing.Options;
using ServiceStack.Redis;

namespace POCEventSourcing.Cache
{
    public abstract class RedisCacheManagerBase
    {
        protected readonly CacheOptions _cacheOptions;

        protected RedisCacheManagerBase(IOptions<CacheOptions> options)
        {
            _cacheOptions = options.Value;
        }

        protected RedisClient GetRedisClient() => new RedisClient(_cacheOptions.Host, _cacheOptions.Port);
    }
}
