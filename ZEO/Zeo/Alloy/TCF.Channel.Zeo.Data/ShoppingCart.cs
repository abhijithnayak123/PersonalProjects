using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Channel.Zeo.Data
{
    public class ShoppingCart
    {
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public long CustomerSessionId { get; set; }
        [DataMember]
        public List<MoneyOrder> MoneyOrders { get; set; }
        [DataMember]
        public List<Check> Checks { get; set; }
        [DataMember]
        public List<BillPayTransaction> Bills { get; set; }
        [DataMember]
        public List<MoneyTransfer> MoneyTransfers { get; set; }
        [DataMember]
        public List<GPRCard> GPRCards { get; set; }
        [DataMember]
        public List<CartCash> Cash { get; set; }
        [DataMember]
        public decimal CheckTotal { get; set; }
        [DataMember]
        public decimal BillTotal { get; set; }
        [DataMember]
        public decimal MoneyTransfeTotal { get; set; }
        [DataMember]
        public decimal GprCardTotal { get; set; }
        [DataMember]
        public decimal CashInTotal { get; set; }
        [DataMember]
        public decimal MoneyOrderTotal { get; set; }
        [DataMember]
        public decimal GenerateAmount { get; set; }
        [DataMember]
        public decimal GenerateFee { get; set; }
        [DataMember]
        public decimal GenerateTotal { get; set; }
        [DataMember]
        public decimal DepletingAmount { get; set; }
        [DataMember]
        public decimal DepletingFee { get; set; }
        [DataMember]
        public decimal DepletingTotal { get; set; }
        [DataMember]
        public decimal SubTotalFee { get; set; }
        [DataMember]
        public decimal TotalDueToCustomer { get; set; }
        [DataMember]
        public bool IsReferral { get; set; }
        [DataMember]
        public bool IsReferralSectionEnabled { set; get; }
        [DataMember]
        public bool IsCheckFrank { set; get; }

        // Are we using below properties
        [DataMember]
        public string KioskId { get; set; }
        [DataMember]
        public string AppId { get; set; }

    }

}
