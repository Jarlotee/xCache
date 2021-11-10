using System;
using System.Threading;
using System.Threading.Tasks;
using xCache.Tests.CacheKeyGenerator;
using Xunit;
using System.Linq;

namespace xCache.Tests.Core
{
    public abstract class CacheEnabledTests
    {
        protected ICacheEnableObject _cached = null;

        protected virtual void PurgeDurableCacheQueue()
        {

        }

        protected virtual void PurgeDictionaryCache()
        {

        }

        [Fact]
        [Trait("Feature", "Caching")]
        public void TestFiveSecondTimeout()
        {
            var now = _cached.GetCurrentDateAsStringFiveSecondCache();

            Thread.Sleep(new TimeSpan(0, 0, 2));

            var cached = _cached.GetCurrentDateAsStringFiveSecondCache();

            Assert.Equal(now, cached);

            Thread.Sleep(new TimeSpan(0, 0, 5));

            var cached2 = _cached.GetCurrentDateAsStringFiveSecondCache();

            Assert.NotEqual(now, cached2);

            Thread.Sleep(new TimeSpan(0, 0, 1));

            var cached3 = _cached.GetCurrentDateAsStringFiveSecondCache();

            Assert.Equal(cached2, cached3);
        }

        [Fact]
        [Trait("Feature", "Caching")]
        public async Task TestFiveSecondTimeoutAsync()
        {
            var now = await _cached.GetCurrentDataAsStringFiveSecondCacheAsync();

            Thread.Sleep(new TimeSpan(0, 0, 2));

            var cached = await _cached.GetCurrentDataAsStringFiveSecondCacheAsync();

            Assert.Equal(now, cached);

            Thread.Sleep(new TimeSpan(0, 0, 5));

            var cached2 = await _cached.GetCurrentDataAsStringFiveSecondCacheAsync();

            Assert.NotEqual(now, cached2);

            Thread.Sleep(new TimeSpan(0, 0, 1));

            var cached3 = await _cached.GetCurrentDataAsStringFiveSecondCacheAsync();

            Assert.Equal(cached2, cached3);
        }

        [Fact]
        [Trait("Feature", "Caching")]
        public void TestFiveSecondTimeoutStruct()
        {
            var now = _cached.GetCurrentDateTimeFiveSecondCache();

            Thread.Sleep(new TimeSpan(0, 0, 2));

            var cached = _cached.GetCurrentDateTimeFiveSecondCache();

            Assert.Equal(now, cached);

            Thread.Sleep(new TimeSpan(0, 0, 5));

            var cached2 = _cached.GetCurrentDateTimeFiveSecondCache();

            Assert.NotEqual(now, cached2);

            Thread.Sleep(new TimeSpan(0, 0, 1));

            var cached3 = _cached.GetCurrentDateTimeFiveSecondCache();

            Assert.Equal(cached2, cached3);
        }

        [Fact]
        [Trait("Feature", "Caching")]
        public async Task TestFiveSecondTimeoutStructAsync()
        {
            var now = await _cached.GetCurrentDateTimeFiveSecondCacheAsync();

            Thread.Sleep(new TimeSpan(0, 0, 2));

            var cached = await _cached.GetCurrentDateTimeFiveSecondCacheAsync();

            Assert.Equal(now, cached);

            Thread.Sleep(new TimeSpan(0, 0, 5));

            var cached2 = await _cached.GetCurrentDateTimeFiveSecondCacheAsync();

            Assert.NotEqual(now, cached2);

            Thread.Sleep(new TimeSpan(0, 0, 1));

            var cached3 = await _cached.GetCurrentDateTimeFiveSecondCacheAsync();

            Assert.Equal(cached2, cached3);
        }

        [Fact]
        [Trait("Feature", "Caching")]
        public void TestNullResult()
        {
            var result = _cached.GetNullAsStringFiveSecondCache();

            Assert.Null(result);
        }

        [Fact]
        [Trait("Feature", "Caching")]
        public async Task TestNullResultAsync()
        {
            var result = await _cached.GetNullAsStringFiveSecondCacheAsync();

            Assert.Null(result);
        }

        [Fact]
        [Trait("Feature", "Performance")]
        public void TestHighVolumneOfCachedElements()
        {
            for (int i = 0; i < 100000; i++)
            {
                _cached.GetCurrentDateAsStringWithParameterFifteenSecondCacheAbsoluteNinetySeconds(i);
            }

            Thread.Sleep(1000 * 16);

            for (int i = 0; i < 100000; i++)
            {
                var item = _cached.GetCurrentDateAsStringWithParameterFifteenSecondCacheAbsoluteNinetySeconds(i);

                Assert.EndsWith(":" + i.ToString(), item);
            }

            Thread.Sleep(1000 * 120);
        }

        [Fact]
        [Trait("Feature", "Async")]
        public void TestFiveSecondTimeoutAsyncHelper()
        {
            var now = AsyncHelpers.RunSync(() => _cached.GetCurrentDateTimeListFiveSecondCacheAsync());

            Thread.Sleep(new TimeSpan(0, 0, 2));

            var cached = AsyncHelpers.RunSync(() => _cached.GetCurrentDateTimeListFiveSecondCacheAsync());

            Assert.Equal(now.First(), cached.First());

            Thread.Sleep(new TimeSpan(0, 0, 5));

            var cached2 = AsyncHelpers.RunSync(() => _cached.GetCurrentDateTimeListFiveSecondCacheAsync());

            Assert.NotEqual(now.First(), cached2.First());

            Thread.Sleep(new TimeSpan(0, 0, 1));

            var cached3 = AsyncHelpers.RunSync(() => _cached.GetCurrentDateTimeListFiveSecondCacheAsync());

            Assert.Equal(cached2.First(), cached3.First());
        }

        [Fact]
        [Trait("Feature", "Null Caching")]
        public void TestNullFiveSecondTimeout()
        {
            var now = _cached.GetNullAsStringFiveSecondCache();

            Thread.Sleep(new TimeSpan(0, 0, 2));

            var cached = _cached.GetNullAsStringFiveSecondCache();

            Assert.Equal(now, cached);

            Thread.Sleep(new TimeSpan(0, 0, 2));

            var cached2 = _cached.GetNullAsStringFiveSecondCache();

            Assert.Equal(1, _cached.GetNumberOfTimesCalled());
        }

        [Fact]
        [Trait("Feature", "Null Caching")]
        public async Task TestNullFiveSecondTimeoutAsync()
        {
            var now = await _cached.GetNullAsStringFiveSecondCacheAsync();

            Thread.Sleep(new TimeSpan(0, 0, 2));

            var cached = await _cached.GetNullAsStringFiveSecondCacheAsync();

            Assert.Equal(now, cached);

            Thread.Sleep(new TimeSpan(0, 0, 2));

            var cached2 = await _cached.GetNullAsStringFiveSecondCacheAsync();

            Assert.Equal(1, _cached.GetNumberOfTimesCalled());
        }

        [Fact]
        [Trait("Feature", "Durable")]
        public void TestTenSecondCacheAbsoluteThirtySeconds()
        {
            var now = _cached.GetCurrentDateAsStringTenSecondCacheAbsoluteThirtySeconds();

            Thread.Sleep(new TimeSpan(0, 0, 0, 10, 500));

            var cached = _cached.GetCurrentDateAsStringTenSecondCacheAbsoluteThirtySeconds();

            //Cache has been refreshed
            Assert.NotEqual(now, cached);

            Thread.Sleep(new TimeSpan(0, 0, 5));

            var cached2 = _cached.GetCurrentDateAsStringTenSecondCacheAbsoluteThirtySeconds();

            //Cache has not yet been refreshed
            Assert.Equal(cached, cached2);

            Thread.Sleep(new TimeSpan(0, 0, 0, 5, 500));

            var cached3 = _cached.GetCurrentDateAsStringTenSecondCacheAbsoluteThirtySeconds();

            //Cache was resceduled
            Assert.NotEqual(cached2, cached3);

            //Check Trace to make sure cache stops refreshing
            Thread.Sleep(new TimeSpan(0, 0, 40));

            var notCached = _cached.GetCurrentDateAsStringTenSecondCacheAbsoluteThirtySeconds();

            Assert.True(DateTime.Parse(notCached).Subtract(DateTime.Now).TotalSeconds < 5,
                "Absolute Cache was not honored");
        }

        [Fact]
        [Trait("Feature", "Durable")]
        public async Task TestTenSecondCacheAbsoluteThirtySecondsAsync()
        {
            var now = await _cached.GetCurrentDateAsStringTenSecondCacheAbsoluteThirtySecondsAsync();

            Thread.Sleep(new TimeSpan(0, 0, 0, 10, 500));

            var cached = await _cached.GetCurrentDateAsStringTenSecondCacheAbsoluteThirtySecondsAsync();

            //Cache has been refreshed
            Assert.NotEqual(now, cached);

            Thread.Sleep(new TimeSpan(0, 0, 5));

            var cached2 = await _cached.GetCurrentDateAsStringTenSecondCacheAbsoluteThirtySecondsAsync();

            //Cache has not yet been refreshed
            Assert.Equal(cached, cached2);

            Thread.Sleep(new TimeSpan(0, 0, 0, 5, 500));

            var cached3 = await _cached.GetCurrentDateAsStringTenSecondCacheAbsoluteThirtySecondsAsync();

            //Cache was resceduled
            Assert.NotEqual(cached2, cached3);

            //Check Trace to make sure cache stops refreshing
            Thread.Sleep(new TimeSpan(0, 0, 40));

            var notCached = _cached.GetCurrentDateAsStringTenSecondCacheAbsoluteThirtySeconds();

            Assert.True(DateTime.Parse(notCached).Subtract(DateTime.Now).TotalSeconds < 5,
                "Absolute Cache was not honored");
        }

        [Fact]
        [Trait("Feature", "Durable")]
        public void TestDurableCacheWithComplicatedParameter()
        {
            var complexObject = new ComplexObject { Ints = { 5, 5, 5 } };

            var now = _cached.GetComplexObjectWithComplexParameterFifeenSecondCacheAbsoluteNinetySeconds(complexObject);
            Assert.Equal(complexObject.Ints, now.Ints);

            Thread.Sleep(new TimeSpan(0, 0, 0, 10, 500));

            var cached = _cached.GetComplexObjectWithComplexParameterFifeenSecondCacheAbsoluteNinetySeconds(complexObject);

            //Cache has been refreshed
            Assert.NotEqual(now.FourthDimension.Keys.First(), cached.FourthDimension.Keys.First());
            Assert.Equal(complexObject.Ints, cached.Ints);

            Thread.Sleep(new TimeSpan(0, 0, 5));

            var cached2 = _cached.GetComplexObjectWithComplexParameterFifeenSecondCacheAbsoluteNinetySeconds(complexObject);

            //Cache has not yet been refreshed
            Assert.Equal(cached.FourthDimension.Keys.First(), cached2.FourthDimension.Keys.First());
            Assert.Equal(complexObject.Ints, cached2.Ints);

            Thread.Sleep(new TimeSpan(0, 0, 0, 5, 500));

            var cached3 = _cached.GetComplexObjectWithComplexParameterFifeenSecondCacheAbsoluteNinetySeconds(complexObject);

            //Cache was resceduled
            Assert.NotEqual(cached2.FourthDimension.Keys.First(), cached3.FourthDimension.Keys.First());
            Assert.Equal(complexObject.Ints, cached3.Ints);

            //Check Trace to make sure cache stops refreshing
            Thread.Sleep(new TimeSpan(0, 0, 40));
        }

        [Fact]
        [Trait("Feature", "Durable")]
        public void TestDurableCacheEvict()
        {
            var now = _cached.GetCurrentDateAsStringTenSecondCacheAbsoluteThirtySeconds();

            PurgeDurableCacheQueue();
            Thread.Sleep(new TimeSpan(0, 0, 0, 11));

            var cached = _cached.GetCurrentDateAsStringTenSecondCacheAbsoluteThirtySeconds();

            //Cache has not been refreshed
            Assert.Equal(now, cached);

            Thread.Sleep(new TimeSpan(0, 0, 11));

            //Cache miss should be detected and queued immediatly
            var cached2 = _cached.GetCurrentDateAsStringTenSecondCacheAbsoluteThirtySeconds();

            //Cache has not yet been refreshed
            Assert.NotEqual(cached, cached2);

            Thread.Sleep(new TimeSpan(0, 0, 2));

            var cached3 = _cached.GetCurrentDateAsStringTenSecondCacheAbsoluteThirtySeconds();

            Assert.Equal(cached2, cached3);

            //Check Trace to make sure cache stops refreshing
            Thread.Sleep(new TimeSpan(0, 0, 40));
        }

        [Fact]
        [Trait("Feature", "Durable")]
        public async Task TestDurableCacheEvictAsync()
        {
            var now = await _cached.GetCurrentDateAsStringTenSecondCacheAbsoluteThirtySecondsAsync();

            PurgeDurableCacheQueue();
            Thread.Sleep(new TimeSpan(0, 0, 0, 11));

            var cached = await _cached.GetCurrentDateAsStringTenSecondCacheAbsoluteThirtySecondsAsync();

            //Cache has not been refreshed
            Assert.Equal(now, cached);

            Thread.Sleep(new TimeSpan(0, 0, 11));

            //Cache miss should be detected and queued immediatly
            var cached2 = await _cached.GetCurrentDateAsStringTenSecondCacheAbsoluteThirtySecondsAsync();

            //Cache has not yet been refreshed
            Assert.NotEqual(cached, cached2);

            Thread.Sleep(new TimeSpan(0, 0, 2));

            var cached3 = await _cached.GetCurrentDateAsStringTenSecondCacheAbsoluteThirtySecondsAsync();

            Assert.Equal(cached2, cached3);

            //Check Trace to make sure cache stops refreshing
            Thread.Sleep(new TimeSpan(0, 0, 40));
        }

        [Fact]
        [Trait("Feature", "Durable")]
        public void TestDurableCacheStalenessRequeue()
        {
            var now = _cached.GetCurrentDateTimeTenSecondCacheAbsoluteNinetySecondsResecheduleStaleness();

            PurgeDurableCacheQueue();
            Thread.Sleep(new TimeSpan(0, 0, 0, 11));

            var cached = _cached.GetCurrentDateTimeTenSecondCacheAbsoluteNinetySecondsResecheduleStaleness();

            //Cache has not been refreshed
            Assert.Equal(now, cached);

            Thread.Sleep(new TimeSpan(0, 0, 11));

            //Cache miss should be detected and differed
            var cached2 = _cached.GetCurrentDateTimeTenSecondCacheAbsoluteNinetySecondsResecheduleStaleness();

            //Cache has not yet been refreshed
            Assert.Equal(cached, cached2);

            Thread.Sleep(new TimeSpan(0, 0, 11));

            var cached3 = _cached.GetCurrentDateTimeTenSecondCacheAbsoluteNinetySecondsResecheduleStaleness();

            Assert.NotEqual(cached2, cached3);

            //Check Trace to make sure cache stops refreshing
            Thread.Sleep(new TimeSpan(0, 0, 40));
        }

        [Fact]
        [Trait("Feature", "Tiers")]
        public void TestTwoTierCache()
        {
            //Should be served from tier 2
            var now = _cached.GetCurrentDateTimeFiveSecondCacheTwoTiers();

            Thread.Sleep(new TimeSpan(0, 0, 10));

            //Should be served from tier 1
            var cached = _cached.GetCurrentDateTimeFiveSecondCacheTwoTiers();

            Assert.Equal(now, cached);

            Thread.Sleep(new TimeSpan(0, 0, 10));

            //Should be served from tier 2
            var cached2 = _cached.GetCurrentDateTimeFiveSecondCacheTwoTiers();

            Assert.NotEqual(now, cached2);
        }

        [Fact]
        [Trait("Feature", "Tiers")]
        public async Task TestTwoTierCacheAsync()
        {
            //Should be served from tier 2
            var now = await _cached.GetCurrentDateTimeFiveSecondCacheTwoTiersAsync();

            Thread.Sleep(new TimeSpan(0, 0, 10));

            //Should be served from tier 1
            var cached = await _cached.GetCurrentDateTimeFiveSecondCacheTwoTiersAsync();

            Assert.Equal(now, cached);

            Thread.Sleep(new TimeSpan(0, 0, 10));

            //Should be served from tier 2
            var cached2 = await _cached.GetCurrentDateTimeFiveSecondCacheTwoTiersAsync();

            Assert.NotEqual(now, cached2);
        }

        [Fact]
        [Trait("Feature", "Tiers")]
        public void TestTwoTierCacheWithDefault()
        {
            //Should be served from tier 2
            var now = _cached.GetCurrentDateTimeFiveSecondCacheTwoTiersWithDefault();

            Thread.Sleep(new TimeSpan(0, 0, 10));

            //Should be served from tier 1 (default)
            var cached = _cached.GetCurrentDateTimeFiveSecondCacheTwoTiersWithDefault();

            Assert.Equal(now, cached);

            Thread.Sleep(new TimeSpan(0, 0, 10));

            //Should be served from tier 2
            var cached2 = _cached.GetCurrentDateTimeFiveSecondCacheTwoTiersWithDefault();

            Assert.NotEqual(now, cached2);
        }

        [Fact]
        [Trait("Feature", "Tiers")]
        public async Task TestTwoTierCacheWithDefaultAsync()
        {
            //Should be served from tier 2
            var now = await _cached.GetCurrentDateTimeFiveSecondCacheTwoTiersWithDefaultAsync();

            Thread.Sleep(new TimeSpan(0, 0, 10));

            //Should be served from tier 1 (default)
            var cached = await _cached.GetCurrentDateTimeFiveSecondCacheTwoTiersWithDefaultAsync();

            Assert.Equal(now, cached);

            Thread.Sleep(new TimeSpan(0, 0, 10));

            //Should be served from tier 2
            var cached2 = await _cached.GetCurrentDateTimeFiveSecondCacheTwoTiersWithDefaultAsync();

            Assert.NotEqual(now, cached2);
        }

        [Fact]
        [Trait("Feature", "Tiers")]
        public void TestTwoTierCacheWithDictionary()
        {
            //Should be served from tier 2
            var now = _cached.GetCurrentDateTimeFiveSecondCacheTwoTiersWithDictionary();

            Thread.Sleep(new TimeSpan(0, 0, 10));

            //Should be served from tier 1
            var cached = _cached.GetCurrentDateTimeFiveSecondCacheTwoTiersWithDictionary();

            Assert.Equal(now, cached);

            PurgeDictionaryCache();

            //Should be served from tier 2
            var cached2 = _cached.GetCurrentDateTimeFiveSecondCacheTwoTiersWithDictionary();

            Assert.NotEqual(now, cached2);

            Thread.Sleep(new TimeSpan(0, 0, 10));
        }
    }
}