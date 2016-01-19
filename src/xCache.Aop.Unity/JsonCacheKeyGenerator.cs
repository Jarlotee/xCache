using Microsoft.Practices.Unity.InterceptionExtension;
using Newtonsoft.Json;

namespace xCache.Aop.Unity
{
    public class JsonCacheKeyGenerator : ICacheKeyGenerator
    {
        JsonSerializerSettings settings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Formatting = Formatting.None,
            NullValueHandling = NullValueHandling.Ignore
        };

        public string GenerateKey(IMethodInvocation invocation)
        {
            var format = new JsonCacheKeyFormat
            {
                MethodBase = invocation.MethodBase,
                Inputs = invocation.Inputs
            };

            return JsonConvert.SerializeObject(format, settings);
        }
    }
}
