using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.ServiceModel;
using MGI.Common.Sys;
using MGI.Core.Partner.Contract;
using MGI.Core.Partner.Data;
using Spring.Aop;
using MGI.Common.Util;
using NxoLimitException = MGI.Biz.Compliance.Contract.BizComplianceLimitException;
using NxoSystemException = MGI.Common.Sys.NexxoException;
using System.Text;
using MGI.Biz.FundsEngine.Contract;
using MGI.Common.TransactionalLogging.Data;

namespace MGI.Channel.DMS.Server.Svc.Advice
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
			string errorReason = string.Empty;
			if (args != null)
			{
				// key format is MajorCode.MinorCode[.ProviderId.ProviderErrorCode]
				string key = string.Format("{0}.{1}", soapFault.MajorCode, soapFault.MinorCode);

				MGI.Channel.DMS.Server.Data.MGIContext mgiContext = null;
				int channelPartnerId = 1;
				if (args.Length > 0)
				{
					mgiContext = args[args.Length - 1] as MGI.Channel.DMS.Server.Data.MGIContext;
					if (mgiContext != null && mgiContext.ChannelPartnerId != 0)
					{
						channelPartnerId = int.Parse(mgiContext.ChannelPartnerId.ToString());
					}
				}

				string exceptionMessage = string.Empty;
				string stackTrace = string.Empty;

				PopulateErrorInfo(nEx, ref exceptionMessage, ref stackTrace);
				NLogger.Error(string.Format("Caught EXCEPTION. Throwing NexxoSOAPFault. Major: {0} Minor: {1} Message: {2}  \n StackTrace: {3} ", nEx.MajorCode, nEx.MinorCode, exceptionMessage, stackTrace));


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
				if (mgiContext != null && !string.IsNullOrEmpty(mgiContext.Language))
				{
					lang = (Language)Enum.Parse(typeof(Language), mgiContext.Language);
				}

				Message storeMessage = null;
				
				try
				{
					storeMessage = _msgStore.Lookup(channelPartnerId, key, lang);
				}
				catch(Exception ex)
				{
					errorReason = string.Join("|", new object[] { string.IsNullOrWhiteSpace(soapFault.Processor) ? "MGiAlloy" : soapFault.Processor, 
						string.Format("{0}.{1}", soapFault.MajorCode, soapFault.MinorCode), ex.Message, 
						string.IsNullOrWhiteSpace(soapFault.AddlDetails) ? "Please contact the System Administrator" : soapFault.AddlDetails });

					throw new FaultException<NexxoSOAPFault>(soapFault, errorReason);
				}

				
				if (storeMessage != null)
				{
					string message = storeMessage.Content;

					string addlDetails = storeMessage.AddlDetails;
					// if it's a limit exception, need to modify the message dynamically with the limit value
					var limitException = nEx as NxoLimitException;
					if (limitException != null)
					{
						try
						{
							message = string.Format(message, limitException.LimitValue);
							addlDetails = string.Format(addlDetails, limitException.LimitValue.ToString("C2"), limitException.Message);
						}
						catch { }
					}
					var fundsException = nEx as BizFundsException;
					if (fundsException != null)
					{
						try
						{
							message = string.Format(message,  nEx.Message);
						}
						catch { }
					}

					
					soapFault.Details = message;
					soapFault.AddlDetails = addlDetails;
					soapFault.Processor = storeMessage.Processor;
				}

				if (soapFault.Processor == null)
				{
					soapFault.Processor = NexxoUtil.GetProcessor(channelPartnerId, nEx.MajorCode);
				}
			}
			//AL-2968 
			errorReason = string.Join("|", new object[] { string.IsNullOrWhiteSpace(soapFault.Processor) ? "MGiAlloy" : soapFault.Processor, 
				string.Format("{0}.{1}", soapFault.MajorCode, soapFault.MinorCode), soapFault.Details, 
				string.IsNullOrWhiteSpace(soapFault.AddlDetails) ? "Please contact the System Administrator" : soapFault.AddlDetails });

			throw new FaultException<NexxoSOAPFault>(soapFault, errorReason);
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
                StackTrace = ex.StackTrace,
                AddlDetails = ex.GetType().ToString()//AL-578
            };


            string exceptionMessage = string.Empty;
            string stackTrace = string.Empty;

            PopulateErrorInfo(ex, ref exceptionMessage, ref stackTrace);
            NLogger.Error(string.Format("Caught Unknown EXCEPTION. Message {0}, \n StackTrace: {1} ", exceptionMessage, stackTrace));

            // if exception is NHibernate Exception then send custom message to controller- AL-578
            if (ex.GetType().ToString() == "NHibernate.Exceptions.GenericADOException")
            {
                soapFault.Details = "Alloy is unable to process your request at this time. Please try again later";
            }
			//AL-2968 
			string errorReason = string.Join("|", new object[] { soapFault.Processor, string.Format("{0}.{1}", soapFault.MajorCode, soapFault.MinorCode), soapFault.Details, soapFault.AddlDetails });

			throw new FaultException<NexxoSOAPFault>(soapFault, errorReason);
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
