using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MGI.Channel.DMS.Web.Models
{
    public class Receivers : BaseModel
    {
        /// <summary>
        /// Gets or sets FirstName
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "PersonalFirstNameRequired")]
        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "ReceiverFirstName")]
        [RegularExpression(@"^[a-zA-Z\- ']*$", ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "PersonalFirstNameRegularExpression")]
        [StringLength(20, ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "PersonalFirstNameStringLength")]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets LastName
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "LastNamePaternalNameRequired")]
        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "LastNamePaternalName")]
        [RegularExpression(@"^[a-zA-Z\- ']*$", ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "LastNamePaternalNameRegularExpression")]
        [StringLength(30, ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "LastNamePaternalNameStringLength")]
        public string LastName { get; set; }


		/// <summary>
		/// Gets or sets Second LastName
		/// </summary>		
		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "ReceiverSecondLastName")]
        [RegularExpression(@"^[a-zA-Z\- ']*$", ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "ReceiverSecondLastNameRegularExpression")]		
		public string SecondLastName { get; set; }

        /// <summary>
        /// Gets or sets Status
        /// </summary>
		//[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "ReceiverStatus")]
		//[Required(ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "ReceiverStatusRequired")]
		//public string Status { get; set; }

        /// <summary>
        /// Gets or sets Gender
        /// </summary>
        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "ReceiverGender")]
        [Required(ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "ReceiverGenderRequired")]
        public string Gender { get; set; }

        /// <summary>
        /// Gets or sets Relationship
        /// </summary>
        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "ReceiverRelationship")]
        public string Relationship { get; set; }

        /// <summary>
        /// Gets or sets Address
        /// </summary>
        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "ReceiverAddress")]        
        [RegularExpression("^[a-zA-Z0-9-_()#:.,/\\\\\\[\\] ]*$", ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "ReceiverAddressRegularExpression")]
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets Country
        /// </summary>
        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "ReceiverCountry")]
        [Required(ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "ReceiverCountryRequired")]
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets GovtIdIssueState
        /// </summary>
        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "ReceiverState")]
        [ConditionalRequired("Country", "GovtIdIssueState", ErrorMessage="State is required.")]
        public string GovtIdIssueState { get; set; }

        /// <summary>
        /// Gets or sets City
        /// </summary>
        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "ReceiverCity")]
		[RegularExpression(@"^[a-zA-Z ]*$", ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "PersonalCityRegularExpression")]
        public string City { get; set; }

		/// <summary>
		/// Gets or sets State Province
		/// </summary>
		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "ReceiverStateProvince")]
		[StringLength(40, ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "ReceiverStateProvinceStringLength")]
        [RegularExpression("^[a-zA-Z0-9-_#:. ]*$", ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "ReceiverStateProvinceRegularExpression")]
		public string StateProvince { get; set; }

		/// <summary>
		/// Gets or sets Zip Code
		/// </summary>
		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "ReceiverZipCode")]
		[RegularExpression(@"^[a-zA-Z0-9]{3,10}", ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "PersonalZipCodeRegularExpression")]
		public string ZipCode { get; set; }


		/// <summary>
		/// Gets or sets Phone
		/// </summary>
		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "ReceiverPhone")]
		//[StringLength(15,MinimumLength=13, ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "ReceiverPhoneStringLength")]
        [RegularExpression(@"^\d{10,15}$", ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "PersonalPrimaryPhoneRegularExpression")]
		public string Phone { get; set; }


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
		//[Required(ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "ReceiverDeliveryOptionsRequired")]
		public string DeliveryOptions { get; set; }


		/// <summary>
		/// Gets or sets Occupation
		/// </summary>
		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "ReceiverOccupation")]
		//[RegularExpression(@"^[a-zA-Z\ ]*$", ErrorMessage = "Please enter only alphabet or spaces.")]
		[RegularExpression(@"^[A-Za-z0-9 _-]*$", ErrorMessage = "Please enter only alphabets numbers _ - or spaces.")]
		[StringLength(30, ErrorMessage = "Maximum length of Occupation is 30.")]
		public string Occupation { get; set; }

        /// <summary>
        /// Gets or sets DOB
        /// </summary>
        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "ReceiverDob")]
		[DateTimeNotFuture(CheckFutureDate = true, ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "IdentificationDateOfBirthDateTimeNotFuture")]
		[DateTime(ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "IdentificationDateOfBirthDateTime")]
		[DataType(DataType.Date)]     //Age Range between 18 to 100.
		[DateRange(18, 100, ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "IdentificationDateOfBirthDateRange")]     //Age Range between 18 to 100.
		public string DateOfBirth { get; set; }

		 
		/// <summary>
		/// Gets or sets CountryOfBirth
		/// </summary>
		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "ReceiverCountryOfBirth")]
		public string CountryOfBirth { get; set; }

        /// <summary>
        /// Gets or sets PrimaryPhoneNumber
        /// </summary>
        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "ReceiverPrimaryPhoneNumber")]
        [RegularExpression(@"^[2-9](\d)(?!\1{8}$)\d*$", ErrorMessage = "Please Enter a valid Phone Number.")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Phone Number should be 10 digits in length.")]
        public string PrimaryPhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets PhoneType
        /// </summary>
        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "ReceiverPhoneType")]
        public string PhoneType { get; set; }

        /// <summary>
        /// Gets or sets MobileProvider
        /// </summary>
        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "ReceiverMobileProvider")]
        public string MobileProvider { get; set; }

        /// <summary>
        /// Gets or sets PaymentMethod
        /// </summary>
        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "ReceiverPaymentMethod")]
        //public string PaymentMethod { get; set; }
        public string PaymentMethod { get; set; }

        /// <summary>
        /// Gets or sets AmountType
        /// </summary>
        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "ReceiverAmountType")]
        public string AmountType { get; set; }

        /// <summary>
        /// Gets or sets AccountNumber
        /// </summary>
        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "ReceiverAccountNumber")]
        [RegularExpression(@"^[1-9](\d)(?!\1{8}$)\d*$", ErrorMessage = "Please Enter a valid Account Number.")]
        [StringLength(14, ErrorMessage = "Account Number cannot exceed 14 digits.")]
        public string AccountNumber { get; set; }

        
        public long ReceiverId { get; set; }

        public Guid Id { get; set; }

        public string AddEdit { get; set; }

        public string showAddEditMessage { get; set; }

        public IEnumerable<SelectListItem> LCountry { get; set; }
        public IEnumerable<SelectListItem> LStates { get; set; }
        public IEnumerable<SelectListItem> LCities { get; set; }
        public IEnumerable<SelectListItem> LPhoneTypes { get; set; }
        public IEnumerable<SelectListItem> LMobileProviders { get; set; }
        public IEnumerable<SelectListItem> LStatus { get; set; }
        public IEnumerable<SelectListItem> LGenders { get; set; }
        public IEnumerable<SelectListItem> LRelationships { get; set; }
        public IEnumerable<SelectListItem> LPayments { get; set; }
        public IEnumerable<SelectListItem> LAmountTypes { get; set; }
		public IEnumerable<SelectListItem> LPickUpCountry { get; set; }
		public IEnumerable<SelectListItem> LPickUpState { get; set; }
		public IEnumerable<SelectListItem> LPickUpCity { get; set; }
        public IEnumerable<SelectListItem> LDeliveryMethods { get; set; }
        public IEnumerable<SelectListItem> LDeliveryOptions { get; set; }
		public IEnumerable<SelectListItem> LCountryOfBirth { get; set; }
    }
}