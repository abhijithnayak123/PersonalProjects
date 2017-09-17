using System.Runtime.Serialization;

namespace MGI.Peripheral.Server.Data
{
	[DataContract]
	public class HostNameResponse
	{
        [DataMember]
        public string MachineName { get; set; }

        [DataMember]
		public string HostName { get; set; }

		[DataMember]
		public string FQDN { get; set; }
	}
}
