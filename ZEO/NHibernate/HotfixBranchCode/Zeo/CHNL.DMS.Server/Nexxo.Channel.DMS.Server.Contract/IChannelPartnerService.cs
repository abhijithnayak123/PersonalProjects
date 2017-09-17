using System;
using System.ServiceModel;
using System.Collections.Generic;
using MGI.Channel.DMS.Server.Data;
using MGI.Common.Sys;

namespace MGI.Channel.DMS.Server.Contract
{
	[ServiceContract]
	public interface IChannelPartnerService
	{
		/// <summary>
		/// This method will get the list of Locations configured per channel partner
		/// </summary>
		/// <param name="agentSessionId">This is unique identifier for agent session</param>
		/// <param name="channelPartner">Name of the channelpartner</param>
		/// <param name="mgiContext">This is the common dictionary parameter used to pass supplimental information</param>
		/// <returns>Returns the list of locations configured per channel partner</returns>
		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
		List<string> Locations(long agentSessionId, string channelPartner, MGIContext mgiContext);

		/// <summary>
		/// This methods gets the detais of attributes which are configured per channel partner
		/// </summary>
		/// <param name="channelPartner">Name of the channelpartner</param>
		/// <param name="mgiContext">This is the common dictionary parameter used to pass supplimental information</param>
		/// <returns>Returns attribute values which are configured per channel partner</returns>
		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
        ChannelPartner ChannelPartnerConfig(string channelPartner, MGIContext mgiContext);

		/// <summary>
		/// This methods gets the list of tips and offers per view based on channel partner
		/// </summary>
		/// <param name="agentSessionId">This is unique identifier for agent session</param>
		/// <param name="channelPartner">Name of the channelpartner</param>
		/// <param name="language">Language English or Spanish</param>
		/// <param name="viewName">Name of the view which contains tips and offers</param>
		/// <param name="mgiContext">This is the common dictionary parameter used to pass supplimental information</param>
		/// <returns>Returns list of Tips and Offers available per view based on channel partner</returns>
        [OperationContract]
        [FaultContract(typeof(NexxoSOAPFault))]
        List<TipsAndOffers> GetTipsAndOffers(long agentSessionId, string channelPartner, string language, string viewName, MGIContext mgiContext);

		/// <summary>
		/// This method gets list of groups available per channel partner. This will be used to provide offers to customer based on the group selected.
		/// </summary>
		/// <param name="channelPartner">Name of the channel partner</param>
		/// <param name="mgiContext">This is the common dictionary parameter used to pass supplimental information</param>
		/// <returns>Returns the list of groups based on channel partner</returns>
		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
        List<string> GetPartnerGroups(string channelPartner, MGIContext mgiContext);

		/// <summary>
		/// This method gets the SSO certificate details per channel partner
		/// </summary>
		/// <param name="channelPartnerId">This is unique identifier for Channel Partner</param>
		/// <param name="issuer">Name of the issuer to identify the SSO certificate </param>
		/// <param name="mgiContext">This is the common dictionary parameter used to pass supplimental information</param>
		/// <returns>Returns the SSO certificate details per channel partner</returns>
		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
        ChannelPartnerCertificate GetChannelPartnerCertificateInfo(long channelPartnerId, string issuer, MGIContext mgiContext);
	}
}
