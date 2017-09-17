using AutoMapper;
using MGI.Common.DataAccess.Contract;
using MGI.Common.Util;
using MGI.Cxn.WU.Common.Contract;
using MGI.Cxn.WU.Common.Data;
using MGI.Cxn.WU.Common.DASService;
using MGI.Cxn.WU.Common.WUCardLookupService;
using MGI.Cxn.WU.Common.WUCardService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using MGI.Common.TransactionalLogging.Data;

namespace MGI.Cxn.WU.Common.Impl
{
	public class BaseIO 
	{
		public const int OCCUPATION_LENGTH = 29;
		public IRepository<WUCredential> WUCredentialRepo { get; set; }
		public IRepository<Country> WUCountryRepo { get; set; }

		public bool IsHardCodedCounterId { get; set; }
		public TLoggerCommon MongoDBLogger { get; set; }
        internal X509Certificate2 WUCertificate = null;

		#region Constructor

		public BaseIO()
		{
			Mapper.CreateMap<BANNERMESSAGE_Type, AgentBanners>();
			//US2054
			Mapper.CreateMap<SwbFlaInfo, WUCardService.swb_fla_info>()
				.ForMember(d => d.swb_operator_id, s => s.MapFrom(c => c.SwbOperatorId))
				.ForMember(d => d.read_privacynotice_flag, s => s.MapFrom(x => x.ReadPrivacyNoticeFlag))
				.ForMember(d => d.read_privacynotice_flagSpecified, s => s.MapFrom(x => x.FlagCertificationFlagSpecified))
				.ForMember(d => d.fla_certification_flag, s => s.MapFrom(x => x.FlagCertificationFlag))
				.ForMember(d => d.fla_certification_flagSpecified, s => s.MapFrom(x => x.ReadPrivacyNoticeFlagSpecified));

			Mapper.CreateMap<GeneralName, WUCardService.general_name>()
				.ForMember(d => d.name_type, s => s.MapFrom(x => x.Type))
				.ForMember(d => d.name_typeSpecified, s => s.MapFrom(x => x.NameTypeSpecified))
				.ForMember(d => d.first_name, s => s.MapFrom(x => x.FirstName))
				.ForMember(d => d.last_name, s => s.MapFrom(x => x.LastName));

			Mapper.CreateMap<SwbFlaInfo, WUCustomerLookup.swb_fla_info>()
				.ForMember(d => d.swb_operator_id, s => s.MapFrom(c => c.SwbOperatorId))
				.ForMember(d => d.read_privacynotice_flag, s => s.MapFrom(x => x.ReadPrivacyNoticeFlag))
				.ForMember(d => d.read_privacynotice_flagSpecified, s => s.MapFrom(x => x.FlagCertificationFlagSpecified))
				.ForMember(d => d.fla_certification_flag, s => s.MapFrom(x => x.FlagCertificationFlag))
				.ForMember(d => d.fla_certification_flagSpecified, s => s.MapFrom(x => x.ReadPrivacyNoticeFlagSpecified));

			Mapper.CreateMap<GeneralName, WUCustomerLookup.general_name>()
				.ForMember(d => d.name_type, s => s.MapFrom(x => x.Type))
				.ForMember(d => d.name_typeSpecified, s => s.MapFrom(x => x.NameTypeSpecified))
				.ForMember(d => d.first_name, s => s.MapFrom(x => x.FirstName))
				.ForMember(d => d.last_name, s => s.MapFrom(x => x.LastName));

			Mapper.CreateMap<SwbFlaInfo, WUCardLookupService.swb_fla_info>()
				.ForMember(d => d.swb_operator_id, s => s.MapFrom(c => c.SwbOperatorId))
				.ForMember(d => d.read_privacynotice_flag, s => s.MapFrom(x => x.ReadPrivacyNoticeFlag))
				.ForMember(d => d.read_privacynotice_flagSpecified, s => s.MapFrom(x => x.FlagCertificationFlagSpecified))
				.ForMember(d => d.fla_certification_flag, s => s.MapFrom(x => x.FlagCertificationFlag))
				.ForMember(d => d.fla_certification_flagSpecified, s => s.MapFrom(x => x.ReadPrivacyNoticeFlagSpecified));

			Mapper.CreateMap<GeneralName, WUCardLookupService.general_name>()
				.ForMember(d => d.name_type, s => s.MapFrom(x => x.Type))
				.ForMember(d => d.name_typeSpecified, s => s.MapFrom(x => x.NameTypeSpecified))
				.ForMember(d => d.first_name, s => s.MapFrom(x => x.FirstName))
				.ForMember(d => d.last_name, s => s.MapFrom(x => x.LastName));
		}

		#endregion

		# region Public method

		internal List<object> getdasResponse(long transactionId, string dasServiceName, long channelPartnerId, MGIContext mgiContext, filters_type queryfilters = null)
		{

			WUCredential wuCredentials = new WUCredential();
			wuCredentials = WUCredentialRepo.FindBy(x => x.ChannelPartnerId == channelPartnerId);
			//ConfigureWUOject(wuCredentials, context);

			string locationId = GetCounterId(wuCredentials, mgiContext);
			List<object> responseList = new List<object>();
			DASInquiryPortTypeClient dc = new DASInquiryPortTypeClient("SOAP_HTTP_Port2", wuCredentials.WUServiceUrl.ToString());

			WUCertificate = WUCertificate == null ? GetWUCredentialCertificate(wuCredentials) : WUCertificate;
			dc.ClientCredentials.ClientCertificate.Certificate = WUCertificate;

			h2hdasrequest request = new h2hdasrequest();
			DASService.channel channel = new DASService.channel();
			channel.type = DASService.channel_type.H2H;
			channel.name = wuCredentials.ChannelName;
			channel.version = wuCredentials.ChannelVersion;
			channel.typeSpecified = true;
			request.channel = channel;

			DASService.foreign_remote_system remotesystem = new DASService.foreign_remote_system();
			remotesystem.reference_no = transactionId.ToString();
			remotesystem.identifier = wuCredentials.AccountIdentifier;
			remotesystem.counter_id = locationId;
			//remotesystem.identifier = "USATEST"; //TODO: remove hard coded USATEST wuCredentials.AccountIdentifier;
			//remotesystem.counter_id = "USATEST"; //TODO: remove hard coded USATEST wuCredentials.CounterId;
			request.foreign_remote_system = remotesystem;
			request.name = dasServiceName;
			request.filters = queryfilters;
			REPLYType responseItems = null;
			try
			{
				#region AL-1014 Transactional Log User Story
				MongoDBLogger.Info<h2hdasrequest>(mgiContext.CustomerSessionId, request, "getdasResponse", AlloyLayerName.CXN,
                    ModuleName.Transaction, "getdasResponse -MGI.Cxn.WU.Common.Impl.BaseIO", "REQUEST", mgiContext);
				#endregion
				responseItems = (REPLYType)dc.DAS_Service(request).MTML.Item;
			}
			catch (FaultException<DASService.errorreply> ex)
			{
				//AL-1014 Transactional Log User Story
				List<string> details = new List<string>();
				details.Add("Transaction Id:" + Convert.ToString(transactionId));
				details.Add("DasService Name:" + dasServiceName);
				details.Add("Transaction Id:" + Convert.ToString(channelPartnerId));
				MongoDBLogger.ListError<string>(details, "getdasResponse", AlloyLayerName.CXN, ModuleName.Transaction,
					"Error in getdasResponse -MGI.Cxn.WU.Common.Impl.BaseIO", ex.Message, ex.StackTrace);
				throw new WUCommonException(WUCommonException.PROVIDER_ERROR, ex.Detail.error, ex);
			}

			if (responseItems.DATA_CONTEXT.RECORDSET != null)
				responseList.AddRange(responseItems.DATA_CONTEXT.RECORDSET.Items.ToList<object>());

            #region AL-1014 Transactional Log User Story
			MongoDBLogger.Info<REPLYType>(mgiContext.CustomerSessionId, responseItems, "getdasResponse", AlloyLayerName.CXN,
				ModuleName.Transaction, "getdasResponse -MGI.Cxn.WU.Common.Impl.BaseIO", "RESPONSE", mgiContext);
			#endregion
			return responseList;
		}
		public CardInfo GetCardInfo(CardLookUpRequest wuCardLookupReq, MGIContext mgiContext)
		{

			CardInfo CardInfo = new CardInfo();
			CardInfo = null;

			WUCardLookupService.channel lookupChannel = null;
			WUCardLookupService.foreign_remote_system lookupRemoteSystem = null;

			try
			{
				WUCardLookupService.wucardlookuprequest cardLookupRequest = new WUCardLookupService.wucardlookuprequest();

				WUCredential wuCredentials = null;
				string errorMessage = string.Format("Error while retrieving credentials for channel partner: {0}", mgiContext.ChannelPartnerId);
				wuCredentials = WUCredentialRepo.FindBy(x => x.ChannelPartnerId == mgiContext.ChannelPartnerId);
				if (wuCredentials == null)
					throw new WUCommonException(WUCommonException.WESTERNUNION_CREDENTIALS_NOTFOUND, errorMessage);

				List<object> responseList = new List<object>();
				WUCardLookupService.WUCardLookupPortTypeClient WUPortTypeClient = new WUCardLookupService.WUCardLookupPortTypeClient("WUCardLookup", wuCredentials.WUServiceUrl.ToString());
				WUCertificate = WUCertificate == null ? GetWUCredentialCertificate(wuCredentials) : WUCertificate;
				WUPortTypeClient.ClientCredentials.ClientCertificate.Certificate = WUCertificate;
				cardLookupRequest = WUCardLookupRequestMapper(wuCardLookupReq);

				lookupChannel = GetCardLookupChannel(wuCredentials);
				lookupRemoteSystem = GetCardLookupRemoteSystem(wuCredentials, mgiContext);

				cardLookupRequest.channel = lookupChannel;
				lookupRemoteSystem.reference_no = DateTime.Now.ToString("yyyyMMddhhmmssff");
				cardLookupRequest.foreign_remote_system = lookupRemoteSystem;
				cardLookupRequest.swb_fla_info = Mapper.Map<SwbFlaInfo, WUCardLookupService.swb_fla_info>(BuildSwbFlaInfo(mgiContext));
				cardLookupRequest.swb_fla_info.fla_name = Mapper.Map<GeneralName, WUCardLookupService.general_name>(BuildGeneralName(mgiContext));
               
                #region AL-1014 Transactional Log User Story
                MongoDBLogger.Info<WUCardLookupService.wucardlookuprequest>(mgiContext.CustomerSessionId, cardLookupRequest, "GetCardInfo", AlloyLayerName.CXN,
                    ModuleName.BillPayment, "GetCardInfo -MGI.Cxn.WU.Common.Impl.BaseIO", "REQUEST", mgiContext);
				#endregion
				WUCardLookupService.wucardlookupreply response = WUPortTypeClient.WuCardLookup(cardLookupRequest);

                #region AL-1014 Transactional Log User Story
                MongoDBLogger.Info<WUCardLookupService.wucardlookupreply>(mgiContext.CustomerSessionId, response, "GetCardInfo", AlloyLayerName.CXN,
                    ModuleName.BillPayment, "GetCardInfo -MGI.Cxn.WU.Common.Impl.BaseIO", "RESPONSE", mgiContext);
				#endregion
				CardInfo = CardLookUpResponse(response);
			}
			catch (System.ServiceModel.FaultException<WUCardLookupService.errorreply> ex)
			{
				//AL-1014 Transactional Log User Story
				MongoDBLogger.Error<CardLookUpRequest>(wuCardLookupReq, "GetCardInfo", AlloyLayerName.CXN, ModuleName.Transaction,
				"Error in GetCardInfo -MGI.Cxn.WU.Common.Impl.BaseIO", ex.Message, ex.StackTrace);
				throw new WUCommonException(WUCommonException.PROVIDER_ERROR, ex.Detail.error, ex);
			}
			catch (Exception ex)
			{
				//AL-1014 Transactional Log User Story
				MongoDBLogger.Error<CardLookUpRequest>(wuCardLookupReq, "GetCardInfo", AlloyLayerName.CXN, ModuleName.Transaction,
					"Error in GetCardInfo -MGI.Cxn.WU.Common.Impl.BaseIO", ex.Message, ex.StackTrace);
				throw new WUCommonException(WUCommonException.PROVIDER_ERROR, ex.Message, ex);
			}
			return CardInfo;
		}
        	

		/// <summary>
		/// US2054
		///WUSouth West Border Location States
		/// </summary>
		/// <param name="stateCode"></param>
		/// <returns></returns>
		public bool IsSWBState(string stateCode)
		{
			bool IsSWBState = false;
			string[] swbStates = { "AZ", "CA", "NM", "TX" };
			if (!string.IsNullOrWhiteSpace(stateCode))
			{
				IsSWBState = swbStates.Contains(stateCode);
			}
			return IsSWBState;
		}
        	
		internal List<Receiver> WUTotalReceivers(List<WUCardLookupService.receiver> receivers)
		{
			List<Receiver> receiversList;

			try
			{
				receiversList = new List<Receiver>();

				foreach (MGI.Cxn.WU.Common.WUCardLookupService.receiver receiversFromReply in receivers)
				{
					Receiver receiver = new Receiver();

					if (receivers.Count > 0)
					{
						// if the WUCustomerLookup.name_type == "C" it means the Billers, WUCustomerLookup.name_type == "D" it means Receivers and WUCustomerLookup.name_type == "M" it means Receivers with maternal and paternal name.
						switch (receiversFromReply.name.name_type)
						{
							case MGI.Cxn.WU.Common.WUCardLookupService.name_type.C:
								if (receiversFromReply.name.business_name != string.Empty && receiversFromReply.debtor_account_number != string.Empty)
								{
									receiver.BusinessName = string.IsNullOrWhiteSpace(receiversFromReply.name.business_name) ? string.Empty : receiversFromReply.name.business_name;
									receiver.Attention = string.IsNullOrWhiteSpace(receiversFromReply.debtor_account_number) ? string.Empty : receiversFromReply.debtor_account_number;
								}
								break;

							case MGI.Cxn.WU.Common.WUCardLookupService.name_type.D:
								if (!string.IsNullOrWhiteSpace(receiversFromReply.name.first_name) && !string.IsNullOrWhiteSpace(receiversFromReply.name.last_name) && !string.IsNullOrWhiteSpace(receiversFromReply.address.Item.iso_code.country_code))
								{
									receiver.FirstName = string.IsNullOrWhiteSpace(receiversFromReply.name.first_name) ? string.Empty : receiversFromReply.name.first_name;
									receiver.LastName = string.IsNullOrWhiteSpace(receiversFromReply.name.last_name) ? string.Empty : receiversFromReply.name.last_name;
								}

								break;

							case MGI.Cxn.WU.Common.WUCardLookupService.name_type.M:
								if (!string.IsNullOrWhiteSpace(receiversFromReply.name.given_name.Trim()) && !string.IsNullOrWhiteSpace(receiversFromReply.name.paternal_name) && !string.IsNullOrWhiteSpace(receiversFromReply.name.maternal_name) && !string.IsNullOrWhiteSpace(receiversFromReply.address.Item.iso_code.country_code))
								{
									receiver.FirstName = string.IsNullOrWhiteSpace(receiversFromReply.name.given_name) ? string.Empty : receiversFromReply.name.given_name.Trim();
									receiver.LastName = string.IsNullOrWhiteSpace(receiversFromReply.name.paternal_name) ? string.Empty : receiversFromReply.name.paternal_name.Trim();
									receiver.SecondLastName = string.IsNullOrWhiteSpace(receiversFromReply.name.maternal_name) ? string.Empty : receiversFromReply.name.maternal_name.Trim();
								}
								break;

							default:
								break;
						}

						receiver.Address = new Address();
						receiver.Address.item = new CountryCurrencyInfo();
						receiver.Address.item.country_code = string.IsNullOrWhiteSpace(receiversFromReply.address.Item.iso_code.country_code) ? string.Empty : receiversFromReply.address.Item.iso_code.country_code;
						receiver.Address.item.country_name = string.IsNullOrWhiteSpace(receiversFromReply.address.Item.iso_code.currency_code) ? string.Empty : receiversFromReply.address.Item.iso_code.currency_code;
						receiver.ReceiverIndexNumber = string.IsNullOrWhiteSpace(receiversFromReply.receiver_index_no) ? string.Empty : receiversFromReply.receiver_index_no;
						receiver.NameType = (WUEnums.name_type)receiversFromReply.name.name_type;
						receiver.ReceiverIndexNumber = receiversFromReply.receiver_index_no.ToString() != String.Empty ? receiversFromReply.receiver_index_no.ToString() : String.Empty;
						receiver.Type = receiversFromReply.type;
					}

					receiversList.Add(receiver);

				}
			}
			catch (Exception ex)
			{
				//AL-1014 Transactional Log User Story
				MongoDBLogger.Error<List<WUCardLookupService.receiver>>(receivers, "WUTotalReceivers", AlloyLayerName.CXN, ModuleName.Transaction,
					"Error in WUTotalReceivers -MGI.Cxn.WU.Common.Impl.BaseIO", ex.Message, ex.StackTrace);
			
				throw new WUCommonException(WUCommonException.PROVIDER_ERROR, ex.Message, ex);
			}

			//WU Response will be converted to the CXN Specific Format and returns the List of Receivers from WU Service.
			return receiversList;
		}
		#endregion

		#region

		internal MGI.Cxn.WU.Common.WUCardService.foreign_remote_system GetCardServiceRemoteSystem(WUCredential wuCredentials, MGIContext mgiContext)
		{
			string locationId = GetCounterId(wuCredentials, mgiContext);
			if (string.IsNullOrWhiteSpace(wuCredentials.AccountIdentifier) || string.IsNullOrWhiteSpace(locationId))
			{
				throw new WUCommonException(WUCommonException.INVALID_ACCOUNTIDENTIFIER_COUNTERID, "Invalid AccountIdentifier or Location Id. AccountIdentier or Location Id cannot be null or empty string");
			}
			MGI.Cxn.WU.Common.WUCardService.foreign_remote_system frs = new MGI.Cxn.WU.Common.WUCardService.foreign_remote_system();
			frs.identifier = wuCredentials.AccountIdentifier;
			frs.counter_id = locationId;
			return frs;
		}
        internal MGI.Cxn.WU.Common.WUCardService.channel GetCardServiceChannel(WUCredential wuCredentials)
		{
			MGI.Cxn.WU.Common.WUCardService.channel chn = new MGI.Cxn.WU.Common.WUCardService.channel();
			chn.type = WUCardService.channel_type.H2H;
			chn.name = wuCredentials.ChannelName.ToString();
			chn.version = wuCredentials.ChannelVersion.ToString();
			chn.typeSpecified = true;
			return chn;
		}
        internal X509Certificate2 GetWUCredentialCertificate(WUCredential wuCredentials)
		{
			//TODO: Check where the certificate will be stored.
			// get the cert from personel store, if it is installed in different store, then what???

			X509Store certificateStore = new X509Store(StoreName.My, StoreLocation.LocalMachine);

			// Open the store.
			certificateStore.Open(OpenFlags.ReadWrite | OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);

			// Find the certificate with the specified subject.
			X509Certificate2Collection certificates = certificateStore.Certificates.Find(X509FindType.FindBySubjectName, wuCredentials.WUClientCertificateSubjectName, false);

			certificateStore.Close();

			if (certificates.Count < 1)
			{
				throw new WUCommonException(WUCommonException.CERIFICATE_NOTFOUND, "Certificate not found for WU Partner Integration");
			}

			return certificates[0];
		}

		//private string GetDeliveryTemplateIndex(Dictionary<string, object> context)
		//{
		//    return Convert.ToString(context["GetDeliveryTemplateIndex"]);
		//}

        internal wucardenrollmentrequest CardEnrollmentRequestMapper(Sender sender, PaymentDetails paymentDetails)
		{
			try
			{
				WUCardService.iso_code isoCode = new WUCardService.iso_code() { country_code = "US", currency_code = "USD" };

				wucardenrollmentrequest request = new wucardenrollmentrequest()
				{
					sender = new WUCardService.sender()
					{
						name = new WUCardService.general_name()
						{
							name_type = WUCardService.name_type.D,
							name_typeSpecified = true,
							first_name = string.IsNullOrEmpty(sender.FirstName) ? string.Empty : sender.FirstName,
							last_name = string.IsNullOrEmpty(sender.LastName) ? string.Empty : sender.LastName,
						},
						address = new WUCardService.address()
						{
							addr_line1 = sender.AddressAddrLine1,
							addr_line2 = sender.AddressAddrLine2,
							city = sender.AddressCity,
							state = sender.AddressState,
							postal_code = sender.AddressPostalCode,
							Item = new WUCardService.country_currency_info() { iso_code = isoCode }
						},
						contact_phone = sender.ContactPhone,
						email = sender.Email
					},
					payment_details = new WUCardService.payment_details()
					{
						recording_country_currency = new WUCardService.country_currency_info() { iso_code = isoCode },
						destination_country_currency = new WUCardService.country_currency_info() { iso_code = isoCode },
						originating_country_currency = new WUCardService.country_currency_info() { iso_code = isoCode },
						original_destination_country_currency = new WUCardService.country_currency_info() { iso_code = isoCode },
					}
				};
				return request;
			}
			catch (Exception ex)
			{
				//AL-1014 Transactional Log User Story
				MongoDBLogger.Error<Sender>(sender, "CardEnrollmentRequestMapper", AlloyLayerName.CXN, ModuleName.Transaction,
					"Error in CardEnrollmentRequestMapper -MGI.Cxn.WU.Common.Impl.BaseIO", ex.Message, ex.StackTrace);

				throw new WUCommonException(WUCommonException.PROVIDER_ERROR, ex.Message, ex);
			}
		}

        internal CardDetails CardEnrollmentResponse(wucardenrollmentreply response)
		{

			CardDetails cardDetails = new CardDetails();
			cardDetails = null;
			try
			{
				cardDetails.AccountNumber = response.sender.preferred_customer.account_nbr;
				cardDetails.ForiegnSystemId = response.foreign_remote_system.identifier;
				cardDetails.ForiegnRefNum = response.foreign_remote_system.reference_no;
				cardDetails.CounterId = response.foreign_remote_system.counter_id;
			}
			catch (Exception ex)
			{
				//AL-1014 Transactional Log User Story
				MongoDBLogger.Error<wucardenrollmentreply>(response, "CardEnrollmentResponse", AlloyLayerName.CXN, ModuleName.Transaction,
					"Error in CardEnrollmentResponse -MGI.Cxn.WU.Common.Impl.BaseIO", ex.Message, ex.StackTrace);

				throw new WUCommonException(WUCommonException.PROVIDER_ERROR, ex.Message, ex);
			}
			return cardDetails;
		}

        internal WUCustomerLookup.wucustomerlookuprequest CardLookupRequestMapper(CardLookUpRequest wucardlookupreq, WUCustomerLookup.foreign_remote_system lookupRemoteSystem)
		{
			WUCustomerLookup.wucustomerlookuprequest request = new WUCustomerLookup.wucustomerlookuprequest();
			try
			{
				request.sender = new WUCustomerLookup.sender()
				{
					name = new WUCustomerLookup.general_name()
					{
						name_type = WUCustomerLookup.name_type.D,
						first_name = wucardlookupreq.firstname,
						last_name = wucardlookupreq.lastname
					}
				};
				WUCustomerLookup.foreign_remote_system frs = new WUCustomerLookup.foreign_remote_system();
				frs.counter_id = lookupRemoteSystem.counter_id;
				frs.identifier = lookupRemoteSystem.identifier;
				request.foreign_remote_system = frs;
				WUCustomerLookup.convenience_search senderConsearchRequest = new WUCustomerLookup.convenience_search();
				senderConsearchRequest.type = "SN";
				request.convenience_search = senderConsearchRequest;
				request.receiver_index_number = wucardlookupreq.receiver_index_number;
				request.wu_card_lookup_context = wucardlookupreq.wu_card_lookup_context;
				request.card_lookup_search_type = "M"; //wucardlookupreq.card_lookup_search_type;
				request.save_key = wucardlookupreq.save_key;
			}
			catch (Exception ex)
			{
				//AL-1014 Transactional Log User Story
				MongoDBLogger.Error<CardLookUpRequest>(wucardlookupreq, "CardLookupRequestMapper", AlloyLayerName.CXN, ModuleName.Transaction,
					"Error in CardLookupRequestMapper -MGI.Cxn.WU.Common.Impl.BaseIO", ex.Message, ex.StackTrace);

				throw new WUCommonException(WUCommonException.PROVIDER_ERROR, ex.Message, ex);
			}
			return request;
		}

        internal List<Sender> CardLookupResponse(WUCustomerLookup.wucustomerlookupreply response)
		{
			//CardLookupDetails cardLookupDetails = new CardLookupDetails();

			List<Sender> senderList = new List<Sender>();

			try
			{
				if (response.sender != null)
				{
					if (response.sender.Length > 0)
					{
						List<string> levelCodes = new List<string>() { "WU6", "WU7", "XXC", "SWP" };

						var senders = from s in response.sender
									  where !levelCodes.Contains(s.preferred_customer.level_code) && !s.preferred_customer.level_code.StartsWith("ZZ")
									  select s;

						//cardLookupDetails.Sender = new Sender[senders.Count()];
						foreach (MGI.Cxn.WU.Common.WUCustomerLookup.sender senderFromReply in senders)
						{
							Sender sender = new Sender();
							sender.FirstName = string.IsNullOrWhiteSpace(senderFromReply.name.first_name) ? string.Empty : senderFromReply.name.first_name;
							sender.LastName = string.IsNullOrWhiteSpace(senderFromReply.name.last_name) ? string.Empty : senderFromReply.name.last_name;
							sender.AddressPostalCode = string.IsNullOrWhiteSpace(senderFromReply.address.postal_code) ? string.Empty : senderFromReply.address.postal_code;
							sender.AddressAddrLine1 = string.IsNullOrWhiteSpace(senderFromReply.address.addr_line1) ? string.Empty : senderFromReply.address.addr_line1;
							sender.MobilePhone = string.IsNullOrWhiteSpace(senderFromReply.contact_phone.ToString()) ? string.Empty : senderFromReply.contact_phone.ToString();
							sender.PreferredCustomerAccountNumber = string.IsNullOrWhiteSpace(senderFromReply.preferred_customer.account_nbr) ? string.Empty : senderFromReply.preferred_customer.account_nbr;
							senderList.Add(sender);
						}
					}
				}
			}
			catch (Exception ex)
			{
				//AL-1014 Transactional Log User Story
				MongoDBLogger.Error<WUCustomerLookup.wucustomerlookupreply>(response, "CardLookupResponse", AlloyLayerName.CXN, ModuleName.Transaction,
					"Error in CardLookupResponse -MGI.Cxn.WU.Common.Impl.BaseIO", ex.Message, ex.StackTrace);

				throw new WUCommonException(WUCommonException.PROVIDER_ERROR, ex.Message, ex);
			}
			return senderList;
		}

		//US1784 WU Gold Card Name Matching
        internal WUCardLookupService.wucardlookuprequest WUCardLookupRequestMapper(CardLookUpRequest wucardlookupreq, WUCardLookupService.foreign_remote_system cardLookupRemoteSystem)
		{
			WUCardLookupService.wucardlookuprequest request = new WUCardLookupService.wucardlookuprequest();

			try
			{
				request.sender = new WUCardLookupService.sender()
				{

				};
				request.sender.name = new WUCardLookupService.general_name();
				request.sender.address = new WUCardLookupService.address();

				request.sender.preferred_customer = new WUCardLookupService.preferred_customer();
				request.sender.preferred_customer.account_nbr = wucardlookupreq.sender.PreferredCustomerAccountNumber;

				WUCardLookupService.foreign_remote_system frs = new WUCardLookupService.foreign_remote_system();
				frs.counter_id = cardLookupRemoteSystem.counter_id;
				frs.identifier = cardLookupRemoteSystem.identifier;
				request.foreign_remote_system = frs;
				WUCardLookupService.convenience_search senderConsearchRequest = new WUCardLookupService.convenience_search();
				request.convenience_search = senderConsearchRequest;
				request.receiver_index_number = wucardlookupreq.receiver_index_number;
				request.wu_card_lookup_context = wucardlookupreq.wu_card_lookup_context;
				request.card_lookup_search_type = "S";
				request.save_key = wucardlookupreq.save_key;
			}
			catch (Exception ex)
			{
				//AL-1014 Transactional Log User Story
				MongoDBLogger.Error<CardLookUpRequest>(wucardlookupreq, "WUCardLookupRequestMapper", AlloyLayerName.CXN, ModuleName.Transaction,
					"Error in WUCardLookupRequestMapper -MGI.Cxn.WU.Common.Impl.BaseIO", ex.Message, ex.StackTrace);


				throw new WUCommonException(WUCommonException.PROVIDER_ERROR, ex.Message, ex);
			}
			return request;
		}

		//US1784 WU Gold Card Name Matching
        internal CardLookupDetails WUCardLookupResponse(WUCardLookupService.wucardlookupreply response)
		{
			CardLookupDetails cardLookupDetails = new CardLookupDetails();
			try
			{
				if (response.sender != null)
				{
					if (response.sender.Length > 0)
					{
						if (response.wu_card != null)
						{
							cardLookupDetails.WuCardTotalPointsEarned = response.wu_card.total_points_earned;
						}

						cardLookupDetails.Sender = new Sender[1];

						cardLookupDetails.Sender[0] = new MGI.Cxn.WU.Common.Data.Sender();
						cardLookupDetails.Sender[0].FirstName = string.IsNullOrWhiteSpace(response.sender.First().name.first_name) ? string.Empty : response.sender.First().name.first_name;
						cardLookupDetails.Sender[0].LastName = string.IsNullOrWhiteSpace(response.sender.First().name.last_name) ? string.Empty : response.sender.First().name.last_name;
						cardLookupDetails.Sender[0].MiddleName = string.IsNullOrWhiteSpace(response.sender.First().name.middle_name) ? string.Empty : response.sender.First().name.middle_name;
					}
				}
			}
			catch (Exception ex)
			{
				//AL-1014 Transactional Log User Story
				MongoDBLogger.Error<WUCardLookupService.wucardlookupreply>(response, "WUCardLookupResponse", AlloyLayerName.CXN, ModuleName.Transaction,
					"Error in WUCardLookupResponse -MGI.Cxn.WU.Common.Impl.BaseIO", ex.Message, ex.StackTrace);

				throw new WUCommonException(WUCommonException.PROVIDER_ERROR, ex.Message, ex);
			}
			return cardLookupDetails;
		}

		internal MGI.Cxn.WU.Common.WUCustomerLookup.channel GetLookupChannel(WUCredential wuCredentials)
		{
			MGI.Cxn.WU.Common.WUCustomerLookup.channel chn = new MGI.Cxn.WU.Common.WUCustomerLookup.channel();
			chn.type = WUCustomerLookup.channel_type.H2H;
			chn.name = wuCredentials.ChannelName.ToString();
			chn.version = wuCredentials.ChannelVersion.ToString();
			chn.typeSpecified = true;
			return chn;
		}
		internal MGI.Cxn.WU.Common.WUCustomerLookup.foreign_remote_system GetLookupRemoteSystem(WUCredential wuCredentials, MGIContext mgiContext)
		{
			string locationId = GetCounterId(wuCredentials, mgiContext);
			if (string.IsNullOrWhiteSpace(wuCredentials.AccountIdentifier) || string.IsNullOrWhiteSpace(locationId))
			{
				throw new WUCommonException(WUCommonException.INVALID_ACCOUNTIDENTIFIER_COUNTERID, "Invalid AccountIdentifier or Location Id. AccountIdentier or Location Id cannot be null or empty string");
			}
			MGI.Cxn.WU.Common.WUCustomerLookup.foreign_remote_system frs = new MGI.Cxn.WU.Common.WUCustomerLookup.foreign_remote_system();
			frs.identifier = wuCredentials.AccountIdentifier;
			frs.counter_id = locationId;
			return frs;
		}

        internal WUCardLookupService.channel GetCardLookupChannel(WUCredential wuCredentials)
		{
			return new WUCardLookupService.channel()
			{
				type = WUCardLookupService.channel_type.H2H,
				typeSpecified = true,
				name = wuCredentials.ChannelName,
				version = wuCredentials.ChannelVersion,
			};
		}
		internal WUCardLookupService.foreign_remote_system GetCardLookupRemoteSystem(WUCredential wuCredentials, MGIContext mgiContext)
		{
			string locationId = GetCounterId(wuCredentials, mgiContext);
			if (string.IsNullOrWhiteSpace(wuCredentials.AccountIdentifier) || string.IsNullOrWhiteSpace(locationId))
			{
				throw new WUCommonException(WUCommonException.INVALID_ACCOUNTIDENTIFIER_COUNTERID, "Invalid AccountIdentifier or Location Id.AccountIdentier or Location Id cannot be null or empty string");
			}

			return new WUCardLookupService.foreign_remote_system()
		{
			identifier = wuCredentials.AccountIdentifier,
			counter_id = locationId,
		};
		}
		#endregion

        internal CardLookUpRequest GetWUCardLookUpRequest(CardLookUpRequest wucardlookuprequest)
        {
            wucardlookuprequest.firstname = NexxoUtil.MassagingValue(wucardlookuprequest.firstname);
            wucardlookuprequest.lastname = NexxoUtil.MassagingValue(wucardlookuprequest.lastname);
            wucardlookuprequest.midname = NexxoUtil.MassagingValue(wucardlookuprequest.midname);

            wucardlookuprequest.sender.AddressAddrLine1 = NexxoUtil.MassagingValue(wucardlookuprequest.sender.AddressAddrLine1);
            wucardlookuprequest.sender.AddressAddrLine2 = NexxoUtil.MassagingValue(wucardlookuprequest.sender.AddressAddrLine2);
            wucardlookuprequest.sender.AddressCity = NexxoUtil.MassagingValue(wucardlookuprequest.sender.AddressCity);
            wucardlookuprequest.sender.AddressState = NexxoUtil.MassagingValue(wucardlookuprequest.sender.AddressState);
            wucardlookuprequest.sender.AddressStreet = NexxoUtil.MassagingValue(wucardlookuprequest.sender.AddressStreet);
            wucardlookuprequest.sender.CountryName = NexxoUtil.MassagingValue(wucardlookuprequest.sender.CountryName);
            wucardlookuprequest.sender.FirstName = NexxoUtil.MassagingValue(wucardlookuprequest.sender.FirstName);
            wucardlookuprequest.sender.LastName = NexxoUtil.MassagingValue(wucardlookuprequest.sender.LastName);
            wucardlookuprequest.sender.MiddleName = NexxoUtil.MassagingValue(wucardlookuprequest.sender.MiddleName);
            return wucardlookuprequest;

        }
        internal WUCredential GetCredential(long channelPartnerId)
		{
			string errorMessage = string.Format("Error while retrieving credentials for channel partner: {0}", channelPartnerId);

			WUCredential wuCredential = WUCredentialRepo.FindBy(x => x.ChannelPartnerId == channelPartnerId);

			if (wuCredential == null)
				throw new WUCommonException(WUCommonException.WESTERNUNION_CREDENTIALS_NOTFOUND, errorMessage);

			return wuCredential;
		}

		public WUBaseRequestResponse CreateRequest(long channelPartnerId, MGIContext mgiContext)
		{
			WUBaseRequestResponse request = new WUBaseRequestResponse();
			WUCredential credential = GetCredential(channelPartnerId);

			string locationId = GetCounterId(credential, mgiContext);

			request.Channel = new Channel() { Name = credential.ChannelName, Version = credential.ChannelVersion };
			request.ForeignRemoteSystem = new ForeignRemoteSystem() { CoutnerId = locationId, Identifier = credential.AccountIdentifier };
			WUCertificate = WUCertificate == null ? GetWUCredentialCertificate(credential) : WUCertificate;

			request.ClientCertificate = WUCertificate;

			request.ServiceUrl = credential.WUServiceUrl;
			return request;
		}

        internal MGI.Cxn.WU.Common.Data.CardDetails EnrollCardResponseMapper(wucardenrollmentreply response, CardDetails cardDetails)
		{
			cardDetails = new CardDetails()
			{
				AccountNumber = response.sender.preferred_customer.account_nbr,
				ForiegnSystemId = response.foreign_remote_system.identifier,
				ForiegnRefNum = response.foreign_remote_system.reference_no,
				CounterId = response.foreign_remote_system.counter_id
			};
			return cardDetails;
		}

		public string GetGovtIDType(string idType)
		{
			Dictionary<string, string> govtIdTypeMapping = new Dictionary<string, string>()
			{
				{"SSN", "1"},
				{"DRIVER'S LICENSE", "1"},
				{"EMPLOYMENT AUTHORIZATION CARD (EAD)", "4"},
				{"GREEN CARD / PERMANENT RESIDENT CARD", "5"},
				{"MILITARY ID", "7"},
				{"PASSPORT", "2"},
				{"U.S. STATE IDENTITY CARD", "3"},
				{"INSTITUTO FEDERAL ELECTORAL", "8"},
				{"LICENCIA DE CONDUCIR", "6"},
				{"MATRICULA CONSULAR", "9"},
				{"NEW YORK BENEFITS ID","3"},
				{"NEW YORK CITY ID","3"}
			};

			return govtIdTypeMapping[idType];
		}

		internal string GetCounterId(WUCredential wuCredential, MGIContext mgiContext)
		{
			if (IsHardCodedCounterId)
			{
				return wuCredential.CounterId;
			}
			else
			{
				return mgiContext.WUCounterId;
			}
		}

        internal WUCardLookupService.wucardlookuprequest WUCardLookupRequestMapper(CardLookUpRequest wucardlookupreq)
		{
			WUCardLookupService.wucardlookuprequest request = new WUCardLookupService.wucardlookuprequest();

			try
			{
				request.sender = new WUCardLookupService.sender()
								{

								};
				request.sender.name = new WUCardLookupService.general_name();
				request.sender.address = new WUCardLookupService.address();

				request.sender.preferred_customer = new WUCardLookupService.preferred_customer();
				request.sender.preferred_customer.account_nbr = wucardlookupreq.sender.PreferredCustomerAccountNumber;
				WUCardLookupService.convenience_search senderConsearchRequest = new WUCardLookupService.convenience_search();
				request.convenience_search = senderConsearchRequest;
				request.receiver_index_number = wucardlookupreq.receiver_index_number;
				request.wu_card_lookup_context = wucardlookupreq.wu_card_lookup_context;
				request.card_lookup_search_type = "S";
				request.save_key = wucardlookupreq.save_key;
			}
			catch (Exception ex)
			{
				//AL-1014 Transactional Log User Story
				MongoDBLogger.Error<CardLookUpRequest>(wucardlookupreq, "WUCardLookupRequestMapper", AlloyLayerName.CXN, ModuleName.Transaction,
					"Error in WUCardLookupRequestMapper -MGI.Cxn.WU.Common.Impl.BaseIO", ex.Message, ex.StackTrace);
			
				throw new WUCommonException(WUCommonException.PROVIDER_ERROR, ex.Message, ex);
			}
			return request;
		}

        internal CardInfo CardLookUpResponse(wucardlookupreply response)
		{
			CardInfo CardInfo = new CardInfo();
			//Mapping the WU Card look up response
			if (response.wu_card != null)
			{
				CardInfo.PromoCode = response.wu_card.promo_code;
				CardInfo.TotalPointsEarned = response.wu_card.total_points_earned;
			}
			return CardInfo;
		}
        internal static T ParseEnum<T>(string value)
		{
			return (T)Enum.Parse(typeof(T), value, true);
		}

        internal WUCardLookupService.wucardlookuprequest MapperCardLookupToWUCardLookUp(CardLookUpRequest wucardlookupreq)
		{
			WUCardLookupService.wucardlookuprequest request = new wucardlookuprequest();
			WUCardLookupService.card_update_indicator status;

			try
			{

				if (wucardlookupreq.sender.PreferredCustomerPermanentChange != null)
				{
					status = ParseEnum<WUCardLookupService.card_update_indicator>(wucardlookupreq.sender.PreferredCustomerPermanentChange.ToString());
				}
				else
					status = 0;

				request.sender = new WUCardLookupService.sender();
				request.sender.preferred_customer = new WUCardLookupService.preferred_customer();
				request.sender.preferred_customer.account_nbr = string.IsNullOrWhiteSpace(wucardlookupreq.sender.PreferredCustomerAccountNumber) ? string.Empty : wucardlookupreq.sender.PreferredCustomerAccountNumber.Trim();
				request.sender.preferred_customer.permanent_change = status;

				// Convenience Search
				WUCardLookupService.convenience_search cardsenderConsearchRequest = new WUCardLookupService.convenience_search();

				if (wucardlookupreq.conveniencesearch != null)
				{
					cardsenderConsearchRequest.type = wucardlookupreq.conveniencesearch.typefield;
					request.convenience_search = cardsenderConsearchRequest;
				}

				request.receiver_index_number = wucardlookupreq.receiver_index_number != String.Empty ? wucardlookupreq.receiver_index_number : String.Empty;
				// todo: check what value is getting passed
				request.wu_card_lookup_context = wucardlookupreq.wu_card_lookup_context != String.Empty ? wucardlookupreq.wu_card_lookup_context : String.Empty;
				request.card_lookup_search_type = "S"; // Need to clarify from WU. It should be "S" or "M"
				request.save_key = wucardlookupreq.save_key != String.Empty ? wucardlookupreq.save_key : String.Empty;
			}

			catch (Exception ex)
			{
				//AL-1014 Transactional Log User Story
				MongoDBLogger.Error<CardLookUpRequest>(wucardlookupreq, "MapperCardLookupToWUCardLookUp", AlloyLayerName.CXN, ModuleName.Transaction,
					"Error in MapperCardLookupToWUCardLookUp -MGI.Cxn.WU.Common.Impl.BaseIO", ex.Message, ex.StackTrace);
			
				throw new WUCommonException(WUCommonException.PROVIDER_ERROR, ex.Message, ex);
			}

			// This will return the request build by the senders details from Biz Layers to the WU Gateways.
			return request;
		}

		///// <summary>
		///// US2054
		///// </summary>
		///// <param name="context"></param>
		///// <returns></returns>
		public SwbFlaInfo BuildSwbFlaInfo(MGIContext mgiContext)
		{
			SwbFlaInfo swFlaInfo = null;
			swFlaInfo = new SwbFlaInfo()
			{
				SwbOperatorId = mgiContext.AgentId != 0 ? Convert.ToString(mgiContext.AgentId) : string.Empty,
				ReadPrivacyNoticeFlagSpecified = true,
				ReadPrivacyNoticeFlag = SwbFlaInfoReadPrivacyNoticeFlag.Y,
				FlagCertificationFlagSpecified = true,
				FlagCertificationFlag = SwbFlaInfoFlaCertificationFlag.Y
			};

			return swFlaInfo;
		}

		///// <summary>
		///// US2054
		///// </summary>
		///// <param name="context"></param>
		///// <returns></returns>
		public GeneralName BuildGeneralName(MGIContext mgiContext)
		{
			GeneralName name = null;
			name = new GeneralName()
			{
				Type = NameType.D,
				NameTypeSpecified = true,
				FirstName = mgiContext.AgentFirstName != null ? Convert.ToString(mgiContext.AgentFirstName) : string.Empty,
				LastName = mgiContext.AgentLastName != null ? Convert.ToString(mgiContext.AgentLastName) : string.Empty
			};
			return name;
		}

		/// <summary>
		/// Trim Occupation at 29 th Character
		/// </summary>
		/// <param name="occupation">string</param>
		/// <returns>string</returns>
		public string TrimOccupation(string occupation)
		{
			if (!string.IsNullOrEmpty(occupation) && occupation.Length > OCCUPATION_LENGTH)
			{
				return occupation.Substring(0, OCCUPATION_LENGTH);
			}
			else
			{
				return occupation;
			}
		}


		public string GetCountryName(string countryCode)
		{
			string countryName = string.Empty;

			if (!string.IsNullOrWhiteSpace(countryCode))
			{
				var country = WUCountryRepo.FindBy(c => c.CountryCode == countryCode);
				if (country != null)
				{
					countryName = country.Name;
				}
			}

			return countryName;
		}
		//	 Begin AL-471 Changes
		//       User Story Number: AL-471 | Web |   Developed by: Sunil Shetty     Date: 25.06.2015
		//       Purpose: This method takes only ssn exception message, if they are ssn exception the it send default provider error exception
		public int GetExceptionMessage(string ExceptionMessage)
		{
			
			string _exceptionMessage = ExceptionMessage.Replace("T", "").Substring(0, 4);
			switch (_exceptionMessage)
			{
				case "0505":
					return WUCommonException.MISSING_SSN_ITIN;
				case "0749":
					return WUCommonException.REQUIRES_SSN_ITIN;
				case "6008":
				case "6009":
					return WUCommonException.INVALID_SOCIAL_SECURITY_NUMBER;
                case "0425":
                    return WUCommonException.TRANSACTION_REQUIRES_ADDITIONAL_CUSTOMER_INFORMATION;//AL-2967
				case "0415":
					return WUCommonException.DO_TRANSACTION_REQUIRES_ADDITIONAL_CUSTOMER_INFORMATION;
				default:
					return WUCommonException.PROVIDER_ERROR;
			}
		}
		//END AL-471 Changes
	}
}
