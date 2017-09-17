using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MGI.Common.DataAccess.Data;

namespace MGI.Cxn.Customer.CCIS.Data
{
    public class CCISConnect : NexxoModel
    {
        public virtual string CustomerNumber { get; set; }
        public virtual string CustomerTaxNumber { get; set; }
        public virtual string PrimaryPhoneNumber { get; set; }
        public virtual string SecondaryPhone { get; set; }
        public virtual string LastName { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string MiddleName { get; set; }
        public virtual string MiddleName2 { get; set; }
        public virtual string AddressStreet { get; set; }
        public virtual string AddressCity { get; set; }
        public virtual string AddressState { get; set; }
        public virtual string ZipCode { get; set; }
		public virtual Nullable<DateTime> DateOfBirth { get; set; }
        public virtual string MothersMaidenName { get; set; }
        public virtual string DriversLicenseNumber { get; set; }
        public virtual string ExternalKey { get; set; }
        public virtual string MetBankNumber { get; set; }
        public virtual string ProgramId { get; set; }
        public virtual string Gender { get; set; }
        //public virtual Nullable<DateTime> DTCreate { get; set; }
    }
}
