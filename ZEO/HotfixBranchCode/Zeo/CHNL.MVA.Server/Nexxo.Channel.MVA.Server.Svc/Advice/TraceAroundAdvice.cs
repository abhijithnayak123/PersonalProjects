using System;
using System.Reflection;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text.RegularExpressions;

using AopAlliance.Intercept;
using MGI.Common.Sys;
using MGI.Common.Util;
using NLog.Config;
using NLog;
using NLog.Targets;
using MGI.Common.Logging.Data;
using System.Threading;


namespace MGI.Channel.MVA.Server.Svc.Advice
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TraceAroundAdvice : IMethodInterceptor
    {
		public List<string> detailedLogIgnoreList { private get; set; }
		public List<string> LogIgnoreList { private get; set; }
		public bool CardNumberLogging { private get; set; }
        public NLoggerCommon NLogger { get; set; }

        public TraceAroundAdvice()
        {
        }

        public object Invoke(IMethodInvocation invocation)
        {
            object returnValue = null;
			string methodName = invocation.Method.Name;

            try
            {
                Thread.CurrentPrincipal = PrepareLogInfo(invocation);
                if (!LogIgnoreList.Contains(methodName))
                {
					NLogger.Info(string.Format("Begin method call {0}", methodName));    
               
                    // Log the arguments passed to the method
                    if (!detailedLogIgnoreList.Contains(methodName))
                    {
                        NLogger.Info("Arguments:");
						NLogger.Info("\t");
						//Trace.Indent();

						if (invocation.Arguments != null)
						{
							// get the parameter names for the method
							ParameterInfo[] parameters = invocation.Method.GetParameters();

							// for each argument, log the parameter name and argument value
							for (int i = 0; i < invocation.Arguments.Length; i++)
							{
                                NLogger.Info(string.Format("{0}: ", parameters[i].Name));                               

								if (parameters[i].Name.ToLower() == "cardnumber")
                                    NLogger.Info(safeCardNumber(invocation.Arguments[i].ToString()));
								else
									traceObject(invocation.Arguments[i]);
							}
						}
						else
                            NLogger.Info("No arguments passed");

						//Trace.Unindent();
                    }
                }
                // Call the actual method
                returnValue = invocation.Proceed();

                if (!LogIgnoreList.Contains(methodName))
                {
                    NLogger.Info("Return value: ");
					NLogger.Info("\t");
					//Trace.Indent();
					traceObject(returnValue);
					//Trace.Unindent();
                }

                // Write "end" trace message
                NLogger.Info(string.Format("Completed method call {0}", methodName));
            }
			catch (FaultException<NexxoSOAPFault> fex)
			{
             throw fex;
			}
			catch (Exception ex)
			{
                NLogger.Error(ex);
				var nex = new NexxoSOAPFault
				{
					MajorCode = "0",
					MinorCode = "0",
					Processor = "MGiAlloy",
					Details = ex.Message,
					StackTrace = ex.StackTrace
				};
				throw new FaultException<NexxoSOAPFault>(nex);
			}
		
            return returnValue;
        }

     
		private string mapLogFilePath(string fileName)
		{
			fileName = string.Format(@"{0}{1}\{2}\{3}", ConfigurationManager.AppSettings["LogPath"], NexxoUtil.CreateYearMonthDayTree(), System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, fileName);

			if (!Regex.IsMatch(fileName, @"\.log$"))
				fileName = string.Format("{0} {1}.log", fileName, DateTime.Now.ToString("s").Replace("-", string.Empty).Replace(":", string.Empty));

			return fileName;
		}


        private void traceObject(object o)
        {											
            if (o != null)
            {
				Type t = o.GetType();

                if (t != typeof(string) && t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
                {
                    var enumerable = (System.Collections.IEnumerable)o;
                    foreach (var item in enumerable)
						traceObject(item);
                }
				else
					NLogger.Info(replaceCardNumber(o.ToString()));
            }
            else
                NLogger.Info("null");
        }

		private string safeCardNumber(string cardNumber)
		{
			if (cardNumber.Length == 16)
			{
				long card;
				if (long.TryParse(cardNumber, out card))
					return (CardNumberLogging ? cardNumber : ISOCard.EncodeCardNumber(card));
			}
				
			return cardNumber;
		}

		private string replaceCardNumber(string objString)
		{
			return Regex.Replace(objString, @"(cardnumber\s*[:=]\s*)(\d{16})", m => { return string.Format("{0}{1}", m.Groups[1].Value, safeCardNumber(m.Groups[2].Value)); }, RegexOptions.IgnoreCase);
		}

		private string getFileName(IMethodInvocation invocation)
		{
			MethodInfo method = invocation.Method;
			string fileName = string.Empty;

			ParameterInfo[] parameters = method.GetParameters();

			// first look for customerSessionId
			for (int i = 0; i < parameters.Length; i++)
			{
				if (parameters[i].Name.ToLower() == "customersessionid")
					return string.Format("CustomerSession.{0}.log", invocation.Arguments[i]);
			}

			// first look for customerSessionId
			for (int i = 0; i < parameters.Length; i++)
			{
				if (parameters[i].Name.ToLower() == "sessionid" || parameters[i].Name.ToLower() == "agentsessionid")
					return string.Format("AgentSession.{0}.log", invocation.Arguments[i]);			}

			return method.Name;
		}

        private LogContext PrepareLogInfo(IMethodInvocation invocation)
        {
            LogContext logContext = new LogContext();
            MethodInfo method = invocation.Method;
            ParameterInfo[] parameters = method.GetParameters();

            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i].Name.ToLower() == "customersessionid")
                    logContext.CustomerSessionId = Convert.ToInt64(invocation.Arguments[i]);
            }
            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i].Name.ToLower() == "sessionid" || parameters[i].Name.ToLower() == "agentsessionid")
                    logContext.AgentSessionId = Convert.ToInt64(invocation.Arguments[i]);
            }
            logContext.ApplicationName = Assembly.GetExecutingAssembly().GetName().Name;
            logContext.Version = Assembly.GetExecutingAssembly().GetName().Version.ToString(2);
            logContext.CommonFileName = invocation.Method.Name;
            logContext.LogFolderPath = System.Configuration.ConfigurationManager.AppSettings["LogPath"];

            return logContext;
        }
    }
}
