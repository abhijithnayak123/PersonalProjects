using System;
using System.Collections.Generic;
using MGI.Common.DataAccess.Data;

namespace MGI.Cxn.BillPay.MG.Data
{
    public class BillerDenomination : NexxoModel
    {
        public virtual BillerLimit BillerLimit { get; set; }
        public virtual decimal DenominationAmount { get; set; }
    }
}
