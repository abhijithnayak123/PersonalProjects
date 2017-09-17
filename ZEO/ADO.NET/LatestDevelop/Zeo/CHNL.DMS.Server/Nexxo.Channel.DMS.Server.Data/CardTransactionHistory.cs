using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace MGI.Channel.DMS.Server.Data
{
	[DataContract]
	public class CardTransactionHistory
	{
		[DataMember]
		public DateTime PostedDateTime { get; set; }

		[DataMember]
		public DateTime TransactionDateTime { get; set; }

		[DataMember]
		public string MerchantName { get; set; }

		[DataMember]
		public string Location { get; set; }

		[DataMember]
		public double TransactionAmount { get; set; }

		[DataMember]
		public string TransactionDescription { get; set; }

		[DataMember]
		public string DeclineReason { get; set; }

		[DataMember]
		public double AvailableBalance { get; set; }

		[DataMember]
		public double ActualBalance { get; set; }

		override public string ToString()
		{
			string str = String.Empty;
			str = String.Concat(str, "PostedDateTime = ", PostedDateTime, "\r\n");
			str = String.Concat(str, "TransactionDateTime = ", TransactionDateTime, "\r\n");
			str = String.Concat(str, "MerchantName = ", MerchantName, "\r\n");
			str = String.Concat(str, "Location = ", Location, "\r\n");
			str = String.Concat(str, "TransactionAmount = ", TransactionAmount, "\r\n");
			str = String.Concat(str, "TransactionDescription = ", TransactionDescription, "\r\n");
			str = String.Concat(str, "DeclineReason = ", DeclineReason, "\r\n");
			str = String.Concat(str, "AvailableBalance = ", AvailableBalance, "\r\n");
			str = String.Concat(str, "ActualBalance = ", ActualBalance, "\r\n");
			return str;
		}
	}
}
