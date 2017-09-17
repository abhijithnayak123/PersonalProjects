using System.Runtime.Serialization;

namespace MGI.Peripheral.Server.Data
{
	[DataContract]
	public class DiagnosticsResponse
	{
		[DataMember]
		public string NpsVersion { get; set; }

		[DataMember]
		public string Status { get; set; }

		[DataMember]
		public string DeviceName { get; set; }

		[DataMember]
		public string SerialNumber { get; set; }

		[DataMember]
		public string FirmwareRevision { get; set; }

		[DataMember]
		public string DeviceStatus { get; set; }
	}
}
