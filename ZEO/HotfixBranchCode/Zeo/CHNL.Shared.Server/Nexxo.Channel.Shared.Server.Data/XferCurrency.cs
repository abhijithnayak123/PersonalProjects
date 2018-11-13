using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace MGI.Channel.Shared.Server.Data
{
    [DataContract]
    public class XferCurrency
    {
        public XferCurrency()
        {
        }
        [DataMember]
        public decimal ExchangeRate { get; set; }
        [DataMember]
        public string DestinationCurrencyCode { get; set; }
        [DataMember]
        public string DestinationCountryCode { get; set; }
       
    }
}
