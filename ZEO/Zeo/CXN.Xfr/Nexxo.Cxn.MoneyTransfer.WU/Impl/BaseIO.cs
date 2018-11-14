using MGI.Common.DataAccess.Contract;
using MGI.Common.Sys;
using MGI.Common.TransactionalLogging.Data;
using MGI.Common.Util;
using MGI.Cxn.MoneyTransfer.Data;
using MGI.Cxn.MoneyTransfer.WU.Data;
using MGI.Cxn.WU.Common.Contract;
using MGI.Cxn.WU.Common.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Text;
using System.Xml.Serialization;
using DAS = MGI.Cxn.MoneyTransfer.WU.DASService;

namespace MGI.Cxn.MoneyTransfer.WU.Impl
{
	public class BaseIO
	{
		public IExceptionHelper ExceptionHelper { get; set; }
        public TLoggerCommon MongoDBLogger { get; set; }
		public string AllowDuplicateTrxWU { get; set; }
		public IRepository<WUCountry> WUCountryRepo { get; set; }
		public IRepository<WUState> WUStateRepo { get; set; }
		public const string DAS_DeliveryServices = "GetDeliveryServices";
		public IRepository<WUCredential> WUCredentialRepo { get; set; }
		public X509Certificate2 _certificate = null;
		public IWUCommonIO WUCommon { get; set; }

		SendMoneyValidation.foreign_remote_system frs = null;
		SendMoneyStore.foreign_remote_system SendMoneyStoreremotesys = null;
		public bool IsHardCodedCounterId { get; set; }
		public string LPMTErrorMessage { get; set; }
		public string _serviceUrl = string.Empty;
		public const string DAS_ENDPOINT_NAME = "DASInquiry";
		public WUBaseRequestResponse _response = null;

		#region Member
		internal string CountryName = "United States";
		internal string CountryCode = "US";
		internal string CountryCurrencyCode = "USD";
		#endregion

		public BaseIO()
		{
			AutoMapper.Mapper.CreateMap<Channel, DAS.channel>();
			AutoMapper.Mapper.CreateMap<ForeignRemoteSystem, DAS.foreign_remote_system>()
				.ForMember(d => d.counter_id, s => s.MapFrom(c => c.CoutnerId));

			AutoMapper.Mapper.CreateMap<Channel, FeeInquiry.channel>();
			AutoMapper.Mapper.CreateMap<ForeignRemoteSystem, FeeInquiry.foreign_remote_system>()
				.ForMember(d => d.counter_id, s => s.MapFrom(c => c.CoutnerId));

			AutoMapper.Mapper.CreateMap<Channel, SendMoneyValidation.channel>();
			AutoMapper.Mapper.CreateMap<ForeignRemoteSystem, SendMoneyValidation.foreign_remote_system>()
				.ForMember(d => d.counter_id, s => s.MapFrom(c => c.CoutnerId));

			AutoMapper.Mapper.CreateMap<Channel, ReceiveMoneySearch.channel>();
			AutoMapper.Mapper.CreateMap<ForeignRemoteSystem, ReceiveMoneySearch.foreign_remote_system>()
				.ForMember(d => d.counter_id, s => s.MapFrom(c => c.CoutnerId));

			AutoMapper.Mapper.CreateMap<Channel, ReceiveMoneyPay.channel>();
			AutoMapper.Mapper.CreateMap<ForeignRemoteSystem, ReceiveMoneyPay.foreign_remote_system>()
				.ForMember(d => d.counter_id, s => s.MapFrom(c => c.CoutnerId));

			AutoMapper.Mapper.CreateMap<Channel, ModifySendMoneySearch.channel>();
			AutoMapper.Mapper.CreateMap<ForeignRemoteSystem, ModifySendMoneySearch.foreign_remote_system>()
				.ForMember(d => d.counter_id, s => s.MapFrom(c => c.CoutnerId));

			AutoMapper.Mapper.CreateMap<Channel, SendMoneyStore.channel>();
			AutoMapper.Mapper.CreateMap<ForeignRemoteSystem, SendMoneyStore.foreign_remote_system>()
				.ForMember(d => d.counter_id, s => s.MapFrom(c => c.CoutnerId));

			AutoMapper.Mapper.CreateMap<Channel, ModifySendMoney.channel>();
			AutoMapper.Mapper.CreateMap<ForeignRemoteSystem, ModifySendMoney.foreign_remote_system>()
				.ForMember(d => d.counter_id, s => s.MapFrom(c => c.CoutnerId));

			AutoMapper.Mapper.CreateMap<Channel, SendMoneyRefund.channel>();
			AutoMapper.Mapper.CreateMap<ForeignRemoteSystem, SendMoneyRefund.foreign_remote_system>()
				.ForMember(d => d.counter_id, s => s.MapFrom(c => c.CoutnerId));

			AutoMapper.Mapper.CreateMap<Channel, Search.channel>();
			AutoMapper.Mapper.CreateMap<ForeignRemoteSystem, Search.foreign_remote_system>()
				.ForMember(d => d.counter_id, s => s.MapFrom(c => c.CoutnerId));

			AutoMapper.Mapper.CreateMap<Channel, SendMoneyPayStatus.channel>();
			AutoMapper.Mapper.CreateMap<ForeignRemoteSystem, SendMoneyPayStatus.foreign_remote_system>()
				.ForMember(d => d.counter_id, s => s.MapFrom(c => c.CoutnerId));
		}

		internal SendMoneyStore.channel SendMoneyStoreChannelSetup(string channelName, string channelVersion)
		{
			SendMoneyStore.channel SendMoneystorechannel = new SendMoneyStore.channel();
			SendMoneystorechannel.type = MGI.Cxn.MoneyTransfer.WU.SendMoneyStore.channel_type.H2H;
			SendMoneystorechannel.name = channelName;
			SendMoneystorechannel.version = channelVersion;
			SendMoneystorechannel.typeSpecified = true;
			return SendMoneystorechannel;
		}

		internal SendMoneyValidation.channel SetSendMoneyValidateChannelSetup(string channelName, string channelVersion)
		{
			SendMoneyValidation.channel channel = new SendMoneyValidation.channel();
			channel.type = MGI.Cxn.MoneyTransfer.WU.SendMoneyValidation.channel_type.H2H;
			channel.name = channelName;
			channel.version = channelVersion;
			channel.typeSpecified = true;
			return channel;
		}

		internal SendMoneyValidation.foreign_remote_system SetRemoteSystem(string wuAccountIdentifier, string locationId)
		{
			frs = new SendMoneyValidation.foreign_remote_system();
			frs.identifier = wuAccountIdentifier;
			//frs.reference_no = DateTime.Now.ToString("yyyyMMddhhmmssff");
			frs.counter_id = locationId;
			return frs;
		}

		internal SendMoneyStore.foreign_remote_system SetSendMoneyStoreRemoteSystem(string wuAccountIdentifier, string locationId)
		{
			SendMoneyStoreremotesys = new SendMoneyStore.foreign_remote_system();
			SendMoneyStoreremotesys.identifier = wuAccountIdentifier;
			//SendMoneyStoreremotesys.reference_no = DateTime.Now.ToString("yyyyMMddhhmmssff");
			SendMoneyStoreremotesys.counter_id = locationId;
			return SendMoneyStoreremotesys;
		}

		internal X509Certificate2 SetWUCredentialCertificate(string wucertificatename)
		{
			X509Store certificateStore = new X509Store(StoreName.My, StoreLocation.LocalMachine);
			// Open the store.
			certificateStore.Open(OpenFlags.ReadWrite | OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
			// Find the certificate with the specified subject.
			X509Certificate2Collection certificates = certificateStore.Certificates.Find(X509FindType.FindBySubjectName, wucertificatename, false);
			certificateStore.Close();
			if (certificates.Count < 1)
			{
				throw new MoneyTransferException(MoneyTransferException.CERIFICATE_NOTFOUND);
			}
			return certificates[0];
		}

		internal string ConvertToXML(object request, Type type)
		{
			MemoryStream stream = null;
			TextWriter writer = null;
			try
			{
				stream = new MemoryStream(); // read xml in memory
				writer = new StreamWriter(stream, Encoding.Unicode);
				// get serialise object
				XmlSerializer serializer = new XmlSerializer(type);
				serializer.Serialize(writer, request); // read object
				int count = (int)stream.Length; // saves object in memory stream
				byte[] arr = new byte[count];
				stream.Seek(0, SeekOrigin.Begin);
				// copy stream contents in byte array
				stream.Read(arr, 0, count);
				UnicodeEncoding utf = new UnicodeEncoding(); // convert byte array to string
				return utf.GetString(arr).Trim();
			}
			catch
			{
				return string.Empty;
			}
			finally
			{
				if (stream != null) stream.Close();
				if (writer != null) writer.Close();
			}
		}

		internal DAS.h2hdasreply ExecuteDASInquiry(string methodName, DAS.filters_type filters, MGIContext mgiContext)
		{
			DAS.channel channel = null;
			DAS.foreign_remote_system foreignRemoteSystem = null;

			PopulateWUObjects(mgiContext.ChannelPartnerId, mgiContext);

			BuildDASObjects(_response, ref channel, ref foreignRemoteSystem);
			foreignRemoteSystem.reference_no = DateTime.Now.ToString("yyyyMMddHHmmssffff");

			DAS.DASInquiryPortTypeClient dasInquiry = new DAS.DASInquiryPortTypeClient(DAS_ENDPOINT_NAME, _serviceUrl);
			dasInquiry.ClientCredentials.ClientCertificate.Certificate = _certificate;

			DAS.h2hdasrequest request = new DAS.h2hdasrequest()
			{
				channel = channel,
				foreign_remote_system = foreignRemoteSystem,
				name = methodName,
				filters = filters
			};

			DAS.h2hdasreply response = null;
			try
			{
				#region AL-3370 Transactional Log User Story
                MongoDBLogger.Info<DAS.h2hdasrequest>(mgiContext.CustomerSessionId, request, "ExecuteDASInquiry", AlloyLayerName.CXN,
                    ModuleName.MoneyTransfer, "ExecuteDASInquiry REQUEST - MGI.Cxn.MoneyTransfer.WU.Impl.BaseIO", mgiContext);
				#endregion
				response = dasInquiry.DAS_Service(request);
				
				#region AL-3370 Transactional Log User Story
				MongoDBLogger.Info<DAS.h2hdasreply>(mgiContext.CustomerSessionId, response, "ExecuteDASInquiry", AlloyLayerName.CXN,
					ModuleName.MoneyTransfer, "ExecuteDASInquiry RESPONSE - MGI.Cxn.MoneyTransfer.WU.Impl.BaseIO", mgiContext);
				#endregion
			}
			catch (FaultException<DAS.errorreply> ex)
			{
				//AL-3370 Transactional Log User Story				
				MongoDBLogger.Error<DAS.h2hdasrequest>(request, "ExecuteDASInquiry", AlloyLayerName.CXN, ModuleName.MoneyTransfer,
					"Error in ExecuteDASInquiry - MGI.Cxn.MoneyTransfer.WU.Impl.BaseIO", ex.Message, ex.StackTrace);
				string code = ExceptionHelper.GetProviderErrorCode(ex.Detail.error);
				throw new MoneyTransferProviderException(code, ex.Detail.error, ex);
			}
			catch (Exception ex)
			{
				MongoDBLogger.Error<DAS.h2hdasrequest>(request, "ExecuteDASInquiry", AlloyLayerName.CXN, ModuleName.MoneyTransfer,
					"Error in ExecuteDASInquiry - MGI.Cxn.MoneyTransfer.WU.Impl.BaseIO", ex.Message, ex.StackTrace);
				HandleException(ex);
				throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_EXECUTEDASINQUIRY_FAILED, ex);
			}
			return response;
		}


		internal void BuildDASObjects(WUBaseRequestResponse wuObjects, ref DAS.channel channel, ref DAS.foreign_remote_system foreignRemoteSystem)
		{
			channel = AutoMapper.Mapper.Map<DAS.channel>(wuObjects.Channel);
			foreignRemoteSystem = AutoMapper.Mapper.Map<DAS.foreign_remote_system>(wuObjects.ForeignRemoteSystem);
			BuildWUCommonObjects(wuObjects);
		}

		internal void BuildFeeInquiryObjects(WUBaseRequestResponse wuObjects, ref FeeInquiry.channel channel,
			ref FeeInquiry.foreign_remote_system foreignRemoteSystem)
		{
			channel = AutoMapper.Mapper.Map<FeeInquiry.channel>(wuObjects.Channel);
			foreignRemoteSystem = AutoMapper.Mapper.Map<FeeInquiry.foreign_remote_system>(wuObjects.ForeignRemoteSystem);
			BuildWUCommonObjects(wuObjects);
		}

		internal void BuildValidationObjects(WUBaseRequestResponse wuObjects, ref SendMoneyValidation.channel channel,
			ref SendMoneyValidation.foreign_remote_system foreignRemoteSystem)
		{
			channel = AutoMapper.Mapper.Map<SendMoneyValidation.channel>(wuObjects.Channel);
			foreignRemoteSystem = AutoMapper.Mapper.Map<SendMoneyValidation.foreign_remote_system>(wuObjects.ForeignRemoteSystem);
			BuildWUCommonObjects(wuObjects);
		}

		internal void BuildReceiveMoneySearchObjects(WUBaseRequestResponse wuObjects, ref ReceiveMoneySearch.channel channel,
			ref ReceiveMoneySearch.foreign_remote_system foreignRemoteSystem)
		{
			channel = AutoMapper.Mapper.Map<ReceiveMoneySearch.channel>(wuObjects.Channel);
			foreignRemoteSystem = AutoMapper.Mapper.Map<ReceiveMoneySearch.foreign_remote_system>(wuObjects.ForeignRemoteSystem);
			BuildWUCommonObjects(wuObjects);
		}

		internal void BuildReceiveMoneyPayObjects(WUBaseRequestResponse wuObjects, ref ReceiveMoneyPay.channel channel,
			ref ReceiveMoneyPay.foreign_remote_system foreignRemoteSystem)
		{
			channel = AutoMapper.Mapper.Map<ReceiveMoneyPay.channel>(wuObjects.Channel);
			foreignRemoteSystem = AutoMapper.Mapper.Map<ReceiveMoneyPay.foreign_remote_system>(wuObjects.ForeignRemoteSystem);
			BuildWUCommonObjects(wuObjects);
		}

		internal void BuildModifySearchObjects(WUBaseRequestResponse wuObjects, ref ModifySendMoneySearch.channel channel,
			ref ModifySendMoneySearch.foreign_remote_system foreignRemoteSystem)
		{
			channel = AutoMapper.Mapper.Map<ModifySendMoneySearch.channel>(wuObjects.Channel);
			foreignRemoteSystem = AutoMapper.Mapper.Map<ModifySendMoneySearch.foreign_remote_system>(wuObjects.ForeignRemoteSystem);
			BuildWUCommonObjects(wuObjects);
		}

		internal void BuildRefundSearchObjects(WUBaseRequestResponse wuObjects, ref Search.channel channel,
			ref Search.foreign_remote_system foreignRemoteSystem)
		{
			channel = AutoMapper.Mapper.Map<Search.channel>(wuObjects.Channel);
			foreignRemoteSystem = AutoMapper.Mapper.Map<Search.foreign_remote_system>(wuObjects.ForeignRemoteSystem);
			BuildWUCommonObjects(wuObjects);
		}

		internal void BuildSendMoneyStoreObjects(WUBaseRequestResponse wuObjects, ref SendMoneyStore.channel channel,
			ref SendMoneyStore.foreign_remote_system foreignRemoteSystem)
		{
			channel = AutoMapper.Mapper.Map<SendMoneyStore.channel>(wuObjects.Channel);
			foreignRemoteSystem = AutoMapper.Mapper.Map<SendMoneyStore.foreign_remote_system>(wuObjects.ForeignRemoteSystem);
			BuildWUCommonObjects(wuObjects);
		}

		internal void BuildModifySendMoneyObjects(WUBaseRequestResponse wuObjects, ref ModifySendMoney.channel channel,
			ref ModifySendMoney.foreign_remote_system foreignRemoteSystem)
		{
			channel = AutoMapper.Mapper.Map<ModifySendMoney.channel>(wuObjects.Channel);
			foreignRemoteSystem = AutoMapper.Mapper.Map<ModifySendMoney.foreign_remote_system>(wuObjects.ForeignRemoteSystem);
			BuildWUCommonObjects(wuObjects);
		}

		internal void BuildRefundSendMoneyObjects(WUBaseRequestResponse wuObjects, ref SendMoneyRefund.channel channel,
			ref SendMoneyRefund.foreign_remote_system foreignRemoteSystem)
		{
			channel = AutoMapper.Mapper.Map<SendMoneyRefund.channel>(wuObjects.Channel);
			foreignRemoteSystem = AutoMapper.Mapper.Map<SendMoneyRefund.foreign_remote_system>(wuObjects.ForeignRemoteSystem);
			BuildWUCommonObjects(wuObjects);
		}

		internal void BuildPayStatusObjects(WUBaseRequestResponse wuObjects, ref SendMoneyPayStatus.channel channel,
			ref SendMoneyPayStatus.foreign_remote_system foreignRemoteSystem)
		{
			channel = AutoMapper.Mapper.Map<SendMoneyPayStatus.channel>(wuObjects.Channel);
			foreignRemoteSystem = AutoMapper.Mapper.Map<SendMoneyPayStatus.foreign_remote_system>(wuObjects.ForeignRemoteSystem);
			BuildWUCommonObjects(wuObjects);
		}

		internal void BuildWUCommonObjects(WUBaseRequestResponse wuObjects)
		{
			_certificate = wuObjects.ClientCertificate;
			_serviceUrl = wuObjects.ServiceUrl;
		}

		public void PopulateWUObjects(long channelPartnerId, MGIContext mgiContext)
		{
			mgiContext.ProductType = "sendmoney";
			_response = WUCommon.CreateRequest(channelPartnerId, mgiContext);
		}

		#region Build Methods
		internal IEnumerable<string> MessageBlockSplit(string strMessage)
		{
			var length = 69;
			for (int i = 0; i < strMessage.Length; i += length)
			{
				yield return strMessage.Substring(i, Math.Min((strMessage.Length - i), Math.Min(length, strMessage.Length)));
			}
		}

		internal string GetCountryName(string countyCode)
		{
			string countryName = string.Empty;

			if (!string.IsNullOrWhiteSpace(countyCode))
			{
				var country = WUCountryRepo.FindBy(c => c.CountryCode == countyCode);
				if (country != null)
				{
					countryName = country.Name;
				}
			}

			return countryName;
		}

		internal string GetStateCode(string stateName)
		{
			string stateCode = string.Empty;

			if (!string.IsNullOrWhiteSpace(stateName))
			{
				var state = WUStateRepo.FindBy(c => c.Name == stateName);
				if (state != null)
				{
					stateCode = state.StateCode;
				}
			}

			return stateCode;
		}

		internal string GetCountryCode(string countyName)
		{
			string countryCode = string.Empty;

			if (!string.IsNullOrWhiteSpace(countyName))
			{
				var country = WUCountryRepo.FindBy(c => c.Name == countyName);
				if (country != null)
				{
					countryCode = country.CountryCode;
				}
			}

			return countryCode;
		}
		#endregion

		public void HandleException(Exception ex)
		{
			Exception faultException = ex as FaultException;
			if (faultException != null)
			{
				throw new MoneyTransferProviderException(MoneyTransferProviderException.PROVIDER_FAULT_ERROR, string.Empty, faultException);
			}
			Exception endpointException = ex as EndpointNotFoundException;
			if (endpointException != null)
			{
				throw new MoneyTransferProviderException(MoneyTransferProviderException.PROVIDER_ENDPOINTNOTFOUND_ERROR, string.Empty, endpointException);
			}
			Exception commException = ex as CommunicationException;
			if (commException != null)
			{
				throw new MoneyTransferProviderException(MoneyTransferProviderException.PROVIDER_COMMUNICATION_ERROR, string.Empty, commException);
			}
			Exception timeOutException = ex as TimeoutException;
			if (timeOutException != null)
			{
				throw new MoneyTransferProviderException(MoneyTransferProviderException.PROVIDER_TIMEOUT_ERROR, string.Empty, timeOutException);
			}
		}
	}
}
