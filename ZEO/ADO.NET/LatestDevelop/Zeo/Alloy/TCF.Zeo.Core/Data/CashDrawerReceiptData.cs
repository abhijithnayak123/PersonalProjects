using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Core.Data
{
  public  class CashDrawerReceiptData :BaseReceiptData
    {
        public DateTime ReportingDate { get; set; }
       
        public decimal CashIn { get; set; }
       
        public decimal CashOut { get; set; }
       
        public string ReportTemplate { get; set; }

    }
}
