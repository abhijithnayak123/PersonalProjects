using System;
using System.Collections.Generic;
using System.Linq;
using MGI.Common.DataAccess.Data;
using MGI.Common.Util;

namespace MGI.Core.CXE.Data
{
	public class Customer : NexxoModel
	{
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
		public virtual long ChannelPartnerId { get; set; }
		public virtual string Gender { get; set; }
		public virtual string PIN { get; set; }
		public virtual bool MailingAddressDifferent { get; set; }
		public virtual string MailingAddress1 { get; set; }
		public virtual string MailingAddress2 { get; set; }
		public virtual string MailingCity { get; set; }
		public virtual string MailingState { get; set; }
		public virtual string MailingZipCode { get; set; }
		public virtual CustomerEmploymentDetails EmploymentDetails { get; set; }
		public virtual CustomerGovernmentId GovernmentId { get; set; }
		public virtual ICollection<Account> Accounts { get; set; }
		public virtual string ReceiptLanguage { get; set; }
		public virtual ProfileStatus ProfileStatus { get; set; }
		public virtual string CountryOfBirth { get; set; }
		public virtual string Notes { get; set; }
		public virtual string ClientID { get; set; }
		public virtual string LegalCode { get; set; }
		public virtual string PrimaryCountryCitizenShip { get; set; }
		public virtual string SecondaryCountryCitizenShip { get; set; }
		public virtual string IDCode { get; set; }
		  

		public Customer()
		{
			Accounts = new List<Account>();
		}

		/// <summary>
		/// Find a customer's account by AccountType & ProviderId
		/// </summary>
		/// <param name="type">AccountType</param>
		/// <param name="providerId">ProviderId</param>
		/// <returns>Account object</returns>
		//public virtual Account GetAccount( int type )
		//{
		//    return Accounts.FirstOrDefault<Account>( a => a.Type == type );
		//}

		/// <summary>
		/// Find a customer's account by the Account Id
		/// </summary>
		/// <param name="id">Account Id</param>
		/// <returns>Account object</returns>
		public virtual Account GetAccount(long id)
		{
			return Accounts.FirstOrDefault<Account>(a => a.Id == id);
		}

		public virtual void UpdateProfile(Customer profile)
		{
			this.Address1 = profile.Address1;
			this.Address2 = profile.Address2;
			this.City = profile.City;
			this.DateOfBirth = profile.DateOfBirth;
			this.DoNotCall = profile.DoNotCall;
			this.DTTerminalCreate = profile.DTTerminalCreate;
			this.DTTerminalLastModified = profile.DTTerminalLastModified;
			this.DTServerLastModified = DateTime.Now;
			this.Email = profile.Email;
			this.FirstName = profile.FirstName;
			this.Gender = profile.Gender;
			this.LastName = profile.LastName;
			this.LastName2 = profile.LastName2;
			this.MailingAddress1 = profile.MailingAddress1;
			this.MailingAddress2 = profile.MailingAddress2;
			this.MailingAddressDifferent = profile.MailingAddressDifferent;
			this.MailingCity = profile.MailingCity;
			this.MailingState = profile.MailingState;
			this.MailingZipCode = profile.MailingZipCode;
			this.MarketingSMSEnabled = profile.MarketingSMSEnabled;
			this.MiddleName = profile.MiddleName;
			this.MothersMaidenName = profile.MothersMaidenName;
			this.Phone1 = profile.Phone1;
			this.Phone1Provider = profile.Phone1Provider;
			this.Phone1Type = profile.Phone1Type;
			this.Phone2 = profile.Phone2;
			this.Phone2Provider = profile.Phone2Provider;
			this.Phone2Type = profile.Phone2Type;
			this.PIN = profile.PIN;
			this.SMSEnabled = profile.SMSEnabled;
			this.SSN = profile.SSN;
			this.State = profile.State;
			this.TaxpayerId = profile.TaxpayerId;
			this.ZipCode = profile.ZipCode;
			this.ReceiptLanguage = profile.ReceiptLanguage;
			this.ProfileStatus = profile.ProfileStatus;
			this.CountryOfBirth = profile.CountryOfBirth;
			this.Notes = profile.Notes;
			this.ClientID = profile.ClientID;
			this.LegalCode = profile.LegalCode;
			this.PrimaryCountryCitizenShip = profile.PrimaryCountryCitizenShip;
			this.SecondaryCountryCitizenShip = profile.SecondaryCountryCitizenShip;
			this.IDCode = profile.IDCode;
		}

		public virtual void AddOrUpdateGovernmentId(CustomerGovernmentId governmentId)
		{
			if (governmentId != null)
{
				governmentId.Customer = this;

			if (this.GovernmentId != null )
			{
				governmentId.Id = this.GovernmentId.Id;
				governmentId.DTServerLastModified = GovernmentId.Customer.DTTerminalLastModified;
				governmentId.DTServerCreate = this.GovernmentId.DTServerCreate;
			}
			else
				governmentId.DTServerCreate = DateTime.Now;
}
			this.GovernmentId = governmentId;
		}

		public virtual void AddOrUpdateEmployment(CustomerEmploymentDetails employment)
		{
			employment.Customer = this;
            if (this.EmploymentDetails != null)
            {
                employment.Id = this.EmploymentDetails.Id;
                employment.DTServerLastModified = employment.Customer.DTTerminalLastModified;//Need to pass timeZone
                employment.DTServerCreate = this.EmploymentDetails.DTServerCreate;
            }
            else
            {
                employment.DTServerCreate = DateTime.Now;
                employment.DTTerminalCreate = DateTime.Now;
            }
			this.EmploymentDetails = employment;
		}
	}
}
