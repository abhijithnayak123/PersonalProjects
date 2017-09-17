using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCF.Zeo.Common.Util;
using static TCF.Zeo.Common.Util.Helper;

namespace TCF.Channel.Zeo.Data
{
    public class SearchRequest
    {
        public string ConfirmationNumber { get; set; }

        public string ReasonDesc { get; set; }
        public string ReasonCode { get; set; }
        public string Comments { get; set; }

        public string RefundStatus { get; set; }

        public long TransactionId { get; set; }

        public long CancelTransactionId { get; set; }
        public long RefundTransactionId { get; set; }

        public Helper.SearchRequestType SearchRequestType { get; set; }

        //Only for Biz Layer - to Write in CXE and PTNR
        public decimal Amount { get; set; }
        public decimal Fee { get; set; }
        public string TransactionSubType { get; set; }
        public long OriginalTransactionId { get; set; }
        public string ReceiverFirstName { get; set; }
        public string ReceiverLastName { get; set; }
    }
}
