using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCF.Zeo.Common.Data;

namespace TCF.Zeo.Cxn.Check.Data
{
    public class CheckTransaction
    {
        public long Id;
        public decimal Amount;
        public string CheckNumber;
        public decimal ReturnAmount;
        public decimal ReturnFee;
        public string WaitTime;
        public CheckStatus Status;
        public CheckType SubmitType;
        public CheckType ReturnType;
        public string DeclineCode;
        public string DeclineMessage;
        public string ConfirmationNumber;
        public int TicketId;
        public bool IsCheckFranked;
        public decimal BaseFee { get; set; }
        public string DiscountName { get; set; }
        public string DiscountDescription { get; set; }
        public decimal DiscountApplied { get; set; }
        public Dictionary<string, object> MetaData { get; set; }
        public byte[] ImageFront { get; set; }
        public decimal Fee { get; set; }
    }
}
