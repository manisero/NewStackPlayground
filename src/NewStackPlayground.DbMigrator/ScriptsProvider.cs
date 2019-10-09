using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DbUp;
using DbUp.Engine;
using DbUp.Engine.Transactions;
using DbUp.ScriptProviders;

namespace NewStackPlayground.DbMigrator
{
    public class ScriptsProvider : IScriptProvider
    {
        private readonly EmbeddedScriptsProvider _embeddedScriptProvider;

        public ScriptsProvider(
            params Assembly[] scriptsAssemblies)
        {
            _embeddedScriptProvider = new EmbeddedScriptsProvider(
                scriptsAssemblies,
                x => x.EndsWith(".sql"),
                DbUpDefaults.DefaultEncoding);
        }

        public IEnumerable<SqlScript> GetScripts(IConnectionManager connectionManager)
        {
            return _embeddedScriptProvider
                   .GetScripts(connectionManager)
                   .Select(x => new SqlScript(
                               x.Name.Split('.').Reverse().ElementAt(1),
                               x.Contents));
        }
    }
}
