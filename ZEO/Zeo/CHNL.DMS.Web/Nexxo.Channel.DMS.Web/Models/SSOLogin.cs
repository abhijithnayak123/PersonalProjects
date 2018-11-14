using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using TCF.Channel.Zeo.Web.Common;
using TCF.Channel.Zeo.Web.SSO;

namespace TCF.Channel.Zeo.Web.Models
{
	public class SSOLogin : BaseModel
	{
		public SSOErrorCodes ErrorCode { get; set; }
        public string Issuer { get; set; }
		public string hostName { get; set; }
        public string SAMLResponse { get; set; }
	}
}