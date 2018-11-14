using AutoMapper;
using TCF.Zeo.Biz.Contract;
using TCF.Channel.Zeo.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeoData = TCF.Channel.Zeo.Data;
using CoreData = TCF.Zeo.Core;
using TCF.Zeo.Common.Util;
using TCF.Zeo.Common.Data;
using TCF.Zeo.Biz.Common.Data.Exceptions;
using commonData = TCF.Zeo.Common.Data;


namespace TCF.Zeo.Biz.Impl
{
    public class ChannelPartnerServiceImpl : Biz.Contract.IChannelPartnerService
    {
        IMapper mapper;
        CoreData.Contract.IChannelPartnerService cpService;
        public ChannelPartnerServiceImpl()
        {
            cpService = new CoreData.Impl.ChannelPartnerServiceImpl();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CoreData.Data.ChannelPartner, ZeoData.ChannelPartner>().ReverseMap();
                cfg.CreateMap<CoreData.Data.ChannelPartnerProductProvider, ZeoData.ChannelPartnerProductProvider>().ReverseMap();
                cfg.CreateMap<CoreData.Data.ChannelPartnerCertificate, ZeoData.ChannelPartnerCertificate>().ReverseMap();

                cfg.CreateMap<CoreData.Data.TipsAndOffers, ZeoData.TipsAndOffers>();
cfg.CreateMap<CoreData.Data.SupportInformation, ZeoData.SupportInformation>();
cfg.CreateMap<CoreData.Data.ProductProviderDetails, ProductProviderDetails>().ReverseMap();
 cfg.CreateMap<CoreData.Data.KeyValuePair, ZeoData. KeyValuePair>();    });
            mapper = config.CreateMapper();
        }

        /// <summary>
        /// This method is to get the collection of channel partner group details by channel partner name
        /// </summary>
        /// <param name="channelPartner"></param>
        /// <returns></returns>
        public List<KeyValuePair> GetGroups(long channelPartnerId, commonData.ZeoContext context)
        {
            try
            {
                return mapper.Map<List<ZeoData.KeyValuePair>>(cpService.GetGroups(channelPartnerId,context));
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new ChannelPartnerException(ChannelPartnerException.CHANNEL_PARTNER_GROUP_GET_FAILED, ex);
            }
        }
        public ZeoData.ChannelPartner ChannelPartnerConfig(string channelPartner, commonData.ZeoContext context)
        {

            CoreData.Data.ChannelPartner cp = cpService.ChannelPartnerConfig(channelPartner,context);
            try
            {
                ZeoData.ChannelPartner partner = mapper.Map<ZeoData.ChannelPartner>(cp);
                partner.Providers = mapper.Map<List<ZeoData.ChannelPartnerProductProvider>>(cp.Providers);
                return partner;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new ChannelPartnerException(ChannelPartnerException.CHANNEL_PARTNER_GET_FAILED, ex);
            }
        }
        public ChannelPartner ChannelPartnerConfig(long channelPartnerId, commonData.ZeoContext context)
        {
            try
            {
                CoreData.Data.ChannelPartner channelPartner = cpService.ChannelPartnerConfig(channelPartnerId,context);
                return mapper.Map<ZeoData.ChannelPartner>(channelPartner);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new ChannelPartnerException(ChannelPartnerException.CHANNEL_PARTNER_GET_FAILED, ex);
            }
        }
      
        public string GetTipsAndOffers(long channelPartnerId, string language, string viewName, string optionalFilter, commonData.ZeoContext context)
        {
            string tipsandOffers = string.Empty;
            try
            {
                List<ZeoData.TipsAndOffers> tipsAndOffersList = mapper.Map<List<ZeoData.TipsAndOffers>>(cpService.GetTipsAndOffers(channelPartnerId, language, viewName,context));
               

                if (tipsAndOffersList.Count > 0 && optionalFilter != null)

                    tipsandOffers = tipsAndOffersList.Where(x => x.OptionalFilter == optionalFilter).FirstOrDefault().TipsAndOffersValue;

                else if (tipsAndOffersList.Count > 0)

                    tipsandOffers = tipsAndOffersList.Where(x => x.OptionalFilter == null || x.OptionalFilter == "").FirstOrDefault().TipsAndOffersValue;

                return tipsandOffers;
            }
            catch (Exception ex)
            {

                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new ChannelPartnerException(ChannelPartnerException.TIPS_AND_OFFERS_GET_FAILED, ex);
            }
        }
        public ChannelPartnerCertificate GetChannelPartnerCertificateInfo(long channelPartnerId, string issuer, commonData.ZeoContext context)
        {
            try
            {
                CoreData.Data.ChannelPartnerCertificate certificateInfo = cpService.GetChannelPartnerCertificateInfo(channelPartnerId, issuer,context);
                return mapper.Map<ChannelPartnerCertificate>(certificateInfo);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new ChannelPartnerException(ChannelPartnerException.CHANNEL_PARTNER_CERTIFICATE_INFO_GET_FAILED, ex);
            }
        }
        public List<ChannelPartnerProductProvider> GetProvidersbyChannelPartnerName(string channelPartnerName, commonData.ZeoContext context)
        {
            try
            {
               List<CoreData.Data.ChannelPartnerProductProvider> channelPartnerProductProviders = cpService.GetProvidersbyChannelPartnerName(channelPartnerName,context);
               return mapper.Map<List<ChannelPartnerProductProvider>>(channelPartnerProductProviders);
            }

            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new ChannelPartnerException(ChannelPartnerException.CHANNEL_PARTNER_GET_PROVIDERS_FAILED, ex);
            }
        }

        public List<SupportInformation> GetSupportInformation(commonData.ZeoContext context)
        {
            try
            {
                List<CoreData.Data.SupportInformation> contactDetails = cpService.GetSupportInformation(context);
                return mapper.Map<List<SupportInformation>>(contactDetails);
            }

            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new ChannelPartnerException(ChannelPartnerException.GET_SUPPORT_INFORMATION_FAILED, ex);
            }
        }
 public List<ProductProviderDetails> GetProductProviders(commonData.ZeoContext context)
        {
            try
            {
                List<CoreData.Data.ProductProviderDetails> productProviders = cpService.GetProductProviderDetails(context);
                return mapper.Map<List<ProductProviderDetails>>(productProviders);
            }

            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new ChannelPartnerException(ChannelPartnerException.CHANNEL_PARTNER_GET_PROVIDERS_FAILED, ex);
            }
        }}
}
