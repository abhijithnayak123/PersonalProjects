using System.Runtime.Serialization;
using System.Text;

namespace MGI.Channel.DMS.Server.Data
{
	[DataContract]
	public class AgentSession
	{
		public AgentSession()
		{
		}

		[DataMember]
		public string SessionId { get; set; }
		[DataMember]
		public Agent Agent { get; set; }
		[DataMember]
		public Terminal Terminal { get; set; }

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("AgentSession:");
			sb.AppendLine(string.Format("	SessionId: {0}", SessionId));
			sb.AppendLine(string.Format("	Agent: {0}", Agent));
			sb.AppendLine(string.Format("	Terminal: {0}", Terminal));
			return sb.ToString();
		}
	}
}
