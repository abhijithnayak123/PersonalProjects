using MGI.Common.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MGI.Common.Util;

namespace MGI.Core.Partner.Data
{
    public class ChannelPartnerProductProvider : NexxoModel
    {
        public virtual ChannelPartner ChannelPartner { get; set; }
        public virtual ProductProcessor ProductProcessor { get; set; }
        public virtual int Sequence { get; set; }
		public bool IsTnCForcePrintRequired { get; set; }
		public virtual CheckEntryTypes CheckEntryType { get; set; }
		public virtual int MinimumTransactAge { get; set; }
		public virtual int CardExpiryPeriod { get; set; }
    }
}
