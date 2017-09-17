using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TCF.Channel.Zeo.Web.Models
{
    public class PinDetails : BaseModel
    {
        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "PhoneNumber")]
        public string PhoneNumber { get; set; }

        //[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "CardNumber")]
        //public string CardNumber { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "Pin")]
        //[RegularExpression(@"(^\d{4}$)",ErrorMessage="Please enter 4 Digit PIN Number.")]
        ////[Required(ErrorMessage="Please enter PIN Number.")]
        [DataType(DataType.Password)]
        public string Pin { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "ReEnter")]
        [DataType(DataType.Password)]
        public string ReEnter { get; set; }
    }
}