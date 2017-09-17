using System.Runtime.Serialization;

namespace MGI.Channel.DMS.Server.Data
{
	public class CheckType
	{
		[DataMember]
		public int Id { get; set; }
		[DataMember]
		public string Name { get; set; }

		override public string ToString()
		{
			string str = string.Empty;
			str = string.Concat(str, "Id = ", Id, "\r\n");
			str = string.Concat(str, "Name = ", Name, "\r\n");
			return str;
		}
	}
}
