using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using MGI.Channel.Shared.Server.Data;

namespace MGI.Channel.DMS.Server.Data
{
	[DataContract]
	public class CheckCashingStatus
	{
		[DataMember]
		public string Status { get; set; }
		[DataMember]
		public string StatusMessage { get; set; }
		[DataMember]
		public decimal Limit { get; set; }
		[DataMember]
		public List<Check> OpenChecks { get; set; }

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("Check Cashing Status:");
			sb.AppendLine(string.Format("	Status: {0}", Status));
			sb.AppendLine(string.Format("	StatusMessage: {0}", StatusMessage));
			sb.AppendLine(string.Format("	Limit: {0}", Limit.ToString("c2")));
			sb.AppendLine(string.Format("	Open Checks:"));
			if (OpenChecks != null)
			{ OpenChecks.ForEach(c => sb.Append(c.ToString())); }
			else
			{ sb.Append(string.Empty); }
			return sb.ToString();
		}
	}
}
