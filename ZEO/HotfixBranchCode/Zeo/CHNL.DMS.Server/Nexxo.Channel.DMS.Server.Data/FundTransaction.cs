using System.Runtime.Serialization;
using MGI.Common.Util;
using System.Collections.Generic;

namespace MGI.Channel.DMS.Server.Data
{
	[DataContract]
	public class FundTransaction
	{
		[DataMember]
		public int FundType { get; set; }
		[DataMember]
		public string CardNumber { get; set; }
		[DataMember]
		public decimal Amount { get; set; }
		[DataMember]
		public decimal Fee { get; set; }
		[DataMember]
		public decimal CardBalance { get; set; }
		[DataMember]
		public string ConfirmationNumber { get; set; }
		[DataMember]
		public int ProviderId { get; set; }
		[DataMember]
		public string PromoCode { get; set; }

		[DataMember]
		public Dictionary<string, object> MetaData { set; get; }
		override public string ToString()
		{
			string str = string.Empty;
			str = string.Concat(str, "FundType = ", FundType, "\r\n");
			str = string.Concat(str, "CardNumber = ", NexxoUtil.cardLastFour(CardNumber), "\r\n");
			str = string.Concat(str, "Amount = ", Amount, "\r\n");
			str = string.Concat(str, "Fee = ", Fee, "\r\n");
			str = string.Concat(str, "CardBalance = ", CardBalance, "\r\n");
			str = string.Concat(str, "ConfirmationNumber = ", ConfirmationNumber, "\r\n");
			str = string.Concat(str, "PromoCode = ", PromoCode, "\r\n");

			if (MetaData != null)
			{
				foreach (KeyValuePair<string, object> meta in MetaData)
				{
					str = string.Concat(str, meta.Key + "=", meta.Value, "\r\n");
				}
			}
			return str;
		}
	}
}
