using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCF.Zeo.Common.Util;

namespace TCF.Channel.Zeo.Data
{
    public class Provision
    {
        public long ProvisionId { get; set; }

        public long PromotionId { get; set; }

        public string CheckTypeIds { get; set; }

        public string LocationIds { get; set; }

        public string Groups { get; set; }

        public decimal? MinTrxAmount { get; set; }

        public decimal? MaxTrxAmount { get; set; }

        public string Value { get; set; }

        public Helper.DiscountType DiscountType { get; set; }

        public int RowId { get; set; }

        public bool IsActive { get; set; }
    }
}
