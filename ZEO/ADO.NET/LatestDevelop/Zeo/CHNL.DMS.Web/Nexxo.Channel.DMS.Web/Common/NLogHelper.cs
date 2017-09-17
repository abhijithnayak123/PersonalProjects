using TCF.Channel.Zeo.Web.Models;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using TCF.Zeo.Common.Logging.Impl;
using System;
using TCF.Zeo.Common.Logging.Data;

namespace TCF.Channel.Zeo.Web.Common
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
            NLogger.Info(Message);
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
            if (login.CustomerSession != null && login.CustomerSession.CustomerSessionId != 0)
            {
                logContext.CustomerSessionId = Convert.ToInt64(login.CustomerSession.CustomerSessionId);
            }
            else if (HttpContext.Current.Session["SessionId"] != null)
            {
                logContext.AgentSessionId = Convert.ToInt64(HttpContext.Current.Session["SessionId"].ToString());
            }
            logContext.ApplicationName = "Zeo";
            logContext.Version = Assembly.GetExecutingAssembly().GetName().Version.ToString(2);
            logContext.CommonFileName = "SSOAgentAthentication_" + DateTime.Now.ToString("yyyyMMddTHHmmss");
            logContext.LogFolderPath = System.Configuration.ConfigurationManager.AppSettings["LogPath"];

            return logContext;
        }
    }
}
