using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MGI.Common.TransactionalLogging.Data;

namespace MGI.Common.TransactionalLogging.Contract.Extensions
{
    public static class TLoggerExtensions
    {
        public static void Log(this ITLogger logger, string message)
        {
            Log(logger, message, null);
        }

        public static void Log(this ITLogger logger, string message, params object[] parameters)
        {
            logger.Log(new TLogEntry { Level = TLogEntryLevel.INFO, Message = message, Parameters = parameters });
        }

        public static void Log(this ITLogger logger, Exception ex)
        {
            logger.Log(new TLogEntry { Level = TLogEntryLevel.ERROR, Message = ex.Message, Exception = ex });
        }

        public static void Log(this ITLogger logger, Exception ex, string message)
        {
            logger.Log(new TLogEntry { Level = LogEntryLevel.Error, Message = message, Exception = ex });
        }

        public static void Debug(this ITLogger logger, string message, params object[] parameters)
        {
            logger.Log(new TLogEntry { Level = LogEntryLevel.Debug, Message = message, Parameters = parameters });
        }

        public static void Warn(this ITLogger logger, string message)
        {
            logger.Log(new TLogEntry { Level = LogEntryLevel.Warn, Message = message });
        }

        public static void Error(this ITLogger logger, string message, params object[] parameters)
        {
            logger.Log(new TLogEntry { Level = LogEntryLevel.Error, Message = message, Parameters = parameters });
        }
    }
}
