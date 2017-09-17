using System.Collections.Generic;
using TCF.Zeo.Common.Util;

namespace TCF.Channel.Zeo.Data
{
    public class Funds
    {
        public decimal Amount { get; set; }
        public decimal BaseFee { get; set; }
        public decimal DiscountApplied { get; set; }
        public string DiscountName { get; set; }
        public decimal Fee { get; set; }
        public Helper.FundType FundsType { get; set; }
        public string Description { get; set; }
        public string PromoCode { get; set; }
        public string CardNumber { get; set; }
        public decimal CardBalance { get; set; }
        public string ConfirmationNumber { get; set; }
        public int ProviderId { get; set; }
        public long TransactionId { get; set; }
        public string ProxyId { get; set; }
        public string PseudoDDA { get; set; }
        public string ExpirationDate { get; set; }
        public string FullCardNumber { get; set; }
        public Dictionary<string, object> MetaData { get; set; }
    }
}
