using System;
using System.Text;

namespace MGI.Biz.Partner.Data
{
	public class AgentSSO
	{
		public string UserName { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string FullName { get; set; }
		public UserRole Role { get; set; }
        public string ClientAgentIdentifier { get; set; }
	}
}
