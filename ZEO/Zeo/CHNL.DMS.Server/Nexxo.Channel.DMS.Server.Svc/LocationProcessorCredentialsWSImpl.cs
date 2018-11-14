using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MGI.Channel.DMS.Server.Contract;
using MGI.Channel.DMS.Server.Impl;
using MGI.Channel.DMS.Server.Data;
using System.ServiceModel;
using MGI.Common.Sys;

namespace MGI.Channel.DMS.Server.Svc
{
	public partial class DesktopWSImpl : ILocationProcessorCredentialsService
	{
		public Response GetLocationProcessorCredentials(long agentSessionId, long locationId, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.GetLocationProcessorCredentials(agentSessionId, locationId, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}

		public Response Save(long agentSessionId, long locationId, Data.ProcessorCredential processorCredentials, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.Save(agentSessionId, locationId, processorCredentials, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}
	}
}