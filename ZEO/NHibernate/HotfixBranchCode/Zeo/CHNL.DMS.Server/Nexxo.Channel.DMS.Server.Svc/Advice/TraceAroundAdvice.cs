using System;
using System.Reflection;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text.RegularExpressions;
using MGI.Common.Util;

using AopAlliance.Intercept;

using MGI.Common.Sys;
using System.Runtime.Serialization;
using NLog.Config;
using NLog;
using NLog.Targets;
using MGI.Common.Logging.Data;
using System.Threading;
using MGI.Common.TransactionalLogging.Data;

namespace MGI.Channel.DMS.Server.Svc.Advice
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
		public TLoggerCommon tLogger { get; set; }

		public TraceAroundAdvice()
		{
			NLogger = new NLoggerCommon();
			tLogger = new TLoggerCommon();
		}

		public object Invoke(IMethodInvocation invocation)
		{
			object returnValue = null;
			string methodName = invocation.Method.Name;
			try
			{
				Thread.CurrentPrincipal = PrepareLogInfo(invocation);

				//Thread.CurrentPrincipal = SessionStorge(invocation);

				if (!LogIgnoreList.Contains(methodName))
				{
					NLogger.Info(string.Format("Begin method call {0}", methodName));

					// tLogger.Info(string.Format("Begin method call {0}", methodName));

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
								//Trace.Write(string.Format("{0}: ", parameters[i].Name));
								NLogger.Info(string.Format("{0}: ", parameters[i].Name));
								//tLogger.Info(string.Format("{0}: ", parameters[i].Name));

								if (parameters[i].Name.ToLower() == "cardnumber")
								{
									NLogger.Info(safeCardNumber(invocation.Arguments[i].ToString()));
								}
								else if (parameters[i].Name.ToLower() == "accountnumber" || parameters[i].Name.ToLower() == "ssn")
								{
									NLogger.Info(NexxoUtil.MaskSensitiveData(invocation.Arguments[i].ToString(), parameters[i].Name));
								}
								else if (parameters[i].Name.ToLower() == "customerlookupcriteria")
								{
									Dictionary<string, object> customerCriterias = (Dictionary<string, object>)invocation.Arguments[i];

									foreach (KeyValuePair<string, object> criteria in customerCriterias)
									{
										if (criteria.Key.ToLower() == "cardnumber")
										{
											NLogger.Info(string.Format("{0}: {1} ", criteria.Key, safeCardNumber(Convert.ToString(criteria.Value))));
										}
										else if (criteria.Key.ToLower() == "accountnumber")
										{
											NLogger.Info(string.Format("{0}: {1} ", criteria.Key, NexxoUtil.MaskSensitiveData(Convert.ToString(criteria.Value), criteria.Key)));
										}
									}
								}
								else
								{
									traceObject(invocation.Arguments[i]);
								}
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
					NLogger.Info("Return value:");
					NLogger.Info("\t");
					//Trace.Indent();
					traceObject(returnValue);
					//Trace.Unindent();
					NLogger.Info(string.Format("Completed method call {0}", methodName));
				}
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
			//finally
			//{
			//    try
			//    {
			//        if (!LogIgnoreList.Contains(methodName))
			//        {

			//        }
			//    }
			//    catch
			//    {
			//        // ignore the error with the log. It just means that the log can't be flushed, closed etc., but the rest of processing must go on...
			//    }
			//}

			// eventually return the result...
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
			List<string> noLogsAttributes = new List<string>() { "governmentidnumber", "ssn", "cardnumber", "alloyid", "accountnumber", "governmentid", "partneraccountnumber", "pin"};

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
				{
					System.Text.StringBuilder sb = new System.Text.StringBuilder();

					if (t != typeof(string) && t.BaseType.Name == "Object")
					{

						PropertyInfo[] properties = t.GetProperties();

						if (properties.Length > 0)
						{
							foreach (PropertyInfo item in properties)
							{
								if (noLogsAttributes.Contains(item.Name.ToLower()))
								{
									string ItemName = item.Name.ToLower();

									object itemValue = item.GetValue(o, null); //.ToString();
									if (itemValue != null)
									{

										sb.AppendLine(item.Name + " : " + maskCustomerPI(itemValue.ToString(), ItemName));

									}

								}
								else
								{
									sb.AppendLine(item.Name + " : " + item.GetValue(o, null));
								}
							}
						}
						else
						{
							sb.AppendLine(o.ToString());
						}
					}
					else
					{
						sb.AppendLine(ReplaceSenitiveData(o.ToString()));
					}
					NLogger.Info(sb.ToString());
				}
			}

			else
				NLogger.Info("null");
		}


		private string safeCardNumber(string cardNumber)
		{
			if (cardNumber.IsCreditCardNumber())
			{
				long card;
				if (long.TryParse(cardNumber, out card))
					return (CardNumberLogging ? cardNumber : ISOCard.EncodeCardNumber(card));
			}

			return cardNumber;
		}

		public string maskCustomerPI(string value1, string property1)
		{
			switch (property1)
			{
				case "ssn":
					if (string.IsNullOrEmpty(value1) || value1.Length < 4)
					{
						return "Incorrect ssn";
					}
					else
						return "XXX-XX-" + value1.Substring(value1.Length - 4, 4);

				case "governmentidnumber":
				case "governmentid":
					if (string.IsNullOrEmpty(value1) || value1.Length < 4)
					{
						return " ";
					}
					else
						return "****" + value1.Substring(value1.Length - 4, 4);
				case "alloyid":
				case "partneraccountnumber":
				case "addoncustomerid":
					if (string.IsNullOrEmpty(value1) || value1.Length < 16)
					{
						return "incorrect AlloyID";
                    }
                    else
                        return value1.Substring(0, 6) + "XXXXXX" + value1.Substring(value1.Length - 4, 4);
                case "cardnumber":
                case "accountnumber":
					if (string.IsNullOrWhiteSpace(value1) || value1.Length < 4)
					{
						return " ";
					}
					else
                    return "*****" + NexxoUtil.getLastFour(value1);
				case "pin":
					if (string.IsNullOrEmpty(value1))
					{
						return " ";
					}
					else
						return "****";
                default:
                    return value1;
            }
        }

		private string ReplaceSenitiveData(string value)
		{
			if (value.ToLower().Contains("cardnumber"))
			{
				return replaceCardNumber(value);
			}
			else if (value.Contains("SSN"))
			{
				if (value.Contains("["))
				{
					return "[SSN , " + "XXX-XX-" + value.Substring(value.Length - 5, 4) + "]"; //[key,value]
				}
				else
				{
					return "XXX-XX-" + value.Substring(value.Length - 5, 4); //[key,value]
				}

			}
			else if (value.ToString().Length == 16) // Assume AlloyID
			{
				return value.Substring(0, 6) + "XXXXXX" + value.Substring(value.Length - 4, 4);
			}
			else
			{
				return value;
			}
		}

		private string replaceCardNumber(string objString)
		{
			return Regex.Replace(objString, @"(cardnumber\s*[:=]\s*)(\d{16})", m => { return string.Format("{0}{1}", m.Groups[1].Value, safeCardNumber(m.Groups[2].Value)); }, RegexOptions.IgnoreCase);
		}

		/* SessionId SessionStorge(IMethodInvocation invocation)
		{
			SessionId sid = new SessionId();
			MethodInfo method = invocation.Method;
			ParameterInfo[] parameters = method.GetParameters();

			for (int i = 0; i < parameters.Length; i++)
			{
				if (parameters[i].Name.ToLower() == "customersessionid")
					sid.CustomerSessionId = Convert.ToInt64(invocation.Arguments[i]);
			}
			for (int i = 0; i < parameters.Length; i++)
			{
				if (parameters[i].Name.ToLower() == "sessionid" || parameters[i].Name.ToLower() == "agentsessionid")
					sid.AgentSessionId = Convert.ToInt64(invocation.Arguments[i]);
			}
           
			return sid;
		}*/

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
