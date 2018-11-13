using MGI.Common.DataAccess.Data;

namespace MGI.Cxn.MoneyTransfer.Data
{
	public class Receiver : NexxoModel
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
        //ReceiverIndex has been added to identify the receiver by Index Number. Added for User Story # US1645.
        public virtual string ReceiverIndexNo { get; set; }
        public virtual string MiddleName { get; set; }
        public virtual string NickName { get; set; }
        public virtual bool IsReceiverHasPhotoId { get; set; }
		public virtual string SecurityQuestion { get; set; }
		public virtual string SecurityAnswer { get; set; }

	}
}
