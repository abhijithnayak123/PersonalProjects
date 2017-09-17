using TCF.Zeo.Cxn.WU.Common.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCF.Zeo.Common.Data;
using TCF.Zeo.Cxn.WU.Common.Data;
using System.ServiceModel;
using TCF.Zeo.Common.Util;
using System.Configuration;
using System.Security.Cryptography.X509Certificates;
using AutoMapper;
using P3Net.Data.Common;
using P3Net.Data;
using System.Data;
using TCF.Zeo.Cxn.WU.Common.DASService;
using CardEnroll = TCF.Zeo.Cxn.WU.Common.WUCardEnrollmentService;
using System.Xml.Serialization;
using System.IO;

namespace TCF.Zeo.Cxn.WU.Common.Impl
{
    public class IO : BaseIO, IWUCommonIO
    {
        #region Properties

        //private bool IsHardCodedCounterId { get; } = Convert.ToBoolean(ConfigurationManager.AppSettings["IsHardCodedCounterId"]);
        //private X509Certificate2 WUCertificate = null;

        #endregion

        public GeneralName BuildGeneralName(ZeoContext context)
        {
            GeneralName name = new GeneralName()
            {
                Type = NameType.D,
                NameTypeSpecified = true,
                FirstName = context.AgentFirstName ?? string.Empty,
                LastName = context.AgentLastName ?? string.Empty
            };
            return name;
        }

        public SwbFlaInfo BuildSwbFlaInfo(ZeoContext context)
        {
            SwbFlaInfo swFlaInfo = new SwbFlaInfo()
            {
                SwbOperatorId = context.AgentId != 0 ? Convert.ToString(context.AgentId) : string.Empty,
                ReadPrivacyNoticeFlagSpecified = true,
                ReadPrivacyNoticeFlag = SwbFlaInfoReadPrivacyNoticeFlag.Y,
                FlagCertificationFlagSpecified = true,
                FlagCertificationFlag = SwbFlaInfoFlaCertificationFlag.Y
            };

            return swFlaInfo;
        }

        public WUBaseRequestResponse CreateRequest(long channelPartnerId, ZeoContext context)
        {
            WUBaseRequestResponse request = new WUBaseRequestResponse();

            WUCredential credential = GetCredential(channelPartnerId, context.ProductType);

            request.Channel = new Channel() { Name = credential.ChannelName, Version = credential.ChannelVersion };

            request.ForeignRemoteSystem = new ForeignRemoteSystem()
            {
                CoutnerId = GetCounterId(credential.CounterId, context.WUCounterId),
                Identifier = credential.AccountIdentifier
            };

            request.ClientCertificate = (WUCertificate == null) ? GetWUCredentialCertificate(credential, context.ProductType) : WUCertificate;

            request.ServiceUrl = credential.WUServiceUrl;

            return request;
        }

        public CardInfo GetCardInfo(CardLookUpRequest wuCardLookupReq, ZeoContext context)
        {
            CardInfo CardInfo = new CardInfo();
            try
            {
                CardInfo = null;

                WUCardLookupService.channel lookupChannel = null;
                WUCardLookupService.foreign_remote_system lookupRemoteSystem = null;

                WUCardLookupService.wucardlookuprequest cardLookupRequest = new WUCardLookupService.wucardlookuprequest();

                WUCredential wuCredentials = new WUCredential();
                wuCredentials = GetWUCredential(context.ChannelPartnerId);
                if (wuCredentials == null)
                    throw new WUCommonException(AlloyUtil.GetProductCode(context.ProductType), WUCommonException.WESTERNUNION_CREDENTIALS_NOTFOUND, null);

                List<object> responseList = new List<object>();
                WUCardLookupService.WUCardLookupPortTypeClient WUPortTypeClient = new WUCardLookupService.WUCardLookupPortTypeClient("WUCardLookup", wuCredentials.WUServiceUrl.ToString());
                WUCertificate = WUCertificate == null ? GetWUCredentialCertificate(wuCredentials, context.ProductType) : WUCertificate;
                WUPortTypeClient.ClientCredentials.ClientCertificate.Certificate = WUCertificate;

                cardLookupRequest = WUCardLookupRequestMapper(wuCardLookupReq);

                lookupChannel = GetCardLookupChannel(wuCredentials);
                lookupRemoteSystem = GetCardLookupRemoteSystem(wuCredentials, context);

                cardLookupRequest.channel = lookupChannel;
                lookupRemoteSystem.reference_no = DateTime.Now.ToString("yyyyMMddhhmmssff");
                cardLookupRequest.foreign_remote_system = lookupRemoteSystem;
                cardLookupRequest.swb_fla_info = mapper.Map<SwbFlaInfo, WUCardLookupService.swb_fla_info>(BuildSwbFlaInfo(context));
                cardLookupRequest.swb_fla_info.fla_name = mapper.Map<GeneralName, WUCardLookupService.general_name>(BuildGeneralName(context));

                WUCardLookupService.wucardlookupreply response = WUPortTypeClient.WuCardLookup(cardLookupRequest);

                CardInfo = CardLookUpResponse(response);
            }
            catch (System.ServiceModel.FaultException<WUCardLookupService.errorreply> ex)
            {
                string code = ExceptionHelper.GetProviderErrorCode(ex.Detail.error);
                throw new WUCommonProviderException(AlloyUtil.GetProductCode(context.ProductType), AlloyUtil.GetProviderCode(context.ProductType), code, ex.Detail.error, ex);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                HandleException(ex, context.ProductType);
                throw new WUCommonException(AlloyUtil.GetProductCode(context.ProductType), WUCommonException.WUCARD_LOOKUP_FAILED, ex);
            }
            return CardInfo;
        }

        public string GetCountryName(string countryCode)
        {
            throw new NotImplementedException();
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

        public List<AgentBanners> GetWUAgentBannerMsgs(ZeoContext context)
        {
            try
            {
                List<AgentBanners> agentbanners = new List<AgentBanners>();
                long transactionId = Convert.ToInt64(DateTime.Now.ToString("yyyyMMddhhmmssff"));
                filters_type filters = new filters_type();
                filters.queryfilter1 = "en";
                List<BANNERMESSAGE_Type> msgs = getdasResponse(transactionId, "GetWUAgentBannerMsgs", context, filters).ConvertAll<BANNERMESSAGE_Type>(t => (BANNERMESSAGE_Type)t);
                agentbanners.AddRange(mapper.Map<List<BANNERMESSAGE_Type>, List<AgentBanners>>(msgs));
                return agentbanners;
            }
            catch (FaultException<DASService.errorreply> ex)
            {
                string code = ExceptionHelper.GetProviderErrorCode(ex.Detail.error); ;
                throw new WUCommonProviderException(AlloyUtil.GetProductCode(context.ProductType), AlloyUtil.GetProviderCode(context.ProductType), code, ex.Detail.error, ex);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                HandleException(ex, context.ProductType);
                throw new WUCommonException(AlloyUtil.GetProductCode(context.ProductType), WUCommonException.BANNERMESSAGE_GET_FAILED, ex);
            }
        }

        public bool IsSWBState(string stateCode)
        {
            bool IsSWBState = false;
            string[] swbStates = { "AZ", "CA", "NM", "TX" };

            if (!string.IsNullOrWhiteSpace(stateCode))
                IsSWBState = swbStates.Contains(stateCode);

            return IsSWBState;
        }

        public string TrimOccupation(string occupation)
        {
            if (!string.IsNullOrEmpty(occupation) && occupation.Length > OCCUPATION_LENGTH)
                return occupation.Substring(0, OCCUPATION_LENGTH);
            else
                return occupation;
        }

        public CardDetails WUCardEnrollment(WUEnrollmentRequest enrollmentReq, ZeoContext context)
        {
            CardEnroll.channel channel = null;
            CardEnroll.foreign_remote_system remotesystem = null;
            CardDetails cardDetails = null;

            try
            {
                CardEnroll.wucardenrollmentrequest cardEnrollRequest = new CardEnroll.wucardenrollmentrequest();

                WUCredential wuCredentials = GetWUCredential(context.ChannelPartnerId);

                channel = GetCardServiceChannel(wuCredentials);
                remotesystem = GetCardServiceRemoteSystem(wuCredentials, context);

                CardEnroll.WUCardEnrollmentPortTypeClient WUPortTypeClient = new CardEnroll.WUCardEnrollmentPortTypeClient("SOAP_HTTP_Port6", wuCredentials.WUServiceUrl);

                WUCertificate = WUCertificate == null ? GetWUCredentialCertificate(wuCredentials, context.ProductType) : WUCertificate;

                WUPortTypeClient.ClientCredentials.ClientCertificate.Certificate = WUCertificate;

                if (enrollmentReq != null)
                {
                    enrollmentReq.AddressAddrLine1 = AlloyUtil.MassagingValue(enrollmentReq.AddressAddrLine1);
                    enrollmentReq.AddressAddrLine2 = AlloyUtil.MassagingValue(enrollmentReq.AddressAddrLine2);
                    enrollmentReq.AddressCity = AlloyUtil.MassagingValue(enrollmentReq.AddressCity);
                    enrollmentReq.AddressState = AlloyUtil.MassagingValue(enrollmentReq.AddressState);
                    enrollmentReq.AddressStreet = AlloyUtil.MassagingValue(enrollmentReq.AddressStreet);
                    enrollmentReq.CountryName = AlloyUtil.MassagingValue(enrollmentReq.CountryName);
                    enrollmentReq.FirstName = AlloyUtil.MassagingValue(enrollmentReq.FirstName);
                    enrollmentReq.LastName = AlloyUtil.MassagingValue(enrollmentReq.LastName);
                    enrollmentReq.MiddleName = AlloyUtil.MassagingValue(enrollmentReq.MiddleName);

                    cardEnrollRequest = CardEnrollmentRequestMapper(enrollmentReq);
                }

                cardEnrollRequest.swb_fla_info = mapper.Map<SwbFlaInfo, CardEnroll.swb_fla_info>(BuildSwbFlaInfo(context));
                cardEnrollRequest.swb_fla_info.fla_name = mapper.Map<GeneralName, CardEnroll.general_name>(BuildGeneralName(context));
                cardEnrollRequest.channel = channel;
                remotesystem.reference_no = DateTime.Now.ToString("yyyyMMddhhmmssff");
                cardEnrollRequest.foreign_remote_system = remotesystem;
                CardEnroll.wucardenrollmentreply response = WUPortTypeClient.WUCardEnrollment(cardEnrollRequest);
                cardDetails = EnrollCardResponseMapper(response);

                return cardDetails;
            }
            catch (System.ServiceModel.FaultException<CardEnroll.errorreply> ex)
            {
                string code = ExceptionHelper.GetProviderErrorCode(ex.Detail.error);
                throw new WUCommonProviderException(AlloyUtil.GetProductCode(context.ProductType), AlloyUtil.GetProviderCode(context.ProductType), code, ex.Detail.error, ex);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                HandleException(ex, context.ProductType);
                throw new WUCommonException(AlloyUtil.GetProductCode(context.ProductType), WUCommonException.WUCARD_ENROLLMENT_FAILED, ex);
            }
        }

        public CardLookupDetails WUCardLookup(CardLookUpRequest wucardlookupreq, ZeoContext context)
        {
            try
            {
                WUCustomerLookupService.channel lookupChannel = null;
                WUCustomerLookupService.foreign_remote_system lookupRemoteSystem = null;

                CardLookupDetails cardLookupDetails = null;
                wucardlookupreq = GetWUCardLookUpRequest(wucardlookupreq);

                WUCustomerLookupService.wucustomerlookuprequest cardLookupRequest = new WUCustomerLookupService.wucustomerlookuprequest();

                WUCredential wuCredentials = GetWUCredential(context.ChannelPartnerId);

                if (wuCredentials == null)
                    throw new WUCommonException(AlloyUtil.GetProductCode(context.ProductType), WUCommonException.WESTERNUNION_CREDENTIALS_NOTFOUND, null);

                //ConfigureLookupWUOject(wuCredentials, context);
                lookupChannel = GetLookupChannel(wuCredentials);
                lookupRemoteSystem = GetLookupRemoteSystem(wuCredentials, context);

                WUCustomerLookupService.CustomerLookupPortTypeClient WUPortTypeClient = new WUCustomerLookupService.CustomerLookupPortTypeClient("SOAP_HTTP_Port8", wuCredentials.WUServiceUrl);

                WUCertificate = WUCertificate ?? GetWUCredentialCertificate(wuCredentials, context.ProductType);
                WUPortTypeClient.ClientCredentials.ClientCertificate.Certificate = WUCertificate;

                cardLookupRequest = CardLookupRequestMapper(wucardlookupreq, lookupRemoteSystem, lookupChannel, context);

                WUCustomerLookupService.wucustomerlookupreply response = new WUCustomerLookupService.wucustomerlookupreply();
                List<Sender> senderList = new List<Sender>();

                do
                {
                    response = WUPortTypeClient.CustomerLookup(cardLookupRequest);

                    if (response != null)
                    {
                        if (senderList.Count == 0)
                        {
                            senderList = CardLookupResponse(response);
                        }
                        else
                        {
                            senderList.AddRange(CardLookupResponse(response));
                        }
                        if (response.more_flag == WUEnums.yes_no.Y.ToString())
                        {
                            cardLookupRequest.save_key = response.save_key;
                        }
                    }
                } while (response != null && response.more_flag == WUEnums.yes_no.Y.ToString());

                cardLookupDetails = new CardLookupDetails()
                {
                    Sender = new Sender[senderList.Count]
                };
                cardLookupDetails.Sender = senderList.ToArray();

                return cardLookupDetails;
            }
            catch (System.ServiceModel.FaultException<WUCustomerLookupService.errorreply> ex)
            {
                string code = ExceptionHelper.GetProviderErrorCode(ex.Detail.error); ;
                throw new WUCommonProviderException(AlloyUtil.GetProductCode(context.ProductType), AlloyUtil.GetProviderCode(context.ProductType), code, ex.Detail.error, ex);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                HandleException(ex, context.ProductType);
                throw new WUCommonException(AlloyUtil.GetProductCode(context.ProductType), WUCommonException.WUCARD_LOOKUP_FAILED, ex);
            }
        }

        public CardLookupDetails WUCardLookupForCardNumber(CardLookUpRequest wuCardLookupReq, ZeoContext context)
        {
            try
            {

                WUCardLookupService.foreign_remote_system cardLookupRemoteSystem = null;

                CardLookupDetails cardLookupDetails = null;
                wuCardLookupReq = GetWUCardLookUpRequest(wuCardLookupReq);

                WUCardLookupService.wucardlookuprequest cardLookupRequest = new WUCardLookupService.wucardlookuprequest();

                WUCredential wuCredentials = GetWUCredential(context.ChannelPartnerId);
                if (wuCredentials == null)
                    throw new WUCommonException(AlloyUtil.GetProductCode(context.ProductType), WUCommonException.WESTERNUNION_CREDENTIALS_NOTFOUND, null);

                cardLookupRemoteSystem = GetCardLookupRemoteSystem(wuCredentials, context);

                WUCardLookupService.WUCardLookupPortTypeClient WUPortTypeClient = new WUCardLookupService.WUCardLookupPortTypeClient("WUCardLookup", wuCredentials.WUServiceUrl);

                WUCertificate = WUCertificate ?? GetWUCredentialCertificate(wuCredentials, context.ProductType);
                WUPortTypeClient.ClientCredentials.ClientCertificate.Certificate = WUCertificate;

                cardLookupRequest = WUCardLookupRequestMapper(wuCardLookupReq, cardLookupRemoteSystem, wuCredentials, context);

                WUCardLookupService.wucardlookupreply response = WUPortTypeClient.WuCardLookup(cardLookupRequest);

                cardLookupDetails = WUCardLookupResponse(response);

                return cardLookupDetails;
            }
            catch (System.ServiceModel.FaultException<WUCardLookupService.errorreply> ex)
            {
                string code = ExceptionHelper.GetProviderErrorCode(ex.Detail.error); ;
                throw new WUCommonProviderException(AlloyUtil.GetProductCode(context.ProductType), AlloyUtil.GetProviderCode(context.ProductType), code, ex.Detail.error, ex);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                HandleException(ex, context.ProductType);
                throw new WUCommonException(AlloyUtil.GetProductCode(context.ProductType), WUCommonException.WUCARD_LOOKUP_FAILED, ex);
            }
        }

        public List<Receiver> WUPastBillersReceivers(CardLookUpRequest wucardlookupreq, ZeoContext context)
        {
            try
            {
                //WUCardLookupService.channel lookupChannel = null;
                WUCardLookupService.foreign_remote_system lookupRemoteSystem = null;
                List<WUCardLookupService.receiver> lstFinalReceivers = new List<WUCardLookupService.receiver>(); // list of receiver of type WU Specific Format
                List<Receiver> lstFinalReceiver = new List<Receiver>(); // list of receiver of CXN-WU-Common

                WUCardLookupService.wucardlookuprequest wuCardLookupReq = new WUCardLookupService.wucardlookuprequest();
                WUCustomerLookupService.wucustomerlookuprequest cardLookupRequest = new WUCustomerLookupService.wucustomerlookuprequest();
                WUCardLookupService.wucardlookupreply cardResponse = null;

                WUCredential wuCredentials = GetCredential(context.ChannelPartnerId, context.ProductType);

                if (wuCredentials == null)
                {
                    throw new WUCommonException(AlloyUtil.GetProductCode(context.ProductType), WUCommonException.WESTERNUNION_CREDENTIALS_NOTFOUND, null);
                }

                WUCardLookupService.WUCardLookupPortTypeClient wuCardPortTypeClient = new WUCardLookupService.WUCardLookupPortTypeClient("WUCardLookup", wuCredentials.WUServiceUrl);
                WUCertificate = WUCertificate == null ? GetWUCredentialCertificate(wuCredentials, context.ProductType) : WUCertificate;
                wuCardPortTypeClient.ClientCredentials.ClientCertificate.Certificate = WUCertificate;

                //lookupChannel = GetCardLookupChannel(wuCredentials);
                lookupRemoteSystem = GetCardLookupRemoteSystem(wuCredentials, context);

                lookupRemoteSystem.reference_no = DateTime.Now.ToString("yyyyMMddhhmmssff");

                wuCardLookupReq = MapperCardLookupToWUCardLookUp(wucardlookupreq);

                wuCardLookupReq = WUCardLookupRequestMapper(wucardlookupreq, lookupRemoteSystem, wuCredentials, context);

                do
                {
                    cardResponse = wuCardPortTypeClient.WuCardLookup(wuCardLookupReq);

                    if (cardResponse.receiver != null && cardResponse.receiver.Length > 0)
                    {
                        foreach (WUCardLookupService.receiver rec in cardResponse.receiver)
                        {
                            switch (rec.name.name_type)
                            {
                                case WUCardLookupService.name_type.C:
                                    if (!string.IsNullOrWhiteSpace(rec.name.business_name) && !string.IsNullOrWhiteSpace(rec.debtor_account_number) && !string.IsNullOrWhiteSpace(rec.address.Item.iso_code.country_code))
                                        lstFinalReceivers.Add(rec);
                                    break;

                                case WUCardLookupService.name_type.D:
                                    if (!string.IsNullOrWhiteSpace(rec.name.first_name) && !string.IsNullOrWhiteSpace(rec.name.last_name) && !string.IsNullOrWhiteSpace(rec.address.Item.iso_code.country_code))
                                        lstFinalReceivers.Add(rec);
                                    break;

                                case WUCardLookupService.name_type.M:
                                    if (!string.IsNullOrWhiteSpace(rec.name.given_name) && !string.IsNullOrWhiteSpace(rec.name.paternal_name.Trim()) && !string.IsNullOrWhiteSpace(rec.name.paternal_name.ToString().Trim()) && !string.IsNullOrWhiteSpace(rec.address.Item.iso_code.country_code))
                                        lstFinalReceivers.Add(rec);
                                    break;
                            }
                        }
                    }
                    if (cardResponse.more_flag.ToUpper() == "Y")
                    {
                        wuCardLookupReq.wu_card_lookup_context = cardResponse.wu_card_lookup_context;
                        wuCardLookupReq.save_key = cardResponse.receiver_index_number;
                    }

                } while (cardResponse.more_flag.ToUpper() == "Y");

                lstFinalReceiver = WUTotalReceivers(lstFinalReceivers);

                return (lstFinalReceiver.Count > 0) ? lstFinalReceiver : null;
            }
            catch (System.ServiceModel.FaultException<WUCustomerLookupService.errorreply> ex)
            {
                string code = ExceptionHelper.GetProviderErrorCode(ex.Detail.error); ;
                throw new WUCommonProviderException(AlloyUtil.GetProductCode(context.ProductType), AlloyUtil.GetProviderCode(context.ProductType), code, ex.Detail.error, ex);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                HandleException(ex, context.ProductType);
                throw new WUCommonException(AlloyUtil.GetProductCode(context.ProductType), WUCommonException.PAST_BILLERS_GET_FAILED, ex);
            }

        }


        #region Private Methods

        private CardEnroll.channel GetCardServiceChannel(WUCredential wuCredentials)
        {
            CardEnroll.channel chn = new CardEnroll.channel();
            chn.type = CardEnroll.channel_type.H2H;
            chn.name = wuCredentials.ChannelName.ToString();
            chn.version = wuCredentials.ChannelVersion.ToString();
            chn.typeSpecified = true;
            return chn;
        }

        private CardEnroll.foreign_remote_system GetCardServiceRemoteSystem(WUCredential wuCredentials, ZeoContext context)
        {
            string locationId = GetCounterId(wuCredentials.CounterId, context.WUCounterId);
            if (string.IsNullOrWhiteSpace(wuCredentials.AccountIdentifier) || string.IsNullOrWhiteSpace(locationId))
            {
                throw new WUCommonException(AlloyUtil.GetProductCode(context.ProductType), WUCommonException.INVALID_ACCOUNTIDENTIFIER_COUNTERID, null);
            }
            CardEnroll.foreign_remote_system frs = new CardEnroll.foreign_remote_system();
            frs.identifier = wuCredentials.AccountIdentifier;
            frs.counter_id = locationId;
            return frs;
        }

        private CardEnroll.wucardenrollmentrequest CardEnrollmentRequestMapper(WUEnrollmentRequest enrollmentReq)
        {
            CardEnroll.iso_code isoCode = new CardEnroll.iso_code() { country_code = "US", currency_code = "USD" };

            CardEnroll.wucardenrollmentrequest request = new CardEnroll.wucardenrollmentrequest()
            {
                sender = new CardEnroll.sender()
                {
                    name = new CardEnroll.general_name()
                    {
                        name_type = CardEnroll.name_type.D,
                        name_typeSpecified = true,
                        first_name = string.IsNullOrEmpty(enrollmentReq.FirstName) ? string.Empty : enrollmentReq.FirstName,
                        last_name = string.IsNullOrEmpty(enrollmentReq.LastName) ? string.Empty : enrollmentReq.LastName,
                    },
                    address = new CardEnroll.address()
                    {
                        addr_line1 = enrollmentReq.AddressAddrLine1,
                        addr_line2 = enrollmentReq.AddressAddrLine2,
                        city = enrollmentReq.AddressCity,
                        state = enrollmentReq.AddressState,
                        postal_code = enrollmentReq.AddressPostalCode,
                        Item = new CardEnroll.country_currency_info() { iso_code = isoCode }
                    },
                    contact_phone = enrollmentReq.ContactPhone,
                    email = enrollmentReq.Email
                },
                payment_details = new CardEnroll.payment_details()
                {
                    recording_country_currency = new CardEnroll.country_currency_info() { iso_code = isoCode },
                    destination_country_currency = new CardEnroll.country_currency_info() { iso_code = isoCode },
                    originating_country_currency = new CardEnroll.country_currency_info() { iso_code = isoCode },
                    original_destination_country_currency = new CardEnroll.country_currency_info() { iso_code = isoCode },
                }
            };
            return request;
        }

        private CardDetails EnrollCardResponseMapper(CardEnroll.wucardenrollmentreply response)
        {
            CardDetails cardDetails = null;
            if (response != null)
            {
                cardDetails = new CardDetails()
                {
                    AccountNumber = response.sender.preferred_customer.account_nbr,
                    ForiegnSystemId = response.foreign_remote_system.identifier,
                    ForiegnRefNum = response.foreign_remote_system.reference_no,
                    CounterId = response.foreign_remote_system.counter_id
                };
            }
            return cardDetails;
        }

        private WUCardLookupService.foreign_remote_system GetCardLookupRemoteSystem(WUCredential wuCredentials, ZeoContext context)
        {
            string locationId = GetCounterId(wuCredentials.CounterId, context.WUCounterId);

            if (string.IsNullOrWhiteSpace(wuCredentials.AccountIdentifier) || string.IsNullOrWhiteSpace(locationId))
                throw new WUCommonException(AlloyUtil.GetProductCode(context.ProductType), WUCommonException.INVALID_ACCOUNTIDENTIFIER_COUNTERID, null);


            return new WUCardLookupService.foreign_remote_system()
            {
                identifier = wuCredentials.AccountIdentifier,
                counter_id = locationId,
            };
        }

        private WUCredential GetCredential(long channelPartnerId, string productType)
        {
            WUCredential wuCredential = GetWUCredential(channelPartnerId);

            if (wuCredential == null)
                throw new WUCommonException(AlloyUtil.GetProductCode(productType), WUCommonException.WESTERNUNION_CREDENTIALS_NOTFOUND, null);

            return wuCredential;
        }

        private string GetCounterId(string hardCodedWUCounterId, string wUCounterId)
        {
            return IsHardCodedCounterId ? hardCodedWUCounterId : wUCounterId;
        }

        private X509Certificate2 GetWUCredentialCertificate(WUCredential wuCredentials, string productType)
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
                throw new WUCommonException(AlloyUtil.GetProductCode(productType), WUCommonException.CERIFICATE_NOTFOUND, null);
            }

            return certificates[0];
        }

        private List<object> getdasResponse(long transactionId, string dasServiceName, ZeoContext context, filters_type queryfilters = null)
        {
            WUCredential wuCredentials = new WUCredential();
            wuCredentials = GetWUCredential(context.ChannelPartnerId);

            string locationId = GetCounterId(wuCredentials.CounterId, context.WUCounterId);
            List<object> responseList = new List<object>();
            DASInquiryPortTypeClient dc = new DASInquiryPortTypeClient("SOAP_HTTP_Port2", wuCredentials.WUServiceUrl);

            WUCertificate = WUCertificate == null ? GetWUCredentialCertificate(wuCredentials, context.ProductType) : WUCertificate;
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
            request.foreign_remote_system = remotesystem;
            request.name = dasServiceName;
            request.filters = queryfilters;
            REPLYType responseItems = null;


            h2hdasreply reply = dc.DAS_Service(request);
            if (reply != null)
            {
                responseItems = (REPLYType)reply.MTML.Item;
            }
            if (responseItems.DATA_CONTEXT.RECORDSET != null)
                responseList.AddRange(responseItems.DATA_CONTEXT.RECORDSET.Items.ToList<object>());

            return responseList;
        }


        private WUCardLookupService.channel GetCardLookupChannel(WUCredential wuCredentials)
        {
            return new WUCardLookupService.channel()
            {
                type = WUCardLookupService.channel_type.H2H,
                typeSpecified = true,
                name = wuCredentials.ChannelName,
                version = wuCredentials.ChannelVersion,
            };
        }

        private WUCardLookupService.wucardlookuprequest MapperCardLookupToWUCardLookUp(CardLookUpRequest wucardlookupreq)
        {
            WUCardLookupService.wucardlookuprequest request = new WUCardLookupService.wucardlookuprequest();
            WUCardLookupService.card_update_indicator status;

            if (wucardlookupreq.sender.PreferredCustomerPermanentChange != null)
                status = ParseEnum<WUCardLookupService.card_update_indicator>(wucardlookupreq.sender.PreferredCustomerPermanentChange.ToString());
            else
                status = 0;

            request.sender = new WUCardLookupService.sender();
            request.sender.preferred_customer = new WUCardLookupService.preferred_customer();
            request.sender.preferred_customer.account_nbr = string.IsNullOrWhiteSpace(wucardlookupreq.sender.PreferredCustomerAccountNumber) ? string.Empty : wucardlookupreq.sender.PreferredCustomerAccountNumber.Trim();
            request.sender.preferred_customer.permanent_change = status;

            WUCardLookupService.convenience_search cardsenderConsearchRequest = new WUCardLookupService.convenience_search();

            if (wucardlookupreq.conveniencesearch != null)
            {
                cardsenderConsearchRequest.type = wucardlookupreq.conveniencesearch.typefield;
                request.convenience_search = cardsenderConsearchRequest;
            }

            request.receiver_index_number = wucardlookupreq.receiver_index_number != String.Empty ? wucardlookupreq.receiver_index_number : String.Empty;

            request.wu_card_lookup_context = wucardlookupreq.wu_card_lookup_context != String.Empty ? wucardlookupreq.wu_card_lookup_context : String.Empty;
            request.card_lookup_search_type = "S";
            request.save_key = wucardlookupreq.save_key != String.Empty ? wucardlookupreq.save_key : String.Empty;

            return request;
        }

        private WUCardLookupService.wucardlookuprequest WUCardLookupRequestMapper(CardLookUpRequest wucardlookupreq, WUCardLookupService.foreign_remote_system cardLookupRemoteSystem, WUCredential wuCredentials, ZeoContext context)
        {
            WUCardLookupService.channel cardLookupChannel = null;

            cardLookupChannel = GetCardLookupChannel(wuCredentials);

            WUCardLookupService.wucardlookuprequest request = new WUCardLookupService.wucardlookuprequest();
            request.sender = new WUCardLookupService.sender();
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
            request.swb_fla_info = mapper.Map<SwbFlaInfo, WUCardLookupService.swb_fla_info>(BuildSwbFlaInfo(context));
            request.swb_fla_info.fla_name = mapper.Map<GeneralName, WUCardLookupService.general_name>(BuildGeneralName(context));
            request.channel = cardLookupChannel;
            cardLookupRemoteSystem.reference_no = DateTime.Now.ToString("yyyyMMddhhmmssff");
            request.foreign_remote_system = cardLookupRemoteSystem;
            return request;
        }

        private List<Receiver> WUTotalReceivers(List<WUCardLookupService.receiver> receivers)
        {
            List<Receiver> receiversList;

            receiversList = new List<Receiver>();

            foreach (WUCardLookupService.receiver receiversFromReply in receivers)
            {
                Receiver receiver = new Receiver();

                if (receivers.Count > 0)
                {
                    // if the WUCustomerLookup.name_type == "C" it means the Billers, WUCustomerLookup.name_type == "D" it means Receivers and WUCustomerLookup.name_type == "M" it means Receivers with maternal and paternal name.
                    switch (receiversFromReply.name.name_type)
                    {
                        case WUCardLookupService.name_type.C:
                            if (receiversFromReply.name.business_name != string.Empty && receiversFromReply.debtor_account_number != string.Empty)
                            {
                                receiver.BusinessName = string.IsNullOrWhiteSpace(receiversFromReply.name.business_name) ? string.Empty : receiversFromReply.name.business_name;
                                receiver.Attention = string.IsNullOrWhiteSpace(receiversFromReply.debtor_account_number) ? string.Empty : receiversFromReply.debtor_account_number;
                            }
                            break;

                        case WUCardLookupService.name_type.D:
                            if (!string.IsNullOrWhiteSpace(receiversFromReply.name.first_name) && !string.IsNullOrWhiteSpace(receiversFromReply.name.last_name) && !string.IsNullOrWhiteSpace(receiversFromReply.address.Item.iso_code.country_code))
                            {
                                receiver.FirstName = string.IsNullOrWhiteSpace(receiversFromReply.name.first_name) ? string.Empty : receiversFromReply.name.first_name;
                                receiver.LastName = string.IsNullOrWhiteSpace(receiversFromReply.name.last_name) ? string.Empty : receiversFromReply.name.last_name;
                            }

                            break;

                        case WUCardLookupService.name_type.M:
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
            return receiversList;
        }

        private static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        private WUCredential GetWUCredential(long channelPartnerId)
        {
            StoredProcedure moneyTransferProcedure = new StoredProcedure("usp_GetWUnionCredentials");

            moneyTransferProcedure.WithParameters(InputParameter.Named("channerlPartnerId").WithValue(channelPartnerId));

            WUCredential credential = null;

            using (IDataReader datareader = DataHelper.GetConnectionManager().ExecuteReader(moneyTransferProcedure))
            {
                while (datareader.Read())
                {
                    credential = new WUCredential();
                    credential.Id = datareader.GetInt64OrDefault("WUCredentialID");
                    credential.WUServiceUrl = datareader.GetStringOrDefault("WUServiceUrl");
                    credential.WUClientCertificateSubjectName = datareader.GetStringOrDefault("WUClientCertificateSubjectName");
                    credential.AccountIdentifier = datareader.GetStringOrDefault("AccountIdentifier");
                    credential.CounterId = datareader.GetStringOrDefault("CounterId");
                    credential.ChannelName = datareader.GetStringOrDefault("ChannelName");
                    credential.ChannelVersion = datareader.GetStringOrDefault("ChannelVersion");
                    credential.ChannelPartnerId = datareader.GetInt64OrDefault("ChannelPartnerId");
                }
            }

            return credential;
        }

        private CardLookUpRequest GetWUCardLookUpRequest(CardLookUpRequest wucardlookuprequest)
        {
            wucardlookuprequest.Firstname = AlloyUtil.MassagingValue(wucardlookuprequest.Firstname);
            wucardlookuprequest.LastName = AlloyUtil.MassagingValue(wucardlookuprequest.LastName);
            wucardlookuprequest.MiddleName = AlloyUtil.MassagingValue(wucardlookuprequest.MiddleName);

            wucardlookuprequest.sender.AddressAddrLine1 = AlloyUtil.MassagingValue(wucardlookuprequest.sender.AddressAddrLine1);
            wucardlookuprequest.sender.AddressAddrLine2 = AlloyUtil.MassagingValue(wucardlookuprequest.sender.AddressAddrLine2);
            wucardlookuprequest.sender.AddressCity = AlloyUtil.MassagingValue(wucardlookuprequest.sender.AddressCity);
            wucardlookuprequest.sender.AddressState = AlloyUtil.MassagingValue(wucardlookuprequest.sender.AddressState);
            wucardlookuprequest.sender.AddressStreet = AlloyUtil.MassagingValue(wucardlookuprequest.sender.AddressStreet);
            wucardlookuprequest.sender.CountryName = AlloyUtil.MassagingValue(wucardlookuprequest.sender.CountryName);
            wucardlookuprequest.sender.FirstName = AlloyUtil.MassagingValue(wucardlookuprequest.sender.FirstName);
            wucardlookuprequest.sender.LastName = AlloyUtil.MassagingValue(wucardlookuprequest.sender.LastName);
            wucardlookuprequest.sender.MiddleName = AlloyUtil.MassagingValue(wucardlookuprequest.sender.MiddleName);
            return wucardlookuprequest;
        }

        private WUCustomerLookupService.channel GetLookupChannel(WUCredential wuCredentials)
        {
            WUCustomerLookupService.channel chn = new WUCustomerLookupService.channel();
            chn.type = WUCustomerLookupService.channel_type.H2H;
            chn.name = wuCredentials.ChannelName.ToString();
            chn.version = wuCredentials.ChannelVersion.ToString();
            chn.typeSpecified = true;
            return chn;
        }

        private WUCustomerLookupService.foreign_remote_system GetLookupRemoteSystem(WUCredential wuCredentials, ZeoContext context)
        {
            string locationId = GetCounterId(wuCredentials, context);
            if (string.IsNullOrWhiteSpace(wuCredentials.AccountIdentifier) || string.IsNullOrWhiteSpace(locationId))
            {
                throw new WUCommonException(AlloyUtil.GetProductCode(context.ProductType), WUCommonException.INVALID_ACCOUNTIDENTIFIER_COUNTERID, null);
            }
            WUCustomerLookupService.foreign_remote_system frs = new WUCustomerLookupService.foreign_remote_system();
            frs.identifier = wuCredentials.AccountIdentifier;
            frs.counter_id = locationId;
            return frs;
        }

        private string GetCounterId(WUCredential wuCredential, ZeoContext context)
        {
            return IsHardCodedCounterId == true ? wuCredential.CounterId : context.WUCounterId;
        }

        private WUCustomerLookupService.wucustomerlookuprequest CardLookupRequestMapper(CardLookUpRequest wucardlookupreq, WUCustomerLookupService.foreign_remote_system lookupRemoteSystem, WUCustomerLookupService.channel lookupChannel, ZeoContext context)
        {
            WUCustomerLookupService.wucustomerlookuprequest request = new WUCustomerLookupService.wucustomerlookuprequest();

            request.sender = new WUCustomerLookupService.sender()
            {
                name = new WUCustomerLookupService.general_name()
                {
                    name_type = WUCustomerLookupService.name_type.D,
                    first_name = wucardlookupreq.Firstname,
                    last_name = wucardlookupreq.LastName
                }
            };
            WUCustomerLookupService.foreign_remote_system frs = new WUCustomerLookupService.foreign_remote_system();
            frs.counter_id = lookupRemoteSystem.counter_id;
            frs.identifier = lookupRemoteSystem.identifier;
            request.foreign_remote_system = frs;
            WUCustomerLookupService.convenience_search senderConsearchRequest = new WUCustomerLookupService.convenience_search();
            senderConsearchRequest.type = "SN";
            request.convenience_search = senderConsearchRequest;
            request.receiver_index_number = wucardlookupreq.receiver_index_number;
            request.wu_card_lookup_context = wucardlookupreq.wu_card_lookup_context;
            request.card_lookup_search_type = "M"; //wucardlookupreq.card_lookup_search_type;
            request.save_key = wucardlookupreq.save_key;

            request.swb_fla_info = mapper.Map<SwbFlaInfo, WUCustomerLookupService.swb_fla_info>(BuildSwbFlaInfo(context));
            request.swb_fla_info.fla_name = mapper.Map<GeneralName, WUCustomerLookupService.general_name>(BuildGeneralName(context));
            request.channel = lookupChannel;
            lookupRemoteSystem.reference_no = DateTime.Now.ToString("yyyyMMddhhmmssff");
            request.foreign_remote_system = lookupRemoteSystem;

            return request;
        }

        private List<Sender> CardLookupResponse(WUCustomerLookupService.wucustomerlookupreply response)
        {
            List<Sender> senderList = new List<Sender>();

            if (response.sender != null)
            {
                if (response.sender.Length > 0)
                {
                    List<string> levelCodes = new List<string>() { "WU6", "WU7", "XXC", "SWP" };

                    var senders = from s in response.sender
                                  where !levelCodes.Contains(s.preferred_customer.level_code) && !s.preferred_customer.level_code.StartsWith("ZZ")
                                  select s;

                    //cardLookupDetails.Sender = new Sender[senders.Count()];
                    foreach (WUCustomerLookupService.sender senderFromReply in senders)
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
            return senderList;
        }

        private CardLookupDetails WUCardLookupResponse(WUCardLookupService.wucardlookupreply response)
        {
            CardLookupDetails cardLookupDetails = new CardLookupDetails();

            if (response.sender != null && response.sender.Length > 0)
            {
                if (response.wu_card != null)
                {
                    cardLookupDetails.WuCardTotalPointsEarned = response.wu_card.total_points_earned;
                }

                cardLookupDetails.Sender = new Sender[1];

                cardLookupDetails.Sender[0] = new Sender();
                WUCardLookupService.general_name name = response.sender.First().name;
                cardLookupDetails.Sender[0].FirstName = string.IsNullOrWhiteSpace(name.first_name) ? string.Empty : name.first_name;
                cardLookupDetails.Sender[0].LastName = string.IsNullOrWhiteSpace(name.last_name) ? string.Empty : name.last_name;
                cardLookupDetails.Sender[0].MiddleName = string.IsNullOrWhiteSpace(name.middle_name) ? string.Empty : name.middle_name;
            }
            return cardLookupDetails;
        }
        #endregion

    }
}
