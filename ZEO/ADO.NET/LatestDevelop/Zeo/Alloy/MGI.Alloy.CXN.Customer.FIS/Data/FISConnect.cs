using MGI.Alloy.CXN.Common;
using System;


namespace MGI.Alloy.CXN.Customer.FIS.Data
{
    /// <summary>
    /// Added this new class to implement the SQL Injection. US#1789
    /// </summary>
    public class FISConnect : BaseRequest
    {
        public virtual string CustomerNumber { get; set; }
        public virtual string PrimaryPhoneNumber { get; set; }
        public virtual string Secondaryphone { get; set; }
        public virtual string CustomerTaxNumber { get; set; }
        public virtual string LastName { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string MiddleName { get; set; }
        public virtual string MiddleName2 { get; set; }
        public virtual string AddressStreet { get; set; }
        public virtual string AddressCity { get; set; }
        public virtual string AddressState { get; set; }
        public virtual string ZipCode { get; set; }
		public virtual Nullable<DateTime> DateOfBirth { get; set; }
        public virtual string DriversLicenseNumber { get; set; }
        public virtual string MothersMaidenName { get; set; }
        public virtual string Gender { get; set; }
        public virtual string ExternalKey { get; set; }
        public virtual string MetBankNumber { get; set; }
        public virtual string ProgramId { get; set; }
		//public virtual Nullable<DateTime> DTCreate { get; set; }
	}
}