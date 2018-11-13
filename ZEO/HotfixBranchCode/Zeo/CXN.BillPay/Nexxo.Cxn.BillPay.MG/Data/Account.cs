using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MGI.Common.DataAccess.Data;

namespace MGI.Cxn.BillPay.MG.Data
{
    public class Account : NexxoModel
    {
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual DateTime? DateOfBirth { get; set; }
        public virtual string Address1 { get; set; }
        public virtual string Address2 { get; set; }
        public virtual string City { get; set; }
        public virtual string Street { get; set; }
        public virtual string State { get; set; }
        public virtual string PostalCode { get; set; }
        public virtual string Email { get; set; }
        public virtual string ContactPhone { get; set; }
        public virtual string MobilePhone { get; set; }
        public virtual string SmsNotificationFlag { get; set; }
    }
}
