using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using xCache.Durable;
using xCache.Extensions;

namespace xCache.Aop.Unity
{
    public class CacheAttributeCallHandler : ICallHandler
    {
        public int Order { get; set; }
        public TimeSpan Expiration { get; set; }
        public TimeSpan? AbsoluteExpiration { get; set; }

        private IUnityContainer _container;
        private readonly ICache _cache;
        private readonly ICacheKeyGenerator _keyGenerator;
        private readonly string _cacheName;

        public CacheAttributeCallHandler(IUnityContainer container, string cacheName)
        {
            _cacheName = cacheName;
            _container = container;
            _cache = string.IsNullOrWhiteSpace(cacheName) ?
                container.Resolve<ICache>() : container.Resolve<ICache>(cacheName);
            _keyGenerator = container.Resolve<ICacheKeyGenerator>();
        }

        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            if (!string.IsNullOrWhiteSpace(_cacheName))
            {
                Trace.TraceInformation("Cache Name {0} was called", _cacheName);
            }

            var cacheKey = _keyGenerator.GenerateKey(input);

            var methodIsTask = input.MethodBase.IsGenericTask();
            var returnType = methodIsTask ? input.MethodBase.GetGenericReturnTypeArgument(0)
                : input.MethodBase.GetReturnType();

            var cachedResult = typeof(ICache).GetMethod("Get")
                    .MakeGenericMethod(returnType)
                    .Invoke(_cache, new object[] { cacheKey });

            object unwrappedResult = null;

            var scheduleDurableRefresh = false;

            //Force cache miss for durable results that are behind
            if (cachedResult != null && AbsoluteExpiration != null)
            {
                var lastUpdate = (DateTime)CacheExtensions.GetLastUpdate((dynamic)cachedResult);

                if (DateTime.UtcNow.Subtract(lastUpdate).TotalMinutes > 2 * Expiration.TotalMinutes)
                {
                    cachedResult = null;
                }
            }

            // Nothing is cached for this method
            if (cachedResult == null)
            {
                // Get a new result
                var newResult = getNext()(input, getNext);

                //Do not cache exceptions
                if (newResult.Exception == null)
                {
                    if (methodIsTask)
                    {
                        var task = Task.Run(async () =>
                        {
                            await (dynamic)typeof(CacheExtensions).GetMethod("AddToCacheAsync")
                                .MakeGenericMethod(returnType)
                                .Invoke(null, new object[] {_cache, newResult.ReturnValue,
                                    cacheKey, DateTime.UtcNow.Add(AbsoluteExpiration ?? Expiration) });
                        });

                        task.Wait();
                    }
                    else
                    {
                        typeof(CacheExtensions).GetMethod("AddToCache")
                                .MakeGenericMethod(returnType)
                                .Invoke(null, new object[] {_cache, newResult.ReturnValue,
                                    cacheKey, DateTime.UtcNow.Add(AbsoluteExpiration ?? Expiration) });
                    }

                    //queue refresh if configured by the user
                    scheduleDurableRefresh = AbsoluteExpiration != null;
                }

                unwrappedResult = newResult.ReturnValue;
            }
            else
            {
                if (methodIsTask)
                {
                    var unwrappedResultMissingTask = CacheExtensions.Unwrap((dynamic)cachedResult);

                    unwrappedResult = typeof(Task).GetMethod("FromResult")
                        .MakeGenericMethod(input.MethodBase.GetGenericReturnTypeArguments())
                        .Invoke(null, new object[] { unwrappedResultMissingTask });
                }
                else
                {
                    unwrappedResult = CacheExtensions.Unwrap((dynamic)cachedResult);
                }
            }

            if (scheduleDurableRefresh)
            {
                var parameters = new object[input.Inputs.Count];
                input.Inputs.CopyTo(parameters, 0);

                var queue = _container.Resolve<IDurableCacheQueue>();

                queue.ScheduleRefresh(new DurableCacheRefreshEvent
                {
                    AbsoluteExpiration = AbsoluteExpiration.Value,
                    Key = cacheKey,
                    MethodBase = input.MethodBase,
                    Parameters = parameters,
                    Type = input.Target.GetType(),
                    RefreshTime = Expiration,
                    ReturnType = returnType,
                    UtcLifetime = DateTime.UtcNow.Add(AbsoluteExpiration.Value),
                });
            }

            var arguments = new object[input.Inputs.Count];
            input.Inputs.CopyTo(arguments, 0);

            return new VirtualMethodReturn(input, unwrappedResult, arguments);
        }
    }
}
