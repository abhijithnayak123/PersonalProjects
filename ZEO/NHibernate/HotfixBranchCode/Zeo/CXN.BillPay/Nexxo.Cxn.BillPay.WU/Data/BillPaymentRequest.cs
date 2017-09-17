using MGI.Cxn.WU.Common.Data;

namespace MGI.Cxn.BillPay.WU.Data
{
	public class BillPaymentRequest
	{
		public long CxnId { get; set; }
		public string BillerName { get; set; }
		public string AccountNumber { get; set; }
		public string Location { get; set; }
		public string DeliveryCode { get; set; }
		public decimal Amount { get; set; }
		public decimal Fee { get; set; }
		public string MTCN { get; set; }
		public string NewMTCN { get; set; }
		public string FillingDate { get; set; }
		public string FillingTime { get; set; }
		public RequestType RequestStatus { get; set; }
		public string SessionCookie { get; set; }

		public string Attention { get; set; }
		public string DateOfBirth { get; set; }
		public string AailableBalance { get; set; }
		public string AccountHolder { get; set; }
		public string Reference { get; set; }
		public string PromoCode { get; set; }
		public string CouponCode { get; set; }

		public string ComplianceTemplateID { get; set; }
		public string PrimaryIdType { get; set; }
		public string PrimaryIdNumber { get; set; }
		public string PrimaryIdCountryOfIssue { get; set; }
		public string PrimaryIdPlaceOfIssue { get; set; }
		public string SecondIdType { get; set; }
		public string SecondIdNumber { get; set; }
		public string SecondIdCountryOfIssue { get; set; }
		public string Occupation { get; set; }
		public string CountryOfBirth { get; set; }
		public string PrimaryIdPlaceOfIssueCode { get; set; }
	}
}
