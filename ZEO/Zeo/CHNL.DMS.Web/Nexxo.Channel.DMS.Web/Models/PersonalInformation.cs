using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using TCF.Zeo.Common.Util;

#region AO
using ZeoClient = TCF.Channel.Zeo.Web.ServiceClient.ZeoService;
#endregion


namespace TCF.Channel.Zeo.Web.Models
{
    /// <summary>
    /// AO - Model for Personal information screen.
    /// </summary>
    public class PersonalInformation : BaseModel
    {
        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "SSNITIN")]
        public string SSN { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "DateOfBirth")]
        public string DateOfBirth { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "LastName")]
        public string LastName { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "SecondLastName")]
        public string SecondLastName { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "FirstName")]
        public string FirstName { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "MiddleName")]
        public string MiddleName { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "Gender")]
        public string Gender { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "PrimaryPhone")]
        public string PrimaryPhone { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "PrimaryPhoneType")]
        public string PrimaryPhoneType { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "PrimaryPhoneProvider")]
        public string PrimaryPhoneProvider { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "SecondaryPhone")]
        public string AlternativePhone { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "SecondaryPhoneType")]
        public string AlternativePhoneType { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "SecondaryPhoneProvider")]
        public string AlternativePhoneProvider { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "Email")]
        public string Email { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "Address1")]
        public string Address1 { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "Address2")]
        public string Address2 { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "City")]
        public string City { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "State")]
        public string State { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "ZipCode")]
        public string ZipCode { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "MailingAddressDifferent")]
        public bool MailingAddressDifferent { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "MailingAddress1")]
        public string MailingAddress1 { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "MailingAddress2")]
        public string MailingAddress2 { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "MailingCity")]
        public string MailingCity { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "MailingState")]
        public string MailingState { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "MailingZipCode")]
        public string MailingZipCode { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "ReceiveTextMessage")]
        public bool ReceiveTextMessage { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "DoNotCall")]
        public bool DoNotCall { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "ReceiptLanguage")]
        public string ReceiptLanguage { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "CustomerProfileStatus")]
        public Helper.ProfileStatus CustomerProfileStatus { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "Group1")]
        public string Group1 { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "Group2")]
        public string Group2 { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "Notes")]
        public string Notes { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "ReferralNumber")]
        public string ReferralNumber { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "WoodForestAccountHolder")]
        public bool WoodForestAccountHolder { get; set; }

        //TODO: Check do we need this.
        public Helper.ProfileStatus ClientProfileStatus { get; set; }

        //TODO: Check do we need this.
        public string ActualSSN { get; set; }

        //TODO: Check do we need this.
        public bool IsCompanionSearch { get; set; }

        public string CustomerMinimumAgeMessage { get; set; }

        public bool IsAutoSearchRequired { get; set; }

        public IEnumerable<SelectListItem> LPrimaryPhonetype { get; set; }
        public IEnumerable<SelectListItem> LPrimaryPhoneProvider { get; set; }
        public IEnumerable<SelectListItem> LStates { get; set; }
        public IEnumerable<SelectListItem> LAlternatePhonetype { get; set; }
        public IEnumerable<SelectListItem> LAlternatePhoneProvider { get; set; }
        public IEnumerable<SelectListItem> LProfileStatus { get; set; }
        public IEnumerable<SelectListItem> LGroup1 { get; set; }
        public IEnumerable<SelectListItem> LGroup2 { get; set; }
        public IEnumerable<SelectListItem> LReceiptLanguage { get; set; }
    }
}