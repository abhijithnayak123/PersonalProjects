using TCF.Zeo.Biz.Impl;
using TCF.Zeo.Common.Util;
using TCF.Channel.Zeo.Data;
using TCF.Channel.Zeo.Service.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using commonData = TCF.Zeo.Common.Data;

namespace TCF.Channel.Zeo.Service.Impl
{
    public partial class ZeoServiceImpl : IChannelPartnerService
    {

        /// <summary>
        /// This method is to get the collection of channel partner group details by channel partner name.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Response GetPartnerGroups(long channelPartnerId, ZeoContext context)
        {
            Response result = new Response();
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);
            ChannelPartnerServiceImpl bizChannelPartnerService = new ChannelPartnerServiceImpl();
            result.Result = bizChannelPartnerService.GetGroups(channelPartnerId, commonContext);
            return result;
        }

        public Response ChannelPartnerConfig(ZeoContext context)
        {

            Response result = new Response();

            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);
            ChannelPartnerServiceImpl bizChannelPartnerService = new ChannelPartnerServiceImpl();
            result.Result = bizChannelPartnerService.ChannelPartnerConfig(context.ChannelPartnerId, commonContext);
            return result;

        }

        public Response ChannelPartnerConfigByName(string channelPartnerName, ZeoContext context)
        {
            Response result = new Response();

            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);
            ChannelPartnerServiceImpl bizChannelPartnerService = new ChannelPartnerServiceImpl();
            result.Result = bizChannelPartnerService.ChannelPartnerConfig(channelPartnerName, commonContext);
            return result;
        }


        public Response GetTipsAndOffers(long ChannelPartnerId, string language, string viewName, string optionalFilter, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

            Response result = new Response();

            ChannelPartnerServiceImpl bizChannelPartnerService = new ChannelPartnerServiceImpl();
            result.Result = bizChannelPartnerService.GetTipsAndOffers(ChannelPartnerId, language, viewName, optionalFilter, commonContext);

            return result;
        }

        public Response GetChannelPartnerCertificateInfo(long channelpartnerId, string issuer, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

            Response result = new Response();

            ChannelPartnerServiceImpl bizChannelPartnerService = new ChannelPartnerServiceImpl();
            result.Result = bizChannelPartnerService.GetChannelPartnerCertificateInfo(channelpartnerId, issuer, commonContext);

            return result;
        }

        public Response GetProvidersbyChannelPartnerName(string channelPartnerName, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

            Response result = new Response();

            ChannelPartnerServiceImpl bizChannelPartnerService = new ChannelPartnerServiceImpl();
            result.Result = bizChannelPartnerService.GetProvidersbyChannelPartnerName(channelPartnerName, commonContext);

            return result;
        }

    }
}
