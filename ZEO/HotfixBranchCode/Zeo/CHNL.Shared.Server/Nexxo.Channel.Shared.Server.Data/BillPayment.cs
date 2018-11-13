using System.Collections.Generic;
using System.Runtime.Serialization;
using MGI.Common.Util;

namespace MGI.Channel.Shared.Server.Data
{
	[DataContract]
	public class BillPayment
	{
		[DataMember]
		public decimal PaymentAmount { get; set; }
		[DataMember]
		public decimal Fee { get; set; }
		[DataMember]
		public string BillerName { get; set; }
		[DataMember]
		public string AccountNumber { get; set; }
		[DataMember]
		public string CouponCode { get; set; }
		[DataMember]
		public long BillerId { get; set; }
		[DataMember]
		public Dictionary<string, object> MetaData { get; set; }

		override public string ToString()
		{
			string str = string.Empty;
			str = string.Concat(str, "paymentAmount = ", PaymentAmount, "\r\n");
			str = string.Concat(str, "fee = ", Fee, "\r\n");
			str = string.Concat(str, "BillerName = ", BillerName, "\r\n");
            str = string.Concat(str, "AccountNumber = ", AccountNumber.IsCreditCardNumber() ? "*****" + NexxoUtil.getLastFour(AccountNumber) : AccountNumber, "\r\n");
			str = string.Concat(str, "CouponCode	 = ", CouponCode, "\r\n");
			str = string.Concat(str, "BillerId	 = ", BillerId, "\r\n");
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
