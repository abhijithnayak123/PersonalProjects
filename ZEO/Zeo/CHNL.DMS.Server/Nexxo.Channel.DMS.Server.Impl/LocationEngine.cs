using System.Collections.Generic;
using AutoMapper;
using MGI.Biz.Partner.Contract;
using MGI.Channel.DMS.Server.Contract;
using MGI.Channel.DMS.Server.Data;
using Spring.Transaction.Interceptor;
using Location = MGI.Biz.Partner.Data.Location;
using LocationDTO = MGI.Channel.DMS.Server.Data.Location;

namespace MGI.Channel.DMS.Server.Impl
{
	public partial class DesktopEngine : ILocationService
	{
		public IManageLocations LocationService { get; set; }

		internal static void LocationEngineConverter()
		{
			Mapper.CreateMap<LocationDTO, Location>();
			Mapper.CreateMap<Location, LocationDTO>();
			Mapper.CreateMap<ProcessorCredential, MGI.Biz.Partner.Data.ProcessorCredentials>();
			Mapper.CreateMap<MGI.Biz.Partner.Data.ProcessorCredentials, ProcessorCredential>();
		}
		[Transaction(ReadOnly = true)]
		public Response GetByName(long agentSessionId, string locationName, MGIContext mgiContext)
		{
			mgiContext = _GetContextFromAgentSession(agentSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			Response response = new Response();

			response.Result = Mapper.Map<LocationDTO>(LocationService.GetByName(agentSessionId, locationName, context));
			return response;
		}

		[Transaction(ReadOnly = true)]
		public Response GetAll()
		{
			Response response = new Response();
			response.Result = Mapper.Map<List<LocationDTO>>(LocationService.GetAll());
			return response;
		}

		[Transaction()]
		public Response Create(long agentSessionId, LocationDTO manageLocation, MGIContext mgiContext)
		{
			mgiContext = _GetContextFromAgentSession(agentSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			Response response = new Response();

			response.Result = LocationService.Create(agentSessionId, Mapper.Map<Location>(manageLocation), context);
			return response;
		}

		[Transaction()]
		public Response Update(long agentSessionId, LocationDTO manageLocation, MGIContext mgiContext)
		{
			mgiContext = _GetContextFromAgentSession(agentSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			Response response = new Response();	

			response.Result = LocationService.Update(agentSessionId, Mapper.Map<Location>(manageLocation), context);
			return response;
		}

		[Transaction(ReadOnly = true)]
		public Response Lookup(string agentSessionId, long locationId, MGIContext mgiContext)
		{
			mgiContext = _GetContextFromAgentSession(long.Parse(agentSessionId), mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			Response response = new Response();

			response.Result = Mapper.Map<LocationDTO>(LocationService.Lookup(locationId, context));
			return response;
		}

		[Transaction(ReadOnly = true)]
		public Response GetAll(long agentSessionId, MGIContext mgiContext)
		{
			mgiContext = _GetContextFromAgentSession(agentSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			Response response = new Response();

			response.Result = Mapper.Map<List<LocationDTO>>(LocationService.GetAll(agentSessionId, context));
			return response;
		}
	}
}
