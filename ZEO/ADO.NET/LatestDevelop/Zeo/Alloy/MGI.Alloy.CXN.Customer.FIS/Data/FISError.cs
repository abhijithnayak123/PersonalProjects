using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Alloy.CXN.Customer.FIS.Data
{
	public class FISError
	{
		public virtual long Id { get; set; }
		public virtual string ErrorNumber { get; set; }
		public virtual string ErrorMessage { get; set; }
		public virtual string BankID { get; set; }
		public virtual string NexxoCustomerId { get; set; }
		public virtual string AppID { get; set; }
		public virtual string FISRelationshipIndicator { get; set; }
		public virtual string FISAddressLineCode1 { get; set; }
		public virtual string FISAddressLineCode2 { get; set; }
		public virtual string FISAddressLineCode3 { get; set; }
		public virtual string FISCurrentNameAddressLine1 { get; set; }
		public virtual string FISCurrentNameAddressLine2 { get; set; }
		public virtual string FISCurrentNameAddressLine3 { get; set; }
		public virtual string FISAccountNumber { get; set; }
		public virtual string FISAcountType { get; set; }

		public virtual string BranchId { get; set; }
		public virtual string NxoEvent { get; set; }
		public virtual DateTime DTTerminalCreate { get; set; }
		public virtual Nullable<DateTime> DTTerminalLastModified { get; set; }
		public virtual DateTime DTServerCreate { get; set; }
		public virtual Nullable<DateTime> DTServerLastModified { get; set; }
	}
}
