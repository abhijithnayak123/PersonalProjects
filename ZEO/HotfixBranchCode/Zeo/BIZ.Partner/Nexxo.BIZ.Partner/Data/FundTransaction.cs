using System.Collections.Generic;
namespace MGI.Biz.Partner.Data
{
	public class FundTransaction
	{
		public int FundType { get; set; }
		public string CardNumber { get; set; }
		public decimal Amount { get; set; }
		public decimal Fee { get; set; }
		public decimal CardBalance { get; set; }
		public string ConfirmationNumber { get; set; }
		public int ProviderId { get; set; }
		public string PromoCode { get; set; }
		public Dictionary<string, object> MetaData { set; get; }
	}
}
