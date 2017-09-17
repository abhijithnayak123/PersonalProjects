using AutoMapper;
using TCF.Zeo.Common.Data;
using TCF.Zeo.Common.Util;
using TCF.Zeo.Cxn.MoneyTransfer.Data;
using TCF.Zeo.Cxn.MoneyTransfer.Data.Exceptions;
using TCF.Zeo.Cxn.MoneyTransfer.WU.Data;
using TCF.Zeo.Cxn.WU.Common.Contract;
using TCF.Zeo.Cxn.WU.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using DAS = TCF.Zeo.Cxn.MoneyTransfer.WU.DASService;
using CxnWu = TCF.Zeo.Cxn.MoneyTransfer.WU;

namespace TCF.Zeo.Cxn.MoneyTransfer.WU.Impl
{
    public class BaseIO
    {
        public const string DAS_DeliveryServices = "GetDeliveryServices";
        public string _serviceUrl = string.Empty;
        public const string DAS_ENDPOINT_NAME = "DASInquiry";
        public WUBaseRequestResponse _response = null;
        public X509Certificate2 _certificate = null;
        public IWUCommonIO WUCommonIO { get; set; }
        public IMapper Mapper { get; set; }


        public BaseIO()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Channel, DAS.channel>();
                cfg.CreateMap<ForeignRemoteSystem, DAS.foreign_remote_system>()
                    .ForMember(d => d.counter_id, s => s.MapFrom(c => c.CoutnerId));
                cfg.CreateMap<Channel, CxnWu.FeeInquiry.channel>();
                cfg.CreateMap<ForeignRemoteSystem, CxnWu.FeeInquiry.foreign_remote_system>()
                    .ForMember(d => d.counter_id, s => s.MapFrom(c => c.CoutnerId));
                cfg.CreateMap<Channel, CxnWu.SendMoneyValidation.channel>();
                cfg.CreateMap<ForeignRemoteSystem, CxnWu.SendMoneyValidation.foreign_remote_system>()
                    .ForMember(d => d.counter_id, s => s.MapFrom(c => c.CoutnerId));
                cfg.CreateMap<Channel, CxnWu.ReceiveMoneySearch.channel>();
                cfg.CreateMap<ForeignRemoteSystem, CxnWu.ReceiveMoneySearch.foreign_remote_system>()
                    .ForMember(d => d.counter_id, s => s.MapFrom(c => c.CoutnerId));
                cfg.CreateMap<Channel, CxnWu.ReceiveMoneyPay.channel>();
                cfg.CreateMap<ForeignRemoteSystem, CxnWu.ReceiveMoneyPay.foreign_remote_system>()
                    .ForMember(d => d.counter_id, s => s.MapFrom(c => c.CoutnerId));
                cfg.CreateMap<Channel, CxnWu.ModifySendMoneySearch.channel>();
                cfg.CreateMap<ForeignRemoteSystem, CxnWu.ModifySendMoneySearch.foreign_remote_system>()
                    .ForMember(d => d.counter_id, s => s.MapFrom(c => c.CoutnerId));
                cfg.CreateMap<Channel, CxnWu.SendMoneyStore.channel>();
                cfg.CreateMap<ForeignRemoteSystem, CxnWu.SendMoneyStore.foreign_remote_system>()
                    .ForMember(d => d.counter_id, s => s.MapFrom(c => c.CoutnerId));
                cfg.CreateMap<Channel, CxnWu.ModifySendMoney.channel>();
                cfg.CreateMap<ForeignRemoteSystem, CxnWu.ModifySendMoney.foreign_remote_system>()
                    .ForMember(d => d.counter_id, s => s.MapFrom(c => c.CoutnerId));
                cfg.CreateMap<Channel, CxnWu.SendMoneyRefund.channel>();
                cfg.CreateMap<ForeignRemoteSystem, CxnWu.SendMoneyRefund.foreign_remote_system>()
                    .ForMember(d => d.counter_id, s => s.MapFrom(c => c.CoutnerId));
                cfg.CreateMap<Channel, CxnWu.Search.channel>();
                cfg.CreateMap<ForeignRemoteSystem, CxnWu.Search.foreign_remote_system>()
                    .ForMember(d => d.counter_id, s => s.MapFrom(c => c.CoutnerId));
                cfg.CreateMap<Channel, CxnWu.SendMoneyPayStatus.channel>();
                cfg.CreateMap<ForeignRemoteSystem, CxnWu.SendMoneyPayStatus.foreign_remote_system>()
                    .ForMember(d => d.counter_id, s => s.MapFrom(c => c.CoutnerId));
            });
            Mapper = config.CreateMapper();
        }

        internal DAS.h2hdasreply ExecuteDASInquiry(string methodName, DAS.filters_type filters, ZeoContext context)
        {
            DAS.channel channel = null;
            DAS.foreign_remote_system foreignRemoteSystem = null;

            PopulateWUObjects(context.ChannelPartnerId, context);

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
                response = dasInquiry.DAS_Service(request);
            }
            catch (FaultException<DAS.errorreply> ex)
            {
                string code = ExceptionHelper.GetWUErrorCode(ex.Detail.error);
                throw new MoneyTransferProviderException(code, ex.Detail.error, ex);
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_EXECUTEDASINQUIRY_FAILED, ex);
            }
            return response;
        }

        public void PopulateWUObjects(long channelPartnerId, ZeoContext context)
        {
            WUCommonIO = new TCF.Zeo.Cxn.WU.Common.Impl.IO();
            context.ProductType = "sendmoney";
            _response = WUCommonIO.CreateRequest(channelPartnerId, context);
        }

        internal void BuildDASObjects(WUBaseRequestResponse wuObjects, ref DAS.channel channel, ref DAS.foreign_remote_system foreignRemoteSystem)
        {
            channel = Mapper.Map<DAS.channel>(wuObjects.Channel);
            foreignRemoteSystem = Mapper.Map<DAS.foreign_remote_system>(wuObjects.ForeignRemoteSystem);
            BuildWUCommonObjects(wuObjects);
        }

        internal void BuildValidationObjects(WUBaseRequestResponse wuObjects, ref CxnWu.SendMoneyValidation.channel channel,
            ref CxnWu.SendMoneyValidation.foreign_remote_system foreignRemoteSystem)
        {
            channel = Mapper.Map<CxnWu.SendMoneyValidation.channel>(wuObjects.Channel);
            foreignRemoteSystem = Mapper.Map<CxnWu.SendMoneyValidation.foreign_remote_system>(wuObjects.ForeignRemoteSystem);
            BuildWUCommonObjects(wuObjects);
        }

        internal void BuildSendMoneyStoreObjects(WUBaseRequestResponse wuObjects, ref CxnWu.SendMoneyStore.channel channel,
            ref CxnWu.SendMoneyStore.foreign_remote_system foreignRemoteSystem)
        {
            channel = Mapper.Map<CxnWu.SendMoneyStore.channel>(wuObjects.Channel);
            foreignRemoteSystem = Mapper.Map<CxnWu.SendMoneyStore.foreign_remote_system>(wuObjects.ForeignRemoteSystem);
            BuildWUCommonObjects(wuObjects);
        }

        internal void BuildRefundSearchObjects(WUBaseRequestResponse wuObjects, ref CxnWu.Search.channel channel,
            ref CxnWu.Search.foreign_remote_system foreignRemoteSystem)
        {
            channel = Mapper.Map<CxnWu.Search.channel>(wuObjects.Channel);
            foreignRemoteSystem = Mapper.Map<CxnWu.Search.foreign_remote_system>(wuObjects.ForeignRemoteSystem);
            BuildWUCommonObjects(wuObjects);
        }

        internal void BuildRefundSendMoneyObjects(WUBaseRequestResponse wuObjects, ref CxnWu.SendMoneyRefund.channel channel,
            ref CxnWu.SendMoneyRefund.foreign_remote_system foreignRemoteSystem)
        {
            channel = Mapper.Map<CxnWu.SendMoneyRefund.channel>(wuObjects.Channel);
            foreignRemoteSystem = Mapper.Map<CxnWu.SendMoneyRefund.foreign_remote_system>(wuObjects.ForeignRemoteSystem);
            BuildWUCommonObjects(wuObjects);
        }

        internal void BuildPayStatusObjects(WUBaseRequestResponse wuObjects, ref CxnWu.SendMoneyPayStatus.channel channel,
            ref CxnWu.SendMoneyPayStatus.foreign_remote_system foreignRemoteSystem)
        {
            channel = Mapper.Map<CxnWu.SendMoneyPayStatus.channel>(wuObjects.Channel);
            foreignRemoteSystem = Mapper.Map<CxnWu.SendMoneyPayStatus.foreign_remote_system>(wuObjects.ForeignRemoteSystem);
            BuildWUCommonObjects(wuObjects);
        }

        internal void BuildFeeInquiryObjects(WUBaseRequestResponse wuObjects, ref CxnWu.FeeInquiry.channel channel,
            ref CxnWu.FeeInquiry.foreign_remote_system foreignRemoteSystem)
        {
            channel = Mapper.Map<CxnWu.FeeInquiry.channel>(wuObjects.Channel);
            foreignRemoteSystem = Mapper.Map<CxnWu.FeeInquiry.foreign_remote_system>(wuObjects.ForeignRemoteSystem);
            BuildWUCommonObjects(wuObjects);
        }

        internal void BuildReceiveMoneySearchObjects(WUBaseRequestResponse wuObjects, ref CxnWu.ReceiveMoneySearch.channel channel,
            ref CxnWu.ReceiveMoneySearch.foreign_remote_system foreignRemoteSystem)
        {
            channel = Mapper.Map<CxnWu.ReceiveMoneySearch.channel>(wuObjects.Channel);
            foreignRemoteSystem = Mapper.Map<CxnWu.ReceiveMoneySearch.foreign_remote_system>(wuObjects.ForeignRemoteSystem);
            BuildWUCommonObjects(wuObjects);
        }

        internal void BuildReceiveMoneyPayObjects(WUBaseRequestResponse wuObjects, ref CxnWu.ReceiveMoneyPay.channel channel,
            ref CxnWu.ReceiveMoneyPay.foreign_remote_system foreignRemoteSystem)
        {
            channel = Mapper.Map<CxnWu.ReceiveMoneyPay.channel>(wuObjects.Channel);
            foreignRemoteSystem = Mapper.Map<CxnWu.ReceiveMoneyPay.foreign_remote_system>(wuObjects.ForeignRemoteSystem);
            BuildWUCommonObjects(wuObjects);
        }

        internal void BuildModifySendMoneyObjects(WUBaseRequestResponse wuObjects, ref CxnWu.ModifySendMoney.channel channel,
            ref CxnWu.ModifySendMoney.foreign_remote_system foreignRemoteSystem)
        {
            channel = Mapper.Map<CxnWu.ModifySendMoney.channel>(wuObjects.Channel);
            foreignRemoteSystem = Mapper.Map<CxnWu.ModifySendMoney.foreign_remote_system>(wuObjects.ForeignRemoteSystem);
            BuildWUCommonObjects(wuObjects);
        }

        internal void BuildModifySearchObjects(WUBaseRequestResponse wuObjects, ref CxnWu.ModifySendMoneySearch.channel channel,
            ref CxnWu.ModifySendMoneySearch.foreign_remote_system foreignRemoteSystem)
        {
            channel = Mapper.Map<CxnWu.ModifySendMoneySearch.channel>(wuObjects.Channel);
            foreignRemoteSystem = Mapper.Map<CxnWu.ModifySendMoneySearch.foreign_remote_system>(wuObjects.ForeignRemoteSystem);
            BuildWUCommonObjects(wuObjects);
        }

        internal void BuildWUCommonObjects(WUBaseRequestResponse wuObjects)
        {
            _certificate = wuObjects.ClientCertificate;
            _serviceUrl = wuObjects.ServiceUrl;
        }

        internal void HandleException(Exception ex)
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
