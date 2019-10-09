using System;
using System.Data.Common;

namespace Manisero.DatabaseAccess.ConnectionResolution
{
    internal interface IDbConnectionResolver
    {
        IDbConnectionWrapper Resolve();
    }

    internal interface IDbConnectionWrapper : IDisposable
    {
        DbConnection Connection { get; }
    }
}
