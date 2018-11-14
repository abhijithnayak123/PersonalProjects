using System;
using TCF.Zeo.Common.Data;

namespace TCF.Zeo.Cxn.Check.Chexar.Data
{

    public class ChexarAccount : ZeoModel
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
        public virtual long CustomerSessionId { get; set; }
        public virtual long CustomerId { get; set; }

    }
}
