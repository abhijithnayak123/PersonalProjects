using TCF.Zeo.Common.Data;
using TCF.Zeo.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Core.Contract
{
    public interface IChannelPartnerService
    {
        /// <summary>
        /// This method is to get the collection of channel partner group details by channel partner name.
        /// </summary>
        /// <param name="channelPartner">Channel Partner Name</param>
        /// <returns>List Of Groups Details</returns>
        List<KeyValuePair> GetGroups(long channelPartnerId, ZeoContext context);

        /// <summary>
        /// This method is to get the channel partner data by ChannelPartnerId
        /// </summary>
        /// <param name="channelPartnerId"></param>
        /// <returns></returns>
        ChannelPartner ChannelPartnerConfig(long channelPartnerId, ZeoContext context);

        /// <summary>
        /// This method is to get the channel partner data by ChannelPartner name
        /// </summary>
        /// <param name="channelPartner"></param>
        /// <returns></returns>
        ChannelPartner ChannelPartnerConfig(string channelPartner, ZeoContext context);

     
       
        ChannelPartnerCertificate GetChannelPartnerCertificateInfo(long channelPartnerId, string issuer, ZeoContext context);


        /// <summary>
        /// Get Tips And Offers by channelPartner and viewName
        /// </summary>
        /// <param name="channelPartner"></param>
        /// <param name="lang"></param>
        /// <param name="viewName"></param>
        /// <returns></returns>
        List<TipsAndOffers> GetTipsAndOffers(long channelPartnerId, string lang, string viewName, ZeoContext context);

        /// <summary>
        /// providers by channel partner
        /// </summary>
        /// <param name="channelPartnerName"></param>
        /// <returns></returns>
        List<ChannelPartnerProductProvider> GetProvidersbyChannelPartnerName(string channelPartnerName, ZeoContext context);

  /// <summary>
        /// Gets customer care contact details
        /// </summary>
        /// <param name="channelPartnerName"></param>
        /// <returns></returns>
        List<SupportInformation> GetSupportInformation(ZeoContext context);
 List<ProductProviderDetails> GetProductProviderDetails(ZeoContext context);
 }
}
