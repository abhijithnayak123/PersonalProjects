using System.Runtime.Serialization;

namespace MGI.Channel.Shared.Server.Data
{
	[DataContract]
	public class ReasonRequest
	{
		[DataMember]
		public string TransactionType { get; set; }

		override public string ToString()
		{
			string str = string.Empty;
			str = string.Concat(str, "TransactionType = ", TransactionType, "\r\n");
			return str;
		}
	}
}
