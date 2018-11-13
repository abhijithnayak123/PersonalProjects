using System;

using MGI.Common.DataAccess.Data;

namespace MGI.Cxn.Fund.FIS.Data
{
	public class FISAccount : NexxoModel
	{
		public virtual long ProgramId { get; set; }
		public virtual string ExternalKey { get; set; }
		public virtual string CardNumber { get; set; }
		public virtual long AccountId { get; set; }
		public virtual long UserId { get; set; }
		public virtual string FirstName { get; set; }
		public virtual string MiddleName { get; set; }
		public virtual string LastName { get; set; }
		public virtual DateTime DOB { get; set; }
		public virtual string SSN { get; set; }
		public virtual string Address1 { get; set; }
		public virtual string Address2 { get; set; }
		public virtual string City { get; set; }
		public virtual string State { get; set; }
		public virtual string ZipCode { get; set; }
		public virtual string Country { get; set; }
		public virtual string Phone { get; set; }
		public virtual string PhoneType { get; set; }
		public virtual bool Activated { get; set; }
		public virtual int FraudScore { get; set; }
		public virtual string FraudResolution { get; set; }

	}
}
