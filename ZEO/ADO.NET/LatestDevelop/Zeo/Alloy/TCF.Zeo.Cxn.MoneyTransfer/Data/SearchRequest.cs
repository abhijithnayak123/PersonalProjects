using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TCF.Zeo.Common.Util.Helper;

namespace TCF.Zeo.Cxn.MoneyTransfer.Data
{
    public class SearchRequest
    {
        public string ConfirmationNumber { set; get; }
        public string ReasonDesc { get; set; }
        public string ReasonCode { get; set; }
        public string Comments { get; set; }
        public string RefundStatus { get; set; }
        public string ReferenceNumber { get; set; }
        public SearchRequestType SearchRequestType { get; set; }
        public long TransactionId { get; set; }
        public long CancelTransactionId { get; set; }
        public long ModifyOrRefundTransactionId { get; set; }
        public DateTime DTTerminalLastModified { get; set; }
        public DateTime DTServerLastModified { get; set; }
    }
}
