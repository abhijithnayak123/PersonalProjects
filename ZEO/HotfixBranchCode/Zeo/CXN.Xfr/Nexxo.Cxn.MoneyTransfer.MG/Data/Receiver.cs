using MGI.Common.DataAccess.Data;
namespace MGI.Cxn.MoneyTransfer.MG.Data
{
	public class Receiver : NexxoModel
	{
		public virtual string FirstName { get; set; }
		public virtual string LastName { get; set; }
		public virtual string SecondLastName { get; set; }
		public virtual bool IsActive { get; set; }
		public virtual string Country { get; set; }
		public virtual string Address { get; set; }
		public virtual string City { get; set; }
		public virtual string State { get; set; }
		public virtual string ZipCode { get; set; }
		public virtual string PhoneNumber { get; set; }
		public virtual string PickupCountry { get; set; }
		public virtual string PickupState { get; set; }
        public virtual System.Nullable<long> CustomerId { get; set; }
        public virtual string MiddleName { get; set; }
	    public virtual string NickName { get; set; }
	    public virtual bool IsReceiverHasPhotoId { get; set; }
		public virtual string SecurityQuestion { get; set; }
		public virtual string SecurityAnswer { get; set; }
	}
}
