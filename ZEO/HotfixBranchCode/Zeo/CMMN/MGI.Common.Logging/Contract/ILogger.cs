
using MGI.Common.Logging.Data;

namespace MGI.Common.Logging.Contract
{
    public interface ILogger
    {
        void Log(LogEntry entry);
		void SetContext(LogContext context);    }
}
