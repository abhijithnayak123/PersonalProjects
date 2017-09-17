using System.Runtime.Serialization;

namespace MGI.Channel.DMS.Server.Data
{
	[DataContract]
	public class Purse
	{
		public Purse() { }

		[DataMember]
		public string PurseName { get; set; }
		[DataMember]
		public int ProcessorId { get; set; }
		[DataMember]
		public string ProcessorAccountId { get; set; }
		[DataMember]
		public string NameOnAccount { get; set; }
		[DataMember]
		public decimal Balance { get; set; }
		[DataMember]
		public string Track2 { get; set; }

		override public string ToString()
		{
			string str = string.Empty;
			str = string.Concat(str, "PurseName = ", PurseName, "\r\n");
			str = string.Concat(str, "ProcessorId = ", ProcessorId, "\r\n");
			str = string.Concat(str, "ProcessorAccountId = ", ProcessorAccountId, "\r\n");
			str = string.Concat(str, "NameOnAccount = ", NameOnAccount, "\r\n");
			str = string.Concat(str, "Balance = ", Balance, "\r\n");
			//str = string.Concat(str, "Track2  = ", Track2, "\r\n");
            str = string.Concat(str, "Track2  After Masking = ", "XXXXXXXXX", "\r\n");
            return str;
		}
	}
}
