using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace MGI.Channel.DMS.NpsInstallerServer.Data
{
    [DataContract]
    public class ChannelPartner
    {
        [DataMember]
        public virtual Guid rowguid { get; set; }

        [DataMember]
        public virtual long Id { get; set; }

        [DataMember]
        public virtual string Name { get; set; }

        public override string ToString()
        {
            StringBuilder str = new StringBuilder();
            str.Append("RowGuid = " + rowguid + "\r\n");
            str.Append("Id = " + Id + "\r\n");
            str.Append("ChannelPartner Name = " + Name + "\r\n");
            return str.ToString();
        }
    }
}
