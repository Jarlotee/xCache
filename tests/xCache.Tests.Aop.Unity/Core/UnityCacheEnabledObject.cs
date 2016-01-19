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
        private int NumberOfCalls = 0;

        public int GetNumberOfTimesCalled()
        {
            return NumberOfCalls;
        }

        [Cache(Seconds = 5)]
        public async Task<string> GetCurrentDataAsStringFiveSecondCacheAsync()
        {
            NumberOfCalls++;
            Thread.Sleep(250);
            return await Task.FromResult(DateTime.Now.ToString());
        }

        [Cache(Seconds = 5)]
        public string GetCurrentDateAsStringFiveSecondCache()
        {
            NumberOfCalls++;
            return DateTime.Now.ToString();
        }

        [Cache(Seconds = 10, AbsoluteSeconds = 30)]
        public string GetCurrentDateAsStringTenSecondCacheAbsoluteThirtySeconds()
        {
            NumberOfCalls++;
            return DateTime.Now.ToString();
        }

        [Cache(Seconds = 10, AbsoluteSeconds = 30)]
        public async Task<string> GetCurrentDateAsStringTenSecondCacheAbsoluteThirtySecondsAsync()
        {
            NumberOfCalls++;
            return await Task.FromResult(DateTime.Now.ToString());
        }

        [Cache(Seconds = 5)]
        public DateTime GetCurrentDateTimeFiveSecondCache()
        {
            NumberOfCalls++;
            return DateTime.Now;
        }

        [Cache(Seconds = 5)]
        public async Task<DateTime> GetCurrentDateTimeFiveSecondCacheAsync()
        {
            NumberOfCalls++;
            return await Task.FromResult(DateTime.Now);
        }

        [Cache(Seconds = 5)]
        public string GetNullAsStringFiveSecondCache()
        {
            NumberOfCalls++;
            return null;
        }

        [Cache(Seconds = 5)]
        public Task<string> GetNullAsStringFiveSecondCacheAsync()
        {
            NumberOfCalls++;
            return Task.FromResult((string)null);
        }

        [Cache(Seconds = 15, AbsoluteSeconds = 90)]
        public string GetCurrentDateAsStringWithParameterFifteenSecondCacheAbsoluteNinetySeconds(int p0)
        {
            NumberOfCalls++;
            return DateTime.Now.ToString() + ":" + p0.ToString();
        }

        [Cache(Seconds = 10, AbsoluteSeconds = 30)]
        public ComplexObject GetComplexObjectWithComplexParameterFifeenSecondCacheAbsoluteNinetySeconds(ComplexObject obj)
        {
            NumberOfCalls++;
            var newObject = new ComplexObject();

            newObject.Ints = obj.Ints;

            return newObject;
        }

        [Cache(Seconds = 15, CacheName = "One", Order = 1)]
        [Cache(Seconds = 5, CacheName = "Two", Order = 2)]
        public string GetCurrentDateTimeFiveSecondCacheTwoTiers()
        {
            NumberOfCalls++;
            return DateTime.Now.ToString();
        }

        [Cache(Seconds = 15, CacheName = "One", Order = 1)]
        [Cache(Seconds = 5, CacheName = "Two", Order = 2)]
        public async Task<string> GetCurrentDateTimeFiveSecondCacheTwoTiersAsync()
        {
            NumberOfCalls++;
            return await Task.FromResult(DateTime.Now.ToString());
        }

        [Cache(Seconds = 15, Order = 1)]
        [Cache(Seconds = 5, CacheName = "Two", Order = 2)]
        public string GetCurrentDateTimeFiveSecondCacheTwoTiersWithDefault()
        {
            NumberOfCalls++;
            return DateTime.Now.ToString();
        }

        [Cache(Seconds = 15, Order = 1)]
        [Cache(Seconds = 5, CacheName = "Two", Order = 2)]
        public async Task<string> GetCurrentDateTimeFiveSecondCacheTwoTiersWithDefaultAsync()
        {
            NumberOfCalls++;
            return await Task.FromResult(DateTime.Now.ToString());
        }

        [Cache(Seconds = 15, CacheName = "DictionaryCache", Order = 1)]
        [Cache(Seconds = 5, CacheName = "Two", Order = 2)]
        public string GetCurrentDateTimeFiveSecondCacheTwoTiersWithDictionary()
        {
            NumberOfCalls++;
            return DateTime.Now.ToString();
        }
    }
}
