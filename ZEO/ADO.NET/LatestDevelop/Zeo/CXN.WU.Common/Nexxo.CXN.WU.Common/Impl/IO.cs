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
using MGI.Cxn.WU.Common.Impl;
using MGI.Common.TransactionalLogging.Data;
using MGI.Common.Sys;


namespace MGI.Cxn.WU.Common.Impl
{
    public class IO : BaseIO, IWUCommonIO
	{
		# region Public method
		public CardDetails WUCardEnrollment(Sender sender, PaymentDetails paymentDetails, MGIContext mgiContext)
        {
			try
			{
				MGI.Cxn.WU.Common.WUCardService.channel channel = null;
				MGI.Cxn.WU.Common.WUCardService.foreign_remote_system remotesystem = null;

				CardDetails cardDetails = new CardDetails();
				cardDetails = null;

				sender.AddressAddrLine1 = NexxoUtil.MassagingValue(sender.AddressAddrLine1);
				sender.AddressAddrLine2 = NexxoUtil.MassagingValue(sender.AddressAddrLine2);
				sender.AddressCity = NexxoUtil.MassagingValue(sender.AddressCity);
				sender.AddressState = NexxoUtil.MassagingValue(sender.AddressState);
				sender.AddressStreet = NexxoUtil.MassagingValue(sender.AddressStreet);
				sender.CountryName = NexxoUtil.MassagingValue(sender.CountryName);
				sender.FirstName = NexxoUtil.MassagingValue(sender.FirstName);
				sender.LastName = NexxoUtil.MassagingValue(sender.LastName);
				sender.MiddleName = NexxoUtil.MassagingValue(sender.MiddleName);

				WUCardService.wucardenrollmentrequest cardEnrollRequest = new wucardenrollmentrequest();
				long channelPartnerId = mgiContext.ChannelPartnerId;

				WUCredential wuCredentials = new WUCredential();
				wuCredentials = null;
                
				wuCredentials = WUCredentialRepo.FindBy(x => x.ChannelPartnerId == channelPartnerId);
				if (wuCredentials == null)
					throw new WUCommonException(MGI.Common.Util.NexxoUtil.GetProductCode(mgiContext.ProductType), WUCommonException.WESTERNUNION_CREDENTIALS_NOTFOUND, null);

				channel = GetCardServiceChannel(wuCredentials);
				remotesystem = GetCardServiceRemoteSystem(wuCredentials, mgiContext);

				List<object> responseList = new List<object>();
				WUCardEnrollmentPortTypeClient WUPortTypeClient = new WUCardEnrollmentPortTypeClient("SOAP_HTTP_Port6", wuCredentials.WUServiceUrl.ToString());

				WUCertificate = WUCertificate == null ? GetWUCredentialCertificate(wuCredentials, mgiContext) : WUCertificate;

				WUPortTypeClient.ClientCredentials.ClientCertificate.Certificate = WUCertificate;

				cardEnrollRequest = CardEnrollmentRequestMapper(sender, paymentDetails);

				cardEnrollRequest.swb_fla_info = Mapper.Map<SwbFlaInfo, WUCardService.swb_fla_info>(BuildSwbFlaInfo(mgiContext));
				cardEnrollRequest.swb_fla_info.fla_name = Mapper.Map<GeneralName, WUCardService.general_name>(BuildGeneralName(mgiContext));
				cardEnrollRequest.channel = channel;
				remotesystem.reference_no = DateTime.Now.ToString("yyyyMMddhhmmssff");
				cardEnrollRequest.foreign_remote_system = remotesystem;

				#region AL-1014 Transactional Log User Story
				//MongoDBLogger.Info<WUCardService.wucardenrollmentrequest>(mgiContext.CustomerSessionId, cardEnrollRequest, "WUCardEnrollment", AlloyLayerName.CXN,
				//	ModuleName.Transaction, "WUCardEnrollment -MGI.Cxn.WU.Common.Impl.IO", "REQUEST", mgiContext);
				#endregion
				wucardenrollmentreply response = WUPortTypeClient.WUCardEnrollment(cardEnrollRequest);

				#region AL-1014 Transactional Log User Story
				//MongoDBLogger.Info<WUCardService.wucardenrollmentreply>(mgiContext.CustomerSessionId, response, "WUCardEnrollment", AlloyLayerName.CXN,
				//	ModuleName.Transaction, "WUCardEnrollment -MGI.Cxn.WU.Common.Impl.IO", "RESPONSE", mgiContext);
				#endregion
				cardDetails = EnrollCardResponseMapper(response, cardDetails);

				return cardDetails;
			}
			catch (System.ServiceModel.FaultException<WUCardService.errorreply> ex)
			{
				//MongoDBLogger.Error<Sender>(sender, "WUCardEnrollment", AlloyLayerName.CXN, ModuleName.Transaction,
    //                "Error in WUCardEnrollment -MGI.Cxn.WU.Common.Impl.IO", ex.Message, ex.StackTrace);
                string code = ExceptionHelper.GetProviderErrorCode(ex.Detail.error);
				throw new WUCommonProviderException(MGI.Common.Util.NexxoUtil.GetProductCode(mgiContext.ProductType), MGI.Common.Util.NexxoUtil.GetProviderCode(mgiContext.ProductType), code, ex.Detail.error, ex);
			}
			catch (Exception ex)
			{
				//MongoDBLogger.Error<Sender>(sender, "WUCardEnrollment", AlloyLayerName.CXN, ModuleName.Transaction,
				//	"Error in WUCardEnrollment -MGI.Cxn.WU.Common.Impl.IO", ex.Message, ex.StackTrace);
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				HandleException(ex, mgiContext.ProductType);
				throw new WUCommonException(MGI.Common.Util.NexxoUtil.GetProductCode(mgiContext.ProductType), WUCommonException.WUCARD_ENROLLMENT_FAILED, ex);
			}
        }

        public List<AgentBanners> GetWUAgentBannerMsgs(MGIContext mgiContext)
		{
			try
			{
				List<AgentBanners> agentbanners = new List<AgentBanners>();
				long transactionId = Convert.ToInt64(DateTime.Now.ToString("yyyyMMddhhmmssff"));
				filters_type filters = new filters_type();
				filters.queryfilter1 = "en";
				List<BANNERMESSAGE_Type> msgs = getdasResponse(transactionId, "GetWUAgentBannerMsgs", mgiContext.ChannelPartnerId, mgiContext, filters).ConvertAll<BANNERMESSAGE_Type>(t => (BANNERMESSAGE_Type)t);
				agentbanners.AddRange(Mapper.Map<List<BANNERMESSAGE_Type>, List<AgentBanners>>(msgs));
				return agentbanners;
			}
			catch(FaultException<DASService.errorreply> ex)
			{
                //MongoDBLogger.Error<MGIContext>(mgiContext, "GetWUAgentBannerMsgs", AlloyLayerName.CXN, ModuleName.Transaction,
                //    "Error in GetWUAgentBannerMsgs -MGI.Cxn.WU.Common.Impl.IO", ex.Message, ex.StackTrace);
                string code = ExceptionHelper.GetProviderErrorCode(ex.Detail.error);;
				throw new WUCommonProviderException(MGI.Common.Util.NexxoUtil.GetProductCode(mgiContext.ProductType), MGI.Common.Util.NexxoUtil.GetProviderCode(mgiContext.ProductType), code, ex.Detail.error, ex);
			}
			catch(Exception ex)
            {
                //MongoDBLogger.Error<MGIContext>(mgiContext, "GetWUAgentBannerMsgs", AlloyLayerName.CXN, ModuleName.Transaction,
                //       "Error in GetWUAgentBannerMsgs -MGI.Cxn.WU.Common.Impl.IO", ex.Message, ex.StackTrace);
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				HandleException(ex, mgiContext.ProductType);
				throw new WUCommonException(MGI.Common.Util.NexxoUtil.GetProductCode(mgiContext.ProductType), WUCommonException.BANNERMESSAGE_GET_FAILED, ex);
			}
		}

		public CardLookupDetails WUCardLookup(CardLookUpRequest wucardlookupreq, MGIContext mgiContext)
		{
			try
			{
				MGI.Cxn.WU.Common.WUCustomerLookup.channel lookupChannel = null;
				MGI.Cxn.WU.Common.WUCustomerLookup.foreign_remote_system lookupRemoteSystem = null;

				CardLookupDetails cardLookupDetails = new CardLookupDetails();
				cardLookupDetails = null;
				wucardlookupreq = GetWUCardLookUpRequest(wucardlookupreq);

				WUCustomerLookup.wucustomerlookuprequest cardLookupRequest = new WUCustomerLookup.wucustomerlookuprequest();
				long channelPartnerId = mgiContext.ChannelPartnerId;
				WUCredential wuCredentials = new WUCredential();
				wuCredentials = null;
				wuCredentials = WUCredentialRepo.FindBy(x => x.ChannelPartnerId == channelPartnerId);
				if (wuCredentials == null)
					throw new WUCommonException(MGI.Common.Util.NexxoUtil.GetProductCode(mgiContext.ProductType), WUCommonException.WESTERNUNION_CREDENTIALS_NOTFOUND, null);

				//ConfigureLookupWUOject(wuCredentials, context);
				lookupChannel = GetLookupChannel(wuCredentials);
				lookupRemoteSystem = GetLookupRemoteSystem(wuCredentials, mgiContext);

				List<object> responseList = new List<object>();
				WUCustomerLookup.CustomerLookupPortTypeClient WUPortTypeClient = new WUCustomerLookup.CustomerLookupPortTypeClient("SOAP_HTTP_Port8", wuCredentials.WUServiceUrl.ToString());

				WUCertificate = WUCertificate == null ? GetWUCredentialCertificate(wuCredentials, mgiContext) : WUCertificate;
				WUPortTypeClient.ClientCredentials.ClientCertificate.Certificate = WUCertificate;

				cardLookupRequest = CardLookupRequestMapper(wucardlookupreq, lookupRemoteSystem);
				cardLookupRequest.swb_fla_info = Mapper.Map<SwbFlaInfo, WUCustomerLookup.swb_fla_info>(BuildSwbFlaInfo(mgiContext));
				cardLookupRequest.swb_fla_info.fla_name = Mapper.Map<GeneralName, WUCustomerLookup.general_name>(BuildGeneralName(mgiContext));
				cardLookupRequest.channel = lookupChannel;
				lookupRemoteSystem.reference_no = DateTime.Now.ToString("yyyyMMddhhmmssff");
				cardLookupRequest.foreign_remote_system = lookupRemoteSystem;

				//#region AL-1014 Transactional Log User Story
				//MongoDBLogger.Info<WUCustomerLookup.wucustomerlookuprequest>(mgiContext.CustomerSessionId, cardLookupRequest, "WUCardLookup", AlloyLayerName.CXN,
				//	ModuleName.Transaction, "WUCardLookup -MGI.Cxn.WU.Common.Impl.IO", "REQUEST", mgiContext);
				//#endregion
				WUCustomerLookup.wucustomerlookupreply response = new WUCustomerLookup.wucustomerlookupreply();
				response = WUPortTypeClient.CustomerLookup(cardLookupRequest);
				if (response != null)
				{
					List<Sender> senderList = CardLookupResponse(response);
					while (response.more_flag == WUEnums.yes_no.Y.ToString())
					{
						cardLookupRequest.save_key = response.save_key;
						response = WUPortTypeClient.CustomerLookup(cardLookupRequest);

						//#region AL-1014 Transactional Log User Story
						//MongoDBLogger.Info<WUCustomerLookup.wucustomerlookupreply>(mgiContext.CustomerSessionId, response, "WUCardLookup", AlloyLayerName.CXN,
						//	ModuleName.Transaction, "WUCardLookup -MGI.Cxn.WU.Common.Impl.IO", "RESPONSE", mgiContext);
						//#endregion
						senderList.AddRange(CardLookupResponse(response));
					}

					cardLookupDetails = new CardLookupDetails()
					{
						Sender = new Sender[senderList.Count]
					};
					cardLookupDetails.Sender = senderList.ToArray();
				}
				return cardLookupDetails;
			}
			catch (System.ServiceModel.FaultException<WUCustomerLookup.errorreply> ex)
			{
				//MongoDBLogger.Error<CardLookUpRequest>(wucardlookupreq, "WUCardLookup", AlloyLayerName.CXN, ModuleName.Transaction,
				//	"Error in WUCardLookup -MGI.Cxn.WU.Common.Impl.IO", ex.Message, ex.StackTrace);
                string code = ExceptionHelper.GetProviderErrorCode(ex.Detail.error);;
				throw new WUCommonProviderException(MGI.Common.Util.NexxoUtil.GetProductCode(mgiContext.ProductType), MGI.Common.Util.NexxoUtil.GetProviderCode(mgiContext.ProductType), code, ex.Detail.error, ex);
			}
			catch (Exception ex)
			{
				//MongoDBLogger.Error<CardLookUpRequest>(wucardlookupreq, "WUCardLookup", AlloyLayerName.CXN, ModuleName.Transaction,
				//	"Error in WUCardLookup -MGI.Cxn.WU.Common.Impl.IO", ex.Message, ex.StackTrace);
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				HandleException(ex, mgiContext.ProductType);
				throw new WUCommonException(MGI.Common.Util.NexxoUtil.GetProductCode(mgiContext.ProductType), WUCommonException.WUCARD_LOOKUP_FAILED, ex);
			}
		}
        
		public CardLookupDetails WUCardLookupForCardNumber(CardLookUpRequest wucardlookupreq, MGIContext mgiContext)
		{
			try
			{
				MGI.Cxn.WU.Common.WUCardLookupService.channel cardLookupChannel = null;
				MGI.Cxn.WU.Common.WUCardLookupService.foreign_remote_system cardLookupRemoteSystem = null;

				CardLookupDetails cardLookupDetails = new CardLookupDetails();
				cardLookupDetails = null;
				wucardlookupreq = GetWUCardLookUpRequest(wucardlookupreq);

				WUCardLookupService.wucardlookuprequest cardLookupRequest = new WUCardLookupService.wucardlookuprequest();

				long channelPartnerId = mgiContext.ChannelPartnerId;
				WUCredential wuCredentials = null;
				wuCredentials = WUCredentialRepo.FindBy(x => x.ChannelPartnerId == channelPartnerId);
				if (wuCredentials == null)
					throw new WUCommonException(MGI.Common.Util.NexxoUtil.GetProductCode(mgiContext.ProductType), WUCommonException.WESTERNUNION_CREDENTIALS_NOTFOUND, null);
				
				cardLookupChannel = GetCardLookupChannel(wuCredentials);
				cardLookupRemoteSystem = GetCardLookupRemoteSystem(wuCredentials, mgiContext);

				List<object> responseList = new List<object>();
				WUCardLookupService.WUCardLookupPortTypeClient WUPortTypeClient = new WUCardLookupService.WUCardLookupPortTypeClient("WUCardLookup", wuCredentials.WUServiceUrl.ToString());

				WUCertificate = WUCertificate == null ? GetWUCredentialCertificate(wuCredentials, mgiContext) : WUCertificate;
				WUPortTypeClient.ClientCredentials.ClientCertificate.Certificate = WUCertificate;

				cardLookupRequest = WUCardLookupRequestMapper(wucardlookupreq, cardLookupRemoteSystem);
				cardLookupRequest.swb_fla_info = Mapper.Map<SwbFlaInfo, WUCardLookupService.swb_fla_info>(BuildSwbFlaInfo(mgiContext));
				cardLookupRequest.swb_fla_info.fla_name = Mapper.Map<GeneralName, WUCardLookupService.general_name>(BuildGeneralName(mgiContext));
				cardLookupRequest.channel = cardLookupChannel;
				cardLookupRemoteSystem.reference_no = DateTime.Now.ToString("yyyyMMddhhmmssff");
				cardLookupRequest.foreign_remote_system = cardLookupRemoteSystem;

				//#region AL-1014 Transactional Log User Story
				//MongoDBLogger.Info<WUCardLookupService.wucardlookuprequest>(mgiContext.CustomerSessionId, cardLookupRequest, "WUCardLookupForCardNumber", AlloyLayerName.CXN,
				//	ModuleName.Transaction, "WUCardLookupForCardNumber -MGI.Cxn.WU.Common.Impl.IO", "REQUEST", mgiContext);
				//#endregion
				WUCardLookupService.wucardlookupreply response = WUPortTypeClient.WuCardLookup(cardLookupRequest);
				cardLookupDetails = WUCardLookupResponse(response);

				//#region AL-1014 Transactional Log User Story
				//MongoDBLogger.Info<WUCardLookupService.wucardlookupreply>(mgiContext.CustomerSessionId, response, "WUCardLookupForCardNumber", AlloyLayerName.CXN,
				//	ModuleName.Transaction, "WUCardLookupForCardNumber -MGI.Cxn.WU.Common.Impl.IO", "RESPONSE", mgiContext);
				//#endregion

				return cardLookupDetails;
			}
			catch (System.ServiceModel.FaultException<WUCardLookupService.errorreply> ex)
			{
				//MongoDBLogger.Error<CardLookUpRequest>(wucardlookupreq, "WUCardLookupForCardNumber", AlloyLayerName.CXN, ModuleName.Transaction,
				//	"Error in WUCardLookupForCardNumber -MGI.Cxn.WU.Common.Impl.IO", ex.Message, ex.StackTrace);
                string code = ExceptionHelper.GetProviderErrorCode(ex.Detail.error);;
				throw new WUCommonProviderException(MGI.Common.Util.NexxoUtil.GetProductCode(mgiContext.ProductType), MGI.Common.Util.NexxoUtil.GetProviderCode(mgiContext.ProductType), code, ex.Detail.error, ex);
			}
			catch (Exception ex)
			{
				//MongoDBLogger.Error<CardLookUpRequest>(wucardlookupreq, "WUCardLookupForCardNumber", AlloyLayerName.CXN, ModuleName.Transaction,
				//		"Error in WUCardLookupForCardNumber -MGI.Cxn.WU.Common.Impl.IO", ex.Message, ex.StackTrace);
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				HandleException(ex, mgiContext.ProductType);
				throw new WUCommonException(MGI.Common.Util.NexxoUtil.GetProductCode(mgiContext.ProductType), WUCommonException.WUCARD_LOOKUP_FAILED, ex);
			}
        }

		public List<Receiver> WUPastBillersReceivers(long customerSessionId, CardLookUpRequest wucardlookupreq, MGIContext mgiContext)
        {
			try
			{
				WUCardLookupService.channel lookupChannel = null;
				WUCardLookupService.foreign_remote_system lookupRemoteSystem = null;
				List<WUCardLookupService.receiver> lstFinalReceivers = new List<WUCardLookupService.receiver>(); // list of receiver of type WU Specific Format
				List<Receiver> lstFinalReceiver = new List<Receiver>(); // list of receiver of CXN-WU-Common

				WUCardLookupService.wucardlookuprequest wuCardLookupReq = new WUCardLookupService.wucardlookuprequest();
				WUCustomerLookup.wucustomerlookuprequest cardLookupRequest = new WUCustomerLookup.wucustomerlookuprequest();
				WUCardLookupService.wucardlookupreply cardResponse = null;

				long channelPartnerId = mgiContext.ChannelPartnerId;
				WUCredential wuCredentials = WUCredentialRepo.FindBy(x => x.ChannelPartnerId == channelPartnerId);

				if (wuCredentials == null)
				{
					throw new WUCommonException(MGI.Common.Util.NexxoUtil.GetProductCode(mgiContext.ProductType), WUCommonException.WESTERNUNION_CREDENTIALS_NOTFOUND, null);
				}

				List<object> responseList = new List<object>();

				WUCardLookupService.WUCardLookupPortTypeClient wuCardPortTypeClient = new WUCardLookupService.WUCardLookupPortTypeClient("WUCardLookup", wuCredentials.WUServiceUrl.ToString());
				WUCertificate = WUCertificate == null ? GetWUCredentialCertificate(wuCredentials, mgiContext) : WUCertificate;
				wuCardPortTypeClient.ClientCredentials.ClientCertificate.Certificate = WUCertificate;

				lookupChannel = GetCardLookupChannel(wuCredentials);
				lookupRemoteSystem = GetCardLookupRemoteSystem(wuCredentials, mgiContext);

				lookupRemoteSystem.reference_no = DateTime.Now.ToString("yyyyMMddhhmmssff");

				wuCardLookupReq = MapperCardLookupToWUCardLookUp(wucardlookupreq);

				wuCardLookupReq.channel = lookupChannel;
				wuCardLookupReq.foreign_remote_system = lookupRemoteSystem;

				wuCardLookupReq = WUCardLookupRequestMapper(wucardlookupreq, lookupRemoteSystem);
				wuCardLookupReq.swb_fla_info = Mapper.Map<SwbFlaInfo, WUCardLookupService.swb_fla_info>(BuildSwbFlaInfo(mgiContext));
				wuCardLookupReq.swb_fla_info.fla_name = Mapper.Map<GeneralName, WUCardLookupService.general_name>(BuildGeneralName(mgiContext));
				wuCardLookupReq.channel = lookupChannel;
				lookupRemoteSystem.reference_no = DateTime.Now.ToString("yyyyMMddhhmmssff");
				wuCardLookupReq.foreign_remote_system = lookupRemoteSystem;

				//#region AL-1014 Transactional Log User Story
				//MongoDBLogger.Info<WUCardLookupService.wucardlookuprequest>(customerSessionId, wuCardLookupReq, "WUPastBillersReceivers", AlloyLayerName.CXN,
				//	ModuleName.Transaction, "WUPastBillersReceivers -MGI.Cxn.WU.Common.Impl.IO", "REQUEST", mgiContext);
				//#endregion
				cardResponse = wuCardPortTypeClient.WuCardLookup(wuCardLookupReq);

				if (cardResponse.receiver != null)
				{
					if (cardResponse.receiver.Length > 0)
					{
						foreach (WUCardLookupService.receiver rec in cardResponse.receiver)
						{
							switch (rec.name.name_type)
							{
								case MGI.Cxn.WU.Common.WUCardLookupService.name_type.C:
									if (!string.IsNullOrWhiteSpace(rec.name.business_name) && !string.IsNullOrWhiteSpace(rec.debtor_account_number) && !string.IsNullOrWhiteSpace(rec.address.Item.iso_code.country_code))
										lstFinalReceivers.Add(rec);
									break;

								case MGI.Cxn.WU.Common.WUCardLookupService.name_type.D:
									if (!string.IsNullOrWhiteSpace(rec.name.first_name) && !string.IsNullOrWhiteSpace(rec.name.last_name) && !string.IsNullOrWhiteSpace(rec.address.Item.iso_code.country_code))
										lstFinalReceivers.Add(rec);
									break;

								case MGI.Cxn.WU.Common.WUCardLookupService.name_type.M:
									if (!string.IsNullOrWhiteSpace(rec.name.given_name) && !string.IsNullOrWhiteSpace(rec.name.paternal_name.Trim()) && !string.IsNullOrWhiteSpace(rec.name.paternal_name.ToString().Trim()) && !string.IsNullOrWhiteSpace(rec.address.Item.iso_code.country_code))
										lstFinalReceivers.Add(rec);
									break;
							}
						}
					}

					if (cardResponse.more_flag.ToUpper() == "Y")
					{
						while (cardResponse.more_flag.ToUpper() == "Y")
						{
							wuCardLookupReq.wu_card_lookup_context = cardResponse.wu_card_lookup_context;
							wuCardLookupReq.save_key = cardResponse.receiver_index_number;

							cardResponse = wuCardPortTypeClient.WuCardLookup(wuCardLookupReq);

							if (cardResponse.receiver != null)
							{
								foreach (WUCardLookupService.receiver rec in cardResponse.receiver)
								{

									switch (rec.name.name_type)
									{
										case MGI.Cxn.WU.Common.WUCardLookupService.name_type.C:
											if (!string.IsNullOrWhiteSpace(rec.name.business_name) && !string.IsNullOrWhiteSpace(rec.debtor_account_number) && !string.IsNullOrWhiteSpace(rec.address.Item.iso_code.country_code))
												lstFinalReceivers.Add(rec);
											break;

										case MGI.Cxn.WU.Common.WUCardLookupService.name_type.D:
											if (!string.IsNullOrWhiteSpace(rec.name.first_name) && !string.IsNullOrWhiteSpace(rec.name.last_name) && !string.IsNullOrWhiteSpace(rec.address.Item.iso_code.country_code))
												lstFinalReceivers.Add(rec);
											break;


										case MGI.Cxn.WU.Common.WUCardLookupService.name_type.M:
											if (!string.IsNullOrWhiteSpace(rec.name.given_name.Trim()) && !string.IsNullOrWhiteSpace(rec.name.paternal_name.Trim()) && !string.IsNullOrWhiteSpace(rec.name.maternal_name.ToString()) && !string.IsNullOrWhiteSpace(rec.address.Item.iso_code.country_code))
												lstFinalReceivers.Add(rec);
											break;
									}
								}
							}

						}
					}

					lstFinalReceiver = WUTotalReceivers(lstFinalReceivers);

					//#region AL-1014 Transactional Log User Story
					//MongoDBLogger.Info<WUCardLookupService.wucardlookupreply>(customerSessionId, cardResponse, "WUPastBillersReceivers", AlloyLayerName.CXN,
					//	ModuleName.Transaction, "WUPastBillersReceivers -MGI.Cxn.WU.Common.Impl.IO", "RESPONSE", mgiContext);
					//#endregion
				}
				else
				{
					return null;
				}
				return (lstFinalReceiver.Count != 0) ? lstFinalReceiver : null;
			}
			catch (System.ServiceModel.FaultException<WUCustomerLookup.errorreply> ex)
			{
				//MongoDBLogger.Error<CardLookUpRequest>(wucardlookupreq, "WUPastBillersReceivers", AlloyLayerName.CXN, ModuleName.Transaction,
				//	"Error in WUPastBillersReceivers -MGI.Cxn.WU.Common.Impl.IO", ex.Message, ex.StackTrace);
                string code = ExceptionHelper.GetProviderErrorCode(ex.Detail.error);;
				throw new WUCommonProviderException(MGI.Common.Util.NexxoUtil.GetProductCode(mgiContext.ProductType), MGI.Common.Util.NexxoUtil.GetProviderCode(mgiContext.ProductType), code, ex.Detail.error, ex);
			}
			catch (Exception ex)
			{
				//MongoDBLogger.Error<CardLookUpRequest>(wucardlookupreq, "WUPastBillersReceivers", AlloyLayerName.CXN, ModuleName.Transaction,
				//"Error in WUPastBillersReceivers -MGI.Cxn.WU.Common.Impl.IO", ex.Message, ex.StackTrace);
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				HandleException(ex, mgiContext.ProductType);
				throw new WUCommonException(MGI.Common.Util.NexxoUtil.GetProductCode(mgiContext.ProductType), WUCommonException.PAST_BILLERS_GET_FAILED, ex);
			}
        }

		public WUBaseRequestResponse CreateRequest(long channelPartnerId, MGIContext mgiContext)
		{
			WUBaseRequestResponse request = new WUBaseRequestResponse();
			WUCredential credential = GetCredential(channelPartnerId, mgiContext);

			string locationId = GetCounterId(credential, mgiContext);

			request.Channel = new Channel() { Name = credential.ChannelName, Version = credential.ChannelVersion };
			request.ForeignRemoteSystem = new ForeignRemoteSystem() { CoutnerId = locationId, Identifier = credential.AccountIdentifier };
			WUCertificate = WUCertificate == null ? GetWUCredentialCertificate(credential, mgiContext) : WUCertificate;

			request.ClientCertificate = WUCertificate;

			request.ServiceUrl = credential.WUServiceUrl;
			return request;
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

		public CardInfo GetCardInfo(CardLookUpRequest wuCardLookupReq, MGIContext mgiContext)
		{
			try
			{
				CardInfo CardInfo = new CardInfo();
				CardInfo = null;

				WUCardLookupService.channel lookupChannel = null;
				WUCardLookupService.foreign_remote_system lookupRemoteSystem = null;

				WUCardLookupService.wucardlookuprequest cardLookupRequest = new WUCardLookupService.wucardlookuprequest();

				WUCredential wuCredentials = null;
				wuCredentials = WUCredentialRepo.FindBy(x => x.ChannelPartnerId == mgiContext.ChannelPartnerId);
				if (wuCredentials == null)
					throw new WUCommonException(MGI.Common.Util.NexxoUtil.GetProductCode(mgiContext.ProductType), WUCommonException.WESTERNUNION_CREDENTIALS_NOTFOUND, null);

				List<object> responseList = new List<object>();
				WUCardLookupService.WUCardLookupPortTypeClient WUPortTypeClient = new WUCardLookupService.WUCardLookupPortTypeClient("WUCardLookup", wuCredentials.WUServiceUrl.ToString());
				WUCertificate = WUCertificate == null ? GetWUCredentialCertificate(wuCredentials, mgiContext) : WUCertificate;
				WUPortTypeClient.ClientCredentials.ClientCertificate.Certificate = WUCertificate;
				cardLookupRequest = WUCardLookupRequestMapper(wuCardLookupReq);

				lookupChannel = GetCardLookupChannel(wuCredentials);
				lookupRemoteSystem = GetCardLookupRemoteSystem(wuCredentials, mgiContext);

				cardLookupRequest.channel = lookupChannel;
				lookupRemoteSystem.reference_no = DateTime.Now.ToString("yyyyMMddhhmmssff");
				cardLookupRequest.foreign_remote_system = lookupRemoteSystem;
				cardLookupRequest.swb_fla_info = Mapper.Map<SwbFlaInfo, WUCardLookupService.swb_fla_info>(BuildSwbFlaInfo(mgiContext));
				cardLookupRequest.swb_fla_info.fla_name = Mapper.Map<GeneralName, WUCardLookupService.general_name>(BuildGeneralName(mgiContext));

				//#region AL-1014 Transactional Log User Story
				//MongoDBLogger.Info<WUCardLookupService.wucardlookuprequest>(mgiContext.CustomerSessionId, cardLookupRequest, "GetCardInfo", AlloyLayerName.CXN,
				//	ModuleName.BillPayment, "GetCardInfo -MGI.Cxn.WU.Common.Impl.BaseIO", "REQUEST", mgiContext);
				//#endregion
				WUCardLookupService.wucardlookupreply response = WUPortTypeClient.WuCardLookup(cardLookupRequest);

				//#region AL-1014 Transactional Log User Story
				//MongoDBLogger.Info<WUCardLookupService.wucardlookupreply>(mgiContext.CustomerSessionId, response, "GetCardInfo", AlloyLayerName.CXN,
				//	ModuleName.BillPayment, "GetCardInfo -MGI.Cxn.WU.Common.Impl.BaseIO", "RESPONSE", mgiContext);
				//#endregion
				CardInfo = CardLookUpResponse(response);
				return CardInfo;
			}
			catch (System.ServiceModel.FaultException<WUCardLookupService.errorreply> ex)
			{
			///*	MongoDBLogger.Error<CardLookUpRequest>(wuCardLookupReq, "GetCardInfo", AlloyLayerName.CXN,*/ ModuleName.Transaction,
			//	"Error in GetCardInfo -MGI.Cxn.WU.Common.Impl.BaseIO", ex.Message, ex.StackTrace);
                string code = ExceptionHelper.GetProviderErrorCode(ex.Detail.error);
				throw new WUCommonProviderException(MGI.Common.Util.NexxoUtil.GetProductCode(mgiContext.ProductType), MGI.Common.Util.NexxoUtil.GetProviderCode(mgiContext.ProductType), code, ex.Detail.error, ex);
			}
			catch (Exception ex)
			{
				//MongoDBLogger.Error<CardLookUpRequest>(wuCardLookupReq, "GetCardInfo", AlloyLayerName.CXN, ModuleName.Transaction,
				//	"Error in GetCardInfo -MGI.Cxn.WU.Common.Impl.BaseIO", ex.Message, ex.StackTrace);
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				HandleException(ex, mgiContext.ProductType);
				throw new WUCommonException(MGI.Common.Util.NexxoUtil.GetProductCode(mgiContext.ProductType), WUCommonException.WUCARD_LOOKUP_FAILED, ex);
			}
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

		/// <summary>
		/// 
		/// </summary>
		/// <param name="countryCode"></param>
		/// <returns></returns>
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

        # endregion
    }
}
