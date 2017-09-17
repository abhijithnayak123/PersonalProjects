using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace TCF.Channel.Zeo.Web.Models
{
	/// <summary>
	/// Send Money Receiver View Model  
	/// </summary>
	public class MoneyGramReceiverViewModel : BaseModel
	{
		/// <summary>
		/// Gets or sets FirstName
		/// </summary>
		[Required(ErrorMessageResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "PersonalFirstNameRequired")]
		[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "ReceiverFirstName")]
		[RegularExpression(@"^[a-zA-Z\- ']*$", ErrorMessageResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "PersonalFirstNameRegularExpression")]
		[StringLength(20, ErrorMessageResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "PersonalFirstNameStringLength")]
		public string FirstName { get; set; }

		/// <summary>
		/// Gets or sets LastName
		/// </summary>
		[Required(ErrorMessageResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "PersonalLastNameRequired")]
		[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "ReceiverLastName")]
		[RegularExpression(@"^[a-zA-Z\- ']*$", ErrorMessageResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "PersonalLastNameRegularExpression")]
		[StringLength(30, ErrorMessageResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "PersonalLastNameStringLength")]
		public string LastName { get; set; }

        /// <summary>
        /// Gets or sets MiddleName
        /// </summary>		
        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "MiddleName")]
        [RegularExpression(@"^[a-zA-Z\- ']*$", ErrorMessageResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "PersonalMiddleNameRegularExpression")]
        public string MiddleName { get; set; }

		/// <summary>
		/// Gets or sets Second LastName
		/// </summary>		
		[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "SecondLastName")]
		[RegularExpression(@"^[a-zA-Z\- ']*$", ErrorMessageResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "PersonalSecondLastNameRegularExpression")]
		public string SecondLastName { get; set; }

		/// <summary>
		/// Gets or sets PickUpCountry
		/// </summary>
		[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "ReceiverPickUpCountry")]
		[Required(ErrorMessageResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "ReceiverPickUpCountryRequired")]
		public string PickUpCountry { get; set; }

		/// <summary>
		/// Gets or sets PickUpState
		/// </summary>
		[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "ReceiverPickUpState")]
        [DropDownRequired(ErrorMessageResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "ReceiverPickUpStateRequired")]
		public string PickUpState { get; set; }

		/// <summary>
		/// Gets or sets PickUpCity
		/// </summary>
		[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "ReceiverPickUpCity")]
		public string PickUpCity { get; set; }
		
		/// <summary>
		/// Gets or sets Address
		/// </summary>
		[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "ReceiverAddress")]
		[RegularExpression("^[a-zA-Z0-9-_()#:.,/\\\\\\[\\] ]*$", ErrorMessageResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "ReceiverAddressRegularExpression")]		
		public string Address { get; set; }

		/// <summary>
		/// Gets or sets City
		/// </summary>
		[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "ReceiverCity")]
		[RegularExpression(@"^[a-zA-Z ]*$", ErrorMessageResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "PersonalCityRegularExpression")]		
		public string City { get; set; }

        /// <summary>
        /// Gets or sets State Province
        /// </summary>
        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "ReceiverStateProvince")]
        [StringLength(40, ErrorMessageResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "ReceiverStateProvinceStringLength")]
        [RegularExpression("^[a-zA-Z0-9-_#:. ]*$", ErrorMessageResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "ReceiverStateProvinceRegularExpression")]
        public string StateProvince { get; set; }

		/// <summary>
		/// Gets or sets Zip Code
		/// </summary>
		[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "ReceiverZipCode")]
		[RegularExpression(@"^[a-zA-Z0-9]{3,10}", ErrorMessageResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "PersonalZipCodeRegularExpression")]
		public string ZipCode { get; set; }

		/// <summary>
		/// Gets or sets Phone
		/// </summary>
		[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "ReceiverPhone")]
		[RegularExpression(@"^\d{10,15}$", ErrorMessageResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "PersonalPrimaryPhoneRegularExpression")]
		public string Phone { get; set; }

		public long ReceiverId { get; set; }
		public Guid Id { get; set; }
		public string AddEdit { get; set; }	

		public IEnumerable<SelectListItem> LPickUpCountry { get; set; }
		public IEnumerable<SelectListItem> LPickUpState { get; set; }
		public IEnumerable<SelectListItem> LPickUpCity { get; set; }		

		//For MGI Added
		[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "SendMoneyIsReceiverHasPhotoId")]
		public  bool IsReceiverHasPhotoId { get; set; }

		[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "SendMoneyMGITestQuestion")]
		[ConditionalReceiverHasPhotoIDAttribute("IsReceiverHasPhotoId", ErrorMessageResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "SendMoneyTestQuestionRequired")]
		[ConditionalRequired("TestQuestion", "TestAnswer", ErrorMessage = "Please enter test answer.")]		
		public string TestQuestion { get; set; }

		[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "SendMoneyMGITestAnswer")]
		[ConditionalReceiverHasPhotoIDAttribute("IsReceiverHasPhotoId", ErrorMessageResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "SendMoneyTestAnswerRequired")]
		[ConditionalRequired("TestAnswer", "TestQuestion", ErrorMessage = "Please enter test question.")]		
		public string TestAnswer { get; set; }
	}
}