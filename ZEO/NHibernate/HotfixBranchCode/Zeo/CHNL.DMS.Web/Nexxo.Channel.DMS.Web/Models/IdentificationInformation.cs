using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MGI.Channel.DMS.Web.Models
{
    /// <summary>
    /// Model for Identification information screen
    /// </summary>
    public class IdentificationInformation : BaseModel
    {
        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "MotherMaidenName")]
        public string MotherMaidenName { get; set; }

        private string _dob;
        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "DateOfBirth")]
        public string DateOfBirth {
            get { return string.IsNullOrWhiteSpace(_dob) ? null : Convert.ToDateTime(_dob).ToString("MM/dd/yyyy"); }
            set { _dob = value; }
        }

        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "Country")]
        public string Country { get; set; }

        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "GovtIDType")]
        public string GovtIDType { get; set; }

        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "GovtIdIssueState")]
        public string GovtIdIssueState { get; set; }

        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "IDNumber")]
		public string GovernmentId { get; set; }

        private string _iDIssuedDate;
        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "IDIssuedDate")]
        public string IDIssuedDate {
			get { return string.IsNullOrWhiteSpace(_iDIssuedDate) ? null : (Convert.ToDateTime(_iDIssuedDate) == DateTime.MinValue ? null : Convert.ToDateTime(_iDIssuedDate).ToString("MM/dd/yyyy")); }
            set { _iDIssuedDate = value; }
        }

        private string _iDExpireDate;
        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "IDExpireDate")]
        public string IDExpireDate {
            get { return string.IsNullOrWhiteSpace(_iDExpireDate) || Convert.ToDateTime(_iDExpireDate) == DateTime.MinValue ? null : Convert.ToDateTime(_iDExpireDate).ToString("MM/dd/yyyy"); }
            set { _iDExpireDate = value; }
        }

        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "CountryOfBirth")]
        public string CountryOfBirth { get; set; }

		//Added By Abhijith for User Story - #US2157
		//Starts Here
		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "ClientID")]
		[StringLength(13)]
		public string ClientID { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "LegalCode")]
		public string LegalCode { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "PrimaryCountryCitizenShip")]
		public string PrimaryCountryCitizenShip { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "SecondaryCountryCitizenShip")]
		public string SecondaryCountryCitizenShip { get; set; }
		//Ends Here
		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "MGiAlloyID")]
		public string MGIAlloyID { get; set; }

		//AL-1626
		public string CustomerMinimumAgeMessage { get; set; }

        public IEnumerable<SelectListItem> LCountry { get; set; }
        public IEnumerable<SelectListItem> LStates { get; set; }
        public IEnumerable<SelectListItem> LGovtIDType { get; set; }
        public IEnumerable<SelectListItem> LCountryOfBirth { get; set; }
		public IEnumerable<SelectListItem> LLegalCodes { get; set; }
		public IEnumerable<SelectListItem> LPrimaryCountryCitizenship { get; set; }
		public IEnumerable<SelectListItem> LSecondaryCountryCitizenship { get; set; } 
    }
}   