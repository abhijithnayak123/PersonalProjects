using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using MGI.Channel.DMS.Server.Contract;
using MGI.Channel.DMS.Server.Data;
using MGI.Channel.DMS.Server.Impl;
using System.ServiceModel;
using MGI.Common.Sys;

namespace MGI.Channel.DMS.Server.Svc
{
	public partial class DesktopWSImpl : IChannelPartnerService
	{
		public Response Locations(long agentSessionId, string channelPartner, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.Locations(agentSessionId, channelPartner, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}

		public Response ChannelPartnerConfig(string channelPartner, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.ChannelPartnerConfig(channelPartner, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;

		}

		public Response GetTipsAndOffers(long agentSessionId, string channelPartner, string language, string viewName, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.GetTipsAndOffers(agentSessionId, channelPartner, language, viewName, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}

		public Response GetPartnerGroups(string channelPartner, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.GetPartnerGroups(channelPartner, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}

		public Response GetChannelPartnerCertificateInfo(long channelPartnerId, string issuer, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.GetChannelPartnerCertificateInfo(channelPartnerId, issuer, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}
	}
}