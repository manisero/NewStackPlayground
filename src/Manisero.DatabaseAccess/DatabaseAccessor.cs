using System;
using System.Data;
using System.Data.Common;
using Manisero.DatabaseAccess.ConnectionResolution;

namespace Manisero.DatabaseAccess
{
    public interface IDatabaseAccessor
    {
        void Access(
            Action<DbConnection> access);

        TOutput Access<TOutput>(
            Func<DbConnection, TOutput> access);
    }

    public abstract class DatabaseAccessor : IDatabaseAccessor
    {
        internal readonly IDbConnectionResolver DbConnectionResolver;

        internal DatabaseAccessor(
            IDbConnectionResolver dbConnectionResolver)
        {
            DbConnectionResolver = dbConnectionResolver;
        }

        public void Access(
            Action<DbConnection> access)
        {
            Access(x =>
            {
                access(x);
                return (object)null;
            });
        }

        public TOutput Access<TOutput>(
            Func<DbConnection, TOutput> access)
        {
            using (var wrapper = DbConnectionResolver.Resolve())
            {
                var connection = wrapper.Connection;

                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                return access(connection);
            }
        }
    }

    /// <summary>
    /// Should be used inside transaction, new instance should be created for each transaction.
    /// </summary>
    public class InsideTransactionDatabaseAccessor : DatabaseAccessor, IDisposable
    {
        public InsideTransactionDatabaseAccessor(
            Func<DbConnection> connectionFactory)
            : base(new LongLivingSqlConnectionResolver(connectionFactory))
        {
        }

        public void Dispose()
        {
            ((IDisposable)DbConnectionResolver).Dispose();
        }
    }

    /// <summary>
    /// Should be used outside transaction, can be singleton.
    /// </summary>
    public class OutsideTransactionDatabaseAccessor : DatabaseAccessor
    {
        public OutsideTransactionDatabaseAccessor(
            Func<DbConnection> connectionFactory)
            : base(new ShortLivingSqlConnectionResolver(connectionFactory))
        {
        }
    }
}
