using System;

namespace xCache
{
    public class CacheRecord
    {
        public Guid Id { get; set; }

        public DateTime Timestamp { get; set; }

        public string Source { get; set; }

        public string Key { get; set; }

        public object Value { get; set; }

        public CacheAction Action { get; set; }
    }

    public enum CacheAction
    {
        Get,
        Add
    }
}
