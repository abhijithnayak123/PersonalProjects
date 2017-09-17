using System;
using MGI.Common.DataAccess.Data;
using MGI.Common.Util;

namespace MGI.Cxn.Customer.CCIS.Data
{
    public class CCISAccount : NexxoModel
    {
        public virtual string FirstName { get; set; }
        public virtual string MiddleName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string LastName2 { get; set; }
        public virtual string MothersMaidenName { get; set; }
		public virtual Nullable<DateTime> DateOfBirth { get; set; }
        public virtual string Address1 { get; set; }
        public virtual string Address2 { get; set; }
        public virtual string City { get; set; }
        public virtual string State { get; set; }
        public virtual string ZipCode { get; set; }
        public virtual string Phone1 { get; set; }
		public virtual string Phone2 { get; set; }
        public virtual string SSN { get; set; }
        public virtual string Gender { get; set; }
        public virtual string PartnerAccountNumber { get; set; }
        public virtual string RelationshipAccountNumber { get; set; }
		public virtual ProfileStatus ProfileStatus { get; set; }
		public virtual string BankId { get; set; }
		public virtual string BranchId { get; set; }
		public virtual string Phone1Type { get; set; }

		public virtual string GovernmentIDType { get; set; }
		public virtual string GovernmentId { get; set; }
		public virtual string IDIssuingCountry { get; set; }
		public virtual string IDIssuingState { get; set; }
		public virtual Nullable<DateTime> IDIssueDate { get; set; }
		public virtual Nullable<DateTime> IDExpirationDate { get; set; }
		
		public virtual string Occupation { get; set; }
		public virtual string EmployerName { get; set; }
		public virtual string IDCode { get; set; }
	}
}