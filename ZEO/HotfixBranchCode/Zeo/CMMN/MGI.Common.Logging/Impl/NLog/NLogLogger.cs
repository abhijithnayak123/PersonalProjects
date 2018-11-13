using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MGI.Common.Logging.Contract;
using MGI.Common.Logging.Data;

using NLog;
using System.Reflection;
using NLog.Targets;
using System.Threading;

namespace MGI.Common.Logging.Impl.NLog
{
    /// <summary>
    /// NLog specific implementation for ILogger
    /// Set Properties of the LogEventInfo to the context
    /// layout string for rendering:  
    ///	"${longdate}|${event-context:item=NAN}|${event-context:item=SessionId}|${event-context:item=AppName}|${event-context:item=Version}|${uppercase:${level}}|${message} ${exception:format=StackTrace:innerFormat=Message,StackTrace:maxInnerExceptionLevel=1}" />
    /// </summary>
    public class NLogLogger : ILogger
    {
        private Dictionary<object, object> _context = null;
        private Object o = new object();
        private Logger _logger;

        public NLogLogger(Logger logger)
        {
            _logger = logger;
        }

        public void Log(LogEntry entry)
        {
            lock(o)
            {
                _logger.Log(MapLogEntry(entry));
            }
        }

        public void SetContext(LogContext context)
        {
            if (_context == null)
                _context = new Dictionary<object, object>();

            AddToContext("AgentSessionId", context.AgentSessionId);
            AddToContext("CustomerSessionId", context.CustomerSessionId);
            AddToContext("FileName", context.FileName);
            AddToContext("ApplicationName", context.ApplicationName);
            AddToContext("Version", context.Version);
            AddToContext("LogFolderPath", context.LogFolderPath);
        }

        private void AddToContext(string name, object value)
        {
            if (_context == null)
                _context = new Dictionary<object, object>();

            if (_context.ContainsKey(name))
                _context[name] = value;
            else
                _context.Add(name, value);
        }

        public LogEventInfo MapLogEntry(LogEntry entry)
        {
            LogEventInfo eventInfo = new LogEventInfo
            {
                Exception = entry.Exception,
                FormatProvider = entry.FormatProvider,
                Level = LogLevel.FromOrdinal((int)entry.Level),
                LoggerName = entry.LoggerName,
                Message = entry.Message,
                Parameters = entry.Parameters,
                TimeStamp = entry.TimeStamp > DateTime.MinValue ? entry.TimeStamp : DateTime.Now,
            };

            //if (_context != null)
            //    foreach (KeyValuePair<object, object> kvp in _context)
            //        eventInfo.Properties.Add(kvp.Key, kvp.Value);

            LogContext logContext = Thread.CurrentPrincipal as LogContext;
            if (logContext != null)
            {
                eventInfo.Properties.Add("CustomerSessionId", logContext.CustomerSessionId);
                eventInfo.Properties.Add("AgentSessionId", logContext.AgentSessionId);
                eventInfo.Properties.Add("ApplicationName", logContext.ApplicationName);
                eventInfo.Properties.Add("Version", logContext.Version);
                eventInfo.Properties.Add("FileName", logContext.FileName);
                eventInfo.Properties.Add("LogFolderPath", logContext.LogFolderPath);
            }
            else
            {
                if (_context != null)
                    foreach (KeyValuePair<object, object> kvp in _context)
                        eventInfo.Properties.Add(kvp.Key, kvp.Value);
            }

            return eventInfo;
        }
    }
}
