using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Channel.Zeo.Data
{
    [DataContract]
    public class CheckFrankingDetails
    {
        [DataMember]
        public string FrankData { get; set; }
        [DataMember]
        public decimal Amount { get; set; }
        [DataMember]
        public string MICR { get; set; }
    }
}
