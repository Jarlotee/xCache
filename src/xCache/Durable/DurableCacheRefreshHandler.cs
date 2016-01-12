using System;
using System.Diagnostics;
using System.Threading.Tasks;
using xCache.Extensions;

namespace xCache.Durable
{
    public class DurableCacheRefreshHandler : IDurableCacheRefreshHandler
    {
        private readonly ICache _cache;

        public DurableCacheRefreshHandler(ICache cache)
        {
            _cache = cache;
        }

        public void Handle(DurableCacheRefreshEvent refreshEvent)
        {
            Trace.TraceInformation("Handling refresh event for key {0}", refreshEvent.Key);

            try
            {
                var obj = Activator.CreateInstance(refreshEvent.Type);

                var value = refreshEvent.MethodBase.Invoke(obj, refreshEvent.Parameters);

                if (value != null)
                {
                    if (refreshEvent.MethodBase.IsGenericTask())
                    {
                        var task = Task.Run(async () =>
                        {
                            await CacheExtensions.AddToCache(_cache, (dynamic)value,
                                refreshEvent.Key, refreshEvent.AbsoluteExpiration, refreshEvent.RefreshTime);
                        });

                        task.Wait();
                    }
                    else
                    {
                        _cache.Add(refreshEvent.Key, value, refreshEvent.AbsoluteExpiration);
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError("Unable to refresh cache for key {0}. Reason: {1}",
                    refreshEvent.Key,
                    ex.Message);
            }
        }
    }
}