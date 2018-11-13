using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.ServiceModel;

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

    // [AtLeastOneProperty(ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "CustomerSearchAtleastOneAttribute")]    
    [AtLeastOneProperty]
    public class CustomerSearch : BaseModel
    {
        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "FirstName")]
        public string FirstName { get; set; }

        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "LastName")]
        public string LastName { get; set; }

        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "PhoneNumber")]    
        public string PhoneNumber { get; set; }

        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "DateOfBirth")]
        [DateTime(ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "CustomerSearchDateofBirth")]
        [DateTimeNotFuture(CheckFutureDate = true, ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "IdentificationDateOfBirthDateTimeNotFuture")]
        [DataType(DataType.Date)]
       // [DateRange(18, 100, ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "CustomerSearchDateofBirth")]
        public string DateOfBirth { get; set; }

        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "CardNumber")]
		[StringLength(19, MinimumLength = 16,ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName ="CardNumberStringlength")]
        public string CardNumber { get; set; }

        [RegularExpression(@"^[\d *]*$", ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "PersonalCardNumberRegularExpression")]
        public string MaskCardNumber { get; set; }

        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "IDNumber")]
		public string GovernmentId { get; set; }

        public string showIdConfirmedPopUp { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "SSNITIN")]
		[RegularExpression(@"^(?!(000|666))([0-9]\d{2}|7([0-9]\d|7[012]))-?(?!00)\d{2}-\d{4}$", ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "IdentificationSSNITINRegularExpression")]
        public string SSN { get; set; }

        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "SearchClosedAccounts")]
        public bool IsIncludeClosed { get; set; }

		public string SearchType { get; set; }
		public string CVV { get; set; }
          
        public CustomerSearch()
        {
            this.CardNumber = null;
            this.CardLength = 16;
        }
    }
}