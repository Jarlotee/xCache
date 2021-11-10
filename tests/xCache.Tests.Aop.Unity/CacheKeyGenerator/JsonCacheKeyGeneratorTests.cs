using NSubstitute;
using System;
using System.Reflection;
using Unity.Interception.PolicyInjection.Pipeline;
using xCache.Aop.Unity;
using xCache.Tests.CacheKeyGenerator;
using Xunit;

namespace xCache.Tests.Aop.Unity.CacheKeyGenerator
{
    [Trait("Feature", "Cache Key Generation")]
    public class JsonCacheKeyGeneratorTests
    {
        ICacheKeyGenerator _generator;
        Type _referenceType;

        public JsonCacheKeyGeneratorTests()
        {
            _generator = new JsonCacheKeyGenerator();
            _referenceType = typeof(ReferenceMethods);
        }

        public IMethodInvocation GetMethodInvocationForMethod(string name, Type[] parameters = null, object[] values = null)
        {
            var invocation = Substitute.For<IMethodInvocation>();

            MethodInfo method;

            method = _referenceType.GetMethod(name,
                    (BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic), 
                    null, 
                    parameters ?? new Type[0], 
                    null);

            invocation.MethodBase.ReturnsForAnyArgs(method);

            if (values != null)
            {
                invocation.Inputs.ReturnsForAnyArgs(new ParameterCollection(values, method.GetParameters(), m => true));
            }

            return invocation;
        }

        [Fact]
        public void NoParametersTest()
        {
            var result = _generator.GenerateKey(
                GetMethodInvocationForMethod("NoParameters"));

            Assert.False(string.IsNullOrWhiteSpace(result));
        }

        [Fact]
        public void SomeParameterTest()
        {
            var parameters = new Type[] { typeof(int), typeof(double) };

            var values = new object[] { 1, 2.6d };

            var result = _generator.GenerateKey(
                GetMethodInvocationForMethod("SomeParameters", parameters, values));

            Assert.False(string.IsNullOrWhiteSpace(result));
        }

        [Fact]
        public void NoParameterReturnsStringTest()
        {
            var values = new object[] { };

            var result = _generator.GenerateKey(
                GetMethodInvocationForMethod("NoParameterReturnsString", null, values));

            Assert.False(string.IsNullOrWhiteSpace(result));
        }


        [Fact]
        public void OverloadedTest1()
        {
            var parameters = new[] { typeof(string) };

            var values = new object[] { "s" };

            var result = _generator.GenerateKey(
                GetMethodInvocationForMethod("Overloaded", parameters, values));

            Assert.False(string.IsNullOrWhiteSpace(result));
        }

        [Fact]
        public void OverloadedTest2()
        {
            var parameters = new[] { typeof(string) };

            var values = new object[] { null };

            var result = _generator.GenerateKey(
                GetMethodInvocationForMethod("Overloaded", parameters, values));

            Assert.False(string.IsNullOrWhiteSpace(result));
        }

        [Fact]
        public void OverloadedTest3()
        {
            var parameters = new[] { typeof(ComplexObject) };

            var values = new object[] { new ComplexObject() };

            var result = _generator.GenerateKey(
                GetMethodInvocationForMethod("Overloaded", parameters, values));

            Assert.False(string.IsNullOrWhiteSpace(result));
        }

        [Fact]
        public void ProtectedMethodTest()
        {
            var parameters = new Type[] { typeof(int) };

            var values = new object[] { 1 };

            var result = _generator.GenerateKey(
                GetMethodInvocationForMethod("ProtectedMethod", parameters, values));

            Assert.False(string.IsNullOrWhiteSpace(result));
        }

        [Fact]
        public void OverriddenVirtualMethodTest()
        {
            var parameters = new Type[] { typeof(int) };

            var values = new object[] { 1 };

            var result = _generator.GenerateKey(
                GetMethodInvocationForMethod("OverriddenVirtualMethod", parameters, values));

            Assert.False(string.IsNullOrWhiteSpace(result));
        }
        

        [Fact]
        public void InternalMethodTest()
        {
            var result = _generator.GenerateKey(
                GetMethodInvocationForMethod("InternalMethod"));

            Assert.False(string.IsNullOrWhiteSpace(result));
        }


        [Fact]
        public void VirtualMethodTest()
        {
            var parameters = new Type[] { typeof(int) };

            var values = new object[] { 1 };

            var result = _generator.GenerateKey(
                GetMethodInvocationForMethod("VirtualMethod", parameters, values));

            Assert.False(string.IsNullOrWhiteSpace(result));
        }

        [Fact]
        public void InheritedProtectedMethodTest()
        {
            var parameters = new Type[] { typeof(int) };

            var values = new object[] { 1 };

            var result = _generator.GenerateKey(
                GetMethodInvocationForMethod("InheritedProtectedMethod", parameters, values));

            Assert.False(string.IsNullOrWhiteSpace(result));
        }
    }
}
