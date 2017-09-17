using System;
using System.Runtime.Serialization;
using System.Text;

#region Zeo References
using TCF.Zeo.Common.Util;
using TCF.Zeo.Common.Data;
#endregion

namespace TCF.Channel.Zeo.Data
{
    [DataContract]
    public class CustomerProfile : BaseRequest
    {
        public CustomerProfile()
        {
        }

        //Personal Information Page
        [DataMember]
        public string SSN { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string MiddleName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string LastName2 { get; set; }
        [DataMember]
        public Helper.Gender? Gender { get; set; }
        [DataMember]
        public Phone Phone1 { get; set; }
        [DataMember]
        public Phone Phone2 { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public Address Address { get; set; }
        [DataMember]
        public bool MailingAddressDifferent { get; set; }
        [DataMember]
        public Address MailingAddress { get; set; }
        [DataMember]
        public string ReferralCode { get; set; }
        [DataMember]
        public bool IsAccountHolder { get; set; }
        [DataMember]
        public bool TextMsgOptIn { get; set; }
        [DataMember]
        public bool DoNotCall { get; set; }
        [DataMember]
        public Helper.ProfileStatus ProfileStatus { get; set; }
        [DataMember]
        public Helper.ProfileStatus ClientProfileStatus { get; set; }
        [DataMember]
        public string Group1 { get; set; }
        [DataMember]
        public string Group2 { get; set; }
        [DataMember]
        public string Notes { get; set; }

        //Identification Screen

        [DataMember]
        public string MothersMaidenName { get; set; }
        [DataMember]
        public string CountryOfBirth { get; set; }
        [DataMember]
        public string PrimaryCountryCitizenShip { get; set; }
        [DataMember]
        public string SecondaryCountryCitizenShip { get; set; }
        [DataMember]
        public string LegalCode { get; set; }
        [DataMember]
        public DateTime? DateOfBirth { get; set; }
        [DataMember]
        public string IdIssuingCountry { get; set; }
        [DataMember]
        public string IdType { get; set; } //TODO:AO - Change this to IdType
        [DataMember]
        public string IdIssuingState { get; set; }
		[DataMember]
        public string IdIssuingStateAbbr { get; set; }
        [DataMember]
        public string IdNumber { get; set; } //TODO:AO- CXN has GovernmentId for this and it has to change
        [DataMember]
        public DateTime? IdIssueDate { get; set; }
        [DataMember]
        public DateTime? IdExpirationDate { get; set; }
        //This Field is added for Get NexxoIDType Name for Get Customer Details for Kiosk
        [DataMember]
        public string IDTypeName { get; set; } //TODO:AO - Do we need this?
        [DataMember]
        public string CustomerId { get; set; }
        [DataMember]
        public string ClientCustomerId { get; set; } //TODO-AO, PartnerAccountNumber in CXN, and it need to change as ClientCustomerId

        //Employment Screen
        [DataMember]
        public string EmployerName { get; set; }
        [DataMember]
        public string EmployerPhone { get; set; }
        [DataMember]
        public string Occupation { get; set; }
        [DataMember]
        public string OccupationDescription { get; set; } // This is hidden and displayed based on channel partner

        //PIN Sscreen
        [DataMember]
        public string PIN { get; set; }

        //Other properties not displayed in screen
        //IDCode is used along with SSN property to determine, where it is SS or ITIN.
        [DataMember]
        public Helper.TaxIDCode? IDCode { get; set; }
        [DataMember]
        public string ReceiptLanguage { get; set; }


        //TODO:AO-Do we need this elsewhere?
        [DataMember]
        public string CardNumber { get; set; }


        //Synovus Specific???
        [DataMember]
        public string ProgramId { get; set; }

        [DataMember]
        public bool SMSEnabled { get; set; }

        [DataMember]
        public Helper.CustomerType CustomerType { get; set; }

        //TODO:AO
        //AL-550, don't think we will need this, as we will store customer profile info in memory until registration.
        //[DataMember]
        //public CustomerScreen CustomerScreen { get; set; }


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("CustomerProfile:");
            return sb.ToString();

        }
    }
}
