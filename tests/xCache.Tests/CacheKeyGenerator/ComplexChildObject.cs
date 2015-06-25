namespace xCache.Tests.CacheKeyGenerator
{
    public class ComplexChildObject
    {
        public ComplexObject Parent { get; set; }
        public string Genie { get; set; }

        public ComplexChildObject()
        {
            Genie = "in a bottle";
        }
    }
}
