using MGI.Common.Logging.Data;
using MGI.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MGI.Biz.CPEngine.Monitor
{
	class Logger
	{
		public static NLoggerCommon nLogger = new NLoggerCommon();

		public static void WriteLog(string exceptionMessage)
		{
			LogContext context = PrepareContext();
			nLogger.SetContext(context);
			nLogger.Info(exceptionMessage);
		}

		public static void WriteLogError(string exceptionMessage)
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
			logContext.CommonFileName = "CPEngineMonitor_" + DateTime.Today.ToString("MMddyyyy");
            logContext.LogFolderPath = System.Configuration.ConfigurationManager.AppSettings["LogPath"];

			return logContext;
		}
	}
}
