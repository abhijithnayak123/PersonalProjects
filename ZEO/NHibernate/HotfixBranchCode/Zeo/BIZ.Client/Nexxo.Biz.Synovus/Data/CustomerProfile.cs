using System;
using MGI.Common.DataAccess.Data;

namespace MGI.Biz.Synovus.Data
{
    public class CustomerProfile : NexxoModel
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string LastName2 { get; set; }
        public string MothersMaidenName { get; set; }
		public Nullable<DateTime> DateOfBirth { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Phone1 { get; set; }
        public string Phone1Type { get; set; }
        public string Phone1Provider { get; set; }
        public string Phone2 { get; set; }
        public string Phone2Type { get; set; }
        public string Phone2Provider { get; set; }
        public string Email { get; set; }
        public string SSN { get; set; }
        public string TaxpayerId { get; set; }
        public bool DoNotCall { get; set; }
        public bool SMSEnabled { get; set; }
        public bool MarketingSMSEnabled { get; set; }
        public int ChannelPartnerId { get; set; }
        public string Gender { get; set; }
        public bool MailingAddressDifferent { get; set; }
        public string MailingAddress1 { get; set; }
        public string MailingAddress2 { get; set; }
        public string MailingCity { get; set; }
        public string MailingState { get; set; }
        public string MailingZipCode { get; set; }
        public bool IsPartnerAccountHolder { get; set; }
        public string ReferralCode { get; set; }
        public string PIN { get; set; }
        public string CardNumber { get; set; }
        public string AccountNumber { get; set; }
        public string ReceiptLanguage { get; set; }
        public bool ProfileStatus { get; set; }
    }
}
