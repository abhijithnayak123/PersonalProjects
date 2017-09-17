using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.ComponentModel;


namespace TCF.Channel.Zeo.Web.Models
{
    public class CheckDetails : BaseModel
    {
		public CheckDetails()
		{
			IsSystemApplied = true;
		}

        public string CheckFrontImage { get; set; }
        public string CheckBackImage { get; set; }
        public string MICRCode { get; set; }
        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "SendMoneyAmount")]
        [Required(ErrorMessageResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "CheckAmountCashAmtRequired")]
        [RegularExpression(@"^[0-9]*(\.[0-9]{2})?$", ErrorMessageResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "CheckAmountCashRegularExpression")]
        [Range(typeof(Decimal), "0.01", "79228162514264337593543950335", ErrorMessageResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "CheckAmountCashRegularExpression")]
        public decimal CheckAmount { get; set; }
        public decimal CheckLimit { get; set; }
        public string CheckId { get; set; }        
        public DateTime CheckSubmissionDate { get; set; }

        public string NpsId { get; set; }

		public string CheckFrontImage_TIFF { get; set; }
		public string CheckBackImage_TIFF { get; set; }
		public string MicrErrorMessage { get; set; }
		[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "PromotionCode")]
		[StringLength(50, ErrorMessageResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "PromotionCodeLength", MinimumLength = 0)]
		[RegularExpression(@"^[a-zA-Z0-9%$\s]*$", ErrorMessageResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "PromotionCodeRegex")]
		public string PromotionCode { get; set; }
		public bool IsSystemApplied { get; set; }

		//AL-437 changes
		public string RoutingNumber { get; set; }
		public string ConfirmRoutingNumber { get; set; }
		public string AccountNumber { get; set; }
		public string ConfirmAccountNumber { get; set; }
		public string CheckNumber { get; set; }
		public string MicrEntryType { get; set; }
        //AL-1090 changes
        public int MicrError { get; set; }
        public bool IsDisabled { get; set; }
 		//Al-3032 Changes
        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "DiscountName")]
        public string DiscountName { get; set; }
        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "DiscountApplied")]
        public decimal DiscountApplied { get; set; }
	}
}