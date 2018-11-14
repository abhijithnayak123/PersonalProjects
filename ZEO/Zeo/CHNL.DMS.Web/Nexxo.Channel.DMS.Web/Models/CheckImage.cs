using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TCF.Channel.Zeo.Web.ServiceClient.ZeoService;

namespace TCF.Channel.Zeo.Web.Models
{
    public class CheckImage : BaseModel
    {
        public string CheckFrontImage { get; set; }
        public string CheckBackImage { get; set; }
        public string MICRCode { get; set; }

        public string NpsId { get; set; }

		public string CheckFrontImage_TIFF { get; set; }
		public string CheckBackImage_TIFF { get; set; }
		public string MicrErrorMessage { get; set; }
		public string NpsURL { get; set; }

		public string PrintData { get; set; }

		public string RoutingNumber { get; set; }
		public string AccountNumber { get; set; }
		public string CheckNumber { get; set; }
        public int MicrError { get; set; }
        public bool IsDisabled { get; set; }

        public HelperProviderId ProductProviderCode { get; set; }

        public int CheckTypeId { get; set; }
    }
}