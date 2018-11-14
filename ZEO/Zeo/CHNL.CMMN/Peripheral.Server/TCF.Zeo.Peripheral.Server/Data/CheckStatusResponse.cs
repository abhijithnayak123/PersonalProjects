using System;
using System.Runtime.Serialization;

namespace TCF.Zeo.Peripheral.Server.Data
{
    [DataContract]
    public class CheckStatusResponse
    {
        [DataMember]
        public int TicketNo { get; set; }

        [DataMember]
        public bool HasNewMessage { get; set; }
    }
}
