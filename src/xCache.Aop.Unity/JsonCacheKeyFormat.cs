using Microsoft.Practices.Unity.InterceptionExtension;
using System.Reflection;

namespace xCache.Aop.Unity
{
    public class JsonCacheKeyFormat
    {
        public MethodBase MethodBase { get; set; }
        public IParameterCollection Inputs { get; set; }
    }
}
