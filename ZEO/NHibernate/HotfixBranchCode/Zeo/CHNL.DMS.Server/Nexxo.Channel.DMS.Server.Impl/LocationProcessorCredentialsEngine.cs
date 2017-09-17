using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MGI.Channel.DMS.Server.Contract;
using AutoMapper;
using MGI.Channel.DMS.Server.Data;
using MGI.Biz.Partner.Contract;
using Spring.Transaction.Interceptor;

namespace MGI.Channel.DMS.Server.Impl
{
    public partial class DesktopEngine : ILocationProcessorCredentialsService
    {
        public ILocationProcessorCredentials LocationProcessorCredentialsService { get; set; }

        [Transaction]
        public IList<ProcessorCredential> GetLocationProcessorCredentials(long agentSessionId, long locationId, MGIContext mgiContext)
        {
			mgiContext = _GetContextFromAgentSession(agentSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
            var processorDetails = LocationProcessorCredentialsService.Get(agentSessionId, locationId, context);

			return Mapper.Map<IList<MGI.Biz.Partner.Data.ProcessorCredentials>, IList<ProcessorCredential>>(processorDetails);
        }

        [Transaction]
		public bool Save(long agentSessionId, long locationId, Data.ProcessorCredential processorCredentials, MGIContext mgiContext)
		{
			mgiContext = _GetContextFromAgentSession(agentSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			var _processorCredentials = Mapper.Map<ProcessorCredential, MGI.Biz.Partner.Data.ProcessorCredentials>(processorCredentials);

            return LocationProcessorCredentialsService.Save(agentSessionId, locationId, _processorCredentials, context);
        }

        public static void LocationProcessorCredentialsEngineConverter()
        {
			Mapper.CreateMap<ProcessorCredential, MGI.Biz.Partner.Data.ProcessorCredentials>();
			Mapper.CreateMap<MGI.Biz.Partner.Data.ProcessorCredentials, ProcessorCredential>();
        }
    }
}
