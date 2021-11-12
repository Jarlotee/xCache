using System.Collections.Generic;

namespace xCache
{
    /// <summary>
    /// Default Cache Strategy
    /// </summary>
    public class MemoryCache : ICache
    {
        System.Runtime.Caching.MemoryCache _cache = System.Runtime.Caching.MemoryCache.Default;

        public void Add<T>(string key, CacheItem<T> item)
        {
            _cache.Set(key, item, item.Expires);
        }

        CacheItem<T> ICache.Get<T>(string key)
        {
            var value = _cache.Get(key);

            return value != null
                ? (CacheItem<T>)value
                : null;
        }

        public bool Remove(string key)
        {
            var item = _cache.Remove(key);
            return item != null;
        }

        public void RemoveAll()
        {
            var items = new List<string>(256);

            foreach (var item in _cache)
                items.Add(item.Key);

            foreach (var item in items)
                _cache.Remove(item);
        }
    }
}
