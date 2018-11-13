using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MGI.Biz.Partner.Data;
using MGI.Common.Util;
namespace MGI.Biz.Partner.Contract
{
    public interface IChannelPartnerService
    {
        /// <summary>
        /// This method will get the list of Locations configured per channel partner
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for agent session</param>
        /// <param name="channelPartner">Name of the channelpartner</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns>list of locations configured per channel partner</returns>
        List<string> Locations(long agentSessionId, string channelPartner, MGIContext mgiContext);
        
        /// <summary>
        /// This methods gets the detais of attributes which are configured per channel partner
        /// </summary>
        /// <param name="channelPartner">Name of the channelpartner</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns>Channel Partner Details.</returns>
		ChannelPartner ChannelPartnerConfig(string channelPartner, MGIContext mgiContext);
        
        /// <summary>
        /// This methods gets the detais of attributes which are configured per channel partner
        /// </summary>
        /// <param name="channelPartnerId">Channel Partner Id.</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns>Channel Partner Details.</returns>
		ChannelPartner ChannelPartnerConfig(int channelPartnerId, MGIContext mgiContext);
        
        /// <summary>
        /// This methods gets the detais of attributes which are configured per channel partner
        /// </summary>
        /// <param name="rowid">This the unique indentifier for channel partner</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns>Channel Partner Details.</returns>
		ChannelPartner ChannelPartnerConfig(Guid rowid, MGIContext mgiContext);
     
        /// <summary>
        /// This method is to get collection of tips and offers.
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for AgentSession</param>
        /// <param name="channelPartner">Channel Partner Name</param>
        /// <param name="language">This is language</param>
        /// <param name="viewName">This is view name</param>
        /// <param name="context">This is the common class parameter used to pass supplimental information</param>
        /// <returns>List Of Tips And Offers</returns>
		List<TipsAndOffers> GetTipsAndOffers(long agentSessionId, string channelPartner, string language, string viewName, MGIContext mgiContext);

        /// <summary>
        /// This method is to get the collection of channel partner group details by channel partner name.
        /// </summary>
        /// <param name="channelPartner">Channel Partner Name</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns>List Of Groups Details</returns>
		List<string> GetGroups(string channelPartner, MGIContext mgiContext);

        /// <summary>
        /// This method is to get the channel partner certificate
        /// </summary>
        /// <param name="channelPartnerId">This is unique identifier for channel partner</param>
        /// <param name="issuer">Name of the issuer to identify the SSO certificate</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns>SSO certificate details per channel partner</returns>
		ChannelPartnerCertificate GetChannelPartnerCertificateInfo(long channelPartnerId, string issuer, MGIContext mgiContext);
    }
}
