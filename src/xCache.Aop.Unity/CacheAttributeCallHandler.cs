using Microsoft.Practices.Unity.InterceptionExtension;
using System;

namespace xCache.Aop.Unity
{
    public class CacheAttributeCallHandler : ICallHandler
    {
        public int Order { get; set; }
        public TimeSpan Timeout { get; set; }

        private readonly ICache _cache;
        private readonly ICacheKeyGenerator _keyGenerator;

        public CacheAttributeCallHandler(ICache cache, ICacheKeyGenerator keyGenerator)
        {
            _cache = cache;
            _keyGenerator = keyGenerator;
        }

        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            var cacheKey = _keyGenerator.GenerateKey(input);
            var cachedResult = _cache.Get<object>(cacheKey);

            // Nothing is cached for this method
            if (cachedResult == null)
            {
                // Get a new result
                var newResult = getNext()(input, getNext);

                //Do not cache null return values or exceptions
                if (newResult.ReturnValue != null && newResult.Exception == null)
                {
                    _cache.Add<object>(cacheKey, newResult.ReturnValue, Timeout);
                }

                cachedResult = newResult.ReturnValue;
            }

            var arguments = new object[input.Inputs.Count];
            input.Inputs.CopyTo(arguments, 0);

            return new VirtualMethodReturn(input, cachedResult, arguments);
        }
    }
}
