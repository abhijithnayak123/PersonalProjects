using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCF.Zeo.Common.Util;

namespace TCF.Channel.Zeo.Data
{
    public class Qualifier
    {
        public long QualifierId { get; set; }

        public long PromotionId { get; set; }

        public MasterData QualifierProduct { get; set; }

        public DateTime? TrxStartDate { get; set; }

        public DateTime? TrxEndDate { get; set; }

        public decimal? TrxAmount { get; set; }

        public int? MinTrxCount { get; set; }

        public string TransactionStates { get; set; }

        public bool IsPaidFee { get; set; }

        public int RowId { get; set; }

        public bool IsActive { get; set; }

        public bool ConsiderParkedTxns { get; set; }
    }
}
