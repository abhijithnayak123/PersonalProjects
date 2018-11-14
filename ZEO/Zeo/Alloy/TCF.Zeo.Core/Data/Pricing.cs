using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Core.Data
{
    public class Pricing
    {
        public int CompareType { get; set; }

        public decimal MinimumAmount { get; set; }

        public decimal MaximumAmount { get; set; }

        public decimal MinimumFee { get; set; }

        public decimal MaximumFee { get; set; }

        public decimal Value { get; set; }

        public bool IsPercentage { get; set; }
    }
}
