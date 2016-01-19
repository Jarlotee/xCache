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

        public int AbsoluteHours { get; set; }
        public int AbsoluteMinutes { get; set; }
        public int AbsoluteSeconds { get; set; }

        public override ICallHandler CreateHandler(IUnityContainer container)
        {
            var expiration = new TimeSpan(Hours, Minutes, Seconds);
            var absoluteExpiration = new TimeSpan(AbsoluteHours, AbsoluteMinutes, AbsoluteSeconds);

            //default to 5 minutes if necessary
            expiration = expiration.TotalSeconds > 0 ? expiration : new TimeSpan(0, 5, 0);

            var handler = new CacheAttributeCallHandler(container)
            {
                Order = 1,
                Expiration = expiration,
                AbsoluteExpiration = absoluteExpiration.TotalSeconds > 0 ? absoluteExpiration : (TimeSpan?)null
            };

            return handler;
        }
    }
}
