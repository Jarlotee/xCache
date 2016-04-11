using System.Collections.Concurrent;

namespace xCache
{
    public class DictionaryCache : ICache
    {
        private readonly ConcurrentDictionary<string, object> _cache;

        public DictionaryCache()
        {
            _cache = new ConcurrentDictionary<string, object>();
        }

        public void Add<T>(string key, CacheItem<T> item)
        {
            _cache[key] = item;
        }

        public CacheItem<T> Get<T>(string key)
        {
            if (_cache.ContainsKey(key))
            {
                return (CacheItem<T>)_cache[key];
            }

            return null;
        }

        public bool Remove(string key)
        {
            object item;
            return _cache.TryRemove(key, out item);
        }

        public void Purge()
        {
            _cache.Clear();
        }
    }
}
