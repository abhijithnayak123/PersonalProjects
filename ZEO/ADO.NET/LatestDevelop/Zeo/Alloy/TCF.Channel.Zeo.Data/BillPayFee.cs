using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace TCF.Channel.Zeo.Data
{
    public class BillPayFee
    {
        [DataMember]
        public long TransactionId { get; set; }
        [DataMember]
        public string SessionCookie { get; set; }
        [DataMember]
        public string AccountHolderName { get; set; }
        [DataMember]
        public string AvailableBalance { get; set; }
        [DataMember]
        public List<DeliveryMethod> DeliveryMethods { get; set; }
        [DataMember]
        public string CityCode { get; set; }

        override public string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("{0} = \r\n", TransactionId);
            stringBuilder.AppendFormat("{0} = \r\n", SessionCookie);
            stringBuilder.AppendFormat("{0} = \r\n", AccountHolderName);
            stringBuilder.AppendFormat("{0} = \r\n", AvailableBalance);
            stringBuilder.AppendFormat("{0} = \r\n", CityCode);

            if (DeliveryMethods != null)
            {
                foreach (DeliveryMethod item in DeliveryMethods)
                {
                    stringBuilder.AppendFormat("Code:{0} Text:{1} Fee:{2} Tax : {3} \r\n", item.Code, item.Text, item.FeeAmount, item.Tax);
                }
            }
            return stringBuilder.ToString();
        }
    }
}
