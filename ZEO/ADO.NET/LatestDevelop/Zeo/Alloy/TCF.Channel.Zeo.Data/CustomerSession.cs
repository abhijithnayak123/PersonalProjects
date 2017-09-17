using TCF.Zeo.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Channel.Zeo.Data
{
    [DataContract]
    public class CustomerSession
    {
        [DataMember]
        public long CustomerSessionId { get; set; }
        [DataMember]
        public long CustomerId { get; set; }
        [DataMember]
        public bool CardPresent { get; set; }
        [DataMember]
        public string TimezoneID { get; set; }
        [DataMember]
        public bool IsGPRCustomer { get; set; }
        [DataMember]
        public string CardNumber { get; set; }
        [DataMember]
        public string TipsAndOffers { get; set; }
        [DataMember]
        public Helper.ProfileStatus ProfileStatus { get; set; }
        [DataMember]
        public CustomerProfile Customer { get; set; }
        [DataMember]
        public long CartId { get; set; }
    }
}
