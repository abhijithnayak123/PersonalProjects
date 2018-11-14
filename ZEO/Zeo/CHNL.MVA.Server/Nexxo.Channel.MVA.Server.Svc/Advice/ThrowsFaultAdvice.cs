using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.ServiceModel;
using MGI.Common.Sys;
using MGI.Core.Partner.Contract;
using MGI.Core.Partner.Data;
using Spring.Aop;
using NxoLimitException = MGI.Biz.Compliance.Contract.BizComplianceLimitException;
using NxoSystemException = MGI.Common.Sys.NexxoException;
using System.Text;
using MGI.Common.Util;

namespace MGI.Channel.MVA.Server.Svc.Advice
{
	public class ThrowsFaultAdvice : IThrowsAdvice
	{
		private IMessageStore _msgStore;
		public IMessageStore MessageStore { set { _msgStore = value; } }

        public NLoggerCommon NLogger { get; set; }

		public void AfterThrowing(MethodInfo method, Object[] args, Object target, NxoSystemException nEx)
		{
			var soapFault = new NexxoSOAPFault()
			{
				MajorCode = nEx.MajorCode.ToString(),
				MinorCode = nEx.MinorCode.ToString(),
				Details = nEx.Message,
				StackTrace = nEx.StackTrace
			};

			if (args != null)
			{
				// key format is MajorCode.MinorCode[.ProviderId.ProviderErrorCode]
				string key = string.Format("{0}.{1}", soapFault.MajorCode, soapFault.MinorCode);

				Dictionary<string, object> context = null;
				int channelPartnerId = 1;
				if (args.Length > 0)
				{
					context = args[args.Length - 1] as Dictionary<string, object>;
					if (context != null && context.ContainsKey("ChannelPartnerId"))
					{
						channelPartnerId = int.Parse(context["ChannelPartnerId"].ToString());
					}
				}

				string exceptionMessage = string.Empty;
				string stackTrace = string.Empty;

				PopulateErrorInfo(nEx, ref exceptionMessage, ref stackTrace);

				NLogger.Error(string.Format("Caught EXCEPTION. Throwing NexxoSOAPFault. Major: {0} Minor: {1} Message: {2}  \n StackTrace: {3} ", 
					nEx.MajorCode, nEx.MinorCode, exceptionMessage, stackTrace));

				if (nEx.InnerException != null)
				{
					var innerProviderEx = nEx.InnerException as ProviderException;
					if (innerProviderEx != null)
					{
						key += string.Format(".{0}.{1}", innerProviderEx.ProviderId, innerProviderEx.Code);
						soapFault.ProviderId = innerProviderEx.ProviderId.ToString();
						soapFault.ProviderErrorCode = innerProviderEx.Code;
						soapFault.ProviderErrorMessage = innerProviderEx.Message;
						soapFault.AddlDetails = innerProviderEx.Message;
						soapFault.Details = innerProviderEx.Message;
						TraceInnerExceptions(innerProviderEx);
					}
					else
					{
						TraceInnerExceptions(nEx.InnerException);
					}
				}

				Language lang = Language.EN;
				if (context != null && context.ContainsKey("Language"))
				{
					lang = (Language)Enum.Parse(typeof(Language), context["Language"].ToString());
				}

				Message storeMessage = _msgStore.Lookup(channelPartnerId, key, lang);
				if (storeMessage != null)
				{
					string message = storeMessage.Content;

					// if it's a limit exception, need to modify the message dynamically with the limit value
                    //var limitException = nEx as NxoLimitException;
                    //if (limitException != null)
                    //{
                    //    try
                    //    {
                    //        message = string.Format(message, limitException.LimitValue);
                    //    }
                    //    catch { }
                    //}

					soapFault.Details = message;
					soapFault.AddlDetails = storeMessage.AddlDetails;
					soapFault.Processor = storeMessage.Processor;
                }

                if (soapFault.Processor == null)
                {
                    soapFault.Processor = NexxoUtil.GetProcessor(channelPartnerId, nEx.MajorCode);
                }
			}

			throw new FaultException<NexxoSOAPFault>(soapFault);
		}

		private void TraceInnerExceptions(Exception ex)
		{
			if (ex != null && ex.InnerException != null)
			{
				var innerEx = ex.InnerException as NxoSystemException; //((MGI.Common.Sys.NexxoException)((ex).InnerException));
				var providerinnerEx = ex.InnerException as ProviderException; //((MGI.Common.Sys.ProviderException)((ex).InnerException));
				if (innerEx != null)
				{
					NLogger.Error(string.Format("Caught NexxoEXCEPTION.InnerException. Major: {0} Minor: {1} Message: {2} StackTrace: {3} ", innerEx.MajorCode, innerEx.MinorCode, innerEx.Message, innerEx.StackTrace));
					TraceInnerExceptions(innerEx);
				}
				else if (providerinnerEx != null)
				{
					NLogger.Error(string.Format("Caught ProviderEXCEPTION.InnerException. ProviderId: {0} Code: {1} Message: {2} StackTrace: {3} ", providerinnerEx.ProviderId.ToString(), providerinnerEx.Code, providerinnerEx.Message, providerinnerEx.StackTrace));
					TraceInnerExceptions(providerinnerEx);
				}
				else
				{
					return;
				}
			}
		}

		public void AfterThrowing(FaultException<NexxoSOAPFault> ex)
		{
			throw ex;
		}

		public void AfterThrowing(Exception ex)
		{
			var soapFault = new NexxoSOAPFault()
			{
				MajorCode = "0",
				MinorCode = "0",
				Processor = "MGiAlloy",
				Details = ex.Message,
				StackTrace = ex.StackTrace
			};

			string exceptionMessage = string.Empty;
			string stackTrace = string.Empty;

			PopulateErrorInfo(ex, ref exceptionMessage, ref stackTrace);

		
            NLogger.Error(string.Format("Caught Unknown EXCEPTION. Message {0}, \n StackTrace: {1} ", exceptionMessage, stackTrace));

			throw new FaultException<NexxoSOAPFault>(soapFault);
		}

		private void PopulateErrorInfo(Exception exception, ref string exceptionMessage, ref string stackTrace)
		{
			StringBuilder messageBuilder = new StringBuilder();
			StringBuilder traceBuilder = new StringBuilder();

			int counter = 1;
			int exceptionDrillDownLimit = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ExceptionDrillDownLimit"]);

			if (exceptionDrillDownLimit == 0)
			{
				// hard coded to 5 in case ExceptionDrillDownLimit value in not available in the config file
				exceptionDrillDownLimit = 5;
			}

			while (exception != null && counter <= exceptionDrillDownLimit)
			{
				ExtractErrorInfo(exception, counter, messageBuilder, traceBuilder);
				exception = exception.InnerException;
				counter++;
			}

			exceptionMessage = messageBuilder.ToString();
			stackTrace = traceBuilder.ToString();
		}

		private void ExtractErrorInfo(Exception nEx, int drillDownIndex, StringBuilder messageBuilder, StringBuilder traceBuilder)
		{
			messageBuilder.AppendFormat("\n Level {0}: {1} \n", drillDownIndex, !string.IsNullOrWhiteSpace(nEx.Message) ? nEx.Message : "No exception message available");
			traceBuilder.AppendFormat("\n Level {0}: {1} \n", drillDownIndex, !string.IsNullOrWhiteSpace(nEx.StackTrace) ? nEx.StackTrace : "No stack trace available");
		}

	}
}
