using System.Reflection;
using Unity.Interception.PolicyInjection.Pipeline;

namespace xCache.Aop.Unity
{
    public class JsonCacheKeyFormat
    {
        public MethodBase MethodBase { get; set; }
        public IParameterCollection Inputs { get; set; }
    }
}
