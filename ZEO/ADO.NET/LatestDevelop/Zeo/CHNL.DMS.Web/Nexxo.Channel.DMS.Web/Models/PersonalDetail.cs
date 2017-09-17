using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace TCF.Channel.Zeo.Web.Models
{
    public class PersonalDetail
    {
        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "FirstName")]
        public string FirstName { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "MiddleName")]
        public string MiddleName { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "LastName")]
        public string LastName { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "SecondLastName")]
        public string SecondLastName { get; set; }

        public string Gender { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "SSNITIN")]
        public string SSN { get; set; }

        public string ActualSSN { get; set; }
    }
}