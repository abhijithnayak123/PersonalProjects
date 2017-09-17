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
	public partial class DesktopWSImpl : INpsTerminalService
	{
		public Response CreateNpsTerminal(long agentSessionId, NpsTerminal npsTerminal, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.CreateNpsTerminal(agentSessionId, npsTerminal, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;

		}

		public Response UpdateNpsTerminal(long agentSessionId, NpsTerminal npsTerminal, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.UpdateNpsTerminal(agentSessionId, npsTerminal, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}

		public Response LookupNpsTerminal(long agentSessionId, long Id, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.LookupNpsTerminal(agentSessionId, Id, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}

		public Response LookupNpsTerminal(long agentSessionId, string ipAddress, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.LookupNpsTerminal(agentSessionId, ipAddress, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}

		public Response LookupNpsTerminalByLocationID(long agentSessionId, long locationId, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.LookupNpsTerminalByLocationID(agentSessionId, locationId, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}

		public Response LookupNpsTerminal(long agentSessionId, string name, ChannelPartner channelPartner, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.LookupNpsTerminal(agentSessionId, name, channelPartner, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}
	}
}