using System;
using System.Runtime.Serialization;
using System.Text;

namespace MGI.Channel.Shared.Server.Data
{
	[DataContract]
	public class Identification
	{
		[DataMember]
		public string Country { get; set; }
		[DataMember]
		public string IDType { get; set; }
		[DataMember]
		public string State { get; set; }
		[DataMember]
		public string GovernmentId { get; set; }
		[DataMember]
		public DateTime? IssueDate { get; set; }
		[DataMember]
		public DateTime? ExpirationDate { get; set; }
		[DataMember]
		public string CountryOfBirth { get; set; }
		//This Field is added for Get NexxoIDType Name for Get Customer Details for Kiosk
		[DataMember]
		public string IDTypeName { get; set; }

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("ID:");
			sb.AppendLine(string.Format("	Country: {0}", Country));
			sb.AppendLine(string.Format("	IDType: {0}", IDType));
			sb.AppendLine(string.Format("	State: {0}", State));
			sb.AppendLine(string.Format("	ID: {0}{1}", "****", string.IsNullOrWhiteSpace(GovernmentId) ? null : GovernmentId.Substring(GovernmentId.Length - 4, 4)));
			sb.AppendLine(string.Format("	IssueDate: {0}", IssueDate != null ? IssueDate.Value.ToShortDateString() : null));
			sb.AppendLine(string.Format("	ExpirationDate: {0}", ExpirationDate != null ? ExpirationDate.Value.ToShortDateString() : null));
			sb.AppendLine(string.Format("	IDTypeName: {0}", IDTypeName));
			return sb.ToString();
		}
	}
}
