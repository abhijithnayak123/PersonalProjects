using System.Runtime.Serialization;

namespace TCF.Channel.Zeo.Data
{
	[DataContract]
	public class CheckTransaction
	{
		[DataMember]
		public string CheckType { get; set; }
		[DataMember]
		public decimal Amount { get; set; }
		[DataMember]
		public decimal Fee { get; set; }
		[DataMember]
		public string ConfirmationNumber { get; set; }

		override public string ToString()
		{
			string str = string.Empty;
			str = string.Concat(str, "CheckType = ", CheckType, "\r\n");
			str = string.Concat(str, "Amount = ", Amount, "\r\n");
			str = string.Concat(str, "Fee = ", Fee, "\r\n");
			str = string.Concat(str, "ConfirmationNumber = ", ConfirmationNumber, "\r\n");
			return str;
		}
	}
}
