using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Griffin.Container.Tests.CustomAttribute;
using Griffin.Container.Tests.Subjects;
using Xunit;

namespace Griffin.Container.Tests
{
    public class ServiceTests
    {
        [Fact]
        public void should_Be_able_To_Register_a_service_using_a_factory_method()
        {
            var reg = new ContainerRegistrar();
            reg.RegisterService(CreateFirstService, Lifetime.Scoped);
            reg.RegisterService(CreateMyService, Lifetime.Singleton);

            var container = reg.Build();
            var actual = container.Resolve<ISomeServiceInterface>();

            Assert.NotNull(actual);
        }

        private ISomeServiceInterface CreateMyService(IServiceLocator arg)
        {
            return new MySomeService();
        }

        private IFirstService CreateFirstService(IServiceLocator arg)
        {
            return new FirstService();
        }
    }
}
