using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Biz.MoneyTransfer.Data
{
    public class Receiver 
    {    
        public  long Id { get; set; }
        
        //Receivers Name
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string SecondLastName { get; set; }

        //Pick Up Details
        public virtual string PickupCountry { get; set; }
        public virtual string PickupState_Province { get; set; }
        public virtual string PickupCity { get; set; }

        //Delivery Details
        public virtual string DeliveryMethod { get; set; }
        public virtual string DeliveryOption { get; set; }

        //Receiver Contact Details
        public virtual string Address { get; set; }
        public virtual string City { get; set; }
        public virtual string State_Province { get; set; }
        public virtual string ZipCode { get; set; }
        public virtual string PhoneNumber { get; set; }

        public virtual string Status { get; set; }
        public virtual System.Nullable<long> CustomerId { get; set; }

        public virtual string MiddleName { get; set; }
        public virtual string NickName { get; set; }
        public virtual bool IsReceiverHasPhotoId { get; set; }		
		public virtual string SecurityQuestion { get; set; }
		public virtual string SecurityAnswer { get; set; }

        //This code is commented as part of MVA Receiver data refactor need to be confirmed 
        //public virtual System.DateTime DTCreate { get; set; }
        //public virtual System.DateTime? DTLastMod { get; set; }  
 

    }
}
