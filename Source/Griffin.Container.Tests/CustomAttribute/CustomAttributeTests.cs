using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Xunit;

namespace Griffin.Container.Tests.CustomAttribute
{
    public class CustomAttributeTests
    {
        [Fact]
        public void TestAttribute()
        {
            var registrar = new ContainerRegistrar(Lifetime.Transient);

            registrar.RegisterUsingAttribute<ContainerServiceAttribute>(Assembly.GetExecutingAssembly());
            var container = registrar.Build();

            Assert.NotNull(container.Resolve<SomeService>());
        }

    }
}
