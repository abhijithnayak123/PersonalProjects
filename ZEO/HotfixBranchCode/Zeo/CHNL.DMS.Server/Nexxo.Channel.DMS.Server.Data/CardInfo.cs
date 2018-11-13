using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MGI.Channel.DMS.Server.Data
{
	[DataContract]
	public class CardInfo
	{
		[DataMember]
		public decimal Balance { get; set; }
		[DataMember]
		public int CardStatus { get; set; }
		[DataMember]
		public DateTime? ClosureDate { get; set; }
		[DataMember]
		public Dictionary<string, object> MetaData { get; set; }

		public override string ToString()
		{
			return base.ToString();
		}
	}
}
