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
	public partial class DesktopWSImpl : ILocationCounterIdService
	{
		public Response UpdateCounterId(long customerSessionId, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response= DesktopEngine.UpdateCounterId(customerSessionId, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}
	}
}