using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AutoMapper;
using MGI.Channel.DMS.Server.Contract;
using Spring.Transaction.Interceptor;

using IDRequirement = MGI.Biz.Partner.Data.NexxoIdType;
using ServerData = MGI.Channel.DMS.Server.Data;
using MGI.Biz.Partner.Contract;
using MGI.Biz.Partner.Data;
using MGI.Channel.DMS.Server.Data;

namespace MGI.Channel.DMS.Server.Impl
{
	public partial class DesktopEngine : IDataStructuresService
	{
        private INexxoDataStructuresService _nexxoDataStructuresService;
        public INexxoDataStructuresService NexxoDataStructuresService
        {
            set { _nexxoDataStructuresService = value; }
        }

        [Transaction(ReadOnly = true)]
        public List<string> Cities(long agentSessionId, string state, MGIContext mgiContext)
		{
            throw new NotImplementedException("Not Implemented, Check once.");
		}

        [Transaction(ReadOnly = true)]
		public List<string> Countries(long agentSessionId, MGIContext mgiContext)
		{
			mgiContext = _GetContextFromAgentSession(agentSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			return _nexxoDataStructuresService.Countries(agentSessionId, context);
		}

        [Transaction(ReadOnly = true)]
		public List<string> States(long agentSessionId, string country, MGIContext mgiContext)
		{
			mgiContext = _GetContextFromAgentSession(agentSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
            return _nexxoDataStructuresService.States(agentSessionId, country, context);
		}

        [Transaction(ReadOnly = true)]
		public List<string> USStates(long agentSessionId, MGIContext mgiContext)
		{
			mgiContext = _GetContextFromAgentSession(agentSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
            return _nexxoDataStructuresService.USStates(agentSessionId, context);
		}

        [Transaction(ReadOnly = true)]
		public List<string> IdCountries(long agentSessionId, long channelPartnerId, MGIContext mgiContext)
		{
			mgiContext = _GetContextFromAgentSession(agentSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
            return _nexxoDataStructuresService.IdCountries(agentSessionId, channelPartnerId, context);
		}

        [Transaction(ReadOnly = true)]
		public List<string> IdStates(long agentSessionId, long channelPartnerId, string country, string idType, MGIContext mgiContext)
		{
			mgiContext = _GetContextFromAgentSession(agentSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
            return _nexxoDataStructuresService.IdStates(agentSessionId, channelPartnerId, country, idType, context);
		}

        [Transaction(ReadOnly = true)]
		public List<string> IdTypes(long agentSessionId, long channelPartnerId, string country, MGIContext mgiContext)
		{
			mgiContext = _GetContextFromAgentSession(agentSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
            return _nexxoDataStructuresService.IdTypes(agentSessionId, channelPartnerId, country, context);
		}
		
		[Transaction(ReadOnly = true)]
		public List<ServerData.LegalCode> GetLegalCodes(long agentSessionId, MGIContext mgiContext)
		{
			mgiContext = _GetContextFromAgentSession(agentSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			List<MGI.Biz.Partner.Data.LegalCode> legalcodes = _nexxoDataStructuresService.GetLegalCodes(agentSessionId, context);
			return Mapper.Map<List<ServerData.LegalCode>>(legalcodes);
		}

		[Transaction(ReadOnly = true)]
		public List<ServerData.Occupation> GetOccupations(long agentSessionId, MGIContext mgiContext)
		{
			mgiContext = _GetContextFromAgentSession(agentSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			List<MGI.Biz.Partner.Data.Occupation> occupations = _nexxoDataStructuresService.GetOccupations(agentSessionId, context);
			return Mapper.Map<List<ServerData.Occupation>>(occupations);
		}

        [Transaction(ReadOnly = true)]
		public ServerData.IDRequirement IdRequirements(long agentSessionId, long channelPartnerId, string country, string idType, string idState, MGIContext mgiContext)
		{
			mgiContext = _GetContextFromAgentSession(agentSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
            IDRequirement idReq = _nexxoDataStructuresService.Find(agentSessionId, channelPartnerId, idType, country, idState, context);
			return Mapper.Map<IDRequirement, ServerData.IDRequirement>(idReq);
		}

        [Transaction(ReadOnly = true)]
		public List<string> PhoneTypes(long agentSessionId, MGIContext mgiContext)
		{
			mgiContext = _GetContextFromAgentSession(agentSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
            return _nexxoDataStructuresService.PhoneTypes(agentSessionId, context);
		}

        [Transaction(ReadOnly = true)]
		public List<string> MobileProviders(long agentSessionId, MGIContext mgiContext)
		{
			mgiContext = _GetContextFromAgentSession(agentSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
            return _nexxoDataStructuresService.MobileProviders(agentSessionId, context);
		}

		[Transaction(ReadOnly = true)]
		public List<ServerData.MasterCountry> MasterCountries(long agentSessionId, long channelPartnerId, MGIContext mgiContext)
		{
			mgiContext = _GetContextFromAgentSession(agentSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			List<MGI.Biz.Partner.Data.MasterCountry> masterCountry = _nexxoDataStructuresService.MasterCountries(agentSessionId, channelPartnerId, context);
			return Mapper.Map<List<ServerData.MasterCountry>>(masterCountry);
		}

		[Transaction(ReadOnly = true)]
		public ServerData.MasterCountry GetMasterCountryByCode(long agentSessionId, string masterCountryAbbr2, MGIContext mgiContext)
		{
			mgiContext = _GetContextFromAgentSession(agentSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			MGI.Biz.Partner.Data.MasterCountry masterCountry = _nexxoDataStructuresService.GetMasterCountryByCode(agentSessionId, masterCountryAbbr2, context);
			return Mapper.Map<ServerData.MasterCountry>(masterCountry);
		}
		
		[Transaction(ReadOnly = true)]
		public List<string> ProfileStatus(long agentSessionId, MGIContext mgiContext)
		{
			List<string> profileStatus = new List<string>() {
				MGI.Common.Util.ProfileStatus.Active.ToString(),
				MGI.Common.Util.ProfileStatus.Inactive.ToString(),
				MGI.Common.Util.ProfileStatus.Closed.ToString() 
			};
			return profileStatus;
		}

		internal static void DataStructuresConverter()
		{
			Mapper.CreateMap<IDRequirement, ServerData.IDRequirement>();
			Mapper.CreateMap<MGI.Biz.Partner.Data.LegalCode, ServerData.LegalCode>();
			Mapper.CreateMap<MGI.Biz.Partner.Data.Occupation, ServerData.Occupation>();
			Mapper.CreateMap<MGI.Biz.Partner.Data.MasterCountry, ServerData.MasterCountry>();
		}

    }
}
