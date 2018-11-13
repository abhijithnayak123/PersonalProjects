using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MGI.Channel.DMS.Web.Models
{
    /// <summary>
    /// Something about what the <c>MySomeFunction</c> does
    /// with some of the sample like
    /// <code>
    /// Some more code statement to comment it better
    /// </code>
    /// For more information seee <see cref="http://www.me.com"/>
    /// </summary>
    /// <param name="someObj">What the input to the function is</param>
    /// <returns>What it returns</returns>
    public class PinDetails : BaseModel
    {
        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "PhoneNumber")]
        public string PhoneNumber { get; set; }

        //[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "CardNumber")]
        //public string CardNumber { get; set; }

        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "Pin")]
        //[RegularExpression(@"(^\d{4}$)",ErrorMessage="Please enter 4 Digit PIN Number.")]
        ////[Required(ErrorMessage="Please enter PIN Number.")]
        [DataType(DataType.Password)]
        public string Pin { get; set; }

        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "ReEnter")]
        [DataType(DataType.Password)]
        public string ReEnter { get; set; }
    }
}