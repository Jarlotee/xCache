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

        [Fact]
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
        public void TestNullResult()
        {
            var result = _cached.GetNullAsStringFiveSecondCache();

            Assert.Equal(result, null);
        }

        [Fact]
        public async Task TestNullResultAsync()
        {
            var result = await _cached.GetNullAsStringFiveSecondCacheAsync();

            Assert.Equal(result, null);
        }

        [Fact]
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

                Assert.True(item.EndsWith(":" + i.ToString()));
            }

            Thread.Sleep(1000 * 120);
        }

        [Fact]
        public void TestDurablewCacheWithComplicatedObject()
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
        public void TestDurableCacheRequeue()
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
        public async Task TestDurableCacheRequeueAsync()
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
    }
}
