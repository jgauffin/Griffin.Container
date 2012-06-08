using System;
using System.Collections.Generic;
using System.Linq;
using Castle.DynamicProxy;

namespace Griffin.Container.Interception
{
    /// <summary>
    /// Base class which purpose is to simplify the decoration
    /// </summary>
    /// <remarks>Inherit this class and implement the abstract methods</remarks>
    public abstract class CastleDecorator : IInstanceDecorator
    {
        private readonly IDecoratorFilter _filter;

        /// <summary>
        /// Initializes a new instance of the <see cref="CastleDecorator"/> class.
        /// </summary>
        protected CastleDecorator()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CastleDecorator"/> class.
        /// </summary>
        /// <param name="filter">Used to limit the types to decorate</param>
        protected CastleDecorator(IDecoratorFilter filter)
        {
            _filter = filter;
        }

        /// <summary>
        /// Allows the decorator to prescan all registered concretes
        /// </summary>
        /// <param name="concretes">All registered concretes</param>
        public abstract void PreScan(IEnumerable<Type> concretes);

        /// <summary>
        /// Create a new interceptor
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>Created interceptor (which will be used to handle the instance)</returns>
        protected abstract IInterceptor CreateInterceptor(DecoratorContext context);

        /// <summary>
        /// Determins if an instance should be decorated or not.
        /// </summary>
        /// <param name="context">Context</param>
        /// <returns><c>true</c> if we should attach a decorator; otherwise <c>false</c>.</returns>
        protected virtual bool CanDecorate(DecoratorContext context)
        {
            return _filter == null || _filter.CanDecorate(context);
        }

        /// <summary>
        /// Gets or sets if the class itself should not be used as a service
        /// </summary>
        /// <remarks>We do currently not support that a class can register itself
        /// as a service if interfaces also are registered (simply just register itself or just the interfaces)</remarks>
        public bool IgnoreClassAsService { get; set; }

        /// <summary>
        /// Decoration request
        /// </summary>
        /// <param name="context">Context info</param>
        /// <remarks>do not have to decorate, but may if it wants to. sorta..</remarks>
        public void Decorate(DecoratorContext context)
        {
            if (!CanDecorate(context))
                return;

            var options = new ProxyGenerationOptions();

            var services = context.Services;
            if (IgnoreClassAsService && services.Length > 1)
                services = services.Where(x => !x.IsClass).ToArray();

            var generator = new ProxyGenerator();
            if (services.Any(x => x.IsClass))
            {
                if (services.Length > 1)
                    throw new InvalidOperationException(
                        "A class that register itself as a service may not also be registered with interfaces. See the remarks in the IgnoreClassAsService property.");

                var clazz = context.Services.Single(x => x.IsClass);
                context.Instance = generator.CreateClassProxyWithTarget(clazz, context.Instance,
                                                                        CreateInterceptor(context));
            }
            else
            {
                var others = services.Where(x => x.IsInterface).Skip(1);
                var first = services.First();
                context.Instance = generator.CreateInterfaceProxyWithTarget
                    (first, others.ToArray(), context.Instance,
                     CreateInterceptor(context));
            }

        }
    }
}