using System.Runtime.Serialization;

namespace MGI.Channel.DMS.Server.Data
{
	[DataContract]
	public class UserSearchCriteria
	{
		[DataMember]
		public string FirstName { get; set; }
		[DataMember]
		public string LastName { get; set; }
		[DataMember]
		public string LocationName { get; set; }
		[DataMember]
		public string UserName { get; set; }
		[DataMember]
		public long ChannelPartnerId { get; set; }

		override public string ToString()
		{
			string str = string.Empty;
			str = string.Concat(str, "FirstName = ", FirstName, "\r\n");
			str = string.Concat(str, "LastName = ", LastName, "\r\n");
			str = string.Concat(str, "LocationName = ", LocationName, "\r\n");
			str = string.Concat(str, "UserName = ", UserName, "\r\n");
			str = string.Concat(str, "ChannelPartnerId = ", ChannelPartnerId, "\r\n");
			return str;
		}
	}
}
