using System;

namespace MGI.Cxn.Check.Data
{
	public class CheckInfo
	{
		public decimal Amount { get; set; }
		public string Micr { get; set; }
		public DateTime IssueDate { get; set; }
		public CheckType Type { get; set; }
		public byte[] FrontImage { get; set; }
		public byte[] BackImage { get; set; }
		public string ImageFormat { get; set; }
		public byte[] FrontImageTIF { get; set; }
		public byte[] BackImageTIF { get; set; }
		public double Latitude { get; set; }
		public double Longitude { get; set; }
		public int MicrEntryType { get; set; }
		public string AccountNumber { get; set; }
		public string RoutingNumber { get; set; }
		public string CheckNumber { get; set; }
	}
}
