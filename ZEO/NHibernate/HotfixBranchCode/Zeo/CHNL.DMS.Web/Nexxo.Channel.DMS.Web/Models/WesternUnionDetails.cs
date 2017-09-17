using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MGI.Channel.DMS.Web.Models
{
	public class WesternUnionDetails : BaseModel
	{
		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "WesternUnionEnrollGoldCardMessage")]
		public string EnrollWesternUnionGoldCard { get; set; }
		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "WesternUnionGoldCard")]
		public string WUGoldCardNumber { get; set; }

		public string WUConfirmationMessage { get; set; }
        public string EditGoldCardFrom { get; set; }
	}
}