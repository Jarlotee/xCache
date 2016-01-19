using System;
using System.Reflection;

namespace xCache.Durable
{
    public class DurableCacheRefreshEvent
    {
        public TimeSpan AbsoluteExpiration { get; set; }
        public string Key { get; set; }
        public MethodBase MethodBase { get; set; }
        public object[] Parameters { get; set; }
        public TimeSpan RefreshTime { get; set; }
        public Type ReturnType { get; set; }
        public Type Type { get; set; }
        public DateTime UtcLifetime { get; set; }
    }
}