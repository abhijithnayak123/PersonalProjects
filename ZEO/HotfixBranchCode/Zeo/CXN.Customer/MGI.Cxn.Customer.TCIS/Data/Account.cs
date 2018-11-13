using MGI.Common.DataAccess.Data;
using MGI.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGI.Cxn.Customer.TCIS.Data
{
    public class Account : NexxoModel
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
		public virtual string Email { get; set; }
        public virtual string SSN { get; set; }
        public virtual string Gender { get; set; }
        public virtual string PartnerAccountNumber { get; set; }
        public virtual string RelationshipAccountNumber { get; set; }
		public virtual ProfileStatus ProfileStatus { get; set; }
        public virtual string BankId { get; set; }
        public virtual string BranchId { get; set; }
        public virtual string Phone1Type { get; set; }
        public virtual string PIN { get; set; }

        public virtual string GovernmentIDType { get; set; }
		public virtual string GovernmentId { get; set; }
        public virtual string IDIssuingCountry { get; set; }
        public virtual string IDIssuingState { get; set; }
		public virtual string IDIssuingStateCode { get; set; }
        public virtual Nullable<DateTime> IDIssueDate { get; set; }
        public virtual Nullable<DateTime> IDExpirationDate { get; set; }

        public virtual string Occupation { get; set; }
		public virtual string OccupationDescription { get; set; }
        public virtual string EmployerName { get; set; }
		public virtual string EmployerPhone { get; set; }
        public virtual string ClientID { get; set; }
        public virtual string LegalCode { get; set; }
        public virtual string PrimaryCountryCitizenShip { get; set; }
        public virtual string SecondaryCountryCitizenShip { get; set; }
        public virtual string CountryOfBirth { get; set; }
              
        public virtual string Phone1Provider { get; set; }
        public virtual string Phone2Type { get; set; }
        public virtual string Phone2Provider { get; set; }
		public virtual bool TcfCustInd { get; set; }
		public virtual string IDCode { get; set; }

    }

}
