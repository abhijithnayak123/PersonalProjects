using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace MGI.Peripheral.Server.Data
{
    public class CheckPrintRequest
    {
        [DataMember]
        public String CheckData { get; set; }

        [DataMember]
        public Guid UniqueId { get; set; }
    }
}
