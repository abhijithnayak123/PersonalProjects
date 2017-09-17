using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace MGI.Channel.Shared.Server.Data
{
    public class Phone
    {
        public Phone() { }
        [DataMember]
        public string Number { get; set; }
        [DataMember]
        public string Type { get; set; }
        [DataMember]
        public string Provider { get; set; }

        override public string ToString()
        {
            string str = string.Empty;
            str = string.Concat(str, "Number = ", Number, "\r\n");
            str = string.Concat(str, "Type = ", Type, "\r\n");
            str = string.Concat(str, "Provider = ", Provider, "\r\n");
            return str;
        }

    }
}
