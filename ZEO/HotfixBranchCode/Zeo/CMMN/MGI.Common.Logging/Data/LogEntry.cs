using System;
using System.Collections.Generic;

namespace MGI.Common.Logging.Data
{
    public class LogEntry
    {
        public Exception Exception { get; set; }
        public IFormatProvider FormatProvider { get; set; }
        public LogEntryLevel Level { get; set; }
        public string Message { get; set; }
        public object[] Parameters { get; set; }
        public string LoggerName { get; set; }
        public DateTime TimeStamp { get; set; }
        public Dictionary<object, object> Properties { get; set; }
    }
}
