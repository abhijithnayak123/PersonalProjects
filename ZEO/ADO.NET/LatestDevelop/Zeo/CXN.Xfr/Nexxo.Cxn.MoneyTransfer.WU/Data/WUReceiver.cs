using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MGI.Common.DataAccess.Data;
using MGI.Cxn.MoneyTransfer.Data;

namespace MGI.Cxn.MoneyTransfer.WU.Data
{
    public class WUReceiver : NexxoModel
    {
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string SecondLastName { get; set; }
        public virtual string Status { get; set; }
        public virtual string Gender { get; set; }
        public virtual string Country { get; set; }
        public virtual string Address { get; set; }
        public virtual string City { get; set; }
        public virtual string State_Province { get; set; }
        public virtual string ZipCode { get; set; }
        public virtual string PhoneNumber { get; set; }
        public virtual string PickupCountry { get; set; }
        public virtual string PickupState_Province { get; set; }
        public virtual string PickupCity { get; set; }
        public virtual string DeliveryMethod { get; set; }
        public virtual string DeliveryOption { get; set; }
        public virtual string Occupation { get; set; }
		public virtual System.Nullable<System.DateTime> DateOfBirth { get; set; }
        public virtual string CountryOfBirth { get; set; }
        public virtual System.Nullable<long> CustomerId { get; set; }
		public virtual string GoldCardNumber { get; set; }
        //ReceiverIndexNo has been added to the table through nHibernate. Added for User Story # US1645.
        public virtual string ReceiverIndexNo { get; set; }
		public virtual string RecieverFirstName { get; set; }
		public virtual string RecieverLastName { get; set; }
		public virtual string RecieverSecondLastName { get; set; }
    }
}