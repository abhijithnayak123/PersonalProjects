using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

using MGI.Channel.DMS.Web.ServiceClient;
//using MGI.Channel.DMS.Web.Entities;
using MGI.Channel.DMS.Server.Data;
using MGI.Channel.Shared.Server.Data;
using MGI.Common.Util;


namespace MGI.Channel.DMS.Web.Models
{
	/// <summary>
	/// 
	/// </summary>
	public class ProfileSummary : BaseModel
	{
		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "SSNITIN")]
		// [Required]
		public string SSN { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "Name")]
		//  [Required]
		public string Name { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "Gender")]
		//[RequiredIfNotChannelPartner("Synovus", ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "PersonalGenderRequired")]
		public string Gender { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "PrimaryPhone")]
		//  [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:###-##-####}")]
		// [Required]
		public string PrimaryPhone { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "Email")]
		public string Email { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "Address")]
		// [Required]
		public string Address { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "MailingAddress")]
		public string MailingAddress { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "MotherMaidenName")]
		// [Required]
		public string MotherMaidenName { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "DateOfBirth")]
		//  [Required]
		public string DateOfBirth { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "Country")]
		// [Required]
		public string Country { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "ProfileGovtIDType")]
		//[Required]
		public string GovtIDType { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "GovtIdIssueState")]
		//  [ConditionalRequired("Country", "GovtIDType", ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "IdentificationGovtIdIssueStateConditionalRequired")]
		public string GovtIdIssueState { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "IDNumber")]
		//   [Required]
		public string GovernmentId { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "IDIssuedDate")]
		public string IDIssueDate { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "IDExpirationDate")]
		public string IDExpirationDate { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "Profession")]
		public string Profession { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "OccupationDescription")]
		public string OccupationDescription { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "EmployerName")]
		public string EmployerName { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "EmployerPhoneNumber")]
		//  [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:###-##-####}")]
		public string EmployerPhoneNumber { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "PhoneNumber")]
		//  [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:###-##-####}")]
		public string PhoneNumber { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "Pin")]
		//[Required]
		public string Pin { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "ReEnter")]
		public string ReEnter { get; set; }


		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "ProfileSummaryCountryOfBirth")]
		//[Required]
		public string CountryOfBirth { get; set; }

		//Added By Abhijith for User Story - #US2157
		//Starts Here
		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "ClientID")]
		public string ClientID { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "LegalCode")]
		public string LegalCode { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "PrimaryCountryCitizenShip")]
		public string PrimaryCountryCitizenShip { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "SecondaryCountryCitizenShip")]
		public string SecondaryCountryCitizenShip { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "MGiAlloyID")]
		public string MGIAlloyID { get; set; }
		//Ends Here

		//[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "WUGoldCardNumber")]
		//public string GoldCardNumber { get; set; }

		//[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "WUSmsOptIn")]
		//public bool SmsOptIn { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "Group1")]
		public string Group1 { get; set; }
		public string Group2 { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "CustomerProfileStatus")]
		public ProfileStatus CustomerProfileStatus { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "Notes")]
		public string Notes { get; set; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="agentSessionId"></param>
		/// <param name="alloyId"></param>
		/// <param name="prospect"></param>
		public bool Activate(string agentSessionId, Prospect prospect, String alloyId, bool editMode = false)
		{
			Desktop service = new Desktop();
			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
			if (!editMode)
			{
				//string alloyId = service.GeneratePAN(agentSessionId, prospect);
				service.SaveCustomerProfile(agentSessionId, Convert.ToInt64(alloyId), prospect, mgiContext);
				
				mgiContext.EditMode = editMode;

				service.NexxoActivate(agentSessionId, Convert.ToInt64(alloyId), mgiContext);
			}

			return true;
		}
	}
}