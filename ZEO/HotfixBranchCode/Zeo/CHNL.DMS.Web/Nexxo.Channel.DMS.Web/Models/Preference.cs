using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using MGI.Common.Util;
namespace MGI.Channel.DMS.Web.Models
{
    public class Preference
    {
        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "ReceiveTextMessage")]
        public bool ReceiveTextMessage { get; set; }

        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "DoNotCall")]
        public bool DoNotCall { get; set; }

        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "ReceiptLanguage")]
        public string ReceiptLanguage { get; set; }

        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "CustomerProfileStatus")]
		public ProfileStatus CustomerProfileStatus { get; set; }

        public IEnumerable<SelectListItem> LReceiptLanguage { get; set; }

		public ProfileStatus ClientProfileStatus { get; set; }
		public IEnumerable<SelectListItem> LProfileStatus { get; set; }

    }
}