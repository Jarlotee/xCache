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
            return string.Format(@"{{ ""MethodBase"" : {0}, ""Inputs"": {1} }}",
                JsonConvert.SerializeObject(invocation.MethodBase, settings),
                JsonConvert.SerializeObject(invocation.Inputs, settings));
        }
    }
}
