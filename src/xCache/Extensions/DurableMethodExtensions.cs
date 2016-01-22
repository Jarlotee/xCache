using System.Reflection;
using xCache.Durable;

namespace xCache.Extensions
{
    public static class DurableMethodExtensions
    {
        public static MethodBase GetMethod(this DurableMethod method)
        {
            return method.DeclaringType.GetMethod(method.Name, method.Parameters);
        }
    }
}
