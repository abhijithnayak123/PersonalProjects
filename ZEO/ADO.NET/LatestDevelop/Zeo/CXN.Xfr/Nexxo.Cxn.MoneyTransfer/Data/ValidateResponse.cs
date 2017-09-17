namespace MGI.Cxn.MoneyTransfer.Data
{
	public class ValidateResponse
	{
		public long TransactionId { get; set; }
		public bool HasLPMTError { get; set; }
	}
}
