using System;
using System.Collections;

namespace xCache
{
    public class DictionaryCache : ICache
    {
        private readonly IDictionary _cache;
        private readonly ICache _successor;

        public DictionaryCache(IDictionary cache, ICache successor)
        {
            _cache = cache;
            _successor = successor;
        }

        public void Add<T>(string key, T item, TimeSpan expiration)
        {
            _cache[key] = item;
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
                    return (T)value;
                }
            }

            if (_successor != null)
            {
                return _successor.Get<T>(key);
            }

            return default(T);
        }
    }
}
