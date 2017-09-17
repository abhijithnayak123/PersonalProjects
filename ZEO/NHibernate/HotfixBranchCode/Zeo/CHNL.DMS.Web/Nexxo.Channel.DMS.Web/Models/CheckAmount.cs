using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
namespace MGI.Channel.DMS.Web.Models
{
    public class CheckAmount : CheckDetails
    {
        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "ReEnterCheckAmount")]
        [Required(ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "ReEnterCheckAmountRequired")]
        [RegularExpression(@"^[0-9]*(\.[0-9]{2})?$", ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "CheckAmountCashRegularExpression")]
        [Range(typeof(Decimal), "0.01", "79228162514264337593543950335", ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "CheckAmountCashRegularExpression")]
        [Compare("CheckAmount", ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "ReEnterCheckAmountValidation")]
        public decimal ReEnterCheckAmount { get; set; }

        [Required(ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "InvalidCheckFee")]
        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "CheckEstablishmentFee")]
        public string CheckEstablishmentFee { get; set; }

        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "CheckDate")]
        [Required(ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "InvalidDate")]
        [DateTimeNotFuture(CheckFutureDate = true, ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "CheckDateCannorBeFutureDate")]
		[DateTime(ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "CheckDateFormat")]
        public string CheckDate { get; set; }

        public decimal NetAmount { get; set; }

        [Required(ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "PickCheckCategory")]
        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "CheckCheckType")]
        public string CheckType { get; set; }

        public IEnumerable<SelectListItem> LCheckTypes { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "EstimatedBaseFee")]
		public decimal BaseFee { get; set; }
		public string BaseFeeWithCurrency { get; set; }      
		public string DiscountAppliedWithCurrency { get; set; }
		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "EstimatedNetFee")]
		public decimal NetFee { get; set; }
		public string NetFeeWithCurrency { get; set; }
    }
}