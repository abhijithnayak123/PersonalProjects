using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MGI.Common.DataAccess.Data;

namespace MGI.Cxn.MoneyTransfer.WU.Data
{
    public class WUAccount :NexxoModel 
    {
        public virtual string NameType { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
		public virtual string MiddleName { get; set; }
		public virtual string SecondLastName { get; set; }
        public virtual string Address { get; set; }
        public virtual string City { get; set; }
        public virtual string State { get; set; }
        public virtual string PostalCode { get; set; }
        public virtual string PreferredCustomerAccountNumber { get; set; }
        public virtual string PreferredCustomerLevelCode { get; set; }
        public virtual string Email { get; set; }
        public virtual string ContactPhone { get; set; }
        public virtual string MobilePhone { get; set; }
        public virtual string SmsNotificationFlag { get; set; }
    }
}
