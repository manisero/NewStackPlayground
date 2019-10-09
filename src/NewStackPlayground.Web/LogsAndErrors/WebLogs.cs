using Manisero.Logger;

namespace NewStackPlayground.Web.LogsAndErrors
{
    [Log(
        "043F099B-49D7-46CA-86AB-03696D2A4F24",
        "Web app started.")]
    public struct WebStartedLog : ILog
    {
        public void Log(Serilog.ILogger logger) => logger.Log(this);
    }

    [Log(
        "CD08F8D4-0B64-44E4-B715-9AC023865563",
        "Web app stopped.")]
    public struct WebStoppedLog : ILog
    {
        public void Log(Serilog.ILogger logger) => logger.Log(this);
    }
}
