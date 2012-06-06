using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Griffin.Container.Tests
{
    public class CircularDependenciesTests
    {
        [Fact]
        public void Test()
        {
            var registrar = new ContainerRegistrar();
            registrar.RegisterConcrete<Service1>();
            registrar.RegisterConcrete<Service2>();
            registrar.RegisterConcrete<Service3>();

            var builder = new ContainerBuilder();
            Assert.Throws<CircularDependenciesException>(() => builder.Build(registrar));


        }

        class Service1
        {
            private readonly Service2 _service2;

            public Service1(Service2 service2)
            {
                _service2 = service2;
            }
            
        }

        class Service2
        {
            private readonly Service3 _service3;

            public Service2(Service3 service3)
            {
                _service3 = service3;
            }
        }

        class Service3
        {
            private readonly Service2 _service2;

            public Service3(Service2 service2)
            {
                _service2 = service2;
            }
        }
    }
}
