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
using MGI.Common.Sys;

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
        public IExceptionHelper ExceptionHelper { get; set; }

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

        #region Public method

        internal List<object> getdasResponse(long transactionId, string dasServiceName, long channelPartnerId, MGIContext mgiContext, filters_type queryfilters = null)
        {
            WUCredential wuCredentials = new WUCredential();
            wuCredentials = WUCredentialRepo.FindBy(x => x.ChannelPartnerId == channelPartnerId);

            string locationId = GetCounterId(wuCredentials, mgiContext);
            List<object> responseList = new List<object>();
            DASInquiryPortTypeClient dc = new DASInquiryPortTypeClient("SOAP_HTTP_Port2", wuCredentials.WUServiceUrl.ToString());

            WUCertificate = WUCertificate == null ? GetWUCredentialCertificate(wuCredentials, mgiContext) : WUCertificate;
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

            #region AL-1014 Transactional Log User Story
            MongoDBLogger.Info<h2hdasrequest>(mgiContext.CustomerSessionId, request, "getdasResponse", AlloyLayerName.CXN,
                ModuleName.Transaction, "getdasResponse -MGI.Cxn.WU.Common.Impl.BaseIO", "REQUEST", mgiContext);
            #endregion
            h2hdasreply reply = dc.DAS_Service(request);
            if (reply != null)
            {
                responseItems = (REPLYType)reply.MTML.Item;
            }
            if (responseItems.DATA_CONTEXT.RECORDSET != null)
                responseList.AddRange(responseItems.DATA_CONTEXT.RECORDSET.Items.ToList<object>());

            #region AL-1014 Transactional Log User Story
            MongoDBLogger.Info<REPLYType>(mgiContext.CustomerSessionId, responseItems, "getdasResponse", AlloyLayerName.CXN,
                ModuleName.Transaction, "getdasResponse -MGI.Cxn.WU.Common.Impl.BaseIO", "RESPONSE", mgiContext);
            #endregion
            return responseList;
        }

        internal List<Receiver> WUTotalReceivers(List<WUCardLookupService.receiver> receivers)
        {
            List<Receiver> receiversList;

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
                throw new WUCommonException(MGI.Common.Util.NexxoUtil.GetProductCode(mgiContext.ProductType), WUCommonException.INVALID_ACCOUNTIDENTIFIER_COUNTERID, null);
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

        internal X509Certificate2 GetWUCredentialCertificate(WUCredential wuCredentials, MGIContext mgiContext)
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
                throw new WUCommonException(MGI.Common.Util.NexxoUtil.GetProductCode(mgiContext.ProductType), WUCommonException.CERIFICATE_NOTFOUND, null);
            }

            return certificates[0];
        }

        internal wucardenrollmentrequest CardEnrollmentRequestMapper(Sender sender, PaymentDetails paymentDetails)
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

        //@@@ is this used anywhere?
        internal CardDetails CardEnrollmentResponse(wucardenrollmentreply response)
        {
            CardDetails cardDetails = new CardDetails();
            cardDetails = null;

            cardDetails.AccountNumber = response.sender.preferred_customer.account_nbr;
            cardDetails.ForiegnSystemId = response.foreign_remote_system.identifier;
            cardDetails.ForiegnRefNum = response.foreign_remote_system.reference_no;
            cardDetails.CounterId = response.foreign_remote_system.counter_id;
            return cardDetails;
        }

        internal WUCustomerLookup.wucustomerlookuprequest CardLookupRequestMapper(CardLookUpRequest wucardlookupreq, WUCustomerLookup.foreign_remote_system lookupRemoteSystem)
        {
            WUCustomerLookup.wucustomerlookuprequest request = new WUCustomerLookup.wucustomerlookuprequest();

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

            return request;
        }

        internal List<Sender> CardLookupResponse(WUCustomerLookup.wucustomerlookupreply response)
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
            return senderList;
        }

        //US1784 WU Gold Card Name Matching
        internal WUCardLookupService.wucardlookuprequest WUCardLookupRequestMapper(CardLookUpRequest wucardlookupreq, WUCardLookupService.foreign_remote_system cardLookupRemoteSystem)
        {
            WUCardLookupService.wucardlookuprequest request = new WUCardLookupService.wucardlookuprequest();

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

            return request;
        }

        internal CardLookupDetails WUCardLookupResponse(WUCardLookupService.wucardlookupreply response)
        {
            CardLookupDetails cardLookupDetails = new CardLookupDetails();

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
                throw new WUCommonException(MGI.Common.Util.NexxoUtil.GetProductCode(mgiContext.ProductType), WUCommonException.INVALID_ACCOUNTIDENTIFIER_COUNTERID, null);
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
                throw new WUCommonException(MGI.Common.Util.NexxoUtil.GetProductCode(mgiContext.ProductType), WUCommonException.INVALID_ACCOUNTIDENTIFIER_COUNTERID, null);
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

        internal WUCredential GetCredential(long channelPartnerId, MGIContext mgiContext)
        {
            WUCredential wuCredential = WUCredentialRepo.FindBy(x => x.ChannelPartnerId == channelPartnerId);

            if (wuCredential == null)
            {
                throw new WUCommonException(MGI.Common.Util.NexxoUtil.GetProductCode(mgiContext.ProductType), WUCommonException.WESTERNUNION_CREDENTIALS_NOTFOUND, null);
            }

            return wuCredential;
        }

        internal MGI.Cxn.WU.Common.Data.CardDetails EnrollCardResponseMapper(wucardenrollmentreply response, CardDetails cardDetails)
        {
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
            return request;
        }

        internal CardInfo CardLookUpResponse(wucardlookupreply response)
        {
            CardInfo CardInfo = new CardInfo();
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

            return request;
        }

        public void HandleException(Exception ex, string productType)
        {
            Exception faultException = ex as FaultException;
            if (faultException != null)
            {
                throw new WUCommonProviderException(MGI.Common.Util.NexxoUtil.GetProductCode(productType), MGI.Common.Util.NexxoUtil.GetProviderCode(productType), WUCommonProviderException.PROVIDER_FAULT_ERROR, string.Empty, faultException);
            }
            Exception endpointException = ex as EndpointNotFoundException;
            if (endpointException != null)
            {
                throw new WUCommonProviderException(MGI.Common.Util.NexxoUtil.GetProductCode(productType), MGI.Common.Util.NexxoUtil.GetProviderCode(productType), WUCommonProviderException.PROVIDER_ENDPOINTNOTFOUND_ERROR, string.Empty, endpointException);
            }
            Exception commException = ex as CommunicationException;
            if (commException != null)
            {
                throw new WUCommonProviderException(MGI.Common.Util.NexxoUtil.GetProductCode(productType), MGI.Common.Util.NexxoUtil.GetProviderCode(productType), WUCommonProviderException.PROVIDER_COMMUNICATION_ERROR, string.Empty, commException);
            }
            Exception timeOutException = ex as TimeoutException;
            if (timeOutException != null)
            {
                throw new WUCommonProviderException(MGI.Common.Util.NexxoUtil.GetProductCode(productType), MGI.Common.Util.NexxoUtil.GetProviderCode(productType), WUCommonProviderException.PROVIDER_TIMEOUT_ERROR, string.Empty, timeOutException);
            }
        }
    }
}
