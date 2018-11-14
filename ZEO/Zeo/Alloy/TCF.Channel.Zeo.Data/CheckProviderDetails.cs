using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using TCF.Zeo.Common.Util;

namespace TCF.Channel.Zeo.Data
{
    [DataContract]
    public class CheckProviderDetails
    {
        [DataMember]
        public int CheckTypeId { get; set; }

        [DataMember]
        public Helper.ProviderId ProductProviderCode { get; set; }
    }
}
