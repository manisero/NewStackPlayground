using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Manisero.CqrsGateway.CommandsHandling;
using Manisero.CqrsGateway.QueriesHandling;
using Manisero.DatabaseAccess;
using FluentValidation;
using NewStackPlayground.Application;
using NewStackPlayground.Gateway.Commands;
using NewStackPlayground.Gateway.Queries;
using Npgsql;
using SimpleInjector;

namespace NewStackPlayground.Gateway.DependencyInjection
{
    internal static class SimpleInjectorContainerBuilder
    {
        public static Container Build(
            string dbConnectionString,
            ScopedLifestyle scopeType)
        {
            var scoped = scopeType != null;

            var container = new Container();

            if (scoped)
            {
                container.Options.DefaultScopedLifestyle = scopeType;
            }

            RegisterTechnical(container, dbConnectionString, scoped);
            RegisterBusiness(container);

            container.Verify(); // TODO: Consider moving to tests
            return container;
        }

        private static void RegisterTechnical(
            Container container,
            string dbConnectionString,
            bool scoped)
        {
            RegisterDataAccess(container, dbConnectionString, scoped);
        }

        private static void RegisterDataAccess(
            Container container,
            string dbConnectionString,
            bool scoped)
        {
            if (scoped)
            {
                container.Register<IDatabaseAccessor>(
                    () => new InsideTransactionDatabaseAccessor(
                        () => new NpgsqlConnection(dbConnectionString)),
                    Lifestyle.Scoped);
            }
            else
            {
                container.Register<IDatabaseAccessor>(
                    () => new OutsideTransactionDatabaseAccessor(
                        () => new NpgsqlConnection(dbConnectionString)),
                    Lifestyle.Singleton);
            }
        }

        private static void RegisterBusiness(
            Container container)
        {
            // TODO: Implement automatic registration
            container.Register<IItemRepository, ItemRepository>();

            // Generics
            container.Register(typeof(ICommandHandler<,>), typeof(CreateItemCommand).Assembly);
            container.Register(typeof(IQueryHandler<,>), typeof(ItemQuery).Assembly);
            container.Register(typeof(IValidator<>), GetValidatorTypes(typeof(CreateItemCommand).Assembly));
        }

        private static IEnumerable<Type> GetValidatorTypes(
            params Assembly[] assemblies)
        {
            var validatorTypeCandidates = assemblies
                                          .SelectMany(x => x.ExportedTypes)
                                          .Where(x => x.IsClass && !x.IsAbstract);


            foreach (var type in validatorTypeCandidates)
            {
                if (type.GetInterfaces().Any(
                    x => x.IsConstructedGenericType &&
                         x.GetGenericTypeDefinition() == typeof(IValidator<>)))
                {
                    yield return type;
                }
            }
        }
    }
}
