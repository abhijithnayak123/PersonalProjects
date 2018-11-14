using TCF.Zeo.Cxn.Common;
using System;
using TCF.Zeo.Common.Util;

namespace TCF.Zeo.Cxn.Fund.Data
{
    public class FundTrx : BaseRequest
    {
        public decimal TransactionAmount;

        public decimal Fee;

        public decimal PreviousCardBalance;

        public decimal CardBalance;

        public long TransactionID;

        public string TransactionType;

        public string TransactionDescription;
        public string PromoCode { get; set; }
        public string CardNumber { get; set; }
        public string FullCardNumber { get; set; }
        public string Description { get; set; }
        public string ConfirmationId { get; set; }
        public long LocationNodeId { get; set; }
        public virtual Helper.TransactionStates Status { get; set; }
        public virtual DateTime? DTTransmission { get; set; }
        public string ProxyId { get; set; }
        public string PseudoDDA { get; set; }
        public string ExpirationDate { get; set; }
    }
}
