using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using MGI.Channel.DMS.Server.Data;
using MGI.Common.Sys;

namespace MGI.Channel.DMS.Server.Contract
{
    [ServiceContract]
    public interface IReportService
    {
        [FaultContract(typeof(NexxoSOAPFault))]
        [OperationContract]
		CashDrawer CashDrawerReport(long agentSessionId, int agentId, long locationId, MGIContext mgiContext);
    }
}
