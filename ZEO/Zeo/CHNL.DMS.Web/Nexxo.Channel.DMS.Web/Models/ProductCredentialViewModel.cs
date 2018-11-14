using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace TCF.Channel.Zeo.Web.Models
{
	public class ProductCredentialViewModel : BaseModel
	{
		[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "ProductCredentialCustomerName")]
		public string Name { get; set; }

		[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "PrepaidCardNumber")]
		[Required(ErrorMessageResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "ProductCredentialCardNumberRequired")]
		[StringLength(19, ErrorMessageResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "ProductCredentialCardNumberRequired", MinimumLength = 16)]
		public string CardNumber { get; set; }

		[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "ProductCredentialAccountNumber")]
		[Required(ErrorMessageResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "ProductCredentialAccountNumberRequired")]
		[StringLength(10, ErrorMessageResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "ProductCredentialAccountNumberRequired", MinimumLength = 10)]
		public string AccountNumber { get; set; }

		[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "ProductCredentialActivationFee")]
		public decimal ActivationFee { get; set; }

		[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "ProductCredentialFraudScore")]
		public int FraudScore { get; set; }

		[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "ProductCredentialResolution")]
		public string Resolution { get; set; }

		public IEnumerable<SelectListItem> Resolutions { get; set; }

		public IEnumerable<SelectListItem> FraudScores { get; set; }

		public bool HasGPRCard { get; set; }
		[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "BaseFee")]
		public decimal BaseFee { get; set; }
		[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "DiscountApplied")]
		public decimal DiscountApplied { get; set; }
		[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "NetFee")]
		public decimal NetFee { get; set; }
		[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "DiscountName")]
		public string DiscountName { get; set; }

		public string BaseFeeWithCurrency { get; set; }
		public string DiscountAppliedWithCurrency { get; set; }
		public string NetFeeWithCurrency { get; set; }

		[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "ProxyId")]
		[Required(ErrorMessageResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "ProxyIdRequired")]
		[RegularExpression(@"^[0-9]{8,19}$", ErrorMessageResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "ProxyIdRegularExpressionMessage")]
		public string ProxyId { set; get; }

		[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "PseudoDDA")]
		[Required(ErrorMessageResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "PseudoDDARequired")]
		[RegularExpression(@"^[0-9]{15,25}$", ErrorMessageResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "PseudoDDARegularExpressionMessage")]
		public string PseudoDDA { get; set; }

		[RegularExpression(@"^(1[0-2]|0[1-9]|\d)\/(20\d{2}|19\d{2}|0(?!0)\d|[1-9]\d)$", ErrorMessageResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "ExpirationDateFormat")]
		[DateTimeWithFutureAttribute(ErrorMessageResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "ExpireDateDateTimeWithFuture")]
        [Required(ErrorMessageResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "ExpirationDateRequired")]
        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "ExpirationDate")]
		public string ExpirationDate { get; set; }

		[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "InitialLoad")]
		[DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
		[Range(typeof(Decimal), "25.00", "1000.00")]
		public decimal InitialLoad { get; set; }

		[DataType(DataType.Currency)]
		[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "PurchaseFee")]
		public string PurchaseFee { get; set; }

		[Required(ErrorMessageResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "ProductCredentialCardNumberRequired")]
		[StringLength(19, MinimumLength = 16, ErrorMessageResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "CardNumberStringlength")]
		[RegularExpression(@"^[\d *]*$", ErrorMessageResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "PersonalCardNumberConditionalCompare")]
		public string MaskCardNumber { get; set; }

		public string CVV { get; set; }
		
		public long TransactionId { get; set; }

        public long PromotionId { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "BillPayeeCouponPromoCode")]
		[StringLength(50, ErrorMessageResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "PrepaidPurchasePromotionCodeLength", MinimumLength = 0)]
		[RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessageResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "PrepaidPurchasePromotionCodeRegex")]
		public string PromoCode { get; set; }
	}
}

