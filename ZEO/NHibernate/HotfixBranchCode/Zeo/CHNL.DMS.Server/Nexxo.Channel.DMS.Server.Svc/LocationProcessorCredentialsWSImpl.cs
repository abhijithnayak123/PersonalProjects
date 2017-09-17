using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MGI.Channel.DMS.Server.Contract;
using MGI.Channel.DMS.Server.Impl;
using MGI.Channel.DMS.Server.Data;

namespace MGI.Channel.DMS.Server.Svc
{
    public partial class DesktopWSImpl : ILocationProcessorCredentialsService
    {
        public IList<Data.ProcessorCredential> GetLocationProcessorCredentials(long agentSessionId, long locationId, MGIContext mgiContext)
        {
            return DesktopEngine.GetLocationProcessorCredentials(agentSessionId, locationId, mgiContext);
        }

        public bool Save(long agentSessionId, long locationId, Data.ProcessorCredential processorCredentials, MGIContext mgiContext)
        {
            return DesktopEngine.Save(agentSessionId, locationId, processorCredentials, mgiContext);
        }
    }
}