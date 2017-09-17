using System;
using System.Runtime.Serialization;
using System.Text;

namespace MGI.Channel.DMS.Server.Data
{
	[DataContract]
	public class AgentSSO
	{
		[DataMember]
		public string UserName { get; set; }
		[DataMember]
		public string FirstName { get; set; }
		[DataMember]
		public string LastName { get; set; }
		[DataMember]
		public string FullName { get; set; }
		[DataMember]
		public UserRole Role { get; set; }
        [DataMember]
        public string ClientAgentIdentifier { get; set; }
		[DataMember]
		public System.Nullable<DateTime> BusinessDate { get; set; }

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("Agent SSO:");
			sb.AppendLine(string.Format("	UserName: {0}", UserName));
			sb.AppendLine(string.Format("	FirstName: {0}", FirstName));
			sb.AppendLine(string.Format("	LastName: {0}", LastName));
			sb.AppendLine(string.Format("	FullName: {0}", FullName));
            sb.AppendLine(string.Format("	Role: {0}", Role));
            sb.AppendLine(string.Format("	Client Agent Identifier: {0}", ClientAgentIdentifier));
			sb.AppendLine(string.Format("   BusinessDate:{0}", BusinessDate));		
			
			return sb.ToString();
		}
	}
}
