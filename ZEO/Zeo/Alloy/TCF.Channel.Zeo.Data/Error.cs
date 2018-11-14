#region System References
using System.Runtime.Serialization;
#endregion

namespace TCF.Channel.Zeo.Data
{
    [DataContract]
    public class Error
    {
        [DataMember]
        public string ProductCode { get; set; }
        [DataMember]
        public string ProviderCode { get; set; }
        [DataMember]
        public string ErrorCode { get; set; }
        [DataMember]
        public string Provider { get; set; }
        [DataMember]
        public string Details { get; set; }
        [DataMember]
        public bool IsHardStop { get; set; } = false;
        [DataMember]
        public string MajorCode { get; set; }
        [DataMember]
        public string MinorCode { get; set; }
        [DataMember]
        public string Processor { get; set; }
        [DataMember]
        public string Exception { get; set; }
    }
}
