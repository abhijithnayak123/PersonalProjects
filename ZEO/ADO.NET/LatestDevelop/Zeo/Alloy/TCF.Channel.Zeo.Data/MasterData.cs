using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Channel.Zeo.Data
{
    [DataContract]
    public class MasterData
    {
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public string Code { get; set; }
        [DataMember]
        public string Name { get; set; }

        override public string ToString()
        {
            string str = string.Empty;
            str = string.Concat(str, "Id = ", Id, "\r\n");
            str = string.Concat(str, "Code = ", Code, "\r\n");
            str = string.Concat(str, "Name = ", Name, "\r\n");
            return str;
        }
    }
}
