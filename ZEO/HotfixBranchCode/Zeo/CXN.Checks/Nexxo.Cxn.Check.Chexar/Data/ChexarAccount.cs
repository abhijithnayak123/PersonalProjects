using System;
using MGI.Common.DataAccess.Data;

namespace MGI.Cxn.Check.Chexar.Data
{
    /// <summary>
    /// TODO: some of these elements could be in Cxn.common (Customer), Or Cxn.Checks. 
    /// For now in the most specific implementation i.e. Chexar. Might reconsider refactoring as more product cxns are added.
    /// </summary>
	public class ChexarAccount : NexxoModel
    {
        public virtual int Badge { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
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
		public virtual string GovernmentId { get; set; }
        public virtual string IDCardType { get; set; }
        public virtual string IDCardIssuedCountry { get; set; }
        public virtual Nullable<DateTime> IDCardIssuedDate { get; set; }
        public virtual byte[] IDCardImage { get; set; }
		public virtual Nullable<DateTime> IDCardExpireDate { get; set; }
        public virtual long CardNumber { get; set; }
        public virtual int CustomerScore { get; set; }
		public virtual string IDCode { get; set; }
    }
}
