using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Core.Data
{
    public class CustomerSearchCriteria
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public Nullable<DateTime> DateOfBirth { get; set; } 
        public string Cardnumber { get; set; }
        public long AlloyID { get; set; }
        public string SSN { get; set; }
    }
}
