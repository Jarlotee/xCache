using System;
using System.Threading;
using System.Threading.Tasks;
using xCache.Aop.Unity;
using xCache.Tests.CacheKeyGenerator;
using xCache.Tests.Core;

namespace xCache.Tests.Aop.Unity.Core
{
    public class UnityCacheEnabledObject : ICacheEnableObject
    {
        [Cache(Seconds = 5)]
        public async Task<string> GetCurrentDataAsStringFiveSecondCacheAsync()
        {
            Thread.Sleep(250);
            return await Task.FromResult(DateTime.Now.ToString());
        }

        [Cache(Seconds = 5)]
        public string GetCurrentDateAsStringFiveSecondCache()
        {
            return DateTime.Now.ToString();
        }

        [Cache(Seconds = 10, AbsoluteSeconds = 30)]
        public string GetCurrentDateAsStringTenSecondCacheAbsoluteThirtySeconds()
        {
            return DateTime.Now.ToString();
        }

        [Cache(Seconds = 10, AbsoluteSeconds = 30)]
        public async Task<string> GetCurrentDateAsStringTenSecondCacheAbsoluteThirtySecondsAsync()
        {
            return await Task.FromResult(DateTime.Now.ToString());
        }

        [Cache(Seconds = 5)]
        public DateTime GetCurrentDateTimeFiveSecondCache()
        {
            return DateTime.Now;
        }

        [Cache(Seconds = 5)]
        public async Task<DateTime> GetCurrentDateTimeFiveSecondCacheAsync()
        {
            return await Task.FromResult(DateTime.Now);
        }

        [Cache(Seconds = 5)]
        public string GetNullAsStringFiveSecondCache()
        {
            return null;
        }

        [Cache(Seconds = 5)]
        public Task<string> GetNullAsStringFiveSecondCacheAsync()
        {
            return Task.FromResult((string)null);
        }

        [Cache(Seconds = 15, AbsoluteSeconds = 90)]
        public string GetCurrentDateAsStringWithParameterFifteenSecondCacheAbsoluteNinetySeconds(int p0)
        {
            return DateTime.Now.ToString() + ":" + p0.ToString();
        }

        [Cache(Seconds = 10, AbsoluteSeconds = 30)]
        public ComplexObject GetComplexObjectWithComplexParameterFifeenSecondCacheAbsoluteNinetySeconds(ComplexObject obj)
        {
            var newObject = new ComplexObject();

            newObject.Ints = obj.Ints;

            return newObject;
        }
    }
}
