using TCF.Zeo.Common.Logging.Data;

namespace TCF.Zeo.Common.Logging.Contract
{
    public interface ILogger
    {
        void Log(LogEntry entry);
		void SetContext(LogContext context);    }
}
