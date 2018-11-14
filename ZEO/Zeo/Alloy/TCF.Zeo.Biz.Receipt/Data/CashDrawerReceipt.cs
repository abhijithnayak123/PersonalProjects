using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Biz.Receipt.Data
{
   public class CashDrawerReceipt
    {
        public DateTime ReportingDate { get; set; }
        public decimal CashIn { get; set; }
        public decimal CashOut { get; set; }
        public string ReportTemplate { get; set; }
    }
}
