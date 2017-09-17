using System.Runtime.Serialization;

namespace TCF.Channel.Zeo.Data
{
    [DataContract]
    public class Phone
    {
        [DataMember]
        public string Number { get; set; }
        [DataMember]
        public string Type { get; set; }
        [DataMember]
        public string Provider { get; set; }
    }

    [DataContract]
    public class Address
    {
        [DataMember]
        public string Address1 { get; set; }
        [DataMember]
        public string Address2 { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string State { get; set; }
        [DataMember]
        public string ZipCode { get; set; }
        [DataMember]
        public string Country { get; set; }
    }
}
