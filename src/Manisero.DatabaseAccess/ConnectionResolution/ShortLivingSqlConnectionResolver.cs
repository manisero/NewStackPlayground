using System;
using System.Data.Common;

namespace Manisero.DatabaseAccess.ConnectionResolution
{
    /// <summary>
    /// Should be used outside transaction, can be singleton.
    /// Returns new connection on each <see cref="Resolve"/> invocation.
    /// The connection is disposed on <see cref="ConnectionWrapper.Dispose"/>.
    /// </summary>
    internal class ShortLivingSqlConnectionResolver : IDbConnectionResolver
    {
        private class ConnectionWrapper : IDbConnectionWrapper
        {
            public ConnectionWrapper(
                DbConnection connection)
            {
                Connection = connection;
            }

            public DbConnection Connection { get; }

            public void Dispose() => Connection.Dispose();
        }

        private readonly Func<DbConnection> _connectionFactory;

        public ShortLivingSqlConnectionResolver(
            Func<DbConnection> connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public IDbConnectionWrapper Resolve()
        {
            var connection = _connectionFactory();

            return new ConnectionWrapper(connection);
        }
    }
}
