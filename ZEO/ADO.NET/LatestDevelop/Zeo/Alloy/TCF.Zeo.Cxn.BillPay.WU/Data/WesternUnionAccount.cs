using TCF.Zeo.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Cxn.BillPay.WU.Data
{
    public class WesternUnionAccount : ZeoModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string CardNumber { get; set; }
        public string PreferredCustomerLevelCode { get; set; }
        public string Email { get; set; }
        public string ContactPhone { get; set; }
        public string MobilePhone { get; set; }
        public string SmsNotificationFlag { get; set; }
    }
}
