using System.Runtime.Serialization;

namespace TCF.Zeo.Peripheral.Server.Data
{
	// used for throwing exceptions, accessed from client catch block
	[DataContract]
	public class FaultInfo
	{
		[DataMember]
		public int ErrorNo { get; set; }

		[DataMember]
		public string ErrorMessage { get; set; }

		[DataMember]
		public string ErrorDetails { get; set; }
	}
}
