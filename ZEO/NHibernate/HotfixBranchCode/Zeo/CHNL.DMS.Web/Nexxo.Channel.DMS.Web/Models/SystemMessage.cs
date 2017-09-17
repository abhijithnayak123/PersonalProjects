using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MGI.Channel.DMS.Web.Models
{
    public class SystemMessage : BaseModel
    {
        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "SystemMessageType")]
        public string Type { get; set; }

        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "SystemMessageNumber")]
        public string Number { get; set; }

        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "SystemMessageMessage")]
        public string Message { get; set; }

        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "SystemMessageAddlDetails")]
        public string  AddlDetails { get; set; }
    }
}