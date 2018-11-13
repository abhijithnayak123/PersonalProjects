using System.Collections.Generic;
using System.Runtime.Serialization;
namespace MGI.Biz.Partner.Data
{
    [DataContract]
    public class CheckPrint
    {
        [DataMember]
        public List<string> Lines;
    }
}
