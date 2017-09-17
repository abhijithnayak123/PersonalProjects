using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;


namespace TCF.Channel.Zeo.Web.Models
{
    public class ProcessorCrendential
    {
		// Identity column of tLocationProcessorCredentials
		public long ProcessorID { get; set; }

		public long ProviderID { get; set; }

		[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "UserName")]
		[Required(ErrorMessageResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "ManageuserNameRequired")]
		public string UserName { get; set; }

		[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "Password")]
		[Required(ErrorMessageResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "ManagePasswordRequired")]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[RegularExpression(@"^[a-zA-Z0-9-]+$", ErrorMessageResourceType = typeof(App_GlobalResources.Nexxo), ErrorMessageResourceName = "ProgramIDRegex")]
		[Required(ErrorMessageResourceType = typeof(App_GlobalResources.Nexxo), ErrorMessageResourceName = "IdentifierRequired")]
		public string Identifier { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9-]+$", ErrorMessageResourceType = typeof(App_GlobalResources.Nexxo), ErrorMessageResourceName = "ProgramIDRegex")]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Nexxo), ErrorMessageResourceName = "IdentifierRequired")]
        public string Identifier2 { get; set; }

        public long LocationID { get; set; }

		public bool IsEnable { get; set; }
    }
}
