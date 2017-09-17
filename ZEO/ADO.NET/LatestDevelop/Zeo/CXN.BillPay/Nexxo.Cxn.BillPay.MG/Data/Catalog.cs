using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MGI.Common.DataAccess.Data;

namespace MGI.Cxn.BillPay.MG.Data
{
    public class Catalog : NexxoModel
    {
        public virtual string ReceiveAgentId { get; set; }
        public virtual string ReceiveCode { get; set; }
        public virtual string BillerName { get; set; }
        public virtual string PoeSvcMsgENText { get; set; }
        public virtual string PoeSvcMsgESText { get; set; }
        public virtual string Keywords { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual long ChannelPartnerId { get; set; }
    }
}
