using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace MGI.Channel.Shared.Server.Data
{
    [DataContract]
    public class BannerMessage
    {
        [DataMember]
        public List<XferMasterData> Message { get; set; }

        [DataMember]
        public Error ExceptionError { get; set; }
    }
}
