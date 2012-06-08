using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Griffin.Container.Tests
{
    public class MissingDependency
    {
        [Fact]
        public void Test()
        {
            var registrar = new ContainerRegistrar();
            registrar.RegisterConcrete<Service1>();

            var builder = new ContainerBuilder();
            Assert.Throws<ConcreteDependencyMissingException>(() =>builder.Build(registrar));


        }

        class Service1
        {
            private readonly MissingDependency _service2;

            public Service1(MissingDependency service2)
            {
                _service2 = service2;
            }
        }
    }
}
