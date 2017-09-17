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


namespace MGI.Cxn.WU.Common.Impl
{
    public class IO : BaseIO, IWUCommonIO
	{		
        
		# region Public method
		public CardDetails WUCardEnrollment(Sender sender, PaymentDetails paymentDetails, MGIContext mgiContext)
        {
            MGI.Cxn.WU.Common.WUCardService.channel channel = null;
            MGI.Cxn.WU.Common.WUCardService.foreign_remote_system remotesystem = null;

            //Create the response object			 
            string ErrorMessage = string.Empty;
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

            try
            {
                WUCardService.wucardenrollmentrequest cardEnrollRequest = new wucardenrollmentrequest();
				long channelPartnerId = mgiContext.ChannelPartnerId;

                WUCredential wuCredentials = new WUCredential();
                wuCredentials = null;

                string errorMessage = string.Format("Error while retrieving credentials for channel partner: {0}", channelPartnerId);

                wuCredentials = WUCredentialRepo.FindBy(x => x.ChannelPartnerId == channelPartnerId);
                if (wuCredentials == null)
                    throw new WUCommonException(WUCommonException.WESTERNUNION_CREDENTIALS_NOTFOUND, errorMessage);

                channel = GetCardServiceChannel(wuCredentials);
				remotesystem = GetCardServiceRemoteSystem(wuCredentials, mgiContext);

                List<object> responseList = new List<object>();
                WUCardEnrollmentPortTypeClient WUPortTypeClient = new WUCardEnrollmentPortTypeClient("SOAP_HTTP_Port6", wuCredentials.WUServiceUrl.ToString());

                WUCertificate = WUCertificate == null ? GetWUCredentialCertificate(wuCredentials) : WUCertificate;

                WUPortTypeClient.ClientCredentials.ClientCertificate.Certificate = WUCertificate;

                //Mapping the request. 
                cardEnrollRequest = CardEnrollmentRequestMapper(sender, paymentDetails);

				cardEnrollRequest.swb_fla_info = Mapper.Map<SwbFlaInfo, WUCardService.swb_fla_info>(BuildSwbFlaInfo(mgiContext));
				cardEnrollRequest.swb_fla_info.fla_name = Mapper.Map<GeneralName, WUCardService.general_name>(BuildGeneralName(mgiContext));
                cardEnrollRequest.channel = channel;
                remotesystem.reference_no = DateTime.Now.ToString("yyyyMMddhhmmssff");
                cardEnrollRequest.foreign_remote_system = remotesystem;

				#region AL-1014 Transactional Log User Story
				MongoDBLogger.Info<WUCardService.wucardenrollmentrequest>(mgiContext.CustomerSessionId, cardEnrollRequest, "WUCardEnrollment", AlloyLayerName.CXN,
					ModuleName.Transaction, "WUCardEnrollment -MGI.Cxn.WU.Common.Impl.IO", "REQUEST", mgiContext);
				#endregion
				//Calling the WUCardenrollment. 
                wucardenrollmentreply response = WUPortTypeClient.WUCardEnrollment(cardEnrollRequest);

				#region AL-1014 Transactional Log User Story
				MongoDBLogger.Info<WUCardService.wucardenrollmentreply>(mgiContext.CustomerSessionId, response, "WUCardEnrollment", AlloyLayerName.CXN,
					ModuleName.Transaction, "WUCardEnrollment -MGI.Cxn.WU.Common.Impl.IO", "RESPONSE", mgiContext);
				#endregion
				//mapping the response 
                cardDetails = EnrollCardResponseMapper(response, cardDetails);

            }
            catch (System.ServiceModel.FaultException<WUCardService.errorreply> ex)
            {
				//AL-1014 Transactional Log User Story
				MongoDBLogger.Error<Sender>(sender, "WUCardEnrollment", AlloyLayerName.CXN, ModuleName.Transaction,
					"Error in WUCardEnrollment -MGI.Cxn.WU.Common.Impl.IO", ex.Message, ex.StackTrace);
				throw new WUCommonException(WUCommonException.PROVIDER_ERROR, ex.Detail.error, ex);				
            }
            catch (Exception ex)
            {
				//AL-1014 Transactional Log User Story
				MongoDBLogger.Error<Sender>(sender, "WUCardEnrollment", AlloyLayerName.CXN, ModuleName.Transaction,
					"Error in WUCardEnrollment -MGI.Cxn.WU.Common.Impl.IO", ex.Message, ex.StackTrace);
				throw new WUCommonException(WUCommonException.PROVIDER_ERROR, ex.Message, ex);				
            }
            return cardDetails;


        }



		public List<AgentBanners> GetWUAgentBannerMsgs(MGIContext mgiContext)
		{
			List<AgentBanners> agentbanners = new List<AgentBanners>();
			try
			{
				long transactionId = Convert.ToInt64(DateTime.Now.ToString("yyyyMMddhhmmssff"));
				filters_type filters = new filters_type();
				filters.queryfilter1 = "en";
				List<BANNERMESSAGE_Type> msgs = getdasResponse(transactionId, "GetWUAgentBannerMsgs", mgiContext.ChannelPartnerId, mgiContext, filters).ConvertAll<BANNERMESSAGE_Type>(t => (BANNERMESSAGE_Type)t);
				agentbanners.AddRange(Mapper.Map<List<BANNERMESSAGE_Type>, List<AgentBanners>>(msgs));
			}
			catch(WUCommonException ex)
			{
				throw new WUCommonException(WUCommonException.PROVIDER_ERROR, ex.Message, ex);
			}
			catch (Exception ex)
			{
				throw new WUCommonException(WUCommonException.PROVIDER_ERROR, ex.Message, ex);				
			}
			return agentbanners;
		}


		public CardLookupDetails WUCardLookup(CardLookUpRequest wucardlookupreq, MGIContext mgiContext)
		{
			MGI.Cxn.WU.Common.WUCustomerLookup.channel lookupChannel = null;
			MGI.Cxn.WU.Common.WUCustomerLookup.foreign_remote_system lookupRemoteSystem = null;

			string ErrorMessage = string.Empty;
			CardLookupDetails cardLookupDetails = new CardLookupDetails();
			cardLookupDetails = null;
            wucardlookupreq = GetWUCardLookUpRequest(wucardlookupreq);
			
			try
			{
				WUCustomerLookup.wucustomerlookuprequest cardLookupRequest = new WUCustomerLookup.wucustomerlookuprequest();
				long channelPartnerId = mgiContext.ChannelPartnerId;
				WUCredential wuCredentials = new WUCredential();
				wuCredentials = null;
				string errorMessage = string.Format("Error while retrieving credentials for channel partner: {0}", channelPartnerId);
				wuCredentials = WUCredentialRepo.FindBy(x => x.ChannelPartnerId == channelPartnerId);
				if (wuCredentials == null)
					throw new WUCommonException(WUCommonException.WESTERNUNION_CREDENTIALS_NOTFOUND, errorMessage);

				//ConfigureLookupWUOject(wuCredentials, context);
				lookupChannel = GetLookupChannel(wuCredentials);
				lookupRemoteSystem = GetLookupRemoteSystem(wuCredentials, mgiContext);

				List<object> responseList = new List<object>();
				WUCustomerLookup.CustomerLookupPortTypeClient WUPortTypeClient = new WUCustomerLookup.CustomerLookupPortTypeClient("SOAP_HTTP_Port8", wuCredentials.WUServiceUrl.ToString());

				WUCertificate = WUCertificate == null ? GetWUCredentialCertificate(wuCredentials) : WUCertificate;
				WUPortTypeClient.ClientCredentials.ClientCertificate.Certificate = WUCertificate;

				cardLookupRequest = CardLookupRequestMapper(wucardlookupreq, lookupRemoteSystem);
				cardLookupRequest.swb_fla_info = Mapper.Map<SwbFlaInfo, WUCustomerLookup.swb_fla_info>(BuildSwbFlaInfo(mgiContext));
				cardLookupRequest.swb_fla_info.fla_name = Mapper.Map<GeneralName, WUCustomerLookup.general_name>(BuildGeneralName(mgiContext));
				cardLookupRequest.channel = lookupChannel;
				lookupRemoteSystem.reference_no = DateTime.Now.ToString("yyyyMMddhhmmssff");
				cardLookupRequest.foreign_remote_system = lookupRemoteSystem;

				#region AL-1014 Transactional Log User Story
				MongoDBLogger.Info<WUCustomerLookup.wucustomerlookuprequest>(mgiContext.CustomerSessionId, cardLookupRequest, "WUCardLookup", AlloyLayerName.CXN,
					ModuleName.Transaction, "WUCardLookup -MGI.Cxn.WU.Common.Impl.IO", "REQUEST", mgiContext);
				#endregion
				WUCustomerLookup.wucustomerlookupreply response = new WUCustomerLookup.wucustomerlookupreply();
				response = WUPortTypeClient.CustomerLookup(cardLookupRequest);
				List<Sender> senderList = CardLookupResponse(response);
				while (response.more_flag == WUEnums.yes_no.Y.ToString())
				{
					cardLookupRequest.save_key = response.save_key;
					response = WUPortTypeClient.CustomerLookup(cardLookupRequest);

					#region AL-1014 Transactional Log User Story
                    MongoDBLogger.Info<WUCustomerLookup.wucustomerlookupreply>(mgiContext.CustomerSessionId, response, "WUCardLookup", AlloyLayerName.CXN,
                        ModuleName.Transaction, "WUCardLookup -MGI.Cxn.WU.Common.Impl.IO", "RESPONSE", mgiContext);
					#endregion
					senderList.AddRange(CardLookupResponse(response));
				}

				cardLookupDetails = new CardLookupDetails()
				{
					Sender = new Sender[senderList.Count]
				};
				cardLookupDetails.Sender = senderList.ToArray();
			}
			catch (System.ServiceModel.FaultException<WUCustomerLookup.errorreply> ex)
			{
				//AL-1014 Transactional Log User Story
				MongoDBLogger.Error<CardLookUpRequest>(wucardlookupreq, "WUCardLookup", AlloyLayerName.CXN, ModuleName.Transaction,
					"Error in WUCardLookup -MGI.Cxn.WU.Common.Impl.IO", ex.Message, ex.StackTrace);
				
				throw new WUCommonException(WUCommonException.PROVIDER_ERROR, ex.Detail.error, ex);
			}
			catch (Exception ex)
			{
				//AL-1014 Transactional Log User Story
				MongoDBLogger.Error<CardLookUpRequest>(wucardlookupreq, "WUCardLookup", AlloyLayerName.CXN, ModuleName.Transaction,
					"Error in WUCardLookup -MGI.Cxn.WU.Common.Impl.IO", ex.Message, ex.StackTrace);
				throw new WUCommonException(WUCommonException.PROVIDER_ERROR, ex.Message, ex);			
			}
			return cardLookupDetails;
		}

		// US1784 - WU Gold Card Name Matching
		public CardLookupDetails WUCardLookupForCardNumber(CardLookUpRequest wucardlookupreq, MGIContext mgiContext)
		{
			MGI.Cxn.WU.Common.WUCardLookupService.channel cardLookupChannel = null;
			MGI.Cxn.WU.Common.WUCardLookupService.foreign_remote_system cardLookupRemoteSystem = null;

			string ErrorMessage = string.Empty;
			CardLookupDetails cardLookupDetails = new CardLookupDetails();
			cardLookupDetails = null;
            wucardlookupreq = GetWUCardLookUpRequest(wucardlookupreq);

			try
			{
				WUCardLookupService.wucardlookuprequest cardLookupRequest = new WUCardLookupService.wucardlookuprequest();

				long channelPartnerId = mgiContext.ChannelPartnerId;
				WUCredential wuCredentials = null;
				string errorMessage = string.Format("Error while retrieving credentials for channel partner: {0}", channelPartnerId);
				wuCredentials = WUCredentialRepo.FindBy(x => x.ChannelPartnerId == channelPartnerId);
				if (wuCredentials == null)
					throw new WUCommonException(WUCommonException.WESTERNUNION_CREDENTIALS_NOTFOUND, errorMessage);

				cardLookupChannel = GetCardLookupChannel(wuCredentials);
				cardLookupRemoteSystem = GetCardLookupRemoteSystem(wuCredentials, mgiContext);

				List<object> responseList = new List<object>();
				WUCardLookupService.WUCardLookupPortTypeClient WUPortTypeClient = new WUCardLookupService.WUCardLookupPortTypeClient("WUCardLookup", wuCredentials.WUServiceUrl.ToString());

				WUCertificate = WUCertificate == null ? GetWUCredentialCertificate(wuCredentials) : WUCertificate;
				WUPortTypeClient.ClientCredentials.ClientCertificate.Certificate = WUCertificate;

				cardLookupRequest = WUCardLookupRequestMapper(wucardlookupreq, cardLookupRemoteSystem);
				cardLookupRequest.swb_fla_info = Mapper.Map<SwbFlaInfo, WUCardLookupService.swb_fla_info>(BuildSwbFlaInfo(mgiContext));
				cardLookupRequest.swb_fla_info.fla_name = Mapper.Map<GeneralName, WUCardLookupService.general_name>(BuildGeneralName(mgiContext));
				cardLookupRequest.channel = cardLookupChannel;
				cardLookupRemoteSystem.reference_no = DateTime.Now.ToString("yyyyMMddhhmmssff");
				cardLookupRequest.foreign_remote_system = cardLookupRemoteSystem;

				#region AL-1014 Transactional Log User Story
				MongoDBLogger.Info<WUCardLookupService.wucardlookuprequest>(mgiContext.CustomerSessionId, cardLookupRequest, "WUCardLookupForCardNumber", AlloyLayerName.CXN,
					ModuleName.Transaction, "WUCardLookupForCardNumber -MGI.Cxn.WU.Common.Impl.IO", "REQUEST", mgiContext);
				#endregion
				WUCardLookupService.wucardlookupreply response = WUPortTypeClient.WuCardLookup(cardLookupRequest);
				cardLookupDetails = WUCardLookupResponse(response);

				#region AL-1014 Transactional Log User Story
				MongoDBLogger.Info<WUCardLookupService.wucardlookupreply>(mgiContext.CustomerSessionId, response, "WUCardLookupForCardNumber", AlloyLayerName.CXN,
					ModuleName.Transaction, "WUCardLookupForCardNumber -MGI.Cxn.WU.Common.Impl.IO", "RESPONSE", mgiContext);
				#endregion
			}
			catch (System.ServiceModel.FaultException<WUCardLookupService.errorreply> ex)
			{
				//AL-1014 Transactional Log User Story
				MongoDBLogger.Error<CardLookUpRequest>(wucardlookupreq, "WUCardLookupForCardNumber", AlloyLayerName.CXN, ModuleName.Transaction,
					"Error in WUCardLookupForCardNumber -MGI.Cxn.WU.Common.Impl.IO", ex.Message, ex.StackTrace);
				
				throw new WUCommonException(WUCommonException.PROVIDER_ERROR, ex.Detail.error, ex);
			}
			catch (Exception ex)
			{
				//AL-1014 Transactional Log User Story
				MongoDBLogger.Error<CardLookUpRequest>(wucardlookupreq, "WUCardLookupForCardNumber", AlloyLayerName.CXN, ModuleName.Transaction,
						"Error in WUCardLookupForCardNumber -MGI.Cxn.WU.Common.Impl.IO", ex.Message, ex.StackTrace);
				throw new WUCommonException(WUCommonException.PROVIDER_ERROR, ex.Message, ex);				
			}
			return cardLookupDetails;
        }

		public List<Receiver> WUPastBillersReceivers(long customerSessionId, CardLookUpRequest wucardlookupreq, MGIContext mgiContext)
        {
            WUCardLookupService.channel lookupChannel = null;
            WUCardLookupService.foreign_remote_system lookupRemoteSystem = null;
            List<WUCardLookupService.receiver> lstFinalReceivers = new List<WUCardLookupService.receiver>(); // list of receiver of type WU Specific Format
            List<Receiver> lstFinalReceiver = new List<Receiver>(); // list of receiver of CXN-WU-Common

            try
            {
                WUCardLookupService.wucardlookuprequest wuCardLookupReq = new WUCardLookupService.wucardlookuprequest();
                WUCustomerLookup.wucustomerlookuprequest cardLookupRequest = new WUCustomerLookup.wucustomerlookuprequest();
                WUCardLookupService.wucardlookupreply cardResponse = null;

				long channelPartnerId = mgiContext.ChannelPartnerId;
                string errorMessage = string.Format("Error while retrieving credentials for channel partner: {0}", channelPartnerId);
                WUCredential wuCredentials = WUCredentialRepo.FindBy(x => x.ChannelPartnerId == channelPartnerId);

                if (wuCredentials == null)
                {
                    throw new WUCommonException(WUCommonException.WESTERNUNION_CREDENTIALS_NOTFOUND, errorMessage);
                }

                List<object> responseList = new List<object>();

                WUCardLookupService.WUCardLookupPortTypeClient wuCardPortTypeClient = new WUCardLookupService.WUCardLookupPortTypeClient("WUCardLookup", wuCredentials.WUServiceUrl.ToString());
                WUCertificate = WUCertificate == null ? GetWUCredentialCertificate(wuCredentials) : WUCertificate;
                wuCardPortTypeClient.ClientCredentials.ClientCertificate.Certificate = WUCertificate;

                lookupChannel = GetCardLookupChannel(wuCredentials);
				lookupRemoteSystem = GetCardLookupRemoteSystem(wuCredentials, mgiContext);

                lookupRemoteSystem.reference_no = DateTime.Now.ToString("yyyyMMddhhmmssff");
                // This method will provide WU Specific Ruquest Format.
                wuCardLookupReq = MapperCardLookupToWUCardLookUp(wucardlookupreq);

                wuCardLookupReq.channel = lookupChannel;
                wuCardLookupReq.foreign_remote_system = lookupRemoteSystem;

                // This is the sample request for Card Lookup for User Story #US1645 and # US1646.
                //string requestXML = ConvertToXML(wuCardLookupReq, typeof(WUCardLookupService.wucardlookuprequest));

                wuCardLookupReq = WUCardLookupRequestMapper(wucardlookupreq, lookupRemoteSystem);
				wuCardLookupReq.swb_fla_info = Mapper.Map<SwbFlaInfo, WUCardLookupService.swb_fla_info>(BuildSwbFlaInfo(mgiContext));
				wuCardLookupReq.swb_fla_info.fla_name = Mapper.Map<GeneralName, WUCardLookupService.general_name>(BuildGeneralName(mgiContext));
                wuCardLookupReq.channel = lookupChannel;
                lookupRemoteSystem.reference_no = DateTime.Now.ToString("yyyyMMddhhmmssff");
                wuCardLookupReq.foreign_remote_system = lookupRemoteSystem;

				#region AL-1014 Transactional Log User Story
				MongoDBLogger.Info<WUCardLookupService.wucardlookuprequest>(customerSessionId, wuCardLookupReq, "WUPastBillersReceivers", AlloyLayerName.CXN,
					ModuleName.Transaction, "WUPastBillersReceivers -MGI.Cxn.WU.Common.Impl.IO", "REQUEST", mgiContext);
				#endregion
				// Request is sent to the WU for past billers and receivers till end-of-request from WU.
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

                            //if (!string.IsNullOrWhiteSpace(rec.name.first_name) && !string.IsNullOrWhiteSpace(rec.name.last_name) && !string.IsNullOrWhiteSpace(rec.address.Item.iso_code.country_code))
                            //lstFinalReceivers.Add(rec);
                        }
                    }

                    if (cardResponse.more_flag.ToUpper() == "Y")
                    {
                        while (cardResponse.more_flag.ToUpper() == "Y")
                        {
                            wuCardLookupReq.wu_card_lookup_context = cardResponse.wu_card_lookup_context;
                            wuCardLookupReq.save_key = cardResponse.receiver_index_number;

                            //string requestXML2 = ConvertToXML(wuCardLookupReq, typeof(WUCardLookupService.wucardlookuprequest));

                            //New Request with Card Lookup Context to get next set of receivers from WU.
                            cardResponse = wuCardPortTypeClient.WuCardLookup(wuCardLookupReq);

                            // Quick Fix.

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

                                    //if (!string.IsNullOrWhiteSpace(rec.name.first_name) && !string.IsNullOrWhiteSpace(rec.name.last_name) && !string.IsNullOrWhiteSpace(rec.address.Item.iso_code.country_code))
                                    //lstFinalReceivers.Add(rec);
                                }
                            }

                        }
                    }

                    lstFinalReceiver = WUTotalReceivers(lstFinalReceivers);

					#region AL-1014 Transactional Log User Story
					MongoDBLogger.Info<WUCardLookupService.wucardlookupreply>(customerSessionId, cardResponse, "WUPastBillersReceivers", AlloyLayerName.CXN,
						ModuleName.Transaction, "WUPastBillersReceivers -MGI.Cxn.WU.Common.Impl.IO", "RESPONSE", mgiContext);
					#endregion
				}
                else
                {
                    return null;
                }

            }
            catch (System.ServiceModel.FaultException<WUCustomerLookup.errorreply> ex)
            {
				//AL-1014 Transactional Log User Story
				MongoDBLogger.Error<CardLookUpRequest>(wucardlookupreq, "WUPastBillersReceivers", AlloyLayerName.CXN, ModuleName.Transaction,
					"Error in WUPastBillersReceivers -MGI.Cxn.WU.Common.Impl.IO", ex.Message, ex.StackTrace);
				
                throw new WUCommonException(WUCommonException.PROVIDER_ERROR, ex.Detail.error, ex);
            }
            catch (Exception ex)
            {
				//AL-1014 Transactional Log User Story
				MongoDBLogger.Error<CardLookUpRequest>(wucardlookupreq, "WUPastBillersReceivers", AlloyLayerName.CXN, ModuleName.Transaction,
				"Error in WUPastBillersReceivers -MGI.Cxn.WU.Common.Impl.IO", ex.Message, ex.StackTrace);
				throw new WUCommonException(WUCommonException.PROVIDER_ERROR, ex.Message, ex);				
            }

            return (lstFinalReceiver.Count != 0) ? lstFinalReceiver : null;

        }
        # endregion
    }
}
