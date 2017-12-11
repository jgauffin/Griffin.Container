using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Griffin.Container.Tests
{
    public class GenericTests
    {
        [Fact]
        public void InterfaceToConcrete()
        {
            var registrar = new ContainerRegistrar();
            registrar.RegisterType(typeof(ISomeDude<>), typeof(SomeDude<>), Lifetime.Transient);
            registrar.RegisterConcrete<Word>(Lifetime.Transient);
            var container = registrar.Build();

            var actual = container.Resolve<ISomeDude<string>>();

            Assert.NotNull(actual);
        }

        [Fact]
        public void Should_be_able_To_resolve_open_generics_in_singleton_scope()
        {
            var registrar = new ContainerRegistrar();
            registrar.RegisterType(typeof(ISomeDude<>), typeof(SomeDude<>), Lifetime.Singleton);
            registrar.RegisterConcrete<Word>(Lifetime.Transient);
            var container = registrar.Build();

            var actual = container.Resolve<ISomeDude<string>>();

            Assert.NotNull(actual);
        }

        [Fact]
        public void Should_be_able_To_resolve_open_generics_which_are_type_restricted()
        {
            var registrar = new ContainerRegistrar();
            registrar.RegisterType(typeof(IRestrictedGeneric<>), typeof(RestrictedGeneric<>), Lifetime.Singleton);
            registrar.RegisterConcrete<Word>(Lifetime.Transient);
            var container = registrar.Build();

            var actual = container.Resolve<IRestrictedGeneric<SomeClass>>();

            Assert.NotNull(actual);
        }

        [Fact]
        public void Should_be_able_To_take_an_open_generic_as_a_dependency()
        {
            var t = typeof(ISomeDude<>);
            var t2 = typeof(ISomeDude<string>);

            var registrar = new ContainerRegistrar();
            registrar.RegisterType(typeof(IRestrictedGeneric<>), typeof(RestrictedGeneric<>), Lifetime.Singleton);
            registrar.RegisterConcrete<Word>(Lifetime.Transient);
            registrar.RegisterConcrete<SomeProcessor>(Lifetime.Singleton);
            var container = registrar.Build();

            var actual = container.Resolve<SomeProcessor>();

            Assert.NotNull(actual);
        }


        [Fact]
        public void Should_be_able_To_take_an_specified_generic_as_a_dependency()
        {
            var t = typeof(ISomeDude<>);
            var t2 = typeof(ISomeDude<string>);

            var registrar = new ContainerRegistrar();
            registrar.RegisterType(typeof(IRestrictedGeneric<SomeClass>), typeof(RestrictedGeneric<SomeClass>), Lifetime.Singleton);
            registrar.RegisterConcrete<Word>(Lifetime.Transient);
            registrar.RegisterConcrete<SomeProcessor>(Lifetime.Singleton);
            var container = registrar.Build();

            var actual = container.Resolve<SomeProcessor>();

            Assert.NotNull(actual);
        }

        [Fact]
        public void RegisteredSpecific()
        {
            var registrar = new ContainerRegistrar();
            registrar.RegisterType(typeof(ISomeDude<>), typeof(SomeDude<>), Lifetime.Transient);
            registrar.RegisterType(typeof(ISomeDude<int>), typeof(MyClass), Lifetime.Transient);
            registrar.RegisterConcrete<Word>(Lifetime.Transient);
            var container = registrar.Build();

            var actual = container.Resolve<ISomeDude<int>>();

            Assert.IsType<MyClass>(actual);
        }

        public class SomeProcessor
        {
            public SomeProcessor(IRestrictedGeneric<SomeClass> settings)
            {

            }
        }

        public class SomeClass : ISomeInterface
        {

        }

        public interface ISomeDude<T>
        {

        }

        public interface IRestrictedGeneric<in T> where T : ISomeInterface, new()
        {
            void Procest(T item);
        }

        public class RestrictedGeneric<T> : IRestrictedGeneric<T> where T : ISomeInterface, new()
        {
            public void Procest(T item)
            {

            }
        }

        public interface ISomeInterface
        {

        }
        public class Word
        {

        }
        public class SomeDude<T> : ISomeDude<T>
        {
            public SomeDude(Word w)
            {

            }
        }

        public class MyClass : ISomeDude<int>
        {

        }
    }
}
