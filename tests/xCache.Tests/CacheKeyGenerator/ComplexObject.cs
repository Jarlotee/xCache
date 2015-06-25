using System;
using System.Collections.Generic;

namespace xCache.Tests.CacheKeyGenerator
{
    public class ComplexObject : AbstractComplexObject
    {
        public List<int> Ints { get; set; }

        public List<double> EmptyDouble { get; set; }

        public Dictionary<DateTime, string> FourthDimension { get; set; }

        public List<ComplexChildObject> Children { get; set; }

        public ComplexObject() : base()
        {
            Ints = new List<int> { 1, 5, 2, 5, 6 };
            FourthDimension = new Dictionary<DateTime, string>
            {
                { DateTime.Now, "Here" }
            };
            Children = new List<ComplexChildObject>
            {
                new ComplexChildObject 
                {
                    Parent = this
                }
            };
        }
    }
}
