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

        public interface ISomeDude<T>
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
