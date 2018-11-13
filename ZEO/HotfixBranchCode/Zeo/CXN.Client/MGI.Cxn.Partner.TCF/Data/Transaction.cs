using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGI.Cxn.Partner.TCF.Data
{
	public class Transaction
	{
		public string ID { get; set; }
		public string AccountNumber { get; set; }
		public string Type { get; set; }
		public string ConfirmationNumber { get; set; }
		public decimal Amount { get; set; }
		public decimal Fee { get; set; }
		public decimal GrossTotalAmount { get; set; }
		public string ToFirstName { get; set; }
		public string ToMiddleName { get; set; }
		public string ToLastName { get; set; }
		public string ToSecondLastName { get; set; }
		public string ToGender { get; set; }
		public string ToCountry { get; set; }
		public string ToAddress { get; set; }
		public string ToPhoneNumber { get; set; }
		public string ToCity { get; set; }
		public string ToState_Province { get; set; }
		public string ToZipCode { get; set; }
		public string ToPickUpCountry { get; set; }
		public string ToPickUpState_Province { get; set; }
		public string ToPickUpCity { get; set; }
		public string ToDeliveryMethod { get; set; }
		public string ToDeliveryOption { get; set; }
		public string ToOccupation { get; set; }
		public Nullable<DateTime> ToDOB { get; set; }
		public string ToCountryOfBirth { get; set; }
		public string Payee { get; set; }
		public string MTCN { get; set; }
		public string CheckType { get; set; }
		public string CheckNumber { get; set; }
		public string Status { get; set; }
		public string InitialPurchase { get; set; }
		public decimal PurchaseFee { get; set; }
		public decimal NewCardBalance { get; set; }
		public string CardNumber { get; set; }
		public string AliasId { get; set; }
		public decimal LoadAmount { get; set; }
		public string TransferType { get; set; }
		public string CashType { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string SecondLastName { get; set; }
		public string Gender { get; set; }
		public string Country { get; set; }
		public string Address { get; set; }
		public string City { get; set; }
		public string State_Province { get; set; }
		public string ZipCode { get; set; }
		public string PhoneNumber { get; set; }
		public string PickUpCountry { get; set; }
		public string PickUpState_Province { get; set; }
		public string PickUpCity { get; set; }
		public string DeliveryMethod { get; set; }
		public string DeliveryOption { get; set; }
		public string Occupation { get; set; }
		public string DateOfBirth { get; set; }
		public string CountryOfBirth { get; set; }
		public decimal WithdrawAmount { get; set; }
	}
}
