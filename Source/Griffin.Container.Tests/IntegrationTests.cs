using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Griffin.Container.Tests.Subjects;
using NSubstitute;
using Xunit;

namespace Griffin.Container.Tests
{


    public class IntegrationTests
    {
        [Fact]
        public void SimpleRegistrationTransient()
        {
            var registrar = new ContainerRegistrar();
            registrar.RegisterConcrete<MySelf>(Lifetime.Transient);
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
            registrar.RegisterConcrete<MySelf>(Lifetime.Transient);
            registrar.RegisterConcrete<OneDepencency>(Lifetime.Singleton);
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
            registrar.RegisterConcrete<Startable1>(Lifetime.Singleton);
            registrar.RegisterConcrete<Startable2>(Lifetime.Singleton);
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
            registrar.RegisterConcrete<MySelf>();
            var builder = new ContainerBuilder();
            var container = builder.Build(registrar);

            MySelf instance;
            using (var childContainer = container.CreateChildContainer())
            {
                instance = childContainer.Resolve<MySelf>();
            }

            Assert.True(instance.IsDisposed);
        }

        [Fact]
        public void TwoChildContainers()
        {
            var registrar = new ContainerRegistrar();
            registrar.RegisterConcrete<MySelf>();
            var builder = new ContainerBuilder();
            var container = builder.Build(registrar);

            MySelf instance1, instance2;
            var childContainer1 = container.CreateChildContainer();
            var childContainer2 = container.CreateChildContainer();
            instance1 = childContainer1.Resolve<MySelf>();
            instance2 = childContainer2.Resolve<MySelf>();
            Assert.False(instance1.IsDisposed);
            Assert.False(instance2.IsDisposed);

            childContainer1.Dispose();

            Assert.True(instance1.IsDisposed);
            Assert.False(instance2.IsDisposed);
            
            childContainer2.Dispose();

            Assert.True(instance2.IsDisposed);
        }

        [Fact]
        public void TestDelegateFactory()
        {
            var registrar = new ContainerRegistrar();
            registrar.RegisterService<MySelf>(ctnr => new MySelf(), Lifetime.Transient);
            registrar.RegisterConcrete<OneDepencency>(Lifetime.Singleton);
            var container = registrar.Build();

            container.Resolve<OneDepencency>();
        }

        [Fact]
        public void TestSingleton()
        {
            var registrar = new ContainerRegistrar();
            registrar.RegisterInstance<MySelf>(new MySelf());
            registrar.RegisterConcrete<OneDepencency>(Lifetime.Singleton);
            var container = registrar.Build();

            container.Resolve<OneDepencency>();
        }
    }
}
