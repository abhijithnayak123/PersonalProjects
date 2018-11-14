using System.Runtime.Serialization;

namespace MGI.Channel.Shared.Server.Data
{
	[DataContract]
	public class FundsProcessorAccount
	{
		public FundsProcessorAccount() { }

		[DataMember]
		public string CardNumber { get; set; }
		[DataMember]
		public string AccountNumber { get; set; }
		[DataMember]
		public decimal CardBalance { get; set; }
		[DataMember]
		public string Resolution { get; set; }
		[DataMember]
		public int FraudScore { get; set; }

		[DataMember]
		public string ProxyId { get; set; }
		[DataMember]
		public string PseudoDDA { get; set; }
		[DataMember]
		public string ExpirationDate { get; set; }

		override public string ToString()
		{
			string str = string.Empty;
			//str = string.Concat(str, "CardNumber = ", CardNumber, "\r\n");
			str = string.Concat(str, "AccountNumber = ", AccountNumber, "\r\n");
			str = string.Concat(str, "CardBalance = ", CardBalance, "\r\n");
			str = string.Concat(str, "Resolution = ", Resolution, "\r\n");
			str = string.Concat(str, "FraudScore = ", FraudScore, "\r\n");
			str = string.Concat(str, "ProxyId = ", ProxyId, "\r\n");
			str = string.Concat(str, "PseudoDDA = ", PseudoDDA, "\r\n");
			str = string.Concat(str, "ExpirationDate = ", ExpirationDate, "\r\n");
			return str;
		}
	}
}