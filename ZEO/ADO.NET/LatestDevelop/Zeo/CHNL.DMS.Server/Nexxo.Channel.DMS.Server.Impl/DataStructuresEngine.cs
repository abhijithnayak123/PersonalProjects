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
        public Response Cities(long agentSessionId, string state, MGIContext mgiContext)
		{
            throw new NotImplementedException("Not Implemented, Check once.");
		}

        [Transaction(ReadOnly = true)]
		public Response Countries(long agentSessionId, MGIContext mgiContext)
		{
			Response response = new Response();
			mgiContext = _GetContextFromAgentSession(agentSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			response.Result = _nexxoDataStructuresService.Countries(agentSessionId, context);
			return response;
		}

        [Transaction(ReadOnly = true)]
		public Response States(long agentSessionId, string country, MGIContext mgiContext)
		{
			Response response = new Response();
			mgiContext = _GetContextFromAgentSession(agentSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			response.Result = _nexxoDataStructuresService.States(agentSessionId, country, context);
			return response;
		}

        [Transaction(ReadOnly = true)]
		public Response USStates(long agentSessionId, MGIContext mgiContext)
		{
			Response response = new Response();
			mgiContext = _GetContextFromAgentSession(agentSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
            response.Result = _nexxoDataStructuresService.USStates(agentSessionId, context);
			return response;
		}

        [Transaction(ReadOnly = true)]
		public Response IdCountries(long agentSessionId, long channelPartnerId, MGIContext mgiContext)
		{
			Response response = new Response();
			mgiContext = _GetContextFromAgentSession(agentSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			response.Result = _nexxoDataStructuresService.IdCountries(agentSessionId, channelPartnerId, context);
			return response;
		}

        [Transaction(ReadOnly = true)]
		public Response IdStates(long agentSessionId, long channelPartnerId, string country, string idType, MGIContext mgiContext)
		{
			Response response = new Response();
			mgiContext = _GetContextFromAgentSession(agentSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
            response.Result = _nexxoDataStructuresService.IdStates(agentSessionId, channelPartnerId, country, idType, context);
			return response;
		}

        [Transaction(ReadOnly = true)]
		public Response IdTypes(long agentSessionId, long channelPartnerId, string country, MGIContext mgiContext)
		{
			Response response = new Response();
			mgiContext = _GetContextFromAgentSession(agentSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			response.Result = _nexxoDataStructuresService.IdTypes(agentSessionId, channelPartnerId, country, context);
			return response;
		}
		
		[Transaction(ReadOnly = true)]
		public Response GetLegalCodes(long agentSessionId, MGIContext mgiContext)
		{
			Response response = new Response();
			mgiContext = _GetContextFromAgentSession(agentSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			List<MGI.Biz.Partner.Data.LegalCode> legalcodes = _nexxoDataStructuresService.GetLegalCodes(agentSessionId, context);
			response.Result = Mapper.Map<List<ServerData.LegalCode>>(legalcodes);
			return response;
		}

		[Transaction(ReadOnly = true)]
		public Response GetOccupations(long agentSessionId, MGIContext mgiContext)
		{
			Response response = new Response();
			mgiContext = _GetContextFromAgentSession(agentSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			List<MGI.Biz.Partner.Data.Occupation> occupations = _nexxoDataStructuresService.GetOccupations(agentSessionId, context);
			response.Result =  Mapper.Map<List<ServerData.Occupation>>(occupations);
			return response;
		}

        [Transaction(ReadOnly = true)]
		public Response IdRequirements(long agentSessionId, long channelPartnerId, string country, string idType, string idState, MGIContext mgiContext)
		{
			Response response = new Response();
			mgiContext = _GetContextFromAgentSession(agentSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
            IDRequirement idReq = _nexxoDataStructuresService.Find(agentSessionId, channelPartnerId, idType, country, idState, context);
			response.Result = Mapper.Map<IDRequirement, ServerData.IDRequirement>(idReq);
			return response;
		}

        [Transaction(ReadOnly = true)]
		public Response PhoneTypes(long agentSessionId, MGIContext mgiContext)
		{
			Response response = new Response();
			mgiContext = _GetContextFromAgentSession(agentSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			response.Result = _nexxoDataStructuresService.PhoneTypes(agentSessionId, context);
			return response;
		}

        [Transaction(ReadOnly = true)]
		public Response MobileProviders(long agentSessionId, MGIContext mgiContext)
		{
			Response response = new Response();
			mgiContext = _GetContextFromAgentSession(agentSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
            response.Result = _nexxoDataStructuresService.MobileProviders(agentSessionId, context);
			return response;
		}

		[Transaction(ReadOnly = true)]
		public Response MasterCountries(long agentSessionId, long channelPartnerId, MGIContext mgiContext)
		{
			Response response = new Response();
			mgiContext = _GetContextFromAgentSession(agentSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			List<MGI.Biz.Partner.Data.MasterCountry> masterCountry = _nexxoDataStructuresService.MasterCountries(agentSessionId, channelPartnerId, context);
			response.Result =  Mapper.Map<List<ServerData.MasterCountry>>(masterCountry);
			return response;
		}

		[Transaction(ReadOnly = true)]
		public Response GetMasterCountryByCode(long agentSessionId, string masterCountryAbbr2, MGIContext mgiContext)
		{
			Response response = new Response();
			mgiContext = _GetContextFromAgentSession(agentSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			MGI.Biz.Partner.Data.MasterCountry masterCountry = _nexxoDataStructuresService.GetMasterCountryByCode(agentSessionId, masterCountryAbbr2, context);
			response.Result = Mapper.Map<ServerData.MasterCountry>(masterCountry);
			return response;
		}
		
		[Transaction(ReadOnly = true)]
		public Response ProfileStatus(long agentSessionId, MGIContext mgiContext)
		{
			Response response = new Response();
			List<string> profileStatus = new List<string>() {
				MGI.Common.Util.ProfileStatus.Active.ToString(),
				MGI.Common.Util.ProfileStatus.Inactive.ToString(),
				MGI.Common.Util.ProfileStatus.Closed.ToString() 
			};
			response.Result = profileStatus;
			return response;
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
