using System;
namespace MGI.CXN.MG.Common.Data
{
	public class BaseRequest
	{
		public string AgentID { get; set; }
		public string AgentSequence { get; set; }
		public string Token { get; set; }
		public string Language { get; set; }
		public DateTime TimeStamp { get; set; }
		public string ApiVersion { get; set; }
		public string ClientSoftwareVersion { get; set; }
	}
}
