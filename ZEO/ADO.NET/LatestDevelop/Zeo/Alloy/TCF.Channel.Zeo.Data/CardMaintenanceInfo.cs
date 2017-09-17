using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TCF.Channel.Zeo.Data
{
    [DataContract]
    public class CardMaintenanceInfo
    {
        [DataMember]
        public string CardStatus { get; set; }

        [DataMember]
        public string ShippingType { get; set; }

        [DataMember]
        public string CardNumber { get; set; }

        [DataMember]
        public string SelectedCardStatus { get; set; }
    }
}
