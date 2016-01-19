using System;
using System.Threading.Tasks;
using xCache.Tests.CacheKeyGenerator;

namespace xCache.Tests.Core
{
    public interface ICacheEnableObject
    {
        string GetCurrentDateAsStringFiveSecondCache();
        Task<string> GetCurrentDataAsStringFiveSecondCacheAsync();
        DateTime GetCurrentDateTimeFiveSecondCache();
        Task<DateTime> GetCurrentDateTimeFiveSecondCacheAsync();
        string GetNullAsStringFiveSecondCache();
        Task<string> GetNullAsStringFiveSecondCacheAsync();
        string GetCurrentDateAsStringTenSecondCacheAbsoluteThirtySeconds();
        Task<string> GetCurrentDateAsStringTenSecondCacheAbsoluteThirtySecondsAsync();
        string GetCurrentDateAsStringWithParameterFifteenSecondCacheAbsoluteNinetySeconds(int p0);
        ComplexObject GetComplexObjectWithComplexParameterFifeenSecondCacheAbsoluteNinetySeconds(ComplexObject obj);
        int GetNumberOfTimesCalled();
    }
}
