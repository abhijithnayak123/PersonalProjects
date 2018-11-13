using System.Runtime.Serialization;
using System.Text;

namespace MGI.Channel.DMS.Server.Data
{
	[DataContract]
	public class CheckProcessorInfo
	{
		[DataMember]
		public string Url { get; set; }

		[DataMember]
		public string Tocken { get; set; }

		[DataMember]
		public int EmployeeId { get; set; }

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("CheckProcessorInfo:");
			sb.AppendLine(string.Format("	Url: {0}", Url));
			sb.AppendLine(string.Format("	Tocken: {0}", Tocken));
			sb.AppendLine(string.Format("	EmployeeId: {0}", EmployeeId));
			return sb.ToString();
		}

	}
}
