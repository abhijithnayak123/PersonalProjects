using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using MGI.Channel.DMS.Server.Contract;
using MGI.Channel.DMS.Server.Data;
using MGI.Channel.DMS.Server.Impl;

namespace MGI.Channel.DMS.Server.Svc
{
    public partial class DesktopWSImpl : ILocationService
    {
        public Location GetByName(long agentSessionId, string locationName, MGIContext mgiContext)
        {
            return DesktopEngine.GetByName(agentSessionId, locationName, mgiContext);
        }

        public List<Location> GetAll()
        {
            return DesktopEngine.GetAll();
        }

        public long Create(long agentSessionId, Location manageLocation, MGIContext mgiContext)
        {
            return DesktopEngine.Create(agentSessionId, manageLocation, mgiContext);
        }

        public bool Update(long agentSessionId, Location manageLocation, MGIContext mgiContext)
        {
            return DesktopEngine.Update(agentSessionId, manageLocation, mgiContext);
        }

        public Location Lookup(string agentSessionId, long locationId, MGIContext mgiContext)
		{
			return DesktopEngine.Lookup(agentSessionId, locationId, mgiContext);
		}
		//Added for filter the locations by channel partner Id
        public List<Location> GetAll(long agentSessionId, MGIContext mgiContext)
		{
			return DesktopEngine.GetAll(agentSessionId, mgiContext);
		}

    }
}