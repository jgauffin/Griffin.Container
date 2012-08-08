using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using Griffin.Container.Commands;
using Xunit;

namespace Griffin.Container.Tests.Commands
{
    public class DecoratorTests
    {
        [Fact]
        public void Test()
        {
            var registrar = new ContainerRegistrar();
            registrar.RegisterConcrete<FailureHandler>(Lifetime.Transient);
            registrar.RegisterConcrete<ExceptionDecoratorFactory>(Lifetime.Singleton);
            registrar.RegisterService<ICommandDispatcher>(x => new ContainerDispatcher(x), Lifetime.Transient);
            var container = registrar.Build();
            var dispatcher = container.Resolve<ICommandDispatcher>();
            
            Assert.Throws<InvalidOperationException>(() => dispatcher.Dispatch(new FailureCommand { FirstName = "Arne" }));
        }

        public class PerformanceMonitor<T> : IHandlerOf<T> where T : class, ICommand
        {
            private readonly IHandlerOf<T> _inner;

            public PerformanceMonitor(IHandlerOf<T> inner)
            {
                _inner = inner;
            }

            public void Invoke(T command)
            {
                var w = new Stopwatch();
                w.Start();
                _inner.Invoke(command);
                w.Stop();
                Console.WriteLine("Invocation of {0} took {1}ms.", command.GetType().Name, w.ElapsedMilliseconds);
            }
        }
        public class FailureCommand : ICommand
        {
            public string FirstName { get; set; }
        }

        public class FailureHandler : IHandlerOf<FailureCommand>
        {
            public void Invoke(FailureCommand command)
            {
                throw new InvalidOperationException("That wont work, dude!");
                //Thread.Sleep(400);
                //throw new NotImplementedException();
            }
        }

        [Component]
        public class ExceptionDecoratorFactory : IDecoratorFactory, IExceptionLogger
        {
            public bool CanDecorate(Type commandType)
            {
                return true;
            }

            public IHandlerOf<T> Create<T>(IHandlerOf<T> inner) where T : class, ICommand
            {
                return new ExceptionDecorator<T>(this, new PerformanceMonitor<T>(inner));
            }

            public void Log(CommandExceptionInfo exceptionInfo)
            {
                Console.WriteLine(exceptionInfo);
            }
        }
    }
}
