using TCF.Zeo.Common.Logging.Contract;
using TCF.Zeo.Common.Logging.Data;
using NLog;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace TCF.Zeo.Common.Logging.Impl
{
    public class NLoggerCommon
    {
        private ILogger _logger;

        public ILogger Logger;

        public NLoggerCommon()
        {
            _logger = new NLogLogger(NLog.LogManager.GetCurrentClassLogger());
        }            

        public void SetContext(LogContext context)
        {
            _logger.SetContext(context);
        }
        public void Info(string message)
        {
            _logger.Log(new LogEntry() { Level = LogEntryLevel.Info, Message = message });
        }

        public  void Info(string message, params object[] parameters)
        {
            _logger.Log(new LogEntry() { Level = LogEntryLevel.Info, Message = message, Parameters = parameters });
        }

        public void Info(string message,bool IsMask, params object[] parameters)
        {
          
            _logger.Log(new  LogEntry() { Level = LogEntryLevel.Info, Message = message, Parameters = parameters });
        }

        public void Info(string message, string category)
        {
            _logger.Log(new LogEntry() {Level =LogEntryLevel.Info,Message =message});
        }

         public void Error(Exception exception)
        {
            _logger.Log(new LogEntry() { Level = LogEntryLevel.Error, Message = exception.Message, Exception = exception });
        }

        public void Debug(string message, params object[] parameters)
         {
             _logger.Log(new LogEntry() { Level = LogEntryLevel.Debug, Message = message, Parameters = parameters });
         }

         public void Warn(string message)
         {
             _logger.Log(new LogEntry() { Level = LogEntryLevel.Warn, Message = message });
         }

         public void Error(string message)
         {
             _logger.Log(new LogEntry { Level = LogEntryLevel.Error, Message = message });
         }
         public void Error(string message, params object[] parameters)
         {
             _logger.Log(new LogEntry { Level = LogEntryLevel.Error, Message = message, Parameters = parameters });
         }

         public void WriteLogIf(bool condition, string message)
         {
             if (condition)
              _logger.Log(new LogEntry() { Level = LogEntryLevel.Info, Message = message });
         }
    }
}
