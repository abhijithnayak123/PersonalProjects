using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using MGI.Channel.DMS.Server.Contract;
using MGI.Channel.DMS.Server.Data;
using MGI.Channel.DMS.Server.Impl;
using System.Net.Mail;
using System.ServiceModel;
using MGI.Common.Sys;

namespace MGI.Channel.DMS.Server.Svc
{
	public partial class DesktopWSImpl : IUserService
	{
		public Response GetUser(long agentSessionId, int UserId, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.GetUser(agentSessionId, UserId, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}

		public Response GetUsers(long agentSessionId, long locationId, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.GetUsers(agentSessionId, locationId, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}
	}
}