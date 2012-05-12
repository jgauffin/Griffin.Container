using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Xunit;

namespace Griffin.Container.Tests
{

    
    public class IntegrationTests
    {
        [Fact]
        public void SimpleRegistrationTransient()
        {
            var registrar = new ContainerRegistrar();
            registrar.RegisterType<MySelf>(Lifetime.Transient);
            var builder = new ContainerBuilder();
            var container = builder.Build(registrar);

            var instance1 = container.Resolve<MySelf>();
            var instance2 = container.Resolve<MySelf>();

            Assert.NotSame(instance1, instance2);
        }


        [Fact]
        public void OneDependencySingleton()
        {
            var registrar = new ContainerRegistrar();
            registrar.RegisterType<MySelf>(Lifetime.Transient);
            registrar.RegisterType<OneDepencency>(Lifetime.Singleton);
            var builder = new ContainerBuilder();
            var container = builder.Build(registrar);

            var instance1 = container.Resolve<OneDepencency>();
            var instance2 = container.Resolve<OneDepencency>();

            Assert.Same(instance1, instance2);
        }

        [Fact]
        public void ResolveAllStartable()
        {
            var registrar = new ContainerRegistrar();
            registrar.RegisterType<Startable1>(Lifetime.Singleton);
            registrar.RegisterType<Startable2>(Lifetime.Singleton);
            var builder = new ContainerBuilder();
            var container = builder.Build(registrar);

            var instances = container.ResolveAll<ISingletonStartable>();

            Assert.Equal(2, instances.Count());
        }

        [Fact]
        public void ResolveFromComponents()
        {
            var registrar = new ContainerRegistrar();
            registrar.RegisterComponents(Lifetime.Singleton, Assembly.GetExecutingAssembly());
            var builder = new ContainerBuilder();
            var container = builder.Build(registrar);

            container.Resolve<OneDepencency>();
        }

        [Fact]
        public void ResolveFromModules()
        {
            var registrar = new ContainerRegistrar();
            registrar.RegisterModules(Assembly.GetExecutingAssembly());
            var builder = new ContainerBuilder();
            var container = builder.Build(registrar);

            container.Resolve<MySelf>();
        }

        [Fact]
        public void ChildContainer()
        {
            var registrar = new ContainerRegistrar();
            registrar.RegisterType<MySelf>();
            var builder = new ContainerBuilder();
            var container = builder.Build(registrar);

            MySelf instance;
            using (var childContainer = container.CreateChildContainer())
            {
                instance = childContainer.Resolve<MySelf>();
            }

            Assert.True(instance.IsDisposed);
        }
    }
}
