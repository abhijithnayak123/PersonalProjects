using System;
using System.Runtime.Serialization;

namespace MGI.Channel.Shared.Server.Data
{
    [DataContract]
    public class Receiver
    {
        [DataMember]
        public long Id { get; set; }
        //Receivers Name
        [DataMember]
        public virtual string FirstName { get; set; }
        [DataMember]
        public virtual string LastName { get; set; }
        [DataMember]
        public virtual string SecondLastName { get; set; }

        //Pick Up Details
        [DataMember]
        public virtual string PickupCountry { get; set; }
        [DataMember]
        public virtual string PickupState_Province { get; set; }
        [DataMember]
        public virtual string PickupCity { get; set; }

        //Delivery Details
        [DataMember]
        public virtual string DeliveryMethod { get; set; }
        [DataMember]
        public virtual string DeliveryOption { get; set; }

        //Receiver Contact Details
        [DataMember]
        public virtual string Address { get; set; }
        [DataMember]
        public virtual string City { get; set; }
        [DataMember]
        public virtual string State_Province { get; set; }
        [DataMember]
        public virtual string ZipCode { get; set; }
        [DataMember]
        public virtual string PhoneNumber { get; set; }

        [DataMember]
        public virtual string Status { get; set; }
        
        [DataMember]
        public virtual string MiddleName { get; set; }

        [DataMember]
        public virtual string NickName { get; set; }
        
        [DataMember]
        public virtual bool IsReceiverHasPhotoId { get; set; }
        
        [DataMember]
        public virtual string SecurityQuestion { get; set; }
        
        [DataMember]
        public virtual string SecurityAnswer { get; set; }

        //This code is commented as part of MVA Receiver data refactor need to be confirmed 
        //[DataMember]
        //public virtual System.DateTime DTCreate { get; set; }
        //[DataMember]
        //public virtual System.DateTime? DTLastMod { get; set; } 

		override public string ToString()
		{
			string str = string.Empty;
			str = string.Concat(str, "Id = ", Id, "\r\n");
			str = string.Concat(str, "FirstName = ", FirstName, "\r\n");
			str = string.Concat(str, "LastName = ", LastName, "\r\n");
			str = string.Concat(str, "SecondLastName = ", SecondLastName, "\r\n");
            str = string.Concat(str, "Status = ", Status, "\r\n");
            str = string.Concat(str, "Address = ", Address, "\r\n");
            str = string.Concat(str, "City = ", City, "\r\n");
            str = string.Concat(str, "State_Province = ", State_Province, "\r\n");
            str = string.Concat(str, "ZipCode = ", ZipCode, "\r\n");
            str = string.Concat(str, "PhoneNumber = ", PhoneNumber, "\r\n");
            str = string.Concat(str, "PickupCountry = ", PickupCountry, "\r\n");
            str = string.Concat(str, "PickupState_Province = ", PickupState_Province, "\r\n");
            str = string.Concat(str, "PickupCity = ", PickupCity, "\r\n");
		    str = string.Concat(str, "Middle Name = ", MiddleName, "\r\n");
            str = string.Concat(str, "Nick Name = ", NickName, "\r\n");
            str = string.Concat(str, "IsReceiverHasPhotoId = ", IsReceiverHasPhotoId, "\r\n");
            str = string.Concat(str, "DeliveryMethod = ", DeliveryMethod, "\r\n");
            str = string.Concat(str, "DeliveryOption = ", DeliveryOption, "\r\n");
			return str;
		}
	}
}
