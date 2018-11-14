using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCF.Channel.Zeo.Data;
using commonData = TCF.Zeo.Common.Data;
namespace TCF.Zeo.Biz.Contract
{
    public interface IChannelPartnerService
    {
        /// <summary>
        /// This method is to get the collection of channel partner group details by channel partner name.
        /// </summary>
        /// <param name="channelPartner">Channel Partner Name</param>
        /// <returns>List Of Groups Details</returns>
        List<KeyValuePair> GetGroups(long channelPartnerId, commonData.ZeoContext context);

        /// <summary>
        /// This method is ChannelPartnerConfig details by channel partner name.
        /// </summary>
        /// <param name="channelPartner"></param>
        /// <returns></returns>
        ChannelPartner ChannelPartnerConfig(string channelPartner, commonData.ZeoContext context);

        /// <summary>
        /// This method is to get the ChannelPartnerConfig details by channel partner id.
        /// </summary>
        /// <param name="channelPartnerId"></param>
        /// <returns></returns>
        ChannelPartner ChannelPartnerConfig(long channelPartnerId, commonData.ZeoContext context);

    
        /// This method is to get GetTipsAndOffers details by channelPartner, language, viewName
        /// </summary>
        /// <param name="agentSessionId"></param>
        /// <param name="channelPartner"></param>
        /// <param name="language"></param>
        /// <param name="viewName"></param>
        /// <returns></returns>
        string GetTipsAndOffers(long channelPartnerId, string language, string viewName, string optionalFilter, commonData.ZeoContext context);

        /// <summary>
        /// This method is to get the ChannelPartnerCertificate  by channelPartnerId, issuer.
        /// </summary>
        /// <param name="channelPartnerId"></param>
        /// <param name="issuer"></param>
        /// <returns></returns>
        ChannelPartnerCertificate GetChannelPartnerCertificateInfo(long channelPartnerId, string issuer, commonData.ZeoContext context);

        /// <summary>
        /// this method gets the channel partner provider details
        /// </summary>
        /// <param name="channelPartnerName"></param>
        /// <returns></returns>
        List<ChannelPartnerProductProvider> GetProvidersbyChannelPartnerName(string channelPartnerName, commonData.ZeoContext context);
         /// <summary>
        /// Gets the customer care conntact details
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
         List<SupportInformation> GetSupportInformation(commonData.ZeoContext context);
    List<ProductProviderDetails> GetProductProviders(commonData.ZeoContext context);

    }
}
