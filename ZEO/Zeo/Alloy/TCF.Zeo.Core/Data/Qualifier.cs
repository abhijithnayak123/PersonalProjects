using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCF.Zeo.Common.Data;
using TCF.Zeo.Common.Util;

namespace TCF.Zeo.Core.Data
{
    public class Qualifier : ZeoModel
    {
        public long QualifierId { get; set; }

        public long PromotionId { get; set; }

        public Product QualifierProduct { get; set; }

        public DateTime? TrxStartDate { get; set; }

        public DateTime? TrxEndDate { get; set; }

        public decimal? TrxAmount { get; set; }

        public int? MinTrxCount { get; set; }

        public string TransactionStates { get; set; }

        public bool IsPaidFee { get; set; }

        public bool IsActive { get; set; }

        public bool ConsiderParkedTxns { get; set; }

    }
}
