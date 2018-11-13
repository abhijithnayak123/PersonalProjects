using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MGI.Channel.DMS.NpsInstallerServer.Contract;
using MGI.Channel.DMS.NpsInstallerServer.Data;

namespace MGI.Channel.DMS.NpsInstallerServer.Svc
{
	public partial class NpsInstallerWSImpl : INpsInstallerService
	{
        public bool CreateTerminal(NpsTerminal npsTerminal)
		{
			return NpsInstallerEngine.CreateTerminal(npsTerminal);
		}

        public bool UpdateTerminal(NpsTerminal npsTerminal)
		{
			return NpsInstallerEngine.UpdateTerminal(npsTerminal);
		}

        public List<string> Locations(string agentName, string agentPassword, string channelPartnerName)
		{
			return NpsInstallerEngine.Locations(agentName, agentPassword, channelPartnerName);
		}

        public NpsTerminal LookupTerminal(string ipAddress)
		{
			return NpsInstallerEngine.LookupTerminal(ipAddress);
		}

		public List<Location> GetAllLocations()
		{
			return NpsInstallerEngine.GetAllLocations();
		}
	}
}