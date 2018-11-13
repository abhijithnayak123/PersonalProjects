using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Biz.Partner.Data
{
    public class Agent
    {
		public int Id { get; set; }
		public string Name { get; set; }
		public AuthenticationStatus AuthStatus { get; set; }
		public PasswordStatus PasswdStatus { get; set; }
		public string UserName { get; set; }
        public string ClientAgentIdentifier { get; set; }
    }
}
