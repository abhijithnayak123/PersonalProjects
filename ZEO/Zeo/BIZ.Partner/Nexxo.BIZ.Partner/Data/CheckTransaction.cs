namespace MGI.Biz.Partner.Data
{
	public class CheckTransaction
	{
		// todo: Move common properties to base class
		public string CheckType { get; set; }
		public decimal Amount { get; set; }
		public decimal Fee { get; set; }
		public string ConfirmationNumber { get; set; }
	}
}
