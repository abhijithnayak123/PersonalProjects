using TCF.Zeo.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Cxn.MoneyTransfer.Data
{
    public class Receiver : ZeoModel
    {
        public  string FirstName { get; set; }
        public  string LastName { get; set; }
        public  string SecondLastName { get; set; }
        public  string Status { get; set; }
        public  string Gender { get; set; }
        public  string Address { get; set; }
        public  string City { get; set; }
        public  string State_Province { get; set; }
        public  string ZipCode { get; set; }
        public  string PhoneNumber { get; set; }
        public  string PickupCountry { get; set; }
        public  string PickupState_Province { get; set; }
        public  string PickupCity { get; set; }
        public  string DeliveryMethod { get; set; }
        public  string DeliveryOption { get; set; }
        //public  string Occupation { get; set; }
        //public  System.Nullable<System.DateTime> DateOfBirth { get; set; }
        //public  string CountryOfBirth { get; set; }
        public  System.Nullable<long> CustomerId { get; set; }
        public  string GoldCardNumber { get; set; }
        public  string ReceiverIndexNo { get; set; }
        public  string Country { get; set; }
        //public  string MiddleName { get; set; }
        //public  string NickName { get; set; }
        //public  bool IsReceiverHasPhotoId { get; set; }
        //public  string SecurityQuestion { get; set; }
        //public  string SecurityAnswer { get; set; }
    }
}
