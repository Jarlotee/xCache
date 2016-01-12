using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using System;
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

        public CacheAttributeCallHandler(IUnityContainer container)
        {
            _container = container;
            _cache = container.Resolve<ICache>();
            _keyGenerator = container.Resolve<ICacheKeyGenerator>();
        }

        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            var cacheKey = _keyGenerator.GenerateKey(input);

            var methodIsTask = input.MethodBase.IsGenericTask();
            var returnType = methodIsTask ? input.MethodBase.GetGenericReturnTypeArgument(0)
                : input.MethodBase.GetReturnType();

            var cachedResult = typeof(ICache).GetMethod("Get")
                    .MakeGenericMethod(returnType)
                    .Invoke(_cache, new object[] { cacheKey });

            // Nothing is cached for this method
            if (cachedResult == null || 
                (returnType.IsValueType && cachedResult.Equals(Activator.CreateInstance(returnType))))
            {
                // Get a new result
                var newResult = getNext()(input, getNext);

                //Do not cache null return values or exceptions
                if (newResult.ReturnValue != null && newResult.Exception == null)
                {
                    if (methodIsTask)
                    {
                        newResult.ReturnValue = CacheExtensions
                            .AddToCache(_cache, (dynamic)newResult.ReturnValue,
                                cacheKey, AbsoluteExpiration, Expiration);
                    }
                    else
                    {
                        _cache.Add(cacheKey, newResult.ReturnValue, AbsoluteExpiration ?? Expiration);
                    }

                    //queue refresh if configured by the user
                    if (AbsoluteExpiration != null)
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
                            UtcLifetime = DateTime.UtcNow.Add(AbsoluteExpiration.Value),
                        });
                    }
                }
                
                cachedResult = newResult.ReturnValue;
            }
            else
            {
                if (methodIsTask)
                {
                    cachedResult = typeof(Task).GetMethod("FromResult")
                        .MakeGenericMethod(input.MethodBase.GetGenericReturnTypeArguments())
                        .Invoke(null, new object[] { cachedResult });
                }
            }

            var arguments = new object[input.Inputs.Count];
            input.Inputs.CopyTo(arguments, 0);

            return new VirtualMethodReturn(input, cachedResult, arguments);
        }
    }
}
