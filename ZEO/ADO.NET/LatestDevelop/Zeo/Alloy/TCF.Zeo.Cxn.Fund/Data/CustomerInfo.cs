using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Cxn.Fund.Data
{
    public class CustomerInfo
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SSN { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string MothersMaidenName { get; set; }
        public string IDCode { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
