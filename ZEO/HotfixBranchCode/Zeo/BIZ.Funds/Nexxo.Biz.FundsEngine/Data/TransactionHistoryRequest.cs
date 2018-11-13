namespace MGI.Biz.FundsEngine.Data
{
	public class TransactionHistoryRequest
	{
		public long AliasId { get; set; }
		public TransactionStatus TransactionStatus { get; set; }
		public int DateRange { get; set; }
	}
}
