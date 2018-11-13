using System.Runtime.Serialization;

namespace MGI.Channel.Shared.Server.Data
{
	[DataContract]
	public class ValidateResponse
	{
		[DataMember]
		public long TransactionId { get; set; }
		[DataMember]
		public bool HasLPMTError { get; set; }
		override public string ToString()
		{
			string str = string.Empty;
			str = string.Concat(str, "TransactionId = ", TransactionId, "\r\n");
			str = string.Concat(str, "HasLPMTError = ", HasLPMTError, "\r\n");
			return str;
		}
	}
}
