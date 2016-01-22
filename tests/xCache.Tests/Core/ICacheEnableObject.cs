using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using xCache.Tests.CacheKeyGenerator;

namespace xCache.Tests.Core
{
    public interface ICacheEnableObject
    {
        int GetNumberOfTimesCalled();
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
        string GetCurrentDateTimeFiveSecondCacheTwoTiers();
        Task<string> GetCurrentDateTimeFiveSecondCacheTwoTiersAsync();
        string GetCurrentDateTimeFiveSecondCacheTwoTiersWithDefault();
        Task<string> GetCurrentDateTimeFiveSecondCacheTwoTiersWithDefaultAsync();
        string GetCurrentDateTimeFiveSecondCacheTwoTiersWithDictionary();
        Task<IEnumerable<string>> GetCurrentDateTimeListFiveSecondCacheAsync();
        string GetCurrentDateTimeTenSecondCacheAbsoluteNinetySecondsResecheduleStaleness();
    }
}
