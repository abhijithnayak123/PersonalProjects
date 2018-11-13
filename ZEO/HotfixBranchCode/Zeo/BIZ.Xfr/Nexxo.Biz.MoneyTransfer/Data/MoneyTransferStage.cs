using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Biz.MoneyTransfer.Data
{
    public class MoneyTransferStage
    {
        public System.Guid Rowguid { get; set; }
        public long Id { get; set; }
        public decimal Amount { get; set; }
        public decimal Fee { get; set; }
        public int Status { get; set; }
        public string ConfirmationNumber { get; set; }
        public string ReceiverName { get; set; }
        public string Destination { get; set; }
        //public System.DateTime Dtcreate { get; set; }
        //public System.Nullable<System.DateTime> Dtlastmod { get; set; }
    }
}
