using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TCF.Channel.Zeo.Web.Models
{
    public class CustomerDetails //: BaseModel
    {
        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "FullName")]
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "DateOfBirth")]
        public string DateOfBirth { get; set; }

        public string Address { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "IDNumber")]
        public string GovermentId { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "PhoneNumber")]
        public string PhoneNumber { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "SSNITIN")]
        public string SSN { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "ZeoCard")]
        public string CardNumber { get; set; }

        public bool IsRcifCustomer { get; set; }

        public string ActualSSN { get; set; }

        public string CustomerId { get; set; }

        public bool IsExpiring { get; set; }

        public bool IsExpired { get; set; }
    }
}