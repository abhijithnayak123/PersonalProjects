using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Collections;
using System.Web.Mvc;

namespace TCF.Channel.Zeo.Web.Models
{
    /// <summary>
    /// AO - Model for Employment Details screen.
    /// </summary>
    public class EmploymentDetails : BaseModel
    {
		public EmploymentDetails()
		{
			Occupations = DefaultListItems();
		}

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "Profession")]
        public string Profession { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "EmployerName")]
        public string EmployerName { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "EmployerPhoneNumber")]
        public string EmployerPhoneNumber { get; set; }

		[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "OccupationDescription")]
		public string OccupationDescription { get; set; }

		public IEnumerable<SelectListItem> Occupations { get; set; }

        //AO - TODO :Check Whether it is required under this model ? 
		public bool IsCompanionSearch { get; set; }

		private List<SelectListItem> DefaultListItems()
		{
			List<SelectListItem> list = new List<SelectListItem>();
			list.Add(new SelectListItem() { Value = string.Empty, Text = "Select", Selected = true });

			return list;
		}
    }
}
