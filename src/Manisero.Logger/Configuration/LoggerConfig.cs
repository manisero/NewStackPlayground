using System.Collections.Generic;
using System.Reflection;

namespace Manisero.Logger.Configuration
{
    public class LoggerConfig
    {
        public class DbConfig
        {
            public string ConnectionString { get; set; }

            public ICollection<Assembly> AssembliesWithLogs { get; set; }
        }

        public class SeqConfig
        {
            public int Port { get; set; }
        }

        public DbConfig Db { get; set; }

        public SeqConfig Seq { get; set; }
    }
}
