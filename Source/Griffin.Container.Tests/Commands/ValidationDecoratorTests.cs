using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Griffin.Container.Commands;
using Xunit;

namespace Griffin.Container.Tests.Commands
{
    public class ValidationDecoratorTests
    {
        [Fact]
        public void Test()
        {
            var reg = new ContainerRegistrar(Lifetime.Transient);
            reg.RegisterConcrete<ValidateFactory>();
            reg.RegisterConcrete<SomeHandler>();
            reg.RegisterService<ICommandDispatcher>(f => new ContainerDispatcher(f));
            var container = reg.Build();

            Assert.Throws<CommandValidationFailedException>(() => container.Resolve<ICommandDispatcher>().Dispatch(new ValidateCommand()));
        }

        public class SomeHandler : IHandlerOf<ValidateCommand>
        {
            public void Invoke(ValidateCommand command)
            {

            }
        }

        public class ValidateFactory : IDecoratorFactory
        {
            public bool CanDecorate(Type commandType)
            {
                return true;
            }

            public IHandlerOf<T> Create<T>(IHandlerOf<T> inner) where T : class, ICommand
            {
                return new ValidationDecorator<T>(inner);
            }
        }

        public class ValidateCommand : ICommand
        {
            [Required]
            public string FirstName { get; set; }
        }
    }
}
