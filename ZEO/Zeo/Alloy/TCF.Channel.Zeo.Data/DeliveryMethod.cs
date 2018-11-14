using System.Runtime.Serialization;

namespace TCF.Channel.Zeo.Data
{
    [DataContract]
    public class DeliveryMethod
    {
        [DataMember]
        public decimal FeeAmount { get; set; }
        [DataMember]
        public decimal Tax { get; set; }
        [DataMember]
        public string Code { get; set; }
        [DataMember]
        public string Text { get; set; }
    }
}
