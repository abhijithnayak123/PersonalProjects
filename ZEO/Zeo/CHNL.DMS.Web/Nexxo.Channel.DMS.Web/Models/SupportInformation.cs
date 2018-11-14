using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ZeoClient = TCF.Channel.Zeo.Web.ServiceClient.ZeoService;

namespace TCF.Channel.Zeo.Web.Models
{
    public class SupportInformation
    {
        public string Name { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "Email")]
        public string EmailId { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "Phone")]
        public string Phone1 { get; set; }

        public string Phone2 { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "ProviderContactDetails")]
        public List<ZeoClient.KeyValuePair> ProviderContactDetails { get; set; }
    }
}