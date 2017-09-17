using TCF.Zeo.Common.Logging.Data;
using TCF.Zeo.Common.Logging.Impl;
using System;
using System.Reflection;

namespace TCF.Zeo.Common.Monitor
{
    public static class Logger
    {
        public static NLoggerCommon nLogger = new NLoggerCommon();
        public static void WriteLog(string exceptionMessage)
        {
            LogContext context = PrepareContext();
            nLogger.SetContext(context);

            nLogger.Error(exceptionMessage);
        }

		public static void WriteLogHeartBeat(string exceptionMessage)
        {
            LogContext context = PrepareContext();
            nLogger.SetContext(context);

            nLogger.Error(exceptionMessage);
		}

        public static LogContext PrepareContext()
        {
            LogContext logContext = new LogContext();

            logContext.ApplicationName = Assembly.GetExecutingAssembly().GetName().Name;
            logContext.Version = Assembly.GetExecutingAssembly().GetName().Version.ToString(2);
            logContext.CommonFileName = "CommonMonitor_" + DateTime.Now.ToString("MMddyyyy");
            logContext.LogFolderPath = System.Configuration.ConfigurationManager.AppSettings["LogPath"];
            return logContext;
        }
    }
}
