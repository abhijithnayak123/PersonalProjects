using MGI.Common.DataAccess.Data;
using System;

namespace MGI.Core.Partner.Data
{
	public abstract class BaseProfile : NexxoModel
	{
		public virtual Guid id { get; set; }
		public virtual string FirstName { get; set; }
		public virtual string MiddleName { get; set; }
		public virtual string LastName { get; set; }
		public virtual string LastName2 { get; set; }
		public virtual string MothersMaidenName { get; set; }
		public virtual Nullable<DateTime> DateOfBirth { get; set; }
		public virtual string Address1 { get; set; }
		public virtual string Address2 { get; set; }
		public virtual string City { get; set; }
		public virtual string State { get; set; }
		public virtual string ZipCode { get; set; }
		public virtual string Phone1 { get; set; }
		public virtual string Phone1Type { get; set; }
		public virtual string Phone1Provider { get; set; }
		public virtual string Phone2 { get; set; }
		public virtual string Phone2Type { get; set; }
		public virtual string Phone2Provider { get; set; }
		public virtual string Email { get; set; }
		public virtual string SSN { get; set; }
		public virtual string TaxpayerId { get; set; }
		public virtual bool DoNotCall { get; set; }
		public virtual bool SMSEnabled { get; set; }
		public virtual bool MarketingSMSEnabled { get; set; }
		public virtual Guid ChannelPartnerId { get; set; }
		public virtual string Gender { get; set; }
		public virtual string PIN { get; set; }
        public virtual bool MailingAddressDifferent { get; set; }
        public virtual string MailingAddress1 { get; set; }
        public virtual string MailingAddress2 { get; set; }
        public virtual string MailingCity { get; set; }
        public virtual string MailingState { get; set; }
		public virtual string MailingZipCode { get; set; }
		public virtual string CountryOfBirth { get; set; }
		public virtual string ClientID { get; set; }
		public virtual string LegalCode { get; set; }
		public virtual string PrimaryCountryCitizenShip { get; set; }
		public virtual string SecondaryCountryCitizenShip { get; set; }
	}
}
