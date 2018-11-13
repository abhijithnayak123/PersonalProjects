using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MGI.Channel.DMS.Web.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class CustomerLookUp : BaseModel
    {

        //ALL THE VALIDATIONS ARE IN THE CUSTOMERLOOKUP JS FILE CORRESPONDING TO THE CHANNEL PARTNER

        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "SSNITIN")]
        public string SSN { get; set; }
        public string ActualSSN { get; set; }

        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "LastName")]
        public string LastName { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "DateOfBirth")]
		public string DateOfBirth { get; set; }

        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "PhoneNumber")]
        public string PrimaryPhone { get; set; }

        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "ZipCode")]
        public string ZipCode { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "AccountNo")]
		public string AccountNumber { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "CardNo")]
		[StringLength(19, ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "ProductCredentialCardNumberRequired", MinimumLength = 16)]
		public string CardNumber { get; set; }

		[RegularExpression(@"^[\d *]*$", ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "PersonalCardNumberRegularExpression")]
		public string MaskCardNumber { get; set; }

        public string ActualCardNumber { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "Appl")]
		public string AccountType { get; set; }
		public string CVV { get; set; }

		//AL-1626
		public string CustomerMinimumAgeMessage { get; set; }

		public bool IsCompanionSearch { get; set; }
    }
}