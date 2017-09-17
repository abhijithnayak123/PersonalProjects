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
	public partial class DesktopWSImpl : ITerminalService
	{
		public Response LookupTerminal(long Id, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.LookupTerminal(Id, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}

		public Response CreateTerminal(long agentSessionId, Terminal terminal, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.CreateTerminal(agentSessionId, terminal, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}

		public Response UpdateTerminal(long agentSessionId, Terminal terminal, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.UpdateTerminal(agentSessionId, terminal, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}

		public Response LookupTerminal(long agentSessionId, string terminalName, int channelPartnerId, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.LookupTerminal(agentSessionId, terminalName, channelPartnerId, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}
	}
}