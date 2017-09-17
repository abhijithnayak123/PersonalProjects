using MGI.Common.Logging.Contract;
using MGI.Common.Logging.Data;
using MGI.Common.Logging.Impl.NLog;
using System;

namespace MGI.Common.Util
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
