using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using MGI.Channel.DMS.Web.Common;
using MGI.Channel.DMS.Web.SSO;

namespace MGI.Channel.DMS.Web.Models
{
	public class SSOLogin : BaseModel
	{
		public SSOErrorCodes ErrorCode { get; set; }
        public string Issuer { get; set; }
		public string hostName { get; set; }
        public string SAMLResponse { get; set; }
	}
}