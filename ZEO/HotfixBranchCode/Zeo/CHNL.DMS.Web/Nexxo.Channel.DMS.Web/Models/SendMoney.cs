using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.ServiceModel;
using MGI.Channel.DMS.Server.Data;
using MGI.Channel.Shared.Server.Data;


namespace MGI.Channel.DMS.Web.Models
{
	public class SendMoney : BaseModel
	{


		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "SendMoneyCouponPromoCode")]
		[CouponPromoAlias("CouponPromoCode")]
		public string CouponPromoCode { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "SendMoneyFirstName")]
		public string FirstName { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "SendMoneyLastName")]
		public string LastName { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "SendMoneySecondLastName")]
		public string SecondLastName { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "SendMoneyCountry")]

		[Required(ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "SendMoneyCountryRequired")]
		public string Country { get; set; }


		public string CountryCode { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "SendMoneyPickUpStateProvince")]
		public string StateProvince { get; set; }

		[ConditionalRequired("CountryCode", "StateProvinceCode", ErrorMessage = "Please select State / Province.")]
		public string StateProvinceCode { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "SendMoneyCity")]
		public string City { get; set; }

		[ConditionalRequired("CountryCode", "CityID", ErrorMessage = "Please select city.")]
		public string CityID { get; set; }

		public string CityName { get; set; }

		public string ReceiverCityName { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "SendMoneyAmount")]
		[RegularExpression(@"^[0-9]*((\.[0-9]{2})|(\.[0-9]{1}))?$", ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "BillAmountCashRegularExpression")]
		[Range(typeof(Decimal), "0.01", "99999.99", ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "SendMoneyAmountRegularExpression")]
		public decimal? Amount { get; set; }

		[RegularExpression(@"^[0-9]*((\.[0-9]{2})|(\.[0-9]{1}))?$", ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "BillAmountCashRegularExpression")]
		[Range(typeof(Decimal), "0.01", "999999999.99", ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "SendMoneyAmountRegularExpression")]
		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "SendMoneyDestinationAmount")]
		public decimal? DestinationAmount { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "SendMoneyDeliveryMethod")]
		[Required(ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "SendMoneyDeliveryMethodRequired")]
		public string DeliveryMethod { get; set; }

		// This has been added for User Story # US1956 - Start

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "ActOnMyBehalf")]
		public string ActOnMyBehalf { get; set; }

		// This has been added for User Story # US1956 - Complete

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "SendMoneyDeliveryOptions")]
		public string DeliveryOptions { get; set; }

		public string ReceiverName { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "SendMoneyTestQuestion")]
		[ConditionalTestQuestion("TestQuestionOption", ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "SendMoneyTestQuestionRequired")]
		[ConditionalRequired("TestAnswer", "TestQuestion", ErrorMessage = "Please enter test answer.")]
		[StringLength(29, ErrorMessage = "Test Question should contain 29 Characters.")]
		public string TestQuestion { get; set; }


		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "SendMoneyTestAnswer")]
		[ConditionalTestQuestion("TestQuestionOption", ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "SendMoneyTestAnswerRequired")]
		[ConditionalRequired("TestQuestion", "TestAnswer", ErrorMessage = "Please enter test question.")]
		[StringLength(10, ErrorMessage = "Test Answer should contain 10 Characters.")]
		public string TestAnswer { get; set; }

		public bool isDomesticTransfer { get; set; }
		public string isDomesticTransferVal { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "SendMoneyTransferFee")]
		public decimal TransferFeeWithAllFees { get; set; }
		public decimal TransferFee { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "SendMoneyTransferTax")]
		public decimal TransferTax { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "SendMoneyTotalAmount")]
		[Required(ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "SendMoneyTotalAmountRequired")]
		public decimal TotalAmount { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "SendMoneyExchangeRate")]
		public decimal ExchangeRate { get; set; }
		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "SendMoneyExchangeRate")]
		public string ExchangeRateConversion { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "SendMoneyOtherFees")]
		public decimal OtherFees { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "SendMoneyTotalToRecipient")]
		public decimal TotalToRecipient { get; set; }

		//public string TotalToRecipientDesc { get; set; }

		[Range(typeof(Decimal), "0.01", "99999.99", ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "SendMoneyAmountRegularExpression")]
		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "DoddFrankConfirmationTransferAmount")]
		public decimal TransferAmount { get; set; }

		public bool enableEditContinue { get; set; }

		public long ReceiverID { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "SendMoneyPromoDiscount")]
		public decimal PromoDiscount { get; set; }

		public string TestQuestionOption { get; set; }

		[StringLength(15)]
		public string AmountWithCurrency { get; set; }
		public string TransferFeeWithCurrency { get; set; }
		public decimal OriginalFee { get; set; }
		public string PromoDiscountWithCurrency { get; set; }
		//[SendMoney("Amount")]
		[StringLength(15)]
		public string DestinationAmountWithCurrency { get; set; }
		public string OtherFeesWithCurrency { get; set; }
		public string TotalToRecipientWithCurrency { get; set; }
		public string TransferAmountWithCurrency { get; set; }
		public string DeliveryMethodDesc { get; set; }
		public string DeliveryOptionDesc { get; set; }
		public string TransferTaxWithCurrency { get; set; }
		public string DestinationAmountWithCurrency1 { get; set; }
		public IEnumerable<SelectListItem> LCountry { get; set; }
		public IEnumerable<SelectListItem> LStates { get; set; }
		public IEnumerable<SelectListItem> LCities { get; set; }
		public IEnumerable<SelectListItem> LDeliveryOptions { get; set; }
		public IEnumerable<SelectListItem> LDelivertyMethods { get; set; }
		public IEnumerable<SelectListItem> LActOnMyBehalf { get; set; }

		public List<Receiver> FrequentReceivers { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "ConsumerProtectionMessage")]
		public bool ConsumerProtectionMessage { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "ProvidedTermsandConditonsMessage")]
		public bool ProvidedTermsandConditonsMessage { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "DoddFrankTermsandConditonsMessage")]
		public bool DoddFrankDisclosure { get; set; }

		public decimal DestinationAmountFromFeeEnquiry { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "PickupLocation")]
		public string PickUpLocation { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "PickupMethod")]
		public string PickUpMethod { get; set; }

		[Required(ErrorMessage = "PickUp Method is required.")]
		public string PickUpMethodId { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "PickupOptions")]
		public string PickUpOptions { get; set; }

		[Required(ErrorMessage = "PickUp Options is required.")]
		public string PickUpOptionsId { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "SendMoneySearchForReceiver")]
		public string SearchForReceiver { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "CurrencyType")]
		public string CurrencyType { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "FailureDescription")]
		public string StatusDescription { get; set; }

		public long TransactionId { get; set; }

		public IEnumerable<SelectListItem> LPickupMethods { get; set; }
		public IEnumerable<SelectListItem> LPickupOptions { get; set; }
		public IEnumerable<SelectListItem> LPickupLocations { get; set; }
		public IEnumerable<SelectListItem> LCountryCurrencies { get; set; }

		public string LastNameFirstName { get; set; }

		public string StateName { get; set; }

		public bool IsFixedOnSend { get; set; }

		public string PromoCodeDescription { get; set; }
		public string PromoName { get; set; }
		public string PromoMessage { get; set; }
		public string PromotionError { get; set; }
		public string ReferenceNo { get; set; }
		[StringLength(966, ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "SendMoneyPersonalMessageStringLength")]
		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "SendMoneyPersonalMessage")]
		public string PersonalMessage { get; set; }
		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "SendMoneyMessageFee")]
		public decimal MessageFee { get; set; }
		public string MessageFeeWithCurrency { get; set; }
        public SendMoney()
        {
			LDelivertyMethods = new List<SelectListItem>() { new SelectListItem() { Text = "Select", Value = string.Empty } };
			LDeliveryOptions = new List<SelectListItem>() { new SelectListItem() { Text = "Select", Value = string.Empty } };
			LActOnMyBehalf = new List<SelectListItem>() { new SelectListItem() { Text = "Select", Value = string.Empty } };
			LCountryCurrencies = new List<SelectListItem>() { new SelectListItem() { Text = "Select", Value = string.Empty } };
		}
		public string PickupCity { get; set; }
        public string PickupState { get; set; }
        public string PickupCountry { get; set; }
		public string HasLPMTError { get; set; }

		public bool ProceedWithLPMTError { get; set; }
        public string ReceiveAgent { get; set; }
	}
}
