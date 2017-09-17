using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace TCF.Channel.Zeo.Web.Models
{
	public class TerminalModel : BaseModel
	{
		[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "TerminalName")]
		public string TerminalName { get; set; }

		[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "TerminalIpAddress")]
		public string IpAddress { get; set; }

		[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "TerminalLocation")]
		public string Location { get; set; }

		public IEnumerable<SelectListItem> Locations { get; set; }

		[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "NpsTerminal")]
		public string NpsTerminal { get; set; }

		public IEnumerable<SelectListItem> NpsTerminals { get; set; }

		public long Id { get; set; }
	}
}