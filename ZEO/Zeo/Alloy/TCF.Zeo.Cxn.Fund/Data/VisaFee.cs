using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Cxn.Fund.Data
{
    public class VisaFee
    {
        public virtual double Fee { get; set; }
        public virtual int FeeCode { get; set; }
        public virtual string StockId { get; set; }
    }
}
