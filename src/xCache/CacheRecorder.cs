using System;
using System.Collections.Concurrent;

namespace xCache
{
    public static class CacheRecorder
    {
        public static ConcurrentStack<CacheRecord> Records = new ConcurrentStack<CacheRecord>();

        public static void Record(string source, string key, object value, CacheAction cacheAction)
        {
            Records.Push(new CacheRecord
            {
                Id = Guid.NewGuid(),
                Timestamp = DateTime.UtcNow,
                Source = source,
                Key = key,
                Value = value,
                Action = cacheAction
            });
        }
    }
}
