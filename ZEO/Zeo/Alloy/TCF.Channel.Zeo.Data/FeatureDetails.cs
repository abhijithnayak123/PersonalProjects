using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Channel.Zeo.Data
{
    [DataContract]
    public class FeatureDetails
    {
        [DataMember]
        public int FeatureId { get; set; }

        [DataMember]
        public string FeatureName { get; set; }

        [DataMember]
        public bool IsEnabled { get; set; }
    }
}
