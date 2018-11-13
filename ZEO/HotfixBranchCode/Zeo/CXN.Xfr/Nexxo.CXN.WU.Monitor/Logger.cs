﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Configuration;
using MGI.Common.Logging.Data;
using MGI.Common.Util;
using System.Reflection;

namespace MGI.Providers.WU.DASMonitor
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
            logContext.CommonFileName = "DASMonitor_" + DateTime.Now.ToString("MMddyyyy");
            logContext.LogFolderPath = System.Configuration.ConfigurationManager.AppSettings["LogPath"];
            return logContext;
        }
    }
}
