using MGI.Common.DataAccess.Data;
using System;
namespace MGI.Cxn.BillPay.WU.Data
{
	public class WesternUnionAccount : NexxoModel
	{
		public virtual string FirstName { get; set; }
		public virtual string LastName { get; set; }
		public virtual DateTime? DateOfBirth { get; set; }
		public virtual string Address1 { get; set; }
		public virtual string Address2 { get; set; }
		public virtual string City { get; set; }
		public virtual string Street { get; set; }
		public virtual string State { get; set; }
		public virtual string PostalCode { get; set; }
		public virtual string CardNumber { get; set; }
		public virtual string PreferredCustomerLevelCode { get; set; }
		public virtual string Email { get; set; }
		public virtual string ContactPhone { get; set; }
		public virtual string MobilePhone { get; set; }
		public virtual string SmsNotificationFlag { get; set; }
	}
}
