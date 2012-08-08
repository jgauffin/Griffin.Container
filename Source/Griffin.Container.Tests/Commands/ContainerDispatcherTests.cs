using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Griffin.Container.Commands;
using NSubstitute;
using Xunit;

namespace Griffin.Container.Tests.Commands
{
    public class ContainerDispatcherTests
    {
        [Fact]
        public void InvokeExistingHandler()
        {
            var resolver = Substitute.For<IServiceLocator>();
            resolver.Resolve(typeof(IHandlerOf<Cmd>)).Returns(x => new Handler());
            var dispatcher = new ContainerDispatcher(resolver);

            var actual = new Cmd();
            dispatcher.Dispatch(actual);
            
            Assert.True(actual.IsInvoked);
        }

        [Fact]
        public void InvokeExistingHandler2()
        {
            var resolver = Substitute.For<IServiceLocator>();
            resolver.Resolve(typeof(IHandlerOf<Cmd>)).Returns(x => new Handler());
            var dispatcher = new ContainerDispatcher(resolver);

            var actual = new Cmd();
            dispatcher.Dispatch(actual);

            Assert.True(actual.IsInvoked);
        }

        public class Cmd : ICommand
        {
            public bool IsInvoked { get; set; }
        }
        public class Handler : IHandlerOf<Cmd>
        {
            public void Invoke(Cmd command)
            {
                command.IsInvoked = true;
            }
        }
    }
}
