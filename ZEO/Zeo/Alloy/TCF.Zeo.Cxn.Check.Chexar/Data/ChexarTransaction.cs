using System;
using TCF.Zeo.Common.Data;
using TCF.Zeo.Cxn.Check.Data;

namespace TCF.Zeo.Cxn.Check.Chexar.Data
{
    public class ChexarTransaction : ZeoModel
    {
        public long TransactionId { set; get; }
        public long ChexarAccountId { set; get; }
        public decimal Amount { get; set; }
        public decimal ChexarAmount { get; set; }
        public decimal ChexarFee { get; set; }
        public DateTime? CheckDate { get; set; }
        public string CheckNumber { get; set; }
        public string RoutingNumber { get; set; }
        public string AccountNumber { get; set; }
        public string Micr { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int InvoiceId { get; set; }
        public int TicketId { get; set; }
        public string WaitTime { get; set; }
        public CheckStatus Status { get; set; }
        public string ChexarStatus { get; set; }
        public int SubmitType { get; set; }
        public int ReturnType { get; set; }
        public CheckType DmsReturnType { get; set; }
        public int DeclineCode { get; set; }
        public string Message { get; set; }
        public string Location { get; set; }
        public bool IsCheckFranked { get; set; }
        public string DeclineMessageKey { get; set; }

        ///Transaction History specific
        public string DiscountName { get; set; }
        public string DiscountDescription { get; set; }
        public decimal DiscountApplied { get; set; }

        public decimal BaseFee { get; set; }
        public byte[] ImageFront { get; set; }

        public decimal Fee { get; set; }
    }
}
