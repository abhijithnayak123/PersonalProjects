using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MGI.Biz.Partner.Contract;

using bizCashDrawerReport = MGI.Biz.Partner.Data.CashDrawerReport;
using coreCashDrawerReport = MGI.Core.Partner.Data.CashDrawerReport;
using coreUserDetails = MGI.Core.Partner.Data.UserDetails;


using coreReportService = MGI.Core.Partner.Contract.IReportService;
using coreManageUsers = MGI.Core.Partner.Contract.IManageUsers;

using MGI.Biz.CashEngine.Data;
using MGI.Biz.Partner.Data;
using MGI.Common.Util;


namespace MGI.Biz.Partner.Impl
{
    public class ReportServiceImpl : IReportService
    {
        private coreReportService _reportService;
        public coreReportService ReportService
        {
            set { _reportService = value; }
        }

        private ReceiptTemplateRepo _receiptRepo;
        public ReceiptTemplateRepo ReceiptRepo
        {
            set { _receiptRepo = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="agentId"></param>
        /// <param name="locationId"></param>
        /// <returns></returns>
		public bizCashDrawerReport GetCashDrawerReport(long agentSessionId, int agentId, long locationId, MGIContext mgiContext)
        {
            List<coreCashDrawerReport> coreCashDrawer = _reportService.Get(agentId, locationId);

            bizCashDrawerReport bizCashDrawer = new bizCashDrawerReport()
            {
                CashIn = coreCashDrawer.Where(c => c.CashType == (int)CashTransactionType.CashIn).Sum(s => s.Amount),
                CashOut = coreCashDrawer.Where(c => c.CashType == (int)CashTransactionType.CashOut).Sum(s => s.Amount),
                ReportDate = DateTime.Now,
                ReportTemplate = _receiptRepo.GetCashDrawerReportTemplate()
            };

            return bizCashDrawer;
        }
    }
}
