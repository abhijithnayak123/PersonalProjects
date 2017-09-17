using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace TCF.Channel.Zeo.Web.Models
{
	public class NpsDiagnosticModel : BaseModel
    {
		[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "TerminalName")]
		public string TerminalName { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "StationId")]
        public string StationId { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "LocationId")]
        public string LocationId { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "Version")]
        public string Version { get; set; }
		
		[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "NexxoPeripheralService")]
        public string NexxoPeripheralService { get; set; }

		[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "Serial")] 
        public string Serial { get; set; }

		[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "Firmware")] 
        public string Firmware { get; set; }

        public string DeviceName { get; set; }

		public string DeviceStatus { get; set; }
		
		public string Status { get; set; } 
    }
}
