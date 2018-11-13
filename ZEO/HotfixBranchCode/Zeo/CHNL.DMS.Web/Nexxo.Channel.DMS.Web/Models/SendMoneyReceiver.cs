using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MGI.Channel.DMS.Web.Models
{
	/// <summary>
	/// Send Money Receiver View Model  
	/// </summary>
	public class SendMoneyReceiver : BaseModel
	{
		/// <summary>
		/// Gets or sets FirstName
		/// </summary>
		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "ReceiverFirstName")]
		public string FirstName { get; set; }

		/// <summary>
		/// Gets or sets LastName
		/// </summary>
		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "ReceiverLastName")]
		public string LastName { get; set; }

		/// <summary>
		/// Gets or sets Second LastName
		/// </summary>		
		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "ReceiverSecondLastName")]
		public string SecondLastName { get; set; }

		/// <summary>
		/// Gets or sets PickUpCountry
		/// </summary>
		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "ReceiverPickUpCountry")]
		[Required(ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "ReceiverPickUpCountryRequired")]
		public string PickUpCountry { get; set; }

		/// <summary>
		/// Gets or sets PickUpState
		/// </summary>
		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "ReceiverPickUpState")]
		public string PickUpState { get; set; }

		/// <summary>
		/// Gets or sets PickUpCity
		/// </summary>
		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "ReceiverPickUpCity")]
		public string PickUpCity { get; set; }

		/// <summary>
		/// Gets or sets DeliveryMethod
		/// </summary>
		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "ReceiverDeliveryMethod")]
		[Required(ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "ReceiverDeliveryMethodRequired")]
		public string DeliveryMethod { get; set; }

		/// <summary>
		/// Gets or sets DeliveryOptions
		/// </summary>
		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "ReceiverDeliveryOptions")]
		public string DeliveryOptions { get; set; }

		/// <summary>
		/// Gets or sets Address
		/// </summary>
		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "ReceiverAddress")]
		[RegularExpression("^[a-zA-Z0-9-_()#:.,/\\\\\\[\\] ]*$", ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "ReceiverAddressRegularExpression")]
		[ConditionalRequired("Address", "DeliveryOptions", ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "ReceiverAddressRequired")]
		public string Address { get; set; }

		/// <summary>
		/// Gets or sets City
		/// </summary>
		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "ReceiverCity")]
		[RegularExpression(@"^[a-zA-Z ]*$", ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "PersonalCityRegularExpression")]
		[ConditionalRequired("City", "DeliveryOptions", ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "PersonalCityRequired")]
		public string City { get; set; }

        /// <summary>
        /// Gets or sets State Province
        /// </summary>
        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "ReceiverStateProvince")]
        [RegularExpression("^[a-zA-Z0-9-_#:. ]*$", ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "ReceiverStateProvinceRegularExpression")]
        [ConditionalRequired("StateProvince", "DeliveryOptions", ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "ReceiverStateProvinceRequired")]
        public string StateProvince { get; set; }

		/// <summary>
		/// Gets or sets Zip Code
		/// </summary>
		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "ReceiverZipCode")]
		[RegularExpression(@"^[a-zA-Z0-9]{3,10}", ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "PersonalZipCodeRegularExpression")]
		[ConditionalRequired("ZipCode", "DeliveryOptions", ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "PersonalZipCodeRequired")]
		public string ZipCode { get; set; }

		/// <summary>
		/// Gets or sets Phone
		/// </summary>
		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "ReceiverPhone")]
		[RegularExpression(@"^\d{10,15}$", ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "PersonalPrimaryPhoneRegularExpression")]
		[ConditionalRequired("Phone", "DeliveryOptions", ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "PersonalPrimaryPhoneRequired")]
		public string Phone { get; set; }

		public long ReceiverId { get; set; }
		public Guid Id { get; set; }
		public string AddEdit { get; set; }
		public string showAddEditMessage { get; set; }

		public IEnumerable<SelectListItem> LPickUpCountry { get; set; }
		public IEnumerable<SelectListItem> LPickUpState { get; set; }
		public IEnumerable<SelectListItem> LPickUpCity { get; set; }
		public IEnumerable<SelectListItem> LDeliveryMethods { get; set; }
		public IEnumerable<SelectListItem> LDeliveryOptions { get; set; }
	}
}