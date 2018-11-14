using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace TCF.Channel.Zeo.Web.Models
{
	public class MoneyGramSendMoneyDetail : BaseModel
	{
		[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "SendMoneyCouponPromoCode")]
		public string CouponPromoCode { get; set; }

		[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "SendMoneyFirstName")]
		public string FirstName { get; set; }

		[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "SendMoneyMiddleName")]
		public string MiddleName { get; set; }

		[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "SendMoneyLastName")]
		public string LastName { get; set; }

		[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "SendMoneySecondLastName")]
		public string SecondLastName { get; set; }

		public string Country { get; set; }

		public string CountryCode { get; set; }

		public string StateProvince { get; set; }

		public string StateProvinceCode { get; set; }

		[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "SendMoneyAmount")]
		public decimal? Amount { get; set; }

		[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "SendMoneyDestinationAmount")]
		public decimal? DestinationAmount { get; set; }

		[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "SendMoneyDeliveryMethod")]
		[Required(ErrorMessageResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "SendMoneyDeliveryMethodRequired")]
		public string DeliveryMethod { get; set; }

		public bool IsDomesticTransfer { get; set; }

		[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "SendMoneyTransferFee")]
		public decimal TransferFeeWithAllFees { get; set; }
		public decimal TransferFee { get; set; }

		[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "SendMoneyTransferTax")]
		public decimal TransferTax { get; set; }

		[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "SendMoneyExchangeRate")]
		public decimal ExchangeRate { get; set; }

		[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "SendMoneyExchangeRate")]
		public string ExchangeRateConversion { get; set; }

        public string ReceiveAgentAbbreviation { get; set; }

		[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "DoddFrankConfirmationTransferAmount")]
		public decimal TransferAmount { get; set; }

		public long ReceiverID { get; set; }

		[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "SendMoneyPromoDiscount")]
		public decimal PromoDiscount { get; set; }

		[StringLength(15)]
		public string AmountWithCurrency { get; set; }
		public string TransferFeeWithCurrency { get; set; }
		public decimal OriginalFee { get; set; }
		public string PromoDiscountWithCurrency { get; set; }

		[StringLength(15)]
		public string DestinationAmountWithCurrency { get; set; }
		public string TransferAmountWithCurrency { get; set; }
		public string TransferTaxWithCurrency { get; set; }
		public string DestinationAmountWithCurrency1 { get; set; }

		public IEnumerable<SelectListItem> LDeliveryMethods { get; set; }

		public decimal DestinationAmountFromFeeEnquiry { get; set; }

		[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "CurrencyType")]
		public string CurrencyType { get; set; }

		public long TransactionId { get; set; }

		public IEnumerable<SelectListItem> ReceiveCurrencies { get; set; }

		// need to remove
		public string StateName { get; set; }

		public string PromoCode { get; set; }

		public MoneyGramSendMoneyDetail()
		{
			LDeliveryMethods = new List<SelectListItem>() { new SelectListItem() { Text = "Select", Value = string.Empty } };
			ReceiveCurrencies = new List<SelectListItem>() { new SelectListItem() { Text = "Select", Value = string.Empty } };
			ReceiveAgents = new List<SelectListItem>() { new SelectListItem() { Text = "Select", Value = string.Empty } };
		}

		public string PickupState { get; set; }
		public string PickupCountry { get; set; }
		public string ReferenceNo { get; set; }
		public string PersonalMessage { get; set; }

		public IEnumerable<SelectListItem> ReceiveAgents { get; set; }
		public string ReceiveAgent { get; set; }
		public List<FieldViewModel> DynamicFields { get; set; }

		public bool IsReceiveAmount { get; set; }

		[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "SendMoneyOtherFees")]
		public string OtherFees { get; set; }

		[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "SendMoneyOtherTaxes")]
		public string OtherTaxes { get; set; }
	}
}