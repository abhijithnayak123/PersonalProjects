using System;
using System.Collections.Generic;
using MGI.Common.DataAccess.Data;

namespace MGI.Cxn.BillPay.MG.Data
{
    public class BillerLimit : NexxoModel
    {
        public virtual string AgentUnitOffice { get; set; }
        public virtual string AgentID { get; set; }
        public virtual DateTime ModifiedDate { get; set; }
        public virtual string ModifiedUser { get; set; }
        public virtual bool AmtFlag { get; set; }
        public virtual decimal MinimumAmount { get; set; }
        public virtual decimal MaximumAmount { get; set; }
        public virtual string MinimumAmountMessage { get; set; }
        public virtual string MaximumAmountMessage { get; set; }
        public virtual string MessageForAmtNotInList { get; set; }
        public virtual string ReceiveCode { get; set; }
        public virtual string TransactingAgentID { get; set; }
        public virtual int ChannelPartnerId { get; set; }
        public virtual IList<BillerDenomination> Denominations { get; set; }
    }
}
