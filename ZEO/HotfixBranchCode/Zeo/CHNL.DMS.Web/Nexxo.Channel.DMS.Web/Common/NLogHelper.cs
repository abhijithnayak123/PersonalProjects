using MGI.Channel.DMS.Web.Models;
using MGI.Common.Logging.Data;
using MGI.Common.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace MGI.Channel.DMS.Web.Common
{
    public static class NLogHelper
    {
		static NLoggerCommon NLogger = new NLoggerCommon();

        internal static void Error(ExceptionContext exceptionContext)
        {
            Thread.CurrentPrincipal = PrepareLogInfo();
			NLogger.Error(string.Format("Exception details {0}", exceptionContext.Exception.ToString()));
        }

        internal static void Info(string Message)
        {
            Thread.CurrentPrincipal = PrepareLogInfo();
            NLogger.Info(string.Format("Message {0}", Message));
        }

        internal static void Error(Exception ex)
        {
            Thread.CurrentPrincipal = PrepareLogInfo();
            NLogger.Error(ex);
        }

        internal static void Error(string message, params object[] parameters)
        {
            Thread.CurrentPrincipal = PrepareLogInfo();
            NLogger.Error(message,parameters);
        }     

		internal static void Debug(string message, params object[] parameters)
        {
            Thread.CurrentPrincipal = PrepareLogInfo();
			NLogger.Debug(message, parameters);
		}

        private static LogContext PrepareLogInfo()
        {
            LogContext logContext = new LogContext();
            BaseModel login = new BaseModel();
            if (login.customerSession != null && !string.IsNullOrWhiteSpace(login.customerSession.CustomerSessionId))
            {
                logContext.CustomerSessionId = Convert.ToInt64(login.customerSession.CustomerSessionId);
            }
            else if (HttpContext.Current.Session["SessionId"] != null)
            {
                logContext.AgentSessionId = Convert.ToInt64(HttpContext.Current.Session["SessionId"].ToString());
            }
            logContext.ApplicationName = Assembly.GetExecutingAssembly().GetName().Name;
            logContext.Version = Assembly.GetExecutingAssembly().GetName().Version.ToString(2);
            logContext.CommonFileName = "SSOAgentAthentication_" + DateTime.Now.ToString("yyyyMMddTHHmmss");
            logContext.LogFolderPath = System.Configuration.ConfigurationManager.AppSettings["LogPath"];

            return logContext;
        }
    }
}
