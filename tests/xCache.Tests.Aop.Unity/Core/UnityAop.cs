using System;
using System.Threading;
using System.Threading.Tasks;
using xCache.Aop.Unity;
using xCache.Tests.Core;

namespace xCache.Tests.Aop.Unity.Core
{
    public class UnityAop : IAop
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

        [Cache(Seconds = 1, Name = "Dictionary")]
        public string GetCurrentDateAsStringOneSecondCache()
        {
            return DateTime.Now.ToString();
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
    }
}
