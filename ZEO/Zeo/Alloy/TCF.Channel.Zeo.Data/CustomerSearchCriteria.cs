using TCF.Zeo.Common.Data;
using TCF.Zeo.Common.Util;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using static TCF.Zeo.Common.Util.Helper;

namespace TCF.Channel.Zeo.Data
{
    [DataContract]
    public class CustomerSearchCriteria : BaseRequest
    {
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public DateTime DateOfBirth { get; set; }
        [DataMember]
        public string CardNumber { get; set; }
        [DataMember]
        public string SSN { get; set; }
        [DataMember]
        public string AccountNumber { get; set; }
        [DataMember]
        public CardType CardType { get; set; }
        [DataMember]
        public bool IsAutoSearch { get; set; }
    }
}
