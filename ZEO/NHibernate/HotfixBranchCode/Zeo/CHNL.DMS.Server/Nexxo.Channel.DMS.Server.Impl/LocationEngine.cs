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
		public LocationDTO GetByName(long agentSessionId, string locationName, MGIContext mgiContext)
        {
			mgiContext = _GetContextFromAgentSession(agentSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
            return Mapper.Map<LocationDTO>(LocationService.GetByName(agentSessionId, locationName, context));
        }

        [Transaction(ReadOnly = true)]
        public List<LocationDTO> GetAll()
        {
            return Mapper.Map<List<LocationDTO>>(LocationService.GetAll());
        }

        [Transaction()]
		public long Create(long agentSessionId, LocationDTO manageLocation, MGIContext mgiContext)
		{
			mgiContext = _GetContextFromAgentSession(agentSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
            return LocationService.Create(agentSessionId, Mapper.Map<Location>(manageLocation),context);
        }

        [Transaction()]
		public bool Update(long agentSessionId, LocationDTO manageLocation, MGIContext mgiContext)
		{
			mgiContext = _GetContextFromAgentSession(agentSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
            return LocationService.Update(agentSessionId, Mapper.Map<Location>(manageLocation), context);
        }

		[Transaction(ReadOnly = true)]
		public LocationDTO Lookup(string agentSessionId, long locationId, MGIContext mgiContext)
		{
			mgiContext = _GetContextFromAgentSession(long.Parse(agentSessionId), mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			return Mapper.Map<LocationDTO>(LocationService.Lookup(locationId, context));
		}

		[Transaction(ReadOnly = true)]
		public List<LocationDTO> GetAll(long agentSessionId, MGIContext mgiContext)
		{
			mgiContext = _GetContextFromAgentSession(agentSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			return Mapper.Map<List<LocationDTO>>(LocationService.GetAll(agentSessionId, context));
		}
    }
}
