using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Biz.Partner.Data
{
    public class CashDrawerReport
    {
        public string AgentName { get; set; }
        public string LocationName { get; set; }
        public DateTime ReportDate { get; set; }
        public decimal CashIn { get; set; }
        public decimal CashOut { get; set; }
        public string ReportTemplate { get; set; }
    }
}
