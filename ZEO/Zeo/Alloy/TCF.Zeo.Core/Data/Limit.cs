using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Core.Data
{
    public class Limit
    {
        public Nullable<decimal> PerTransactionMaximum { get; set; }
        public Nullable<decimal> PerTransactionMinimum { get; set; }
        public string RollingLimits { get; set; }
        public Dictionary<int, decimal> NDaysLimit { get { return GetLimits(); } }

        public Dictionary<int, decimal> GetLimits()
        {
            Dictionary<int, decimal> _nDaysLimits = new Dictionary<int, decimal>();
            if (!String.IsNullOrEmpty(RollingLimits))
            {
                _nDaysLimits = RollingLimits.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                                            .Select(part => part.Split(':')).ToList()
                                            .ToDictionary(split => Int32.Parse(split[0]), split => decimal.Parse(split[1]));
            }

            return _nDaysLimits;
        }
    }
}
