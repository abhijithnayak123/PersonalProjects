using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TCF.Channel.Zeo.Data
{
    [DataContract]
    public class ProcessorCredential
    {
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
		public string Identifier { get; set; }
		[DataMember]
		public long ProviderId { get; set; }

        override public string ToString()
        {
            string str = string.Empty;
            str = string.Concat(str, "User Name = ", UserName, "\r\n");
            str = string.Concat(str, "Password = ", Password, "\r\n");
			str = string.Concat(str, "Identifier = ", Identifier, "\r\n");
			str = string.Concat(str, "ProviderId = ", ProviderId, "\r\n");
            return str;
        }
    }
}
