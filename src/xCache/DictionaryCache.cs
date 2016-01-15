using System;
using System.Collections;

namespace xCache
{
    public class DictionaryCache : ICache
    {
        private readonly IDictionary _cache;

        public DictionaryCache(IDictionary cache)
        {
            _cache = cache;
        }

        public void Add<T>(string key, T item, TimeSpan expiration)
        {
            _cache[key] = item;
        }

        public T Get<T>(string key)
        {
            if (_cache.Contains(key))
            {
                var value = _cache[key];

                if (value != null)
                {
                    return (T)value;
                }
            }
            return default(T);
        }
    }
}
