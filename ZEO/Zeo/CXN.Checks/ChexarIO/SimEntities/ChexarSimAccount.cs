using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChexarIO.SimEntities
{
	public class ChexarSimAccount
	{
		public virtual Guid rowguid { get; set; }
		public virtual int Badge { get; set; }
		public virtual string FName { get; set; }
		public virtual string LName { get; set; }
		public virtual string ITIN { get; set; }
		public virtual string SSN { get; set; }
		public virtual DateTime? DateOfBirth { get; set; }
		public virtual string Address1 { get; set; }
		public virtual string Address2 { get; set; }
		public virtual string City { get; set; }
		public virtual string State { get; set; }
		public virtual string Zip { get; set; }
		public virtual string Phone { get; set; }
		public virtual string Occupation { get; set; }
		public virtual string Employer { get; set; }
		public virtual string EmployerPhone { get; set; }
		public virtual ChexarIDTypes IDType { get; set; }
		public virtual string IDCountry { get; set; }
		public virtual DateTime? IDExpDate { get; set; }
		public virtual string GovernmentId { get; set; }
		public virtual byte[] IDImage { get; set; }
		public virtual string CardNumber { get; set; }
		public virtual int CustomerScore { get; set; }
		public virtual DateTime DTServerCreate { get; set; }
		public virtual DateTime? DTServerLastModified { get; set; }
		public virtual string IDCode { get; set; }
	}
}
