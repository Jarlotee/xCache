using System;

namespace xCache
{
    /// <summary>
    /// Default Cache Strategy
    /// </summary>
    public class MemoryCache : ICache
    {
        private readonly bool _recordCacheEvent;
        System.Runtime.Caching.MemoryCache _cache = System.Runtime.Caching.MemoryCache.Default;

        public MemoryCache(bool recordCacheEvent)
        {
            _recordCacheEvent = recordCacheEvent;
        }

        public void Add<T>(string key, T item, TimeSpan expiration)
        {
            _cache.Add(key, item, new DateTimeOffset(DateTime.Now.Add(expiration)));
            PossiblyRecordEvent(key, item, CacheAction.Add);
        }

        public T Get<T>(string key)
        {
            var value = _cache.Get(key);

            if (value != null)
            {
                PossiblyRecordEvent(key, value, CacheAction.Get);
                return (T)value;
            }
            else
            {
                return default(T);
            }
        }

        private void PossiblyRecordEvent(string key, object value, CacheAction cacheAction)
        {
            if (_recordCacheEvent)
            {
                CacheRecorder.Record("MemoryCache", key, value, cacheAction);
            }
        }
    }
}
