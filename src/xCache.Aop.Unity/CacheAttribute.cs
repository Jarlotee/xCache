using System;
using Unity;
using Unity.Interception.PolicyInjection.Pipeline;
using Unity.Interception.PolicyInjection.Policies;

namespace xCache.Aop.Unity
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class CacheAttribute : HandlerAttribute
    {
        public int Hours { get; set; }
        public int Minutes { get; set; }
        public int Seconds { get; set; }

        public int AbsoluteHours { get; set; }
        public int AbsoluteMinutes { get; set; }
        public int AbsoluteSeconds { get; set; }

        public int MaximumStalenessHours { get; set; }
        public int MaximumStalenessMinutes { get; set; }
        public int MaximumStalenessSeconds { get; set; }

        public string CacheName { get; set; }
        public bool RescheduleStale { get; set; }

        public override ICallHandler CreateHandler(IUnityContainer container)
        {
            var expiration = new TimeSpan(Hours, Minutes, Seconds);
            var absoluteExpiration = new TimeSpan(AbsoluteHours, AbsoluteMinutes, AbsoluteSeconds);
            var maximumStaleness = new TimeSpan(MaximumStalenessHours, MaximumStalenessMinutes, MaximumStalenessSeconds);

            //default to 5 minutes if necessary
            expiration = expiration.TotalSeconds > 0 ? expiration : new TimeSpan(0, 5, 0);
            //default to twice expiration
            maximumStaleness = maximumStaleness.TotalSeconds > 0 ? maximumStaleness
                : expiration.Add(expiration);

            var handler = new CacheAttributeCallHandler(container, CacheName)
            {
                Order = Order,
                Expiration = expiration,
                AbsoluteExpiration = absoluteExpiration.TotalSeconds > 0 ? absoluteExpiration : (TimeSpan?)null,
                MaximumStaleness = maximumStaleness,
                RescheduleStale = RescheduleStale
            };

            return handler;
        }
    }
}
