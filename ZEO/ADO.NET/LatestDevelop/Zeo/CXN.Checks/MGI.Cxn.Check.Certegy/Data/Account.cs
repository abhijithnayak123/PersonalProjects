using System;
using MGI.Common.DataAccess.Data;
using System.Collections.Generic;


namespace MGI.Cxn.Check.Certegy.Data
{
	public class Account: NexxoModel
	{		
		public virtual string FirstName { get; set; }
		public virtual string LastName { get; set; }
		public virtual string SecondLastName { get; set; }
		public virtual string Ssn { get; set; }
		public virtual DateTime DateOfBirth { get; set; }
		public virtual string Address1 { get; set; }
		public virtual string Address2 { get; set; }
		public virtual string City { get; set; }
		public virtual string State { get; set; }
		public virtual string Zip { get; set; }
		public virtual string Phone { get; set; }

		public virtual string IDType { get; set; }
		public virtual string Idcardnumber { get; set; }
		public virtual string IdState { get; set; }
		public virtual string IDCode { get; set; }
		
		public virtual ICollection<Transaction> CertegyTrxs { get; set; }
	}
}
