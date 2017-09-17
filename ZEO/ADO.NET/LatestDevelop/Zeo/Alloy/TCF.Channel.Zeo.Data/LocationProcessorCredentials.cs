using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TCF.Channel.Zeo.Data
{
    [DataContract]
    public class LocationProcessorCredentials 
    {
        [DataMember]
        public virtual long locationId { get; set; }
        [DataMember]
        public virtual long ProviderId { get; set; }
        [DataMember]
        public virtual string UserName { get; set; }
        [DataMember]
        public virtual string Password { get; set; }
        [DataMember]
        public virtual string Identifier { get; set; }
        [DataMember]
        public virtual string Identifier2 { get; set; }
    }
}
