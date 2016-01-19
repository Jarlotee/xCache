using System;

namespace xCache
{
    public class CacheItem<T>
    {
        public DateTime Expires { get; set; }
        public T Item { get; set; }
        public DateTime LastUpdate { get; set; }

    }
}
