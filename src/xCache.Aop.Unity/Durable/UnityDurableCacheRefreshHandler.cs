using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Unity;
using xCache.Durable;
using xCache.Extensions;

namespace xCache.Aop.Unity.Durable
{
    public class UnityDurableCacheRefreshHandler : IDurableCacheRefreshHandler
    {
        private readonly IUnityContainer _container;

        public UnityDurableCacheRefreshHandler(IUnityContainer container)
        {
            _container = container;
        }

        public void Handle(DurableCacheRefreshEvent refreshEvent)
        {
            try
            {
                var cache = string.IsNullOrWhiteSpace(refreshEvent.CacheName) ?
                    _container.Resolve<ICache>() :
                    _container.Resolve<ICache>(refreshEvent.CacheName);

                var bypassKey = refreshEvent.Method.DeclaringType.FullName + ".Intercept.Bypass";

                if (!_container.IsRegistered(refreshEvent.Method.Interface, bypassKey))
                {
                    //Wont work in the case of a custom factory or custom constructors
                    _container.RegisterType(refreshEvent.Method.Interface, refreshEvent.Method.DeclaringType, bypassKey);
                }

                var obj = _container.Resolve(refreshEvent.Method.Interface, bypassKey);

                var method = refreshEvent.Method.GetMethod();
                var value = method.Invoke(obj, refreshEvent.Parameters);

                if (method.IsGenericTask())
                {
                    var task = Task.Run(async () =>
                    {
                        await (dynamic)typeof(CacheExtensions).GetMethod("AddToCacheAsync")
                                .MakeGenericMethod(method.GetReturnType())
                                .Invoke(null, new object[] {cache, value,
                                    refreshEvent.Key, refreshEvent.UtcLifetime});
                    });

                    task.Wait();
                }
                else
                {
                    typeof(CacheExtensions).GetMethod("AddToCache")
                                .MakeGenericMethod(method.GetReturnType())
                                .Invoke(null, new object[] {cache, value,
                                    refreshEvent.Key, refreshEvent.UtcLifetime});
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