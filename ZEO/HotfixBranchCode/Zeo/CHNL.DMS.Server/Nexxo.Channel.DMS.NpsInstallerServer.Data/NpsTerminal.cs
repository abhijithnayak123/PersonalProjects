using System;
using System.Runtime.Serialization;
using System.Text;

namespace MGI.Channel.DMS.NpsInstallerServer.Data
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
        public virtual string Port { get; set; }
        [DataMember]
        public virtual string PeripheralServiceUrl { get; set; }
        [DataMember]
        public virtual Location Location { get; set; }
        [DataMember]
        public virtual ChannelPartner ChannelPartner { get; set; }
        [DataMember]
        public virtual long ChannelPartnerId { get; set; }
        [DataMember]
        public virtual DateTime DTCreate { get; set; }
        [DataMember]
        public virtual DateTime? DTLastMod { get; set; }

        public override string ToString()
        {
            StringBuilder str = new StringBuilder();
            str.Append("RowGuid = " + rowguid + "\r\n");
            str.Append("Id = " + Id + "\r\n");
            str.Append("Name = " + Name + "\r\n");
            str.Append("Status = " + Status + "\r\n");
            str.Append("Description = " + Description + "\r\n");
            str.Append("Ip Address = " + IpAddress + "\r\n");
            str.Append("Port = " + Port + "\r\n");
            str.Append("PeripheralServiceUrl = " + PeripheralServiceUrl + "\r\n");
            str.Append("ChannelPartnerId = " + ChannelPartnerId + "\r\n");
            str.Append("DtCreate = " + DTCreate + "\r\n");
            str.Append("DtLastMod = " + DTLastMod + "\r\n");
            return str.ToString();
        }
	}
}
