using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Griffin.Container.Tests
{
    public class EnumerableConstructorParameterTests
    {
        [Fact]
        public void Test()
        {
            var registrar = new ContainerRegistrar(Lifetime.Transient);
            registrar.RegisterConcrete<Will>();
            registrar.RegisterConcrete<Wont>();
            registrar.RegisterConcrete<Subject>();

            var c = registrar.Build();
            var actual = c.Resolve<Subject>();

            Assert.NotNull(actual);
            Assert.IsAssignableFrom<Subject>(actual);
        }

        [Fact]
        public void RegisterServiceAsInstance()
        {
            var registrar = new ContainerRegistrar(Lifetime.Transient);
            registrar.RegisterInstance<IThinkNot>(new Will());
            registrar.RegisterConcrete<InstanceSubject>();

            var c = registrar.Build();
            var actual = c.Resolve<InstanceSubject>();

            Assert.NotNull(actual);
            Assert.IsAssignableFrom<InstanceSubject>(actual);
        }

        [Fact]
        public void NoRegisteredConcretes()
        {
            var registrar = new ContainerRegistrar(Lifetime.Transient);
            registrar.RegisterConcrete<NoInstancesSubject>();

            var c = registrar.Build();
            var actual = c.Resolve<NoInstancesSubject>();

            Assert.NotNull(actual);
            Assert.IsAssignableFrom<NoInstancesSubject>(actual);
        }

        public class Subject
        {
            public Subject(IEnumerable<IThinkNot> classes)
            {
                Assert.True(!classes.Any(x=> x == null));
                Assert.True(classes.Any(x=>x.GetType() == typeof(Will)));
                Assert.True(classes.Any(x => x.GetType() == typeof(Wont)));
            }
        }

        public class InstanceSubject
        {
            public InstanceSubject(IEnumerable<IThinkNot> classes)
            {
                Assert.True(!classes.Any(x => x == null));
                Assert.True(classes.Any(x => x.GetType() == typeof(Will)));
            }
        }

        public class NoInstancesSubject
        {
            public NoInstancesSubject(IEnumerable<IThinkNot> classes)
            {
                Assert.False(classes.Any());
            }
        }

        public class Will : IThinkNot
        {
            
        }

        public class Wont : IThinkNot
        {
            
        }
        public interface IThinkNot
        {
            
        }
    }
}
