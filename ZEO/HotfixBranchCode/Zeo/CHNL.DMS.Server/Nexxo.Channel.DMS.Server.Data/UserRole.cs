using System.Runtime.Serialization;

namespace MGI.Channel.DMS.Server.Data
{
	[DataContract]
	public class UserRole
	{
		[DataMember]
		public int Id { get; set; }
		[DataMember]
		public string role { get; set; }

		override public string ToString()
		{
			string str = string.Empty;
			str = string.Concat(str, "Id = ", Id, "\r\n");
			str = string.Concat(str, "role = ", role, "\r\n");
			return str;
		}
	}
}
