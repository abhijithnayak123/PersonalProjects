using System.ComponentModel.DataAnnotations;

namespace TCF.Channel.Zeo.Web.Models
{
    /// <summary>
    /// Something about what the <c>MySomeFunction</c> does
    /// with some of the sample like
    /// <code>
    /// Some more code statement to comment it better
    /// </code>
    /// For more information seee <see cref="http://www.me.com"/>
    /// </summary>
    /// <param name="someObj">What the input to the function is</param>
    /// <returns>What it returns</returns>

    public class CustomerProfile : BaseModel
    {
        //[ConditionalCompareRequired("CardNumber", "AuthenticationType", "CardLength", ErrorMessageResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "PersonalCardNumberConditionalCompare")]
        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "CardNumber")]
        [RegularExpression(@"^\d*$", ErrorMessageResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "PersonalCardNumberRegularExpression")]
        public string CardNumber { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public long AlloyID { get; set; }

        public string Address { get; set; }

        public string Address1 { get; set; }
        public string City { get; set; }
        public string StateZipCode { get; set; }

        public string MaskedCardNumber { get; set; }

        public string PhoneNumber { get; set; }
        public string PhoneType { get; set; }

        public CardBalance CardBalance { get; set; }

        //Personal Information Page
        //public string SSN { get; set; }

        //public string FirstName { get; set; }

        //public string MiddleName { get; set; }

        //public string LastName { get; set; }

        //public string LastName2 { get; set; }

        //public Gender Gender { get; set; }

        //public Phone Phone1 { get; set; }

        //public Phone Phone2 { get; set; }

        //public string Email { get; set; }

        //public Address Address { get; set; }

        //public bool MailingAddressDifferent { get; set; }

        //public Address MailingAddress { get; set; }

        //public string ReferralCode { get; set; }

        //public bool IsAccountHolder { get; set; }

        //public bool TextMsgOptIn { get; set; }

        //public bool DoNotCall { get; set; }

        //public ProfileStatus ProfileStatus { get; set; }

        //public string Group1 { get; set; }

        //public string Group2 { get; set; }

        //public string Notes { get; set; }

        ////Identification Screen


        //public string MothersMaidenName { get; set; }

        //public string CountryOfBirth { get; set; }

        //public string PrimaryCountryCitizenShip { get; set; }

        //public string SecondaryCountryCitizenShip { get; set; }

        //public string LegalCode { get; set; }

        //public DateTime? DateOfBirth { get; set; }

        //public string IdIssuingCountry { get; set; }

        //public string IdType { get; set; } //TODO:AO - Change this to IdType

        //public string IdIssuingState { get; set; }

        //public string IdNumber { get; set; } //TODO:AO- CXN has GovernmentId for this and it has to change

        //public DateTime? IdIssueDate { get; set; }

        //public DateTime? IdExpirationDate { get; set; }
        ////This Field is added for Get NexxoIDType Name for Get Customer Details for Kiosk

        //public string IDTypeName { get; set; } //TODO:AO - Do we need this?

        //public string CustomerId { get; set; }

        //public string ClientCustomerId { get; set; } //TODO-AO, PartnerAccountNumber in CXN, and it need to change as ClientCustomerId

        ////Employment Screen

        //public string EmployerName { get; set; }

        //public string EmployerPhone { get; set; }

        //public string Occupation { get; set; }

        //public string OccupationDescription { get; set; } // This is hidden and displayed based on channel partner

        ////PIN Sscreen

        //public string PIN { get; set; }

        ////Other properties not displayed in screen
        ////IDCode is used along with SSN property to determine, where it is SS or ITIN.

        //public Helper.TaxIDCode? IDCode { get; set; }

        //public string ReceiptLanguage { get; set; }


        ////TODO:AO-Do we need this elsewhere?

        //public string CardNumber { get; set; }


        ////Synovus Specific???

        //public string ProgramId { get; set; }


        //public bool SMSEnabled { get; set; }

        //TODO:AO
        //AL-550, don't think we will need this, as we will store customer profile info in memory until registration.
        //
        //public CustomerScreen CustomerScreen { get; set; }

    }

    //public class Phone
    //{
    //    public string Number { get; set; }
    //    public string Type { get; set; }
    //    public string Provider { get; set; }
    //}

    //public class Address
    //{
    //    public string Address1 { get; set; }
    //    public string Address2 { get; set; }
    //    public string City { get; set; }
    //    public string State { get; set; }
    //    public string ZipCode { get; set; }
    //    public string Country { get; set; }
    //}

    //public enum Gender
    //{
    //    MALE = 1,
    //    FEMALE = 2
    //}

    //public enum TaxIDCode
    //{
    //    S = 1,
    //    I
    //}

    //public enum ProfileStatus
    //{
    //    Active = 1,
    //    Inactive = 0,
    //    Closed = 2
    //}
}