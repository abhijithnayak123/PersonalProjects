using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TCF.Channel.Zeo.Data
{
    [DataContract]
    public class MoneyOrderCheckPrint
    {
        [DataMember]
        public List<string> Lines;
    }
}
