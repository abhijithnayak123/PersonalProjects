using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Channel.Zeo.Data
{
    [DataContract]
    public class ValidateResponse
    {
        [DataMember]
        public long TransactionId { get; set; }
        [DataMember]
        public bool HasLPMTError { get; set; }
    }
}
