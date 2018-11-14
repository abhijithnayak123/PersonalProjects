using System.Runtime.Serialization;

namespace TCF.Zeo.Peripheral.Server.Data
{
	[DataContract]
	public class RedirectResponse
	{
        [DataMember]
        public string RedirectHost{ get; set; }
    }
}
