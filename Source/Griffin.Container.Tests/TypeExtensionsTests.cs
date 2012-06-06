using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Griffin.Container.Tests
{
    public class TypeExtensionsTests
    {
        interface IBase<T>
        {
             
        }
        interface IBase2<T>
        {

        }


        class Class1<T> : IBase<T>
        {
             
        }

        class Class2<T> : Class1<T>
        {
             
        }

        [Fact]
        public void ConcreteToInterface()
        {
            var concrete = typeof (Class1<>);
            var service = typeof (IBase<>);

            Assert.True(service.IsAssignableFromGeneric(concrete));
        }

        [Fact]
        public void ConcreteToOtherInterface()
        {
            var concrete = typeof(Class1<>);
            var service = typeof(IBase2<>);

            Assert.False(service.IsAssignableFromGeneric(concrete));
        }

        [Fact]
        public void ConcreteToBase()
        {
            var concrete = typeof(Class2<>);
            var service = typeof(Class1<>);

            Assert.True(service.IsAssignableFromGeneric(concrete));
        }
    }
}
