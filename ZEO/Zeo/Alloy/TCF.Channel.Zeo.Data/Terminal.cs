using System;
using System.Runtime.Serialization;

namespace TCF.Channel.Zeo.Data
{
	[DataContract]
	public class Terminal
	{
		[DataMember]
		public long TerminalId { get; set; }
		[DataMember]
		public string Name { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
		public string MacAddress { get; set; }
		[DataMember]
		public string IpAddress { get; set; }
        [DataMember]
        public string PeripheralServerId { get; set; }
        [DataMember]
        public string PeripheralServerUrl { get; set; }
        [DataMember]
        public long ChannelPartnerId { get; set; }
        [DataMember]
        public long LocationId { get; set; }
        [DataMember]
        public string LocationName { get; set; }
        [DataMember]
        public string PeripheralName { get; set; }
        [DataMember]
        public bool IsLocationActive { get; set; }
        override public string ToString()
		{
			string str = string.Empty;
			str = string.Concat(str, "Id = ", TerminalId, "\r\n");
			str = string.Concat(str, "Name = ", Name, "\r\n");
			str = string.Concat(str, "MacAddress = ", MacAddress, "\r\n");
			str = string.Concat(str, "IpAddress = ", IpAddress, "\r\n");
            str = string.Concat(str, "LocationId=", LocationId,"\r\n");
            str = string.Concat(str, "ChannelPartnerId=", ChannelPartnerId, "\r\n");
            str = string.Concat(str, "PeripheralServerId=", PeripheralServerId, "\r\n");
            return str;
		}
	}
}
