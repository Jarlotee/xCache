using System;

namespace xCache
{
    /// <summary>
    /// Default Cache Strategy
    /// </summary>
    public class MemoryCache : ICache
    {
        System.Runtime.Caching.MemoryCache _cache = System.Runtime.Caching.MemoryCache.Default;

        public void Add<T>(string key, T item, TimeSpan expiration)
        {
            _cache.Add(key, item, new DateTimeOffset(DateTime.Now.Add(expiration)));
        }

        public T Get<T>(string key)
        {
            return (T)_cache.Get(key);
        }
    }
}
