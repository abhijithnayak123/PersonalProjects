using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace TCF.Channel.Zeo.Web.Models
{
    /// <summary>
    /// AO - Model for Identification information screen
    /// </summary>
    public class IdentificationInformation : BaseModel
    {
        public IdentificationInformation()
        {
			LCountry = DefaultListItems();
			LStates = DefaultListItems();
			LGovtIDType = DefaultListItems();
			LCountryOfBirth = DefaultListItems();
			LLegalCodes = DefaultListItems();
			LPrimaryCountryCitizenship = DefaultListItems();
			LSecondaryCountryCitizenship = DefaultListItems(); 
        }
        
        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "MotherMaidenName")]
        public string MotherMaidenName { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "CountryOfBirth")]
        public string CountryOfBirth { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "PrimaryCountryCitizenShip")]
        public string PrimaryCountryCitizenShip { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "SecondaryCountryCitizenShip")]
        public string SecondaryCountryCitizenShip { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "LegalCode")]
        public string LegalCode { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "Country")]
        public string Country { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "GovtIDType")]
        public string GovtIDType { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "GovtIdIssueState")]
        public string GovtIdIssueState { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "IDNumber")]
        public string GovernmentId { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "IDIssuedDate")]
        public string IDIssuedDate { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "IDExpireDate")]
        public string IDExpireDate { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "ClientID")]
        [StringLength(13)]
        public string ClientID { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "MGiAlloyID")]
        public string MGIAlloyID { get; set; }

        //AO- TODO : Need to check whether these two properties are really needed here.
        public bool IsCompanionSearch { get; set; }

        public IEnumerable<SelectListItem> LCountry { get; set; }
        public IEnumerable<SelectListItem> LStates { get; set; }
        public IEnumerable<SelectListItem> LGovtIDType { get; set; }
        public IEnumerable<SelectListItem> LCountryOfBirth { get; set; }
        public IEnumerable<SelectListItem> LLegalCodes { get; set; }
        public IEnumerable<SelectListItem> LPrimaryCountryCitizenship { get; set; }
        public IEnumerable<SelectListItem> LSecondaryCountryCitizenship { get; set; }

        private List<SelectListItem> DefaultListItems()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem() { Value = string.Empty, Text = "Select", Selected = true });

            return list;
        }


    }
}   