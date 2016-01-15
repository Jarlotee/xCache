using Microsoft.Practices.Unity.InterceptionExtension;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace xCache.Aop.Unity
{
    public class CacheAttributeCallHandler : ICallHandler
    {
        public int Order { get; set; }
        public TimeSpan Timeout { get; set; }
        public bool AbortMethodCallOnCacheMiss { get; set; }

        private readonly ICache _cache;
        private readonly ICacheKeyGenerator _keyGenerator;

        public CacheAttributeCallHandler(ICache cache, ICacheKeyGenerator keyGenerator)
        {
            _cache = cache;
            _keyGenerator = keyGenerator;
        }

        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            var methodInfo = (MethodInfo)input.MethodBase;
            var methodIsTask = typeof(Task).IsAssignableFrom(methodInfo.ReturnType)
                && methodInfo.ReturnType.IsGenericType
                && methodInfo.ReturnType.GetGenericTypeDefinition() == typeof(Task<>);
            var cacheKey = _keyGenerator.GenerateKey(input);
            var underlyingReturnType = methodIsTask ? methodInfo.ReturnType.GenericTypeArguments[0]
                : methodInfo.ReturnType;

            var cachedResult = _cache.GetType().GetMethod("Get")
                    .MakeGenericMethod(underlyingReturnType)
                    .Invoke(_cache, new object[] { cacheKey });

            if (cachedResult == null && AbortMethodCallOnCacheMiss)
            {
                return input.CreateMethodReturn(null);
            }

            // Nothing is cached for this method
            if (cachedResult == null || 
                (underlyingReturnType.IsValueType && cachedResult.Equals(Activator.CreateInstance(underlyingReturnType))))
            {
                // Get a new result
                var newResult = getNext()(input, getNext);

                //Do not cache null return values or exceptions
                if (newResult.ReturnValue != null && newResult.Exception == null)
                {
                    if (methodIsTask)
                    {
                        newResult.ReturnValue = InterceptAsync((dynamic)newResult.ReturnValue, cacheKey);
                    }
                    else
                    {
                        _cache.Add(cacheKey, newResult.ReturnValue, Timeout);
                    }
                }

                cachedResult = newResult.ReturnValue;
            }
            else
            {
                if (methodIsTask)
                {
                    cachedResult = typeof(Task).GetMethod("FromResult")
                        .MakeGenericMethod(methodInfo.ReturnType.GetGenericArguments())
                        .Invoke(null, new object[] { cachedResult });
                }
            }

            var arguments = new object[input.Inputs.Count];
            input.Inputs.CopyTo(arguments, 0);

            return new VirtualMethodReturn(input, cachedResult, arguments);
        }

        private async Task<T> InterceptAsync<T>(Task<T> task, string cacheKey)
        {
            var value = await task.ConfigureAwait(false);

            if(value != null)
            { 
                _cache.Add(cacheKey, value, Timeout);
            }

            return value;
        }
    }
}
