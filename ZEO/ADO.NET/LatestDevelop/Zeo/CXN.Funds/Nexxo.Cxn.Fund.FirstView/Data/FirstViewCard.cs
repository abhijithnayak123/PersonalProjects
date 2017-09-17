using System;
using MGI.Common.DataAccess.Data;
namespace MGI.Cxn.Fund.FirstView.Data
{
    public class FirstViewCard : NexxoModel 
    {
        public virtual string CardNumber { get; set; }
        public virtual string AccountNumber { get; set; }
        public virtual string BSAccountNumber { get; set; }
        public virtual string NameAsOnCard { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string MiddleName { get; set; }
        public virtual string LastName { get; set; }
        public virtual Nullable<System.DateTime> DateOfBirth { get; set; }
        public virtual Nullable<long> SSN { get; set; }
		public virtual string GovernmentId { get; set; }
        public virtual string IDNumber { get; set; }
        public virtual Nullable<DateTime> GovtIdExpirationDate { get; set; }
        public virtual string GovtIDIssueCountry { get; set; }
        public virtual Nullable<DateTime> GovtIDIssueDate { get; set; }
        public virtual string GovtIDIssueState { get; set; }
        public virtual string AddressLine1 { get; set; }
        public virtual string AddressLine2 { get; set; }
        public virtual string City { get; set; }
        public virtual string State { get; set; }
        public virtual string PostalCode { get; set; }
        public virtual string HomePhoneNumber { get; set; }
        public virtual string ShippingContactName { get; set; }
        public virtual string ShippingAddressLine1 { get; set; }
        public virtual string ShippingAddressLine2 { get; set; }
        public virtual string ShippingCity { get; set; }
        public virtual string ShippingState { get; set; }
        public virtual string ShippingZipCode { get; set; }
        public virtual Nullable<System.DateTime> ExpiryDate { get; set; }
        public virtual Nullable<bool> IsActive { get; set; }
        public virtual Nullable<System.DateTime> DTActivated { get; set; }
        public virtual Nullable<int> ActivatedBy { get; set; }
        public virtual Nullable<System.DateTime> DTDeactivated { get; set; }
        public virtual Nullable<int> DeactivatedBy { get; set; }
        public virtual string DeactivatedReason { get; set; }
    }
}