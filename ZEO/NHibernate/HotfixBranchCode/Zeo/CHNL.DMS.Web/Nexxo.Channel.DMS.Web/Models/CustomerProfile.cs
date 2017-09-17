using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.ServiceModel;
using MGI.Channel.DMS.Server.Data;

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
   
    public class CustomerProfile : BaseModel
    {
        //[ConditionalCompareRequired("CardNumber", "AuthenticationType", "CardLength", ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "PersonalCardNumberConditionalCompare")]
        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "CardNumber")]
        [RegularExpression(@"^\d*$", ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "PersonalCardNumberRegularExpression")]
        public string CardNumber { get; set; }

        public string Address { get; set; }

        public string Address1 { get; set; }
        public string City { get; set; }
        public string StateZipCode { get; set; }

        public string MaskedCardNumber { get; set; }

        public string PhoneNumber { get; set; }
        public string PhoneType { get; set; }

        public CardBalance CardBalance { get; set; }
    }
}