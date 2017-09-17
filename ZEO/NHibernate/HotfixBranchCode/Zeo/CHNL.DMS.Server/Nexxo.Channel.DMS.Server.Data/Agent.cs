using System;
using System.Runtime.Serialization;
using System.Text;

namespace MGI.Channel.DMS.Server.Data
{
	[DataContract]
	public class Agent
	{
		public Agent()
		{
		}

		[DataMember]
		public int Id { get; set; }
		[DataMember]
		public string Name { get; set; }
		[DataMember]
		public int AuthenticationStatus { get; set; }
		[DataMember]
		public DateTime PasswordExpirationDate { get; set; }
		[DataMember]
		public bool PasswordChangeRequired { get; set; }
		[DataMember]
		public string UserName { get; set; }

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("Agent:");
			sb.AppendLine(string.Format("	Id: {0}", Id));
			sb.AppendLine(string.Format("	Name: {0}", Name));
			sb.AppendLine(string.Format("	AuthenticationStatus: {0}", AuthenticationStatus));
			sb.AppendLine(string.Format("	PasswordExpirationDate: {0}", PasswordExpirationDate.ToString()));
			sb.AppendLine(string.Format("	PasswordChangeRequired: {0}", PasswordChangeRequired));
			sb.AppendLine(string.Format("	UserName: {0}", UserName));
			return sb.ToString();
		}
	}

}
