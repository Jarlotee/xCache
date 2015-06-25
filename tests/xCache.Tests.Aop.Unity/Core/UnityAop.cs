using System;
using xCache.Aop.Unity;
using xCache.Tests.Core;

namespace xCache.Tests.Aop.Unity.Core
{
    public class UnityAop : IAop
    {
        [Cache(Seconds = 5)]
        public string GetCurrentDateAsStringFiveSecondCache()
        {
            return DateTime.Now.ToString();
        }
    }
}
