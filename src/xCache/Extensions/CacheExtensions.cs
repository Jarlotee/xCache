using System;
using System.Threading.Tasks;

namespace xCache.Extensions
{
    public static class CacheExtensions
    {
        public static async Task<T> AddToCache<T>(ICache cache, Task<T> task, string cacheKey,
            TimeSpan? absoluteExpiration,
            TimeSpan expiration)
        {
            var value = await task.ConfigureAwait(false);

            if (value != null)
            {
                cache.Add(cacheKey, value, absoluteExpiration ?? expiration);
            }

            return value;
        }
    }
}
