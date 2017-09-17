using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace MGI.Channel.DMS.Server.Data
{
    [DataContract]
    public enum ErrorType
    {
        [EnumMember]
        INFO,
        [EnumMember]
        WARNING,
        [EnumMember]
        ERROR
    }
}
