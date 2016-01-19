using System;
using System.Collections;

namespace xCache
{
    public class DictionaryCache : ICache
    {
        private readonly IDictionary _cache;
        private readonly ICache _successor;
        private readonly bool _recordCacheEvent;

        public DictionaryCache(IDictionary cache, ICache successor, bool recordCacheEvent)
        {
            _cache = cache;
            _successor = successor;
            _recordCacheEvent = recordCacheEvent;
        }

        public void Add<T>(string key, T item, TimeSpan expiration)
        {
            _cache[key] = item;
            PossiblyRecordEvent(key, item, CacheAction.Add);
            if (_successor != null)
            {
                _successor.Add(key, item, expiration);
            }
        }

        public T Get<T>(string key)
        {
            if (_cache.Contains(key))
            {
                var value = _cache[key];

                if (value != null)
                {
                    PossiblyRecordEvent(key, value, CacheAction.Get);
                    return (T)value;
                }
            }

            if (_successor != null)
            {
                return _successor.Get<T>(key);
            }

            return default(T);
        }

        private void PossiblyRecordEvent(string key, object value, CacheAction cacheAction)
        {
            if (_recordCacheEvent)
            {
                CacheRecorder.Record("DictionaryCache", key, value, cacheAction);
            }
        }
    }
}
