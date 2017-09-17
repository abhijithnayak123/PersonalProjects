using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Collections;
using System.Web.Mvc;

namespace MGI.Channel.DMS.Web.Models
{
    /// <summary>
    /// Model for Employment view
    /// </summary>
    public class EmploymentDetails : BaseModel
    {
        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "Profession")]
        public string Profession { get; set; }

        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "EmployerName")]
        public string EmployerName { get; set; }

        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "EmployerPhoneNumber")]
        public string EmployerPhoneNumber { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "OccupationDescription")]
		public string OccupationDescription { get; set; }

		public IEnumerable<SelectListItem> Occupations { get; set; }
    }
}
