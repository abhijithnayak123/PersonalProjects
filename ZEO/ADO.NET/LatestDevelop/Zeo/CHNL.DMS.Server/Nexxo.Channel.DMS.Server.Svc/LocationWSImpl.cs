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
	public partial class DesktopWSImpl : ILocationService
	{
		public Response GetByName(long agentSessionId, string locationName, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.GetByName(agentSessionId, locationName, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}

		public Response GetAll()
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.GetAll();
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}

		public Response Create(long agentSessionId, Location manageLocation, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.Create(agentSessionId, manageLocation, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}

		public Response Update(long agentSessionId, Location manageLocation, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.Update(agentSessionId, manageLocation, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}

		public Response Lookup(string agentSessionId, long locationId, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.Lookup(agentSessionId, locationId, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}

		//Added for filter the locations by channel partner Id
		public Response GetAll(long agentSessionId, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.GetAll(agentSessionId, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}

	}
}