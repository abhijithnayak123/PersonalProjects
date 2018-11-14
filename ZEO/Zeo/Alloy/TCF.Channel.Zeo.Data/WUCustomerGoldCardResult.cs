using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Channel.Zeo.Data
{
    [DataContract]
    public class WUCustomerGoldCardResult
    {
        [DataMember]
        public string FullName { get; set; }

        [DataMember]
        public string Address { get; set; }

        [DataMember]
        public string ZipCode { get; set; }

        [DataMember]
        public string PhoneNumber { get; set; }

        [DataMember]
        public string WUGoldCardNumber { get; set; }
    }
}
