using System;
using System.Threading.Tasks;
using xCache.Durable;
using xCache.Extensions;
using System.Linq;
using System.Collections.Concurrent;
using Unity.Interception.PolicyInjection.Pipeline;
using Unity;
using Unity.Interception.Interceptors.TypeInterceptors.VirtualMethodInterception;

namespace xCache.Aop.Unity
{
    public class CacheAttributeCallHandler : ICallHandler
    {
        public int Order { get; set; }
        public TimeSpan Expiration { get; set; }
        public TimeSpan? AbsoluteExpiration { get; set; }
        public TimeSpan MaximumStaleness { get; set; }
        public bool RescheduleStale { get; set; }

        private IUnityContainer _container;
        private readonly ICache _cache;
        private readonly ICacheKeyGenerator _keyGenerator;
        private readonly string _cacheName;
        private static readonly ConcurrentDictionary<string, object> _locks =
            new ConcurrentDictionary<string, object>();

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
            var cacheKey = _keyGenerator.GenerateKey(input);

            var methodIsTask = input.MethodBase.IsGenericTask();
            var returnType = input.MethodBase.GetReturnType();

            var cachedResult = typeof(ICache).GetMethod("Get")
                    .MakeGenericMethod(returnType)
                    .Invoke(_cache, new object[] { cacheKey });

            object unwrappedResult = null;

            var scheduleDurableRefresh = false;

            //Force cache miss for durable results that are behind
            if (cachedResult != null && AbsoluteExpiration != null)
            {
                var lastUpdate = (DateTime)CacheExtensions.GetLastUpdate((dynamic)cachedResult);
                var expires = (DateTime)CacheExtensions.GetExpires((dynamic)cachedResult);

                if (DateTime.UtcNow.Subtract(lastUpdate).TotalSeconds > MaximumStaleness.TotalSeconds)
                {
                    if (RescheduleStale && DateTime.UtcNow.Add(Expiration) < expires)
                    {
                        scheduleDurableRefresh = true;
                    }
                    else
                    {
                        cachedResult = null;
                    }
                }
            }

            // Nothing is cached for this method
            object removedObject;
            try
            {
                if (cachedResult == null)
                {
                    lock (_locks.GetOrAdd(cacheKey, new object()))
                    {
                        if (cachedResult == null)
                        {
                            // Get a new result
                            var newResult = getNext()(input, getNext);

                            //Do not cache exceptions
                            if (newResult.Exception == null)
                            {
                                if (methodIsTask)
                                {
                                    newResult.ReturnValue = (dynamic)typeof(CacheExtensions).GetMethod("AddToCacheAsync")
                                        .MakeGenericMethod(returnType)
                                        .Invoke(null, new object[] {_cache, newResult.ReturnValue,
                                cacheKey, DateTime.UtcNow.Add(AbsoluteExpiration ?? Expiration) });
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
                    }
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
            }
            finally
            {
                _locks.TryRemove(cacheKey, out removedObject);
            }

            if (scheduleDurableRefresh)
            {
                var parameters = new object[input.Inputs.Count];
                input.Inputs.CopyTo(parameters, 0);

                var queue = _container.Resolve<IDurableCacheQueue>();
                
                queue.ScheduleRefresh(new DurableCacheRefreshEvent
                {
                    AbsoluteExpiration = AbsoluteExpiration.Value,
                    CacheName = _cacheName,
                    Key = cacheKey,
                    Method = new DurableMethod
                    {
                        DeclaringType = input.Target.GetType(),
                        Interface = input.MethodBase.DeclaringType,
                        Name = input.MethodBase.Name,
                        Parameters = input.MethodBase.GetParameters()
                            .Select(p => p.ParameterType).ToArray()
                    },
                    Parameters = parameters,
                    RefreshTime = Expiration,
                    UtcLifetime = cachedResult != null ? (DateTime)CacheExtensions.GetExpires((dynamic)cachedResult) : 
                        DateTime.UtcNow.Add(AbsoluteExpiration.Value),
                });
            }

            var arguments = new object[input.Inputs.Count];
            input.Inputs.CopyTo(arguments, 0);

            return new VirtualMethodReturn(input, unwrappedResult, arguments);
        }
    }
}
