using MGI.Common.DataAccess.Data;
using System;

namespace MGI.Cxn.Fund.FirstView.Data
{
    public class FirstViewTransaction : NexxoModel
    {
        public virtual FirstViewCard Account { get; set; }     
        public virtual int ProcessorId { get; set; }
        public virtual System.Nullable<long> PrimaryAccountNumber { get; set; }
		public virtual string TransactionType { get; set; }
        public virtual System.Nullable<decimal> TransactionAmount { get; set; }
        public virtual string CardAcceptorIdCode { get; set; }
        public virtual string CardAcceptorTerminalID { get; set; }
        public virtual System.Nullable<int> CardAcceptorBusinessCode { get; set; }
        public virtual string TransactionDescription { get; set; }
        public virtual string MessageTypeIdentifier { get; set; }
        public virtual string TransactionCurrencyCode { get; set; }
        public virtual System.Nullable<System.DateTime> DTLocalTransaction { get; set; }
		public virtual System.Nullable<System.DateTime> DTTransmission { get; set; }
		public virtual System.Nullable<decimal> PreviousCardBalance { get; set; }
        public virtual System.Nullable<int> CreditPlanMaster { get; set; }
        public virtual string AccountNumber { get; set; }
        public virtual string TransactionID { get; set; }
        public virtual System.Nullable<decimal> CardBalance { get; set; }
        public virtual string ErrorCode { get; set; }
        public virtual string ErrorMsg { get; set; }      
		public virtual string CardStatus { get; set; }
		public virtual string ActivationRequired { get; set; }
        public virtual System.Nullable<long> ChannelPartnerID { get; set; }
	}
}