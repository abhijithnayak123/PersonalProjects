using System.ComponentModel.DataAnnotations;
using TCF.Zeo.Common.Util;
#region AO
using ZeoClient = TCF.Channel.Zeo.Web.ServiceClient.ZeoService;
#endregion



namespace TCF.Channel.Zeo.Web.Models
{
    /// <summary>
    /// AO - Model for Profile Summary screen.
    /// </summary>
    public class ProfileSummary : BaseModel
	{
        #region Personal Information

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "SSNITIN")]
		public string SSN { get; set; }

		[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "Name")]
		public string Name { get; set; }

		[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "Gender")]
		public string Gender { get; set; }

		[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "PrimaryPhone")]
		public string PrimaryPhone { get; set; }

		[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "Email")]
		public string Email { get; set; }

		[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "Address")]
		public string Address { get; set; }

		[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "MailingAddress")]
		public string MailingAddress { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "CustomerProfileStatus")]
        public Helper.ProfileStatus CustomerProfileStatus { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "Group1")]
        public string Group1 { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "Group2")]
        public string Group2 { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "Notes")]
        public string Notes { get; set; }

        #endregion

        #region Identification

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "MotherMaidenName")]
		public string MotherMaidenName { get; set; }

		[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "DateOfBirth")]
		public string DateOfBirth { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "Country")]
        public string Country { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "ProfileSummaryCountryOfBirth")]
        public string CountryOfBirth { get; set; }

		[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "ProfileGovtIDType")]
		public string GovtIDType { get; set; }

		[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "GovtIdIssueState")]
		public string GovtIdIssueState { get; set; }

		[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "IDNumber")]
		public string GovernmentId { get; set; }

		[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "IDIssuedDate")]
		public string IDIssueDate { get; set; }

		[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "IDExpirationDate")]
		public string IDExpirationDate { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "LegalCode")]
        public string LegalCode { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "PrimaryCountryCitizenShip")]
        public string PrimaryCountryCitizenShip { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "SecondaryCountryCitizenShip")]
        public string SecondaryCountryCitizenShip { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "MGiAlloyID")]
        public string MGIAlloyID { get; set; }

        //Added By Abhijith for User Story - #US2157
        //Starts Here
        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "ClientID")]
        public string ClientID { get; set; }

        #endregion

        #region Employment

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "Profession")]
		public string Profession { get; set; }

		[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "OccupationDescription")]
		public string OccupationDescription { get; set; }

		[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "EmployerName")]
		public string EmployerName { get; set; }

		[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "EmployerPhoneNumber")]
		public string EmployerPhoneNumber { get; set; }

		[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "PhoneNumber")]
		public string PhoneNumber { get; set; }

        #endregion

        #region PIN Details

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "Pin")]
        public string Pin { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "ReEnter")]
        public string ReEnter { get; set; }

        #endregion

        public bool IsNewCustomer { get; set; }
    }
}