using System;

namespace MGI.Biz.CPEngine.Data
{
	public class CheckSubmission
	{
		public decimal Amount { get; set; }
		public string CheckType { get; set; }
		public decimal Fee { get; set; }
		public string MICR { get; set; }
		public DateTime IssueDate { get; set; }
		public string ImageFormat { get; set; }
		public byte[] FrontImage { get; set; }
		public byte[] BackImage { get; set; }
		public byte[] FrontImageTIFF { get; set; }
		public byte[] BackImageTIFF { get; set; }
		//US1799 Promotions
		public string PromoCode { get; set; }
		public bool IsSystemApplied { get; set; }
		public string AccountNumber { get; set; }		
		public string RoutingNumber { get; set; }		
		public string CheckNumber { get; set; }
		public int MicrEntryType { get; set; }
	}
}
