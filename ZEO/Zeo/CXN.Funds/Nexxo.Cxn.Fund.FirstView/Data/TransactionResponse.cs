namespace MGI.Cxn.Fund.FirstView.Data
{
	public class TransactionResponse
	{
		public string MessageTypeIdentifier { get; set; }
		public string CustNbr { get; set; }
		public string PrimaryAccountNumber { get; set; }
		public string TranType { get; set; }
		public string TransactionAmount { get; set; }
		public string TransmissionDateTime { get; set; }
		public string CreditPlanMaster { get; set; }
		public string TransactionDescription { get; set; }
		public string TransactionCurrencyCode { get; set; }
		public string CardAcceptorIdCode { get; set; }
		public string CardAcceptorTerminalID { get; set; }
		public string CardAcceptorBusinessCode { get; set; }
		public string DateTimeLocalTransaction { get; set; }
		public string SystemTraceAuditNumber { get; set; }
		public string MerchantType { get; set; }
		public string TransactionFeeBnp { get; set; }
		public string LateFeeBnp { get; set; }
		public string MemberShipFeeBnp { get; set; }
		public string OverLimitFeeBnp { get; set; }
		public string InsufficientFundFeeBnp { get; set; }
		public string CollectionFeeBnp { get; set; }
		public string RecoveryFeeBnp { get; set; }
		public string InsuranceBnp { get; set; }
		public string InterestBnp { get; set; }
		public string CurrentBalance { get; set; }
		public string Principal { get; set; }
		public string DaysDelinquent { get; set; }
		public string AmountDue { get; set; }
		public string PastDue { get; set; }
		public string PostingFlag { get; set; }
		public string PostingNote { get; set; }
		public string ADMIN_NO { get; set; }
		public string TRANSACTION_ID { get; set; }
	}
}
