using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Griffin.Container.Tests.Subjects;
using Xunit;

namespace Griffin.Container.Tests
{
    public class MultipleImplementationsDifferentLifetimes
    {
        public class TestWithDependency
        {
            public TestWithDependency(IFirstService service)
            {
                
            }
        }
        public class TestWithDependency2
        {
            public TestWithDependency2(IFirstService service)
            {

            }
        }

        [Fact]
        public void Test()
        {
            var reg = new ContainerRegistrar();
            reg.RegisterConcrete<FirstService>(Lifetime.Singleton);
            reg.RegisterConcrete<AnotherFirstService>(Lifetime.Scoped);
            reg.RegisterConcrete<TestWithDependency>(Lifetime.Singleton);
            reg.RegisterConcrete<TestWithDependency2>(Lifetime.Scoped);
            var container = reg.Build();

            var single = container.Resolve<IFirstService>();
            container.Resolve<TestWithDependency>();
            IFirstService scoped;

            using (var scope = container.CreateChildContainer())
            {
                scoped = scope.Resolve<IFirstService>();
                scope.Resolve<TestWithDependency2>();
            }

            Assert.IsType<FirstService>(single);
            Assert.IsType<AnotherFirstService>(scoped);
        }
    }
}
