using System;
using System.Threading;
using Xunit;

namespace xCache.Tests.Core
{
    public abstract class AopTests
    {
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
    }
}
