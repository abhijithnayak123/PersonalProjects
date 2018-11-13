using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MGI.Channel.DMS.Server.Contract;
using MGI.Channel.DMS.Server.Data;
using MGI.Channel.DMS.Server.Impl;

namespace MGI.Channel.DMS.Server.Svc
{
    public partial class DesktopWSImpl : IReportService
    {
        public CashDrawer CashDrawerReport(long agentSessionId, int agentId, long locationId, MGIContext mgiContext)
        {
            return DesktopEngine.CashDrawerReport(agentSessionId, agentId, locationId, mgiContext);
        }
    }
}