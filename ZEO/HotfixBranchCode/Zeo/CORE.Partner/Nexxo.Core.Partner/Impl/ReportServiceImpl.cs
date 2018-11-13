using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MGI.Core.Partner.Data;
using MGI.Core.Partner.Contract;

using MGI.Common.DataAccess.Contract;


namespace MGI.Core.Partner.Impl
{
    public class ReportServiceImpl : IReportService
    {
        private IRepository<CashDrawerReport> _cashDrawerReportRepo;
        public IRepository<CashDrawerReport> CashDrawerReportRepo { set { _cashDrawerReportRepo = value; } }

        public List<CashDrawerReport> Get(int agentId, long locationId)
        {
            IQueryable<CashDrawerReport> cashDrawerReport = _cashDrawerReportRepo.FilterBy(x => x.AgentId == agentId && x.LocationId == locationId);
            return cashDrawerReport.ToList<CashDrawerReport>();
        }
    }
}
