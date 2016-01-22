using System;

namespace xCache.Durable
{
    public class DurableCacheRefreshEvent
    {
        public TimeSpan AbsoluteExpiration { get; set; }
        public string CacheName { get; set; }
        public string Key { get; set; }
        public DurableMethod Method { get; set; }
        public object[] Parameters { get; set; }
        public TimeSpan RefreshTime { get; set; }
        public DateTime UtcLifetime { get; set; }
    }
}