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
    public class PersonalInformation : BaseModel
    {
        public PersonalDetail personalDetail { get; set; }
        public ContactDetail contactDetail { get; set; }
        public Preference Preference { get; set; }

        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "ReferralNumber")]
        public string ReferralNumber { get; set; }

        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "WoodForestAccountHolder")]
        public bool WoodForestAccountHolder { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "Group1")]
		public string Group1 { get; set; }
		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "Group1")]
		public string Group2 { get; set; }
		public string Notes { get; set; }
		public bool IsCompanionSearch { get; set; }

		public IEnumerable<SelectListItem> LGroup1 { get; set; }
		public IEnumerable<SelectListItem> LGroup2 { get; set; }

        public PersonalInformation()
        {
            personalDetail = new PersonalDetail();
            contactDetail = new ContactDetail();
            Preference = new Preference();
        }
    }
}