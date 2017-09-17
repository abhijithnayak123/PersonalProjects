using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.ServiceModel;
using MGI.Channel.DMS.Server.Data;

namespace MGI.Channel.DMS.Web.Models
{
    [AtLeastOneProperty(ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "UserSearchAtleastOneAttribute")]
    public class ManageUsers : BaseModel
    {
        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "LastName")]
        [RegularExpression(@"^[a-zA-Z\- ']*$", ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "ManageUsersLastNameRegex")]
        [StringLength(50, ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "ManageUserLastNameStringLength")]
        [Required(ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "PersonalLastNameRequired")]
        public string LastName { get; set; }

        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "LastName")]
        [RegularExpression(@"^[a-zA-Z\- ']*$", ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "ManageUsersLastNameRegex")]
        [StringLength(50, ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "ManageUserLastNameStringLength")]
        public string SearchCriteriaLastName { get; set; }

        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "FirstName")]
        [RegularExpression(@"^[a-zA-Z\- ']*$", ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "ManageUsersLastNameRegex")]
        [StringLength(50, ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "ManageUserFirstNameStringLength")]
        [Required(ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "PersonalFirstNameRequired")]
        public string FirstName { get; set; }

        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "FirstName")]
        [RegularExpression(@"^[a-zA-Z\- ']*$", ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "ManageUsersLastNameRegex")]
        [StringLength(50, ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "ManageUserFirstNameStringLength")]
        public string SearchCriteriaFirstName { get; set; }

        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "MngUserUserName")]
        [RegularExpression(@"^[a-zA-Z\- ']*$", ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "ManageUserNameRegularExpression")]
        [StringLength(50, ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "ManageUserNameStringLength")]
        [Required(ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "ManageuserNameRequired")]
        public string ManageUserName { get; set; }

        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "MngUserPrimaryLocation")]
        [Required(ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "ManagePrimaryLocationRequired")]
        public string PrimaryLocation { get; set; }

        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "MngUserDepartment")]
        [Required(ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "ManageDepartmentRequired")]
        public string Department { get; set; }

        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "MngUserManager")]
        [Required(ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "ManageManagerRequired")]
        public string Manager { get; set; }

        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "MngUserUserRole")]
        [Required(ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "ManageuserRoleRequired")]
        public string UserRole { get; set; }

        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "MngUserUserStatus")]
        [Required(ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "ManageuserStatusRequired")]
        public string UserStatus { get; set; }

        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "MngUserPhone")]
		[PhoneNumberSequence("Phone", ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "MngUserPhoneRegularExpression")]
        public string Phone { get; set; }

        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "Email")]
		[RegularExpression(@"^(([^<>()[\]\.,;:\s@""]+(\.[^<>()[\]\\.,;:\s@""]+)*)|("".+""))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$", ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "MngUserEmailRegularExpression")]
        [StringLength(200, ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "MngUserEmailStringLength")]
        public string Email { get; set; }

        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "MngUserTempPassword")]
        //[RegularExpression(@"^((?=.*\d)(?=.*[A-Z]).{8,16})[/W]*$", ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "ManagetempPasswordRegex")]
		[TempPasswordRequired("UserStatus", ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "ManageUserTemppwdRequired")]
        [StringLength(16, MinimumLength = 8, ErrorMessage = "Temp Password must be 8 to 16 characters.")]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?!.*\s).*", ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "ManagetempPasswordRegex")]
        public string TempPassword { get; set; }

        public string AddEditMode { get; set; }

        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "MngUserLocationName")]
        public string LocationName { get; set; }
        public IEnumerable<SelectListItem> LPrimaryLocation { get; set; }
        public IEnumerable<SelectListItem> LDepartment { get; set; }
        public IEnumerable<SelectListItem> LManager { get; set; }
        public IEnumerable<SelectListItem> LUserRole { get; set; }
        public IEnumerable<SelectListItem> LUserStatus { get; set; }
        public List<User> Lusers { get; set; }

        public ManageUsers()
        {
            LManager = new List<SelectListItem>();
            LPrimaryLocation = new List<SelectListItem>();
            LUserRole = new List<SelectListItem>();
            LUserStatus = new List<SelectListItem>();
            Lusers = new List<User>();      
        }

    }

}