using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;


namespace MGI.Channel.DMS.Web.Models
{
    public class ProcessorCrendential
    {
		// Identity column of tLocationProcessorCredentials
		public long ProcessorID { get; set; }

		public long ProviderID { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "UserName")]
		[Required(ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "ManageuserNameRequired")]
		public string UserName { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "Password")]
		[Required(ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "ManagePasswordRequired")]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[RegularExpression(@"^[a-z0-9-]+$", ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "ProgramIDRegex")]
		[Required(ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "IdentifierRequired")]
		public string Identifier { get; set; }

		public long LocationID { get; set; }

		public bool IsEnable { get; set; }
    }
}
