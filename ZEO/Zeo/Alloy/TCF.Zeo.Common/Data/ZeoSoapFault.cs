using System.Runtime.Serialization;


namespace TCF.Zeo.Common.Data
{
    [DataContract]
    public class ZeoSoapFault
    {
        [DataMember]
        public string ProductCode { get; set; }

        [DataMember]
        public string ProviderCode { get; set; }

        [DataMember]
        public string ErrorCode { get; set; }

        [DataMember]
        public string MajorCode { get; set; }

        [DataMember]
        public string MinorCode { get; set; }

        [DataMember]
        public string ProviderId { get; set; }

        [DataMember]
        public string ProviderErrorCode { get; set; }

        [DataMember]
        public string ProviderErrorMessage { get; set; }

        [DataMember]
        public string Details { get; set; }

        [DataMember]
        public string StackTrace { get; set; }

        [DataMember]
        public string AddlDetails { get; set; }

        [DataMember]
        public string Processor { get; set; }
    }
}
