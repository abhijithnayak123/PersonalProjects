using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using TCF.Zeo.Common.Util;

namespace TCF.Channel.Zeo.Web.Models
{
    public class Preference
    {
        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "ReceiveTextMessage")]
        public bool ReceiveTextMessage { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "DoNotCall")]
        public bool DoNotCall { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "ReceiptLanguage")]
        public string ReceiptLanguage { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "CustomerProfileStatus")]
		public Helper.ProfileStatus CustomerProfileStatus { get; set; }

        public IEnumerable<SelectListItem> LReceiptLanguage { get; set; }

		public Helper.ProfileStatus ClientProfileStatus { get; set; }
		public IEnumerable<SelectListItem> LProfileStatus { get; set; }

    }
}