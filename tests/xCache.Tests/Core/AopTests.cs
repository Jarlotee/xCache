using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace xCache.Tests.Core
{
    public abstract class AopTests
    {
        public static IDictionary DictionaryCache = new Hashtable();

        protected IAop _aop = null;

        [Fact]
        public void TestFiveSecondTimeout()
        {
            var now = _aop.GetCurrentDateAsStringFiveSecondCache();

            Thread.Sleep(new TimeSpan(0, 0, 2));

            var cached = _aop.GetCurrentDateAsStringFiveSecondCache();

            Assert.Equal(now, cached);

            Thread.Sleep(new TimeSpan(0, 0, 5));

            var cached2 = _aop.GetCurrentDateAsStringFiveSecondCache();

            Assert.NotEqual(now, cached2);

            Thread.Sleep(new TimeSpan(0, 0, 1));

            var cached3 = _aop.GetCurrentDateAsStringFiveSecondCache();

            Assert.Equal(cached2, cached3);
        }

        [Fact]
        public async Task TestFiveSecondTimeoutAsync()
        {
            var now = await _aop.GetCurrentDataAsStringFiveSecondCacheAsync();

            Thread.Sleep(new TimeSpan(0, 0, 2));

            var cached = await _aop.GetCurrentDataAsStringFiveSecondCacheAsync();

            Assert.Equal(now, cached);

            Thread.Sleep(new TimeSpan(0, 0, 5));

            var cached2 = await _aop.GetCurrentDataAsStringFiveSecondCacheAsync();

            Assert.NotEqual(now, cached2);

            Thread.Sleep(new TimeSpan(0, 0, 1));

            var cached3 = await _aop.GetCurrentDataAsStringFiveSecondCacheAsync();

            Assert.Equal(cached2, cached3);
        }

        [Fact]
        public void TestFiveSecondTimeoutStruct()
        {
            var now = _aop.GetCurrentDateTimeFiveSecondCache();

            Thread.Sleep(new TimeSpan(0, 0, 2));

            var cached = _aop.GetCurrentDateTimeFiveSecondCache();

            Assert.Equal(now, cached);

            Thread.Sleep(new TimeSpan(0, 0, 5));

            var cached2 = _aop.GetCurrentDateTimeFiveSecondCache();

            Assert.NotEqual(now, cached2);

            Thread.Sleep(new TimeSpan(0, 0, 1));

            var cached3 = _aop.GetCurrentDateTimeFiveSecondCache();

            Assert.Equal(cached2, cached3);
        }

        [Fact]
        public async Task TestFiveSecondTimeoutStructAsync()
        {
            var now = await _aop.GetCurrentDateTimeFiveSecondCacheAsync();

            Thread.Sleep(new TimeSpan(0, 0, 2));

            var cached = await _aop.GetCurrentDateTimeFiveSecondCacheAsync();

            Assert.Equal(now, cached);

            Thread.Sleep(new TimeSpan(0, 0, 5));

            var cached2 = await _aop.GetCurrentDateTimeFiveSecondCacheAsync();

            Assert.NotEqual(now, cached2);

            Thread.Sleep(new TimeSpan(0, 0, 1));

            var cached3 = await _aop.GetCurrentDateTimeFiveSecondCacheAsync();

            Assert.Equal(cached2, cached3);
        }

        [Fact]
        public void TestNullResult()
        {
            var result = _aop.GetNullAsStringFiveSecondCache();

            Assert.Equal(result, null);
        }

        [Fact]
        public async Task TestNullResultAsync()
        {
            var result = await _aop.GetNullAsStringFiveSecondCacheAsync();

            Assert.Equal(result, null);
        }

        [Fact]
        public void TestSecondLevelCache()
        {
            CacheRecord record;

            _aop.GetCurrentDateAsStringOneSecondCache();
            CacheRecorder.Records.TryPop(out record);
            Assert.Equal("MemoryCache", record.Source);

            _aop.GetCurrentDateAsStringOneSecondCache();
            CacheRecorder.Records.TryPop(out record);
            Assert.Equal("DictionaryCache", record.Source);

            DictionaryCache.Clear();
            _aop.GetCurrentDateAsStringOneSecondCache();
            CacheRecorder.Records.TryPop(out record);
            Assert.Equal("MemoryCache", record.Source);
        }
    }
}
