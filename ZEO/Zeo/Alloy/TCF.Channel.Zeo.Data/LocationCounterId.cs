using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TCF.Channel.Zeo.Data
{
    [DataContract]
    public class LocationCounterId 
	{
        [DataMember]
        public virtual int LocationId { get; set; }
        [DataMember]
        public virtual int ProviderId { get; set; }
        [DataMember]
        public virtual string CounterId { get; set; }
        [DataMember]
        public virtual bool IsAvailable { get; set; }
	}
}
