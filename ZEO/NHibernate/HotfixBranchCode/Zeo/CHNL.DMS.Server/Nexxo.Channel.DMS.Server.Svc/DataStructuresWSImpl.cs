using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using MGI.Channel.DMS.Server.Data;
using MGI.Channel.DMS.Server.Contract;
using MGI.Channel.DMS.Server.Impl;

namespace MGI.Channel.DMS.Server.Svc
{
	public partial class DesktopWSImpl : IDataStructuresService
	{
        public List<string> Cities(long agentSessionId, string state, MGIContext mgiContext)
		{
			return DesktopEngine.Cities(agentSessionId, state, mgiContext);
		}

        public List<string> Countries(long agentSessionId, MGIContext mgiContext)
		{
			return DesktopEngine.Countries(agentSessionId, mgiContext);
		}

        public List<string> States(long agentSessionId, string country, MGIContext mgiContext)
		{
			return DesktopEngine.States(agentSessionId, country, mgiContext);
		}

        public List<string> USStates(long agentSessionId, MGIContext mgiContext)
		{
			return DesktopEngine.USStates(agentSessionId, mgiContext);
		}

        public List<string> IdCountries(long agentSessionId, long channelPartnerId, MGIContext mgiContext)
		{
			return DesktopEngine.IdCountries(agentSessionId, channelPartnerId, mgiContext);
		}

        public List<string> IdStates(long agentSessionId, long channelPartnerId, string country, string idType, MGIContext mgiContext)
		{
			return DesktopEngine.IdStates(agentSessionId, channelPartnerId, country, idType, mgiContext);
		}

        public List<string> IdTypes(long agentSessionId, long channelPartnerId, string country, MGIContext mgiContext)
		{
			return DesktopEngine.IdTypes(agentSessionId, channelPartnerId, country, mgiContext);
		}
        public List<LegalCode> GetLegalCodes(long agentSessionId, MGIContext mgiContext)
		{
			return DesktopEngine.GetLegalCodes(agentSessionId, mgiContext);
		}

        public List<Occupation> GetOccupations(long agentSessionId, MGIContext mgiContext)
		{
			return DesktopEngine.GetOccupations(agentSessionId, mgiContext);
		}

        public IDRequirement IdRequirements(long agentSessionId, long channelPartnerId, string country, string idType, string state, MGIContext mgiContext)
		{
			return DesktopEngine.IdRequirements(agentSessionId, channelPartnerId, country, idType, state, mgiContext);
		}

        public List<string> PhoneTypes(long agentSessionId, MGIContext mgiContext)
		{
			return DesktopEngine.PhoneTypes(agentSessionId, mgiContext);
		}

        public List<string> MobileProviders(long agentSessionId, MGIContext mgiContext)
		{
			return DesktopEngine.MobileProviders(agentSessionId, mgiContext);
		}

        public List<MasterCountry> MasterCountries(long agentSessionId, long channelPartnerId, MGIContext mgiContext)
		{
			return DesktopEngine.MasterCountries(agentSessionId, channelPartnerId, mgiContext);
		}

        public MasterCountry GetMasterCountryByCode(long agentSessionId, string masterCountryAbbr2, MGIContext mgiContext)
		{
			return DesktopEngine.GetMasterCountryByCode(agentSessionId, masterCountryAbbr2, mgiContext);
		}

        public List<string> ProfileStatus(long agentSessionId, MGIContext mgiContext)
		{
			return DesktopEngine.ProfileStatus(agentSessionId, mgiContext);
		}
	}
}
