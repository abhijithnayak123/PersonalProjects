using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Cxn.MoneyTransfer.Data
{
    public class BaseRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SecondLastName { get; set; }
        public string MiddleName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string ContactPhone { get; set; }
        public string State { get; set; }
        public string MobilePhone { get; set; }
        public string PhoneNumber { get; set; }
    }
}
