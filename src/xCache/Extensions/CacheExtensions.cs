using System;
using System.Threading.Tasks;

namespace xCache.Extensions
{
    public static class CacheExtensions
    {
        public static async Task<T> AddToCacheAsync<T>(ICache cache, Task<T> task, string cacheKey,
            DateTime expires)
        {
            var value = await task.ConfigureAwait(false);

            cache.Add(cacheKey, Wrap(value, expires));

            return value;
        }

        public static T AddToCache<T>(ICache cache, T value, string cacheKey,
            DateTime expires)
        {
            cache.Add(cacheKey, Wrap(value, expires));

            return value;
        }

        public static CacheItem<T> Wrap<T>(T value, DateTime expires)
        {
            return new CacheItem<T>
            {
                Expires = expires,
                Item = value,
                LastUpdate = DateTime.UtcNow
            };
        }

        public static DateTime GetLastUpdate<T>(CacheItem<T> item)
        {
            return item.LastUpdate;
        }

        public static DateTime GetExpires<T>(CacheItem<T> item)
        {
            return item.Expires;
        }

        public static T Unwrap<T>(CacheItem<T> item)
        {
            return item.Item;
        }
    }
}
