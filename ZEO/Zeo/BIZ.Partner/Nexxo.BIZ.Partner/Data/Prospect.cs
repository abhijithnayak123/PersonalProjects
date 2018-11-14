using System;
using System.Collections.Generic;
using MGI.Common.Util;

namespace MGI.Biz.Partner.Data
{
	public class Prospect
	{
		public Prospect ()
		{
		}

		public string FName {get; set;}
        public string MName {get; set;}
        public string LName {get; set;}
		public string LName2 { get; set; }
		public string MoMaName { get; set; }
		public string Gender { get; set; }
		public DateTime? DateOfBirth { get; set; }
		public string PIN { get; set; }
        public string Address1 {get;set;}
        public string Address2{get;set;}
        public string City {get; set;}
        public string State {get;set;}
        public string PostalCode {get;set;}
		public string Phone1 { get; set; }
		public string Phone1Type { get; set; }
		public string Phone1Provider { get; set; }
		public string Phone2 { get; set; }
		public string Phone2Type { get; set; }
		public string Phone2Provider { get; set; }
		public bool TextMsgOptIn { get; set; }
		public string Email { get; set; }
		public string Occupation { get; set; }
		public string OccupationDescription { get; set; }
		public string Employer { get; set; }
		public string EmployerPhone { get; set; }
		public string SSN { get; set; }
		public Identification ID { get; set; }
		//public List<Beneficiary> Beneficiaries { get; set; }
		public bool DoNotCall { get; set; }
		public bool IsAccountHolder { get; set; }
		public string ReferralCode { get; set; }
		//public string CardNumber { get; set; }
		//public string WUGoldCardNumber {get; set;}
		//public bool WUSMSNotification { get; set; }
        public bool MailingAddressDifferent { get; set; }
        public string MailingAddress1 { get; set; }
        public string MailingAddress2 { get; set; }
        public string MailingCity { get; set; }
        public string MailingState { get; set; }
        public string MailingZipCode { get; set; }
        public Guid ChannelPartnerId { get; set; }
        public string ReceiptLanguage { get; set; }
		public ProfileStatus ProfileStatus { get; set; }
		public List<string> Groups { get; set; }
		public string ClientID { get; set; }
		public string LegalCode { get; set; }
		public string PrimaryCountryCitizenShip { get; set; }
		public string SecondaryCountryCitizenShip { get; set; }
		public string Notes { get; set; }
		public CustomerScreen CustomerScreen { get; set; }
	}
}

