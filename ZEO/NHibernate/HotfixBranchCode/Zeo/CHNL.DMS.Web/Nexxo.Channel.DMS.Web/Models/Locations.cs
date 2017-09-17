using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;


namespace MGI.Channel.DMS.Web.Models
{
    public class Locations:BaseModel
    {
        /// <summary>
        /// Gets or Sets the BankID
        /// </summary>
        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "LocationBankID")]
		public string BankID { get; set; }

        /// <summary>
        /// Gets or Sets the BranchID
        /// </summary>
        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "LocationBranchID")]
		public string BranchID { get; set; }


		/// <summary>
		/// Gets Sets the LocationID
		/// </summary>
		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "LocationIdentifier")]
		public string LocationIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the LocationName
        /// </summary>
        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "LocationLocationName")]
        public string LocationName { get; set; }

        /// <summary>
        /// Gets or sets the LocationStatus
        /// </summary>
        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "LocationLocationStatus")]
        [Required(ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "LocationLocationStatusRequired")]
        public string LocationStatus { get; set; }

        /// <summary>
        /// Gets or sets the LLocationStatus
        /// </summary>
        public IEnumerable<SelectListItem> LLocationStatus { get; set; }

        /// <summary>
        /// Gets or sets the LocationUSState
        /// </summary>
        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "LocationLocationUSState")]
        [Required(ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "LocationLocationStateRequired")]
        public string LocationUSState { get; set; }
               
        /// <summary>
        /// Gets or sets the LLocationUSStates
        /// </summary>
        public IEnumerable<SelectListItem> LLocationUSStates { get; set; }

        /// <summary>
        /// Gets or sets the City
        /// </summary>
        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "LocationCity")]
        public string City { get; set; }

        /// <summary>
        ///  Gets or sets the Address1
        /// </summary>
        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "LocationAddress1")]
		[RegularExpression(@"^(?!.*(P|p)\.? ?(O|o)\.? ?(Box|bOx|boX|BOX|BOx|box))[-a-zA-Z\d .,/@#!]*$", ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "LocationLocationAddress1Regex")]
        public string Address1 { get; set; }

        /// <summary>
        ///  Gets or sets the Address2
        /// </summary>
        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "LocationAddress2")]
        [RegularExpression(@"^(?!.*(P|p)\.? ?(O|o)\.? ?(Box|bOx|boX|BOX|BOx|box))[-a-zA-Z\d .,/@#!]*$", ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "LocationLocationAddress2Regex")]
        public string Address2 { get; set; }

        /// <summary>
        ///  Gets or sets the ZipCode
        /// </summary>
        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "LocationZipCode")]
        public string ZipCode { get; set; }

        /// <summary>
        ///  Gets or sets the PHONE
        /// </summary>
		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "LocationPhone")]
		[PhoneNumberSequence("Phone", ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "LocationLocationPhoneRegex")]
        [Required(ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "LocationPhoneRequired")]
		public string Phone { get; set; }

        /// <summary>
        /// Gets or Sets the TimeZone
        /// </summary>
        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "LocationTimeZone")]
        [Required(ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "LocationTimeZoneRequired")]
        public string TimeZone { get; set; }

        /// <summary>
        /// Gets or sets the LTimeZone
        /// </summary>
        public IEnumerable<SelectListItem> LTimeZone { get; set; }

        /// <summary>
        /// Gets or sets the Source
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        ///  Gets or sets the LocationLists list
        /// </summary>
        public List<MGI.Channel.DMS.Server.Data.Location> LocationLists { get; set; }

        /// <summary>
        ///  Gets or sets the AddEdit 
        /// </summary>
        public string AddEdit { get; set; }

        /// <summary>
        /// Gets or sets the Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the rowguid 
        /// </summary>
        public Guid rowguid  { get; set; }

        public Locations()
        {
            LLocationUSStates = new List<SelectListItem>();
            LLocationStatus = new List<SelectListItem>();
            LocationLists = new List<DMS.Server.Data.Location>();
            AddEdit = "Enter New Locations";
        }

    }
}