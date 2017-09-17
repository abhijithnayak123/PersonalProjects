using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MGI.Channel.DMS.Web.Models
{
    public class MoneyOrderSetup : BaseModel
    {
        /// <summary>
        /// Gets or sets the Amount
        /// </summary>
        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "MoneyOrderAmount")]
        [Required(ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "MoneyOrderAmountRequired")]
        [RegularExpression(@"^[0-9]*(\.[0-9]{0,2})?$", ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "MoneyOrderAmountRegularExpression")]
        public decimal Amount { get; set; }

        /// <summary>
        /// Gets or sets the Fee
        /// </summary>
        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "MoneyOrderFee")]
        public decimal Fee { get; set; }

        /// <summary>
        /// Gets or sets the Total
        /// </summary>
        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "MoneyOrderTotal")]
        public decimal Total { get; set; }

        /// <summary>
        /// Gets or sets the StatusDescription
        /// </summary>
		public string StatusDescription { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "BaseFee")]
		public decimal BaseFee { get; set; }
		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "DiscountApplied")]
		public decimal DiscountApplied { get; set; }
		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "NetFee")]
		public decimal NetFee { get; set; }
		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "DiscountName")]
		public string DiscountName { get; set; }

        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "PromotionCode")]
		[StringLength(50, ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "PromotionCodeLength", MinimumLength = 0)]
		[RegularExpression(@"^[a-zA-Z0-9%$\s]*$", ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "PromotionCodeRegex")]
        public string PromotionCode { get; set; }

		public string BaseFeeWithCurrency { get; set; }
		public string DiscountAppliedWithCurrency { get; set; }
		public string NetFeeWithCurrency { get; set; }
		public bool IsSystemApplied { get; set; }
    }
}