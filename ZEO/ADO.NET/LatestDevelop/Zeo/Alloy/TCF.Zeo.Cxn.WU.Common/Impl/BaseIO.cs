using AutoMapper;
using TCF.Zeo.Common.Data;
using TCF.Zeo.Common.Util;
using TCF.Zeo.Cxn.WU.Common.DASService;
using TCF.Zeo.Cxn.WU.Common.Data;
using TCF.Zeo.Cxn.WU.Common.WUCardLookupService;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Cxn.WU.Common.Impl
{
    public class BaseIO
    {
        #region Dependancies
        public IMapper mapper;
        public const int OCCUPATION_LENGTH = 29;
        internal X509Certificate2 WUCertificate = null;
        public IExceptionHelper ExceptionHelper = new WUCommonProviderException();
        public bool IsHardCodedCounterId { get; set; } = Convert.ToBoolean(ConfigurationManager.AppSettings["IsHardCodedCounterId"].ToString());
        #endregion

        public BaseIO()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<BANNERMESSAGE_Type, AgentBanners>();
                cfg.CreateMap<SwbFlaInfo, WUCardEnrollmentService.swb_fla_info>()
                    .ForMember(d => d.swb_operator_id, s => s.MapFrom(c => c.SwbOperatorId))
                    .ForMember(d => d.read_privacynotice_flag, s => s.MapFrom(x => x.ReadPrivacyNoticeFlag))
                    .ForMember(d => d.read_privacynotice_flagSpecified, s => s.MapFrom(x => x.FlagCertificationFlagSpecified))
                    .ForMember(d => d.fla_certification_flag, s => s.MapFrom(x => x.FlagCertificationFlag))
                    .ForMember(d => d.fla_certification_flagSpecified, s => s.MapFrom(x => x.ReadPrivacyNoticeFlagSpecified));

                cfg.CreateMap<GeneralName, WUCardEnrollmentService.general_name>()
                    .ForMember(d => d.name_type, s => s.MapFrom(x => x.Type))
                    .ForMember(d => d.name_typeSpecified, s => s.MapFrom(x => x.NameTypeSpecified))
                    .ForMember(d => d.first_name, s => s.MapFrom(x => x.FirstName))
                    .ForMember(d => d.last_name, s => s.MapFrom(x => x.LastName));

                cfg.CreateMap<SwbFlaInfo, WUCustomerLookupService.swb_fla_info>()
                    .ForMember(d => d.swb_operator_id, s => s.MapFrom(c => c.SwbOperatorId))
                    .ForMember(d => d.read_privacynotice_flag, s => s.MapFrom(x => x.ReadPrivacyNoticeFlag))
                    .ForMember(d => d.read_privacynotice_flagSpecified, s => s.MapFrom(x => x.FlagCertificationFlagSpecified))
                    .ForMember(d => d.fla_certification_flag, s => s.MapFrom(x => x.FlagCertificationFlag))
                    .ForMember(d => d.fla_certification_flagSpecified, s => s.MapFrom(x => x.ReadPrivacyNoticeFlagSpecified));

                cfg.CreateMap<GeneralName, WUCustomerLookupService.general_name>()
                    .ForMember(d => d.name_type, s => s.MapFrom(x => x.Type))
                    .ForMember(d => d.name_typeSpecified, s => s.MapFrom(x => x.NameTypeSpecified))
                    .ForMember(d => d.first_name, s => s.MapFrom(x => x.FirstName))
                    .ForMember(d => d.last_name, s => s.MapFrom(x => x.LastName));

                cfg.CreateMap<SwbFlaInfo, WUCardLookupService.swb_fla_info>()
                    .ForMember(d => d.swb_operator_id, s => s.MapFrom(c => c.SwbOperatorId))
                    .ForMember(d => d.read_privacynotice_flag, s => s.MapFrom(x => x.ReadPrivacyNoticeFlag))
                    .ForMember(d => d.read_privacynotice_flagSpecified, s => s.MapFrom(x => x.FlagCertificationFlagSpecified))
                    .ForMember(d => d.fla_certification_flag, s => s.MapFrom(x => x.FlagCertificationFlag))
                    .ForMember(d => d.fla_certification_flagSpecified, s => s.MapFrom(x => x.ReadPrivacyNoticeFlagSpecified));

                cfg.CreateMap<GeneralName, WUCardLookupService.general_name>()
                    .ForMember(d => d.name_type, s => s.MapFrom(x => x.Type))
                    .ForMember(d => d.name_typeSpecified, s => s.MapFrom(x => x.NameTypeSpecified))
                    .ForMember(d => d.first_name, s => s.MapFrom(x => x.FirstName))
                    .ForMember(d => d.last_name, s => s.MapFrom(x => x.LastName));

            });

            mapper = config.CreateMapper();
        }

        #region BaseIO methods        

        public void HandleException(Exception ex, string productType)
        {
            Exception faultException = ex as FaultException;
            if (faultException != null)
            {
                throw new WUCommonProviderException(AlloyUtil.GetProductCode(productType), AlloyUtil.GetProviderCode(productType), WUCommonProviderException.PROVIDER_FAULT_ERROR, string.Empty, faultException);
            }
            Exception endpointException = ex as EndpointNotFoundException;
            if (endpointException != null)
            {
                throw new WUCommonProviderException(AlloyUtil.GetProductCode(productType), AlloyUtil.GetProviderCode(productType), WUCommonProviderException.PROVIDER_ENDPOINTNOTFOUND_ERROR, string.Empty, endpointException);
            }
            Exception commException = ex as CommunicationException;
            if (commException != null)
            {
                throw new WUCommonProviderException(AlloyUtil.GetProductCode(productType), AlloyUtil.GetProviderCode(productType), WUCommonProviderException.PROVIDER_COMMUNICATION_ERROR, string.Empty, commException);
            }
            Exception timeOutException = ex as TimeoutException;
            if (timeOutException != null)
            {
                throw new WUCommonProviderException(AlloyUtil.GetProductCode(productType), AlloyUtil.GetProviderCode(productType), WUCommonProviderException.PROVIDER_TIMEOUT_ERROR, string.Empty, timeOutException);
            }
        }


        #endregion

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
    }
}
