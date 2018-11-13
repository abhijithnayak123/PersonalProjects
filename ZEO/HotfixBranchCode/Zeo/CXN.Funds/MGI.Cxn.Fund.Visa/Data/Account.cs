using MGI.Common.DataAccess.Data;
using System;

namespace MGI.Cxn.Fund.Visa.Data
{
	public class Account : NexxoModel
	{
		public virtual string ProxyId { get; set; }
		public virtual string PseudoDDA { get; set; }
		public virtual string CardNumber { get; set; }
		public virtual string CardAliasId { get; set; }

		public virtual string FirstName { get; set; }
		public virtual string MiddleName { get; set; }
		public virtual string LastName { get; set; }
		public virtual DateTime DateOfBirth { get; set; }
		public virtual string SSN { get; set; }
		public virtual string Phone { get; set; }
		public virtual string Address1 { get; set; }
		public virtual string Address2 { get; set; }
		public virtual string City { get; set; }
		public virtual string State { get; set; }
		public virtual string ZipCode { get; set; }
		public virtual string Country { get; set; }
		public virtual bool Activated { get; set; }
		public virtual int? FraudScore { get; set; }

		public virtual int ExpirationMonth { get; set; }
		public virtual int ExpirationYear { get; set; }
		public virtual long SubClientNodeId { get; set; }
		public virtual DateTime? DTAccountClosed { get; set; }
		public virtual string IDCode { get; set; }
		//AL-2999 changes
		public virtual string Email { get; set; }
		//AL-3054
		public virtual string MothersMaidenName { get; set; }
		//AL-2108
		public virtual string PrimaryCardAliasId { get; set; }

		//AL-5945
		public virtual long ActivatedLocationNodeId { get; set; }
	}
}
