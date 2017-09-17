using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TCF.Channel.Zeo.Web.Models
{
	public class WesternUnionDetails : BaseModel
	{
		[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "WesternUnionEnrollGoldCardMessage")]
		public string EnrollWesternUnionGoldCard { get; set; }
		[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "WesternUnionGoldCard")]
		public string WUGoldCardNumber { get; set; }

		public string WUConfirmationMessage { get; set; }
        public string EditGoldCardFrom { get; set; }
	}
}