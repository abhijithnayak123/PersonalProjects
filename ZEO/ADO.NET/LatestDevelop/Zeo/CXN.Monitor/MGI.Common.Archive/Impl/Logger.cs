using System;
using System.Reflection;
using TCF.Zeo.Common.Logging.Data;
using TCF.Zeo.Common.Logging.Impl;

namespace MGI.Common.Archive
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

		public static void WriteDebug(string exceptionMessage)
		{
			LogContext context = PrepareContext();
			nLogger.SetContext(context);
			nLogger.Debug(exceptionMessage);
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
			logContext.CommonFileName = "ArchiveImages_" + DateTime.Today.ToString("MMddyyyy");
            logContext.LogFolderPath = System.Configuration.ConfigurationManager.AppSettings["LogPath"];

			return logContext;
		}
	}
}
