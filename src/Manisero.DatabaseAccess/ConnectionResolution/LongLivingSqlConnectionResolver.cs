using System;
using System.Data.Common;

namespace Manisero.DatabaseAccess.ConnectionResolution
{
    /// <summary>
    /// Should be used inside transaction, new instance should be created for each transaction.
    /// Creates single connection and returns it on each <see cref="Resolve"/> invocation.
    /// Disposes the connection on <see cref="Dispose"/>.
    /// <see cref="ConnectionWrapper.Dispose"/> does nothing.
    /// </summary>
    internal class LongLivingSqlConnectionResolver : IDbConnectionResolver, IDisposable
    {
        private class ConnectionWrapper : IDbConnectionWrapper
        {
            public ConnectionWrapper(
                DbConnection connection)
            {
                Connection = connection;
            }

            public DbConnection Connection { get; }

            public void Dispose() { }
        }

        private readonly Lazy<DbConnection> _connection;

        public LongLivingSqlConnectionResolver(
            Func<DbConnection> connectionFactory)
        {
            _connection = new Lazy<DbConnection>(connectionFactory);
        }

        public IDbConnectionWrapper Resolve()
        {
            return new ConnectionWrapper(_connection.Value);
        }

        public void Dispose()
        {
            if (_connection.IsValueCreated)
            {
                _connection.Value.Dispose();
            }
        }
    }
}
