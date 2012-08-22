using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Griffin.Container.Tests
{
    public class TypeExtensionTests
    {
        [Fact]
        public void Success()
        {
            var type = typeof (IEnumerable<TypeExtensionTests>);
            Assert.Equal("System.Collections.Generic.IEnumerable<Griffin.Container.Tests.TypeExtensionTests>", type.ToBetterString());
        }
    }
}
