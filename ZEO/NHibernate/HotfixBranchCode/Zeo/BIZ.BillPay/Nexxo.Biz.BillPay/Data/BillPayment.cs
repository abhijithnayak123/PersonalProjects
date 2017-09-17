using System.Collections.Generic;

namespace MGI.Biz.BillPay.Data
{
	public class BillPayment
	{
		public string BillerName { get; set; }
        public string AccountNumber { get; set; }//Renamed CustomerAccountNumber to AccountNumber
		public decimal PaymentAmount { get; set; }
		public decimal Fee { get; set; }
		public string CouponCode { get; set; }
        public Dictionary<string, object> MetaData { get; set; }
        public long billerID { get; set; }
        //Todo:Commented By Sakhatech 
        //public string Location { get; set; }
        //public string DeliveryCode { get; set; }
        //public string SessionCookie { get; set; }
        //public string Attention { get; set; }
        //public string DateOfBirth { get; set; }
        //public string AailableBalance { get; set; }
        //public string AccountHolder { get; set; }
        //public string Reference { get; set; }
        //public string PromoCode { get; set; }
	}
}