using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TCF.Channel.Zeo.Data
{
    [DataContract]
    public class CheckLogin
    {
        [DataMember]
        public string URL { get; set; }
        [DataMember]
        public string CompanyToken { get; set; }
        [DataMember]
        public int EmployeeId { get; set; }
        [DataMember]
        public int BranchId { get; set; }
        [DataMember]
        public int ChannelPartnerId { get; set; }
        [DataMember]
        public string LocationIdentifier { get; set; }
    }
}
