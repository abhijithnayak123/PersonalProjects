using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace TCF.Channel.Zeo.Web.Models
{
    public class ContactDetail
    {
        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "PrimaryPhone")]
        public string PrimaryPhone { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "PrimaryPhoneType")]
        public string PrimaryPhoneType { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "PrimaryPhoneProvider")]
        public string PrimaryPhoneProvider { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "AlternativePhone")]
        public string AlternativePhone { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "AlternativePhoneType")]
        public string AlternativePhoneType { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "AlternativePhoneProvider")]
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

        public IEnumerable<SelectListItem> LPrimaryPhonetype { get; set; }
        public IEnumerable<SelectListItem> LPrimaryPhoneProvider { get; set; }
        public IEnumerable<SelectListItem> LStates { get; set; }
        public IEnumerable<SelectListItem> LAlternatePhonetype { get; set; }
        public IEnumerable<SelectListItem> LAlternatePhoneProvider { get; set; }

        public ContactDetail()
        {

        }
    }
}
