using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.ServiceModel;
using static TCF.Zeo.Common.Util.Helper;

namespace TCF.Channel.Zeo.Web.Models
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
 
    [AtLeastOneProperty]
    public class CustomerSearch : BaseModel
    {
        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "CardNumber")]
        [StringLength(19, MinimumLength = 16, ErrorMessageResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "CardNumberStringlength")]
        public string CardNumber { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "DateOfBirth")]
        [DateTime(ErrorMessageResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "CustomerSearchDateofBirth")]
        [DateTimeNotFuture(CheckFutureDate = true, ErrorMessageResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "IdentificationDateOfBirthDateTimeNotFuture")]
        [DataType(DataType.Date)]
        public string DateOfBirth { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "SSNITIN")]
        public string SSN { get; set; }

        public string ActualSSN { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "LastName")]
        public string LastName { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "TCFAccountNumber")]
        public string AccountNumber { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "MaskCardNumber")]
        public string MaskCardNumber { get; set; }
        
		public string SearchType { get; set; }

		public string CVV { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "DateOfBirth")]
        [DateTime(ErrorMessageResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "CustomerSearchDateofBirth")]
        [DateTimeNotFuture(CheckFutureDate = true, ErrorMessageResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "IdentificationDateOfBirthDateTimeNotFuture")]
        [DataType(DataType.Date)]
        public string TCFCheckDateOfBirth { get; set; }

        public bool IsZeoCard { get; set; } // Later we need to change card type to enum

        public CustomerSearch()
        {
            this.CardNumber = null;
            this.CardLength = 16;
        }
    }
}