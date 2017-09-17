using System.Runtime.Serialization;

namespace TCF.Channel.Zeo.Data
{
    [DataContract]
    public class KeyValuePair
    {
        [DataMember]
        public string Key { set; get; }
        [DataMember]
        public object Value { set; get; }
    }
}
