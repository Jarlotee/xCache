using System;

namespace xCache.Durable
{
    public class DurableMethod
    {
        public Type DeclaringType { get; set; }
        public Type Interface { get; set; }
        public string Name { get; set; }
        public Type[] Parameters { get; set; }
    }
}
