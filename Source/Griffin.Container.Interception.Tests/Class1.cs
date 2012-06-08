using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Griffin.Container.Interception.Tests
{
    public class Class1 : IExceptionLogger
    {
        private Exception _exception;

        [Fact]
        public void TestDecorateHiearchy()
        {
            // register services
            var registrar = new ContainerRegistrar();
            registrar.RegisterConcrete<Decorated1>(Lifetime.Transient);
            registrar.RegisterConcrete<Decorated2>(Lifetime.Transient);
            registrar.RegisterConcrete<Decorated3>(Lifetime.Transient);
            var container = registrar.Build();

            // only log transient services
            var filter = new DelegateDecoratorFilter(ctx => ctx.Lifetime == Lifetime.Transient);
            var decorator = new ExceptionLoggerDecorator(this, filter);
            container.AddDecorator(decorator);

            // exception will be logged.
            var all = container.ResolveAll<IShouldBeDecorated>();
            Assert.True(all.All(x => x.GetType().Name.Contains("Proxy")));
        }

        [Fact]
        public void TestLoggingDecorator()
        {
            // register services
            var registrar = new ContainerRegistrar();
            registrar.RegisterConcrete<TotalFailure>(Lifetime.Transient);
            var container = registrar.Build();

            // only log transient services
            var filter = new DelegateDecoratorFilter(ctx => ctx.Lifetime == Lifetime.Transient);
            var decorator = new ExceptionLoggerDecorator(this, filter);
            container.AddDecorator(decorator);

            // exception will be logged.
            var tmp = container.Resolve<TotalFailure>();
            Assert.Throws<InvalidOperationException>(() => tmp.Fail("Big!"));
            Assert.IsType<InvalidOperationException>(_exception);
        }

        public class TotalFailure
        {
            public virtual void Fail(string value)
            {
                throw new InvalidOperationException("Operation not allowed");
            }
        }

        public void LogException(CallContext context, Exception err)
        {
            _exception = err;
        }

        public interface IShouldBeDecorated
        {
             
        }

        public class Decorated1 : IShouldBeDecorated, IDecorated1
        {
            
        }
        public class Decorated2 : IShouldBeDecorated, IDecorated2
        {
            private readonly IDecorated1 _decorated1;

            public Decorated2(IDecorated1 decorated1)
            {
                _decorated1 = decorated1;
            }
            
        }
        public class Decorated3 : IShouldBeDecorated, IDecorated3
        {
            private readonly IDecorated2 _decorated2;

            public Decorated3(IDecorated2 decorated2)
            {
                _decorated2 = decorated2;
            }
        }

        public interface IDecorated1
        {
        }
        public interface IDecorated2
        {
        }
        public interface IDecorated3
        {
        }
        
    }

}
