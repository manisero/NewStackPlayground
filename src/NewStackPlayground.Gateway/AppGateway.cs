using System;
using Manisero.CqrsGateway;
using Manisero.CqrsGateway.CommandsHandling;
using Manisero.CqrsGateway.QueriesHandling;
using Manisero.CqrsGateway.Validation;
using FluentValidation;
using NewStackPlayground.Gateway.DependencyInjection;
using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace NewStackPlayground.Gateway
{
    public class AppGateway : IDisposable
    {
        private class ServiceResolver : IValidatorResolver, ICommandHandlerResolver, IQueryHandlerResolver
        {
            private readonly IDependencyResolver _dependencyResolver;

            public ServiceResolver(
                IDependencyResolver dependencyResolver)
            {
                _dependencyResolver = dependencyResolver;
            }

            public IValidator<TItem> Resolve<TItem>()
                => _dependencyResolver.Resolve<IValidator<TItem>>(true);

            ICommandHandler<TCommand, TResult> ICommandHandlerResolver.Resolve<TCommand, TResult>()
                => _dependencyResolver.Resolve<ICommandHandler<TCommand, TResult>>();

            IQueryHandler<TQuery, TResult> IQueryHandlerResolver.Resolve<TQuery, TResult>()
                => _dependencyResolver.Resolve<IQueryHandler<TQuery, TResult>>();
        }

        private readonly Container _commandsDiContainer;
        private readonly Container _queriesDiContainer;

        public ICommandsGateway CommandsGateway { get; }
        public IQueriesGateway QueriesGateway { get; }

        public AppGateway(
            string dbConnectionString)
        {
            _commandsDiContainer = SimpleInjectorContainerBuilder.Build(dbConnectionString, new AsyncScopedLifestyle());
            var commandsServiceResolver = new ServiceResolver(new SimpleInjectorDependencyResolver(_commandsDiContainer));

            CommandsGateway = new CommandsGateway(
                commandsServiceResolver,
                () => AsyncScopedLifestyle.BeginScope(_commandsDiContainer),
                commandsServiceResolver);

            _queriesDiContainer = SimpleInjectorContainerBuilder.Build(dbConnectionString, new AsyncScopedLifestyle());
            var queriesServiceResolver = new ServiceResolver(new SimpleInjectorDependencyResolver(_queriesDiContainer));

            QueriesGateway = new QueriesGateway(
                queriesServiceResolver,
                () => AsyncScopedLifestyle.BeginScope(_queriesDiContainer));
        }

        public void Dispose()
        {
            _commandsDiContainer.Dispose();
            _queriesDiContainer.Dispose();
        }
    }
}
