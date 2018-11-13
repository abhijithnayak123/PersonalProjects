using System;
using System.Reflection;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using System.Net;
using System.Messaging;
using AopAlliance.Intercept;
using MGI.Common.Util;
using MGI.Common.Sys;

using MGI.Channel.DMS.Server.Data;

namespace MGI.Channel.DMS.Server.Svc.Advice
{
	public class ActivityLoggerAdvice : IMethodInterceptor
    {
        public NLoggerCommon NLogger { get; set; }
	    public List<string> LogIgnoreList { private get; set; }
		public bool CardNumberLogging { private get; set; }
		public string EventsQueue { private get; set; }

		private class ActivityLogInfo
		{
			public string cSession;
			public string aSession;
			public string initiator;
			public string eventType;
			public string eventDescription;
			public DateTime reqTime;
			public DateTime resTime;
			public string resType;
			public RequestMethodInfo request;
			public object response;
		}

		private class RequestMethodInfo
		{
			public string methodName;
			public Dictionary<string, object> requestArgs;

			public RequestMethodInfo(IMethodInvocation invocation)
			{
				methodName = invocation.Method.Name;

				// setup arguments with parameter names in info object
				if (invocation.Arguments != null)
				{
					requestArgs = new Dictionary<string, object>();

					// get the parameter names for the method
					ParameterInfo[] parameters = invocation.Method.GetParameters();

					// for each argument, log the parameter name and argument value
					for (int i = 0; i < invocation.Arguments.Length; i++)
						requestArgs.Add(parameters[i].Name.ToLower(), invocation.Arguments[i]);
				}
			}
		}

		public ActivityLoggerAdvice()
        {
        }

        public object Invoke(IMethodInvocation invocation)
        {
            NLogger.Error("RequestResponseAdvice begin");     

            object returnValue = null;

			var requestMethod = new RequestMethodInfo(invocation);
			
			// set up object to be serialized
			ActivityLogInfo info = new ActivityLogInfo
			{
				reqTime = DateTime.Now,
				initiator = "DMS.Web",
				request = requestMethod
			};

			// add teller/agent session and customer session
			if (requestMethod.requestArgs != null)
			{
				if (requestMethod.requestArgs.ContainsKey("customersessionid"))
					info.cSession = (string)requestMethod.requestArgs["customersessionid"];
				if (requestMethod.requestArgs.ContainsKey("sessionid"))
					info.aSession = (string)requestMethod.requestArgs["sessionid"];
			}

			// set the event type based on the DMSMethodAttribute
			DMSMethodAttribute[] methodAttributes= (DMSMethodAttribute[])invocation.Method.GetCustomAttributes(typeof(DMSMethodAttribute), false);

			if (methodAttributes != null && methodAttributes.Length > 0)
			{
				info.eventType = methodAttributes[0].FunctionalArea.ToString();
				info.eventDescription = methodAttributes[0].Description;
			}
			else
			{
				info.eventType = DMSFunctionalArea.Other.ToString();
				info.eventDescription = requestMethod.methodName;
			}

            try
            {
                // Call the method, and add the return value to the info object
                returnValue = invocation.Proceed();
				info.response = returnValue;
				info.resTime = DateTime.Now;
				info.resType = "success";
            }
			catch (FaultException<NexxoSOAPFault> fex)
			{
				info.response = fex.Detail;
				info.resTime = DateTime.Now;
				info.resType = "fail";
				throw fex;
			}
			catch (Exception ex)
			{
				var nex = new NexxoSOAPFault
				{
					MajorCode = "0",
					MinorCode = "0",
					Processor = "MGiAlloy",
					Details = ex.Message,
					StackTrace = ex.StackTrace
				};

				info.response = nex;
				info.resTime = DateTime.Now;
				info.resType = "fail";

				throw new FaultException<NexxoSOAPFault>(nex);
			}
            finally
            {
				try
				{
					if (!LogIgnoreList.Contains(info.request.methodName))
					{
						// convert data to json
						var json = new JavaScriptSerializer().Serialize(info);
                        NLogger.Debug(replaceCardNumber(json));

						// add to queue
						MessageQueue messageQueue = null;
						if (MessageQueue.Exists(EventsQueue))
						{
							messageQueue = new MessageQueue(EventsQueue);							
                            NLogger.Info(string.Format("Found existing queue: {0}", messageQueue.Label));
						}
						else
						{
							// Create the Queue
							messageQueue = MessageQueue.Create(EventsQueue);
							messageQueue.SetPermissions("Everyone", MessageQueueAccessRights.ReceiveMessage);
                            NLogger.Info(string.Format("created new queue: {0}", messageQueue.Label));
						}

						messageQueue = new MessageQueue(EventsQueue);
						messageQueue.Label = "MGi Alloy Log Event Queue";

						messageQueue.Send(replaceCardNumber(json));
					}
				}
				catch (MessageQueueException mex)
				{
                    NLogger.Error(string.Format("failure when sending json to queue: {0} ({1})", mex.Message, mex.MessageQueueErrorCode));
                    NLogger.Error(mex);
                    
				}
				catch (Exception ex)
				{			
                    NLogger.Error(string.Format("failure when serializing and sending json: {0}", ex.Message));
				}
            }

            // return the result
            return returnValue;
        }

		private string replaceCardNumber(string objString)
		{
			return Regex.Replace(objString, @"(""cardnumber""\s*[:=]\s*)(\d{16})", m => { return string.Format("{0}{1}", m.Groups[1].Value, safeCardNumber(m.Groups[2].Value)); }, RegexOptions.IgnoreCase);
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
    }
}
