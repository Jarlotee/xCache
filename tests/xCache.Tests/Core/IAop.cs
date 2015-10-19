using System;
using System.Threading.Tasks;

namespace xCache.Tests.Core
{
    public interface IAop
    {
        string GetCurrentDateAsStringFiveSecondCache();
        Task<string> GetCurrentDataAsStringFiveSecondCacheAsync();
        DateTime GetCurrentDateTimeFiveSecondCache();
        Task<DateTime> GetCurrentDateTimeFiveSecondCacheAsync();
    }
}
