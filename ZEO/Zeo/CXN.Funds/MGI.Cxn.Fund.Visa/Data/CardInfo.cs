using System;

namespace MGI.Cxn.Fund.Visa.Data
{
	public class CardInfo
	{
		public long AliasId { get; set; }
		public long PrimaryAliasId { get; set; }
		public string CardNumber { get; set; }
		public double Balance { get; set; }
		public string CurrencyCode { get; set; }
		public string Status { get; set; }

		public long SubClientNodeId { get; set; }
		public string ProxyId { get; set; }

		public int ExpirationMonth { get; set; }
		public int ExpirationYear { get; set; }

		public string SSN { get; set; }
		public string LastName { get; set; }
		public string PsedoDDA { get; set; }

		public DateTime CardIssueDate { get; set; }
		public string PromotionCode { get; set; }
	}
}
