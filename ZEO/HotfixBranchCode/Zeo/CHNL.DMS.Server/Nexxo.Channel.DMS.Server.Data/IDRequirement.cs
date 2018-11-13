using System.Runtime.Serialization;

namespace MGI.Channel.DMS.Server.Data
{
	[DataContract]
	public class IDRequirement
	{
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Mask { get; set; }
        [DataMember]
        public bool HasExpirationDate { get; set; }
        [DataMember]
        public string Country { get; set; }
        [DataMember]
        public string State { get; set; }

		override public string ToString()
		{
			string str = string.Empty;
			str = string.Concat(str, "Id = ", Id, "\r\n");
			str = string.Concat(str, "Name = ", Name, "\r\n");
			str = string.Concat(str, "Mask = ", Mask, "\r\n");
			str = string.Concat(str, "HasExpirationDate = ", HasExpirationDate, "\r\n");
			str = string.Concat(str, "Country = ", Country, "\r\n");
			str = string.Concat(str, "State = ", State, "\r\n");
			return str;
		}
	}
}
