using System;
using System.Reflection;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using AopAlliance.Intercept;

using System.Threading;
using TCF.Zeo.Common.Logging.Impl;
using TCF.Zeo.Common.Data;
using TCF.Zeo.Common.Logging.Data;
using TCF.Zeo.Common.Util;
using System.Text;
using System.Collections;
using TCF.Channel.Zeo.Data;

namespace TCF.Channel.Zeo.Service.Svc.Advice
{
    public class TraceAroundAdvice : IMethodInterceptor
    {
        public NLoggerCommon NLogger { get; set; }
        public List<string> IgnorePropertyList { get; set; }
        public List<string> PCIList { get; set; }
        public List<string> LogIgnoreMethodList { get; set; }
        public TraceAroundAdvice()
        {
            NLogger = new NLoggerCommon();
        }

        public object Invoke(IMethodInvocation invocation)
        {
            object returnValue = null;
            string methodName = invocation.Method.Name;
            ParameterInfo[] parameters = null;
            try
            {
                Thread.CurrentPrincipal = PrepareLogInfo(invocation);

                if (!LogIgnoreMethodList.Contains(methodName))
                {
                    NLogger.Info(string.Format("Begin method call {0}.", methodName));

                    NLogger.Info("Arguments:");

                    if (invocation.Arguments != null)
                    {
                        parameters = invocation.Method.GetParameters();

                        for (int i = 0; i < invocation.Arguments.Length; i++)
                        {
                            StringBuilder sb = new StringBuilder();
                            TraceObject(parameters[i].Name, invocation.Arguments[i], sb);
                            NLogger.Info(sb.ToString());
                        }
                    }
                    else
                        NLogger.Info("No arguments passed");
                }

                returnValue = invocation.Proceed();
                
            }
            catch (Exception ex)
            {
                //if there is an exception the method need to be logged irrespective of whether it is configured to log or not.
                //Else we will not know, if the method not configured for logging fails for some reason.
                if (LogIgnoreMethodList.Contains(methodName))
                {
                    NLogger.Info(string.Format("{0} is not configured for logging all the details. There may not be a begin method statement for this.", methodName));
                }
                NLogger.Error(string.Format("Error completing method call {0}.", methodName));
                //TODO : How do we get this without hardcoding?
                Data.ZeoContext context = new Data.ZeoContext() { ChannelPartnerId = 34 };
                //TODO : What happens if alloycontext is null?
                if (parameters != null)
                {
                    int index = parameters.ToList().FindIndex(x => x.ParameterType.Name == "ZeoContext");
                    context = invocation.Arguments[index] as Data.ZeoContext;
                }
                Error error = PrepareError(ex, context);
                returnValue = new Response() { Error = error };
            }
            if (!LogIgnoreMethodList.Contains(methodName))
            {
                NLogger.Info("Response:");
                StringBuilder sb = new StringBuilder();
                TraceObject("Return Value", returnValue, sb);
                NLogger.Info(sb.ToString());

                NLogger.Info(string.Format("Completed method call {0}.\n", methodName));
            }
            return returnValue;
        }

        private StringBuilder TraceObject(string name, object o, StringBuilder sb)
        {
            if (IgnorePropertyList.Contains(name.ToLower()))
            {
                sb.AppendLine(string.Format("{0} : {1}", name, "IGNORED"));
                return sb;
            }
            if (o == null)
            {
                sb.AppendLine(string.Format("{0} : {1}", name, "NULL"));
                return sb;
            }
            if (PCIList.Contains(name.ToLower()))
            {
                o = MaskPCIData(name, o.ToString());
            }
            else if (o.GetType() == typeof(Data.Response))
            {
                PropertyInfo[] properties = o.GetType().GetProperties();
                foreach (var prop in properties)
                {
                    TraceObject(prop.Name, prop.GetValue(o), sb);
                }
            }
            else if (o.GetType() == typeof(string))
            {
                sb.AppendLine(string.Format("{0} : {1}", name, o.ToString()));
            }
            else if (o.GetType().Name.Contains("Dictionary"))
            {
                var list = (IDictionary<string, object>)o;
                foreach (var item in list)
                {
                    sb.AppendLine(string.Format("{0} : {1}", item.Key, item.Value));
                }
            }
            else if (o.GetType().IsEnumerable())
            {
                var list = (IList)o;
                int i = 0;
                foreach (var item in list)
                {
                    sb.AppendLine(string.Format("{0} : {1}", item.GetType().Name, i++));
                    TraceObject(item.GetType().Name, item, sb);
                }
            }
            else if (o.GetType().BaseType.Name == "Object" || o.GetType().BaseType.Name == "BaseRequest")
            {
                PropertyInfo[] properties = o.GetType().GetProperties();
                foreach (var prop in properties)
                {
                    TraceObject(prop.Name, prop.GetValue(o), sb);
                }
            }
            else
            {
                sb.AppendLine(string.Format("{0} : {1}", name, o.ToString()));
            }
            return sb;
        }

        public string MaskPCIData(string property, string value)
        {
            switch (property.ToLower())
            {
                case "ssn":
                    if (string.IsNullOrEmpty(value) || value.Length < 4)
                    {
                        return "INVALID SSN";
                    }
                    else
                        return "***-**-" + value.Substring(value.Length - 4, 4);

                case "governmentidnumber":
                case "governmentid":
                    if (string.IsNullOrEmpty(value) || value.Length < 4)
                    {
                        return "INVALID GOVERNMENT ID NUMBER";
                    }
                    else
                        return "****" + value.Substring(value.Length - 4, 4);
                case "alloyid":
                case "partneraccountnumber":
                case "addoncustomerid":
                    if (string.IsNullOrEmpty(value) || value.Length < 16)
                    {
                        return "INVALID ALLOY ID";
                    }
                    else
                        return value.Substring(0, 6) + "******" + value.Substring(value.Length - 4, 4);
                case "cardnumber":
                case "accountnumber":
                    return "";
                case "pin":
                    if (string.IsNullOrEmpty(value))
                    {
                        return "INVALID PIN";
                    }
                    else
                        return "****";
                default:
                    return value;
            }
        }

        private LogContext PrepareLogInfo(IMethodInvocation invocation)
        {
            LogContext logContext = new LogContext();
            MethodInfo method = invocation.Method;
            ParameterInfo[] parameters = method.GetParameters();

            for (int i = 0; i < parameters.Length; i++)
            {
                string parameterName = parameters[i].Name.ToLower();
                switch (parameterName)
                {
                    case "alloycontext":
                    //TODO: AO - remove context after all the places
                    case "context":
                        Data.ZeoContext context = invocation.Arguments[i] as Data.ZeoContext;
                        logContext.CustomerSessionId = context.CustomerSessionId;
                        logContext.AgentSessionId = context.AgentSessionId;
                        break;
                    case "agentsessionid":
                        logContext.AgentSessionId = Convert.ToInt64(invocation.Arguments[i]);
                        break;
                    case "customersessionid":
                        logContext.CustomerSessionId = Convert.ToInt64(invocation.Arguments[i]);
                        break;
                    default:
                        break;
                }
            }
            logContext.ApplicationName = "Zeo";
            logContext.Version = Assembly.GetExecutingAssembly().GetName().Version.ToString(2);
            logContext.CommonFileName = invocation.Method.Name;
            logContext.LogFolderPath = ConfigurationManager.AppSettings["LogPath"];
            return logContext;
        }

        internal Error PrepareError(Exception ex, Data.ZeoContext context)
        {
            Contract.IMessageStoreService _msgStore = new Impl.ZeoServiceImpl();
            try
            {
                Exception innerException = null;
                string errorCode = string.Empty;
                string productCode = string.Empty;
                string providerCode = string.Empty;

                ProviderException providerException = ex as ProviderException;
                if (providerException != null)
                {
                    errorCode = providerException.ProviderErrorCode;
                    productCode = providerException.ProductCode;
                    providerCode = providerException.ProviderCode;
                }

                ZeoException alloyException = ex as ZeoException;
                if (alloyException != null)
                {
                    errorCode = alloyException.ZeoErrorCode;
                    productCode = alloyException.ProductCode;
                    providerCode = alloyException.ProviderCode;
                }

                MessageStoreSearch search = new MessageStoreSearch()
                {
                    ErrorCode = errorCode,
                    Language = Helper.Language.English,
                    ProductCode = productCode,
                    ProviderCode = providerCode
                };

                Response response = _msgStore.LookUp(search, context);
                Message messageStore = response.Result as Message;

                var limitException = ex as TCF.Zeo.Biz.Common.Data.Exceptions.BizComplianceLimitException;

                if (limitException != null)
                {
                    messageStore.Content = string.Format(messageStore.Content, limitException.LimitValue.ToString("c"));
                }

                string exceptionKey = string.Format("{0}.{1}.{2}", productCode, providerCode, errorCode);
                string errorType = ((Helper.ErrorType)messageStore.ErrorType).ToString();

                StringBuilder errmsg = new StringBuilder().AppendLine(string.Format("CAUGHT ALLOY ERROR. \nERROR CODE : {0} \nERROR MESSAGE : {1} \nERROR ADDLDETAILS : {2}", exceptionKey, messageStore.Content, messageStore.AddlDetails));

                ////GetActualException(ex, ref innerException);
                ////if (innerException != null && (innerException as ZeoException) == null)
                ////    errmsg.AppendLine(string.Format("ERROR LOG : {0}", innerException.ToString()));

                // Log the Error message and stack trace.
                PopulateErrorInfo(ex, ref errmsg);

                string errorMessage = (messageStore != null && !string.IsNullOrWhiteSpace(messageStore.Content)) ? messageStore.Content : ex.Message;

                errorMessage = (!string.IsNullOrEmpty(ex.Message) && limitException == null) ? ex.Message : errorMessage;

                Error error = new Error()
                {
                    MajorCode = productCode.ToString(),
                    MinorCode = errorCode,
                    Details = string.Join("|", new object[] { messageStore.Processor, exceptionKey, errorMessage, messageStore.AddlDetails, errorType }),
                    Exception = errmsg.ToString()
                };
                return error;
            }
            catch (Exception exec)
            {
                MessageStoreSearch search = new MessageStoreSearch()
                {
                    ErrorCode = "9999",
                    Language = Helper.Language.English,
                    ProductCode = ((int)Helper.ProductCode.Alloy).ToString(),
                    ProviderCode = ((int)Helper.ProviderId.Alloy).ToString()
                };

                Response response = _msgStore.LookUp(search, context);
                Message messageStore = response.Result as Message;

                Error error = new Error()
                {
                    Details = string.Join("|", new object[] { messageStore.Processor, messageStore.MessageKey, messageStore.Content, messageStore.AddlDetails, messageStore.ErrorType }),
                    Exception = exec.ToString()
                };
                return error;
            }
        }

        private void GetActualException(Exception exception, ref Exception actualException)
        {
            actualException = exception;
            while (actualException.InnerException != null)
            {
                actualException = actualException.InnerException;
            }
        }

        private void PopulateErrorInfo(Exception ex, ref StringBuilder messageBuilder)
        {
            int counter = 1;

            int exceptionDrillDownLimit = 5;

            while (ex != null && counter <= exceptionDrillDownLimit)
            {
                messageBuilder.AppendFormat("\n Level {0}: {1} \n", counter, !string.IsNullOrWhiteSpace(ex.Message) ? ex.Message : "No exception message available");
                messageBuilder.AppendFormat("\n Stack Trace Level {0}: {1} \n", counter, !string.IsNullOrWhiteSpace(ex.StackTrace) ? ex.StackTrace : "No stack trace available");
                ex = ex.InnerException;
                counter++;
            }

        }
    }
}
