using MGI.Common.DataAccess.Data;
using System;
namespace MGI.Cxn.BillPay.Data
{
	public class BillPayAccount : NexxoModel
    {
        public string FirstName { get; set; }
		public string LastName { get; set; }
		public DateTime? DateOfBirth { get; set; }
		public string Address1 { get; set; }
		public string Address2 { get; set; }
		public string City { get; set; }
		public string Street { get; set; }
		public string State { get; set; }
		public string PostalCode { get; set; }
		public string CardNumber { get; set; }
		public string Email { get; set; }
		public string ContactPhone { get; set; }
		public string MobilePhone { get; set; }
		public string SmsNotificationFlag { get; set; }
    }
}
