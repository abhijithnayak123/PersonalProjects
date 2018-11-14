using System;

using MGI.Common.DataAccess.Data;

namespace MGI.Core.CXE.Data
{
	public class Receiver : NexxoModel
	{
		public virtual string FirstName { get; set; }
		public virtual string MiddleName { get; set; }
		public virtual string LastName { get; set; }
		public virtual string LastName2 { get; set; }
		public virtual string MothersMaidenName { get; set; }
		public virtual Nullable<DateTime> DateOfBirth { get; set; }
		public virtual string Address { get; set; }
		public virtual string City { get; set; }
		public virtual string State { get; set; }
		public virtual string ZipCode { get; set; }
		public virtual string Phone { get; set; }

		public virtual string Relationship { get; set; }
		public virtual string AccountNumber { get; set; }
		public virtual string AccountType { get; set; }
		public virtual string Gender { get; set; }
	}
}
