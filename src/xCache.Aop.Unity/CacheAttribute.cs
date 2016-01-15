using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using System;

namespace xCache.Aop.Unity
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CacheAttribute : HandlerAttribute
    {
        public int Hours { get; set; }
        public int Minutes { get; set; }
        public int Seconds { get; set; }
        public string Name { get; set; }

        public override ICallHandler CreateHandler(IUnityContainer container)
        {
            var cache = container.Resolve<ICache>(Name);
            var keyGenerator = container.Resolve<ICacheKeyGenerator>();

            var handler = new CacheAttributeCallHandler(cache, keyGenerator)
            {
                Order = 1,
                Timeout = new TimeSpan(Hours, Minutes, Seconds)
            };

            //Set Default to 5
            if (handler.Timeout.TotalSeconds <= 0)
            {
                handler.Timeout = new TimeSpan(0, 5, 0);
            }

            return handler;
        }
    }
}
