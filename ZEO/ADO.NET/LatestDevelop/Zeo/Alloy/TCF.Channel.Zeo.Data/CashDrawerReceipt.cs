using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Channel.Zeo.Data
{
 
   public  class CashDrawerReceipt
    {   

        public DateTime ReportingDate { get; set; }

 
        public decimal CashIn { get; set; }

       
        public decimal CashOut { get; set; }

       
        public string ReportTemplate { get; set; }
    }
}
