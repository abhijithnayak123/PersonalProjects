using System;
using TCF.Zeo.Common.Data;

namespace TCF.Zeo.Cxn.Check.TCF.Data
{

    public class TCFOnusAccount : ZeoModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ITIN { get; set; }
        public string SSN { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string Occupation { get; set; }
        public string Employer { get; set; }
        public string EmployerPhone { get; set; }
        public string GovernmentId { get; set; }
        public string IDCardType { get; set; }
        public string IDCardIssuedCountry { get; set; }
        public Nullable<DateTime> IDCardIssuedDate { get; set; }
        public byte[] IDCardImage { get; set; }
        public Nullable<DateTime> IDCardExpireDate { get; set; }
        public long CardNumber { get; set; }
        public int CustomerScore { get; set; }
        public string IDCode { get; set; }
        public long CustomerSessionId { get; set; }
        public long CustomerId { get; set; }

    }
}
