using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using MGI.Channel.DMS.Server.Contract;
using MGI.Channel.DMS.Server.Data;
using MGI.Channel.DMS.Server.Impl;

namespace MGI.Channel.DMS.Server.Svc
{
    public partial class DesktopWSImpl : ILocationCounterIdService
    {
		public bool UpdateCounterId(long customerSessionId, MGIContext mgiContext)
		{
			return DesktopEngine.UpdateCounterId(customerSessionId, mgiContext);
		}
	}
}