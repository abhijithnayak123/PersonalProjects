using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MGI.Channel.DMS.Web.Models
{
	public class MoneyGramSendMoney : BaseModel
	{
		[RegularExpression(@"^[a-zA-Z\- ']*$", ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "SendMoneyFirstNameRegex")]
		[StringLength(20, ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "SendMoneyFirstNameStringLength")]
		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "SendMoneyFirstName")]
		[Required(ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "SendMoneyFirstNameRequired")]
		public string FirstName { get; set; }

		[RegularExpression(@"^[a-zA-Z\- ']*$", ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "SendMoneyLastNameRegex")]
		[StringLength(30, ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "SendMoneyLastNameStringLength")]
		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "SendMoneyLastName")]
		[Required(ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "SendMoneyLastNameRequired")]
		public string LastName { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "MiddleName")]
		public string MiddleName { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "SendMoneySecondLastName")]
		public string SecondLastName { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "SendMoneyCountry")]
		[Required(ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "SendMoneyCountryRequired")]
		public string PickupCountry { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "SendMoneyPickUpStateProvince")]
        [DropDownRequired(ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "ReceiverPickUpStateRequired")]
		public string PickupState { get; set; }

		public string Amount { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "SendMoneyDestinationAmount")]
		public string DestinationAmount { get; set; }

		public string CouponPromoCode { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "SendMoneyDeliveryMethod")]
		[Required(ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "SendMoneyDeliveryMethodRequired")]
		public string DeliveryMethod { get; set; }

		public string CurrencyCode { get; set; }

		public decimal Fee { get; set; }
		public long TransactionId { get; set; }
		public long ReceiverId { get; set; }

		public FrequentReceivers FrequentReceivers { get; set; }

		public IEnumerable<SelectListItem> PickupCountries { get; set; }
		public IEnumerable<SelectListItem> PickupStates { get; set; }
		public IEnumerable<SelectListItem> DeliveryMethods { get; set; }

		public MoneyGramSendMoney()
		{
			PickupCountries = new List<SelectListItem>();
			PickupStates = new List<SelectListItem>();
			DeliveryMethods = new List<SelectListItem>();
		}
	}
}