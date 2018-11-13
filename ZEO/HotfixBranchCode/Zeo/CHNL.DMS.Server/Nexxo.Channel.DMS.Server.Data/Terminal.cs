using System;
using System.Runtime.Serialization;

namespace MGI.Channel.DMS.Server.Data
{
	[DataContract]
	public class Terminal
	{
		[DataMember]
		public long Id { get; set; }
		[DataMember]
		public Guid rowguid { get; set; }
		[DataMember]
		public string Name { get; set; }
		[DataMember]
		public string MacAddress { get; set; }
		[DataMember]
		public string IpAddress { get; set; }
		[DataMember]
		public Location Location { get; set; }
		[DataMember]
		public NpsTerminal PeripheralServer { get; set; }
        //[DataMember]
        //public DateTime DTCreate { get; set; }
        //[DataMember]
        //public DateTime? DTLastMod { get; set; }
		[DataMember]
		public ChannelPartner ChannelPartner { get; set; }

		override public string ToString()
		{
			string str = string.Empty;
			str = string.Concat(str, "Id = ", Id, "\r\n");
			str = string.Concat(str, "rowguid = ", rowguid, "\r\n");
			str = string.Concat(str, "Name = ", Name, "\r\n");
			str = string.Concat(str, "MacAddress = ", MacAddress, "\r\n");
			str = string.Concat(str, "IpAddress = ", IpAddress, "\r\n");
			str = string.Concat(str, "Location = ", Location, "\r\n");
			str = string.Concat(str, "PeripheralServer = ", PeripheralServer, "\r\n");
            //str = string.Concat(str, "DTCreate = ", DTCreate, "\r\n");
            //str = string.Concat(str, "DTLastMod = ", DTLastMod, "\r\n");
			str = string.Concat(str, "ChannelPartner = ", ChannelPartner, "\r\n");
			return str;
		}
	}
}
