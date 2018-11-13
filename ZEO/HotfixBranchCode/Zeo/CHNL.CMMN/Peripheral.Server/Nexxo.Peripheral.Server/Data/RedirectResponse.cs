using System.Runtime.Serialization;

namespace MGI.Peripheral.Server.Data
{
	[DataContract]
	public class RedirectResponse
	{
        [DataMember]
        public string RedirectHost{ get; set; }
    }
}
