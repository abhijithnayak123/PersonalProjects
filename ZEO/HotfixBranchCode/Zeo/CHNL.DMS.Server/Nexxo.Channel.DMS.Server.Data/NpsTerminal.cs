using System;
using System.Runtime.Serialization;

namespace MGI.Channel.DMS.Server.Data
{
	[DataContract]
	public class NpsTerminal
	{
		[DataMember]
		public virtual long Id { get; set; }
		[DataMember]
		public virtual Guid rowguid { get; set; }
		[DataMember]
		public virtual string Name { get; set; }
		[DataMember]
		public virtual string Status { get; set; }
		[DataMember]
		public virtual string Description { get; set; }
		[DataMember]
		public virtual string IpAddress { get; set; }
		[DataMember]
		public string PeripheralServiceUrl { get; set; }
		[DataMember]
		public virtual string Port { get; set; }
		[DataMember]
		public virtual Location Location { get; set; }
        //[DataMember]
        //public virtual DateTime DTCreate { get; set; }
        //[DataMember]
        //public virtual DateTime? DTLastMod { get; set; }
		[DataMember]
		public virtual ChannelPartner ChannelPartner { get; set; }

		override public string ToString()
		{
			string str = String.Empty;
			str = String.Concat(str, "Id = ", Id, "\r\n");
			str = String.Concat(str, "rowguid = ", rowguid, "\r\n");
			str = String.Concat(str, "Name = ", Name, "\r\n");
			str = String.Concat(str, "Status = ", Status, "\r\n");
			str = String.Concat(str, "Description = ", Description, "\r\n");
			str = String.Concat(str, "IpAddress = ", IpAddress, "\r\n");
			str = String.Concat(str, "PeripheralServiceUrl = ", PeripheralServiceUrl, "\r\n");
			str = String.Concat(str, "Port = ", Port, "\r\n");
			str = String.Concat(str, "Location = ", Location, "\r\n");
            //str = String.Concat(str, "DTCreate = ", DTCreate, "\r\n");
            //str = String.Concat(str, "DTLastMod = ", DTLastMod, "\r\n");
			return str;
		}
	}
}
