using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace MGI.Channel.DMS.Server.Data
{
	[DataContract]
	public class TransactionHistoryRequest
	{
		[DataMember]
		public long AliasId { get; set; }
		[DataMember]
		public TransactionStatus TransactionStatus { get; set; }
		[DataMember]
		public int DateRange { get; set; }

		override public string ToString()
		{
			string str = String.Empty;
			str = String.Concat(str, "AliasId = ", AliasId, "\r\n");
			str = String.Concat(str, "TransactionStatus = ", TransactionStatus.ToString(), "\r\n");
			str = String.Concat(str, "DateRange = ", DateRange, "\r\n");
			return str;
		}
	}
}
