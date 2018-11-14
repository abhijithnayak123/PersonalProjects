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
using System.Text;
using MGI.Biz.FundsEngine.Contract;
using MGI.Common.TransactionalLogging.Data;
using MGI.Biz.CPEngine.Contract;
using MGI.Channel.DMS.Server.Data;
using MGI.Biz.Partner.Contract;

namespace MGI.Channel.DMS.Server.Svc.Advice
{
	public class ThrowsFaultAdvice : IThrowsAdvice
	{
		private Core.Partner.Contract.IMessageStore _msgStore;
		public Core.Partner.Contract.IMessageStore MessageStore { set { _msgStore = value; } }
		public NLoggerCommon NLogger { get; set; }
		public TLoggerCommon MongoDBLogger { get; set; }

		public void AfterThrowing(MethodInfo method, Object[] args, Object target, AlloyException alloyException)
		{
			string exceptionMessage = string.Empty;
			string stackTrace = string.Empty;
			Exception innerException = null;

			long partnerId = 1;
			if (args != null && args.Length > 0)
			{
				var mgiContext = args[args.Length - 1] as MGI.Channel.DMS.Server.Data.MGIContext;
				if (mgiContext != null && mgiContext.ChannelPartnerId != 0)
				{
					partnerId = (int)mgiContext.ChannelPartnerId;
				}
			}

			string exceptionKey = string.Format("{0}.{1}.{2}", alloyException.ProductCode, alloyException.ProviderCode, alloyException.AlloyErrorCode);
			MGI.Core.Partner.Data.Message messageStore = _msgStore.Lookup(partnerId, exceptionKey, MGI.Core.Partner.Data.Language.EN);

			if (messageStore == null)
			{
				messageStore = _msgStore.Lookup(1, exceptionKey, MGI.Core.Partner.Data.Language.EN);
				if (messageStore == null)
				{
					//The below will not fetch anything as we dont have DB infrastructure yet for this.
					string exceptionPartialKey = string.Format("{0}.{1}", alloyException.ProviderCode, alloyException.AlloyErrorCode);
					messageStore = _msgStore.Lookup(1, exceptionPartialKey, MGI.Core.Partner.Data.Language.EN);
					if (messageStore == null)
					{
						exceptionPartialKey = string.Format("{0}.{1}", alloyException.ProductCode, alloyException.ProviderCode);
						messageStore = _msgStore.Lookup(1, exceptionPartialKey, MGI.Core.Partner.Data.Language.EN);
						if(messageStore == null)
						{
							messageStore = new Core.Partner.Data.Message()
							{
								MessageKey = exceptionKey,
								Content = "This operation could not be completed",
								AddlDetails = "Please contact your technical support team for more information", 
								ErrorType = (int)ErrorType.ERROR,
								Processor = "MGI Alloy",
							};
					}
				}
			}
			}

			var limitException = alloyException as NxoLimitException;
			if (limitException != null)
			{
				try
				{
					messageStore.Content = string.Format(messageStore.Content, limitException.LimitValue);
					messageStore.AddlDetails = string.Format(messageStore.AddlDetails, limitException.LimitValue.ToString("C2"), limitException.Message);
				}
				catch { }
			}

			string errorType = ((ErrorType)messageStore.ErrorType).ToString();
			string errorMessage = messageStore.Content;
			string addlMessage = messageStore.AddlDetails;

			StringBuilder errmsg = new StringBuilder().AppendLine(string.Format("CAUGHT ALLOY ERROR. \nERROR CODE : {0} \nERROR MESSAGE : {1} \nERROR ADDLDETAILS : {2}", exceptionKey, errorMessage, addlMessage));

			GetActualException(alloyException, ref innerException);
			if (innerException != null && (innerException as AlloyException) == null)
				errmsg.AppendLine(string.Format("ERROR LOG : {0}", innerException.ToString()));

			NLogger.Error(errmsg.ToString());

			string errorReason = string.Join("|", new object[] { messageStore.Processor, exceptionKey, errorMessage, addlMessage, errorType });

			var soapFault = new NexxoSOAPFault()
			{
				MajorCode = alloyException.ProductCode.ToString(),
				ProviderId = alloyException.ProviderCode.ToString(),
				MinorCode = alloyException.AlloyErrorCode
			};

			throw new FaultException<NexxoSOAPFault>(soapFault, errorReason);
		}

		public void AfterThrowing(FaultException<NexxoSOAPFault> ex)
		{
			throw ex;
		}

		public void AfterThrowing(MethodInfo method, Object[] args, Object target, ProviderException providerException)
		{
			string exceptionMessage = string.Empty;
			string stackTrace = string.Empty;

			long partnerId = 1;
			if (args != null && args.Length > 0)
			{
				var mgiContext = args[args.Length - 1] as MGI.Channel.DMS.Server.Data.MGIContext;
				if (mgiContext != null && mgiContext.ChannelPartnerId != 0)
				{
					partnerId = (int)mgiContext.ChannelPartnerId;
				}
			}

			string exceptionKey = string.Format("{0}.{1}.{2}", providerException.ProductCode, providerException.ProviderCode, providerException.ProviderErrorCode);
			MGI.Core.Partner.Data.Message messageStore = _msgStore.Lookup(partnerId, exceptionKey, MGI.Core.Partner.Data.Language.EN);

			if (messageStore == null)
			{
				messageStore = _msgStore.Lookup(1, exceptionKey, MGI.Core.Partner.Data.Language.EN);
				if (messageStore == null)
				{
					//The below will not fetch anything as we dont have DB infrastructure yet for this.
					string exceptionPartialKey = string.Format("{0}.{1}", providerException.ProviderCode, providerException.ProviderErrorCode);
					messageStore = _msgStore.Lookup(1, exceptionPartialKey, MGI.Core.Partner.Data.Language.EN);
					if (messageStore == null)
					{
						exceptionPartialKey = string.Format("{0}.{1}", providerException.ProductCode, providerException.ProviderCode);
						messageStore = _msgStore.Lookup(1, exceptionPartialKey, MGI.Core.Partner.Data.Language.EN);
					}
				}
			}

			string errorType = ((ErrorType)messageStore.ErrorType).ToString();
			string errorMessage = (messageStore != null && !string.IsNullOrWhiteSpace(messageStore.Content)) ? messageStore.Content : providerException.Message;
			string addlMessage = messageStore.AddlDetails;

			NLogger.Error(string.Format("CAUGHT PROVIDER ERROR. \nERROR CODE : {0} \nERROR MESSAGE : {1} \nERROR ADDLDETAILS : {2}", exceptionKey, errorMessage, addlMessage));

			string errorReason = string.Join("|", new object[] { messageStore.Processor, exceptionKey, errorMessage, addlMessage, errorType });

			var soapFault = new NexxoSOAPFault()
			{
				MajorCode = providerException.ProductCode.ToString(),
				ProviderId = providerException.ProviderCode.ToString(),
				MinorCode = providerException.ProviderErrorCode
			};

			throw new FaultException<NexxoSOAPFault>(soapFault, errorReason);
		}

		public void AfterThrowing(Exception ex)
		{

			var soapFault = new NexxoSOAPFault()
			{
				MajorCode = "1000",
				MinorCode = "100",
				ProviderErrorCode = "9999",
				Processor = "MGiAlloy",
				Details = ex.Message,
				StackTrace = ex.StackTrace,
				AddlDetails = ex.GetType().ToString()//AL-578
			};

			string exceptionPartialKey = string.Format("{0}.{1}.{2}", soapFault.MajorCode, soapFault.MinorCode, soapFault.ProviderErrorCode);
			MGI.Core.Partner.Data.Message messageStore = _msgStore.Lookup(1, exceptionPartialKey, MGI.Core.Partner.Data.Language.EN);

			string exceptionMessage = string.Empty;
			string stackTrace = string.Empty;

			NLogger.Error(string.Format("CAUGHT UNKNOWN EXCEPTION. \nEXCEPTION LOG : {0}", ex.ToString()));

			// if exception is NHibernate Exception then send custom message to controller- AL-578
			if (ex.GetType().ToString() == "NHibernate.Exceptions.GenericADOException")
			{
				soapFault.Details = "Alloy is unable to process your request at this time. Please try again later";
			}
			//AL-2968 
			string errorReason = string.Join("|", new object[] { messageStore.Processor, messageStore.MessageKey, messageStore.Content, messageStore.AddlDetails, ErrorType.ERROR.ToString() });

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

		private void GetActualException(Exception exception, ref Exception actualException)
		{
			actualException = exception;
			while (actualException.InnerException != null)
			{
				actualException = actualException.InnerException;
			}
		}

		private void ExtractErrorInfo(Exception nEx, int drillDownIndex, StringBuilder messageBuilder, StringBuilder traceBuilder)
		{
			messageBuilder.AppendFormat("\n Level {0}: {1} \n", drillDownIndex, !string.IsNullOrWhiteSpace(nEx.Message) ? nEx.Message : "No exception message available");
			traceBuilder.AppendFormat("\n Level {0}: {1} \n", drillDownIndex, !string.IsNullOrWhiteSpace(nEx.StackTrace) ? nEx.StackTrace : "No stack trace available");
		}

		private string GetProviderByCode(string code)
		{
			// Funds 101 - 199		
			//FirstView = 101,
			//TSys = 102,
			//Visa = 103,

			//// Checks 200 - 299
			//Ingo = 200,
			//Certegy = 201,

			//// Money Transfer 300 - 399
			//NexxoMoneyTransfer = 300,
			//WesternUnion = 301,
			//MoneyGram = 302, //Changes for MGI 

			//// Bill Pay AND TopUp 400 - 499
			//CheckFree = 400,
			//WesternUnionBillPay = 401,
			//TIO = 402,
			//Movilix = 403,
			//MoneyGramBillPay=405,

			//// Money Order 500 - 599
			//OrderExpress = 500,
			//WoodForest = 501,
			//Nexxo = 502,
			//Continental =503,
			//TCF = 504,
			//MGIMoneyOrder=505,

			////Customer 600 - 699
			//FIS = 600,
			//CCISCustomer=601,
			//TCISCustomer=602
			string provider = "Unknown";
			switch (code)
			{
				case "101":
					provider = "First View";
					break;
				case "102":
					provider = "TSys";
					break;
				case "103":
					provider = "Visa";
					break;
				case "200":
					provider = "Ingo";
					break;
				case "201":
					provider = "Certegy";
					break;
				case "301":
				case "401":
					provider = "Western Union";
					break;
				case "600":
					provider = "FIS";
					break;
				case "601":
					provider = "CCIS";
					break;
				case "602":
					provider = "TCIS";
					break;
			}
			return provider;
		}
	}
}
