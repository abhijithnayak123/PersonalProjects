using System.Runtime.Serialization;

namespace MGI.Channel.DMS.Server.Data
{
	[DataContract]
	public class UserStatus
	{
		[DataMember]
		public int Id { get; set; }
		[DataMember]
		public string Status { get; set; }

		override public string ToString()
		{
			string str = string.Empty;
			str = string.Concat(str, "Id = ", Id, "\r\n");
			str = string.Concat(str, "Status = ", Status, "\r\n");
			return str;
		}
	}
}
