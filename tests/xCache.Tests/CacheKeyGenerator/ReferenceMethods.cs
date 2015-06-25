namespace xCache.Tests.CacheKeyGenerator
{
    public class ReferenceMethods : AbstractReferenceMethods
    {
        public void NoParameters() { }

        public void SomeParameters(int i, double s) { }

        public string NoParameterReturnsString()
        {
            return string.Empty;
        }

        private ComplexObject Overloaded(string s)
        {
            return new ComplexObject();
        }

        private ComplexObject Overloaded(ComplexObject t)
        {
            return new ComplexObject();
        }

        protected ComplexObject ProtectedMethod(int i)
        {
            return new ComplexObject();
        }

        public override string OverriddenVirtualMethod(int i)
        {
            return (i + 1).ToString();
        }

        internal void InternalMethod() { }
    }
}
