using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace MGI.Channel.Shared.Server.Data
{
    [DataContract]
    public class BillerInfo
    {
        public BillerInfo() { }
        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public List<decimal> Denominations { get; set; }

        [DataMember]
        public string DeliveryOption { get; set; }

        [DataMember]
        public string BillerState { get; set; }

        [DataMember]
        public string BillerCity { get; set; }

        override public string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("Message = {0} \r\n", Message);
            stringBuilder.AppendFormat("Denominations = \r\n");
            if (Denominations != null)
            {
                foreach (var denomination in Denominations)
                {
                    stringBuilder.AppendFormat("{0} \r\n", denomination);
                }
            }
            stringBuilder.AppendFormat("DeliveryOption = {0} \r\n", DeliveryOption);
            stringBuilder.AppendFormat("BillerState = {0} \r\n", BillerState);
            stringBuilder.AppendFormat("BillerCity = {0} \r\n", BillerCity);
            return stringBuilder.ToString();
        }
    }
}
