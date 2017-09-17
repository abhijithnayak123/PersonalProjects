using System;

namespace MGI.Cxn.Fund.Data
{
    public class FundTrx
    {
        public decimal TransactionAmount;
        public decimal Fee;
        public System.Nullable<decimal> PreviousCardBalance;
        public System.Nullable<decimal> CardBalance;
        public string TransactionID;
        public string TransactionType;
        public string TransactionDescription;
        public CardAccount Account;
		public string PromoCode { get; set; }
    }
}
