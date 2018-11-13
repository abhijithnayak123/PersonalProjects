using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace MGI.Channel.Shared.Server.Data
{
	[DataContract]
	public class BillPayTransaction
	{
		[DataMember]
		public string BillerName { get; set; }
		[DataMember]
		public string AccountNumber { get; set; }
		[DataMember]
		public decimal Amount { get; set; }
		[DataMember]
		public decimal Fee { get; set; }
        [DataMember]
        public int ProviderId { get; set; }
	    [DataMember]
        public Dictionary<string, object> MetaData { get; set; }
        
        [DataMember]
        public string ProviderName { get; set; }
        

		override public string ToString()
		{
			var sb = new StringBuilder();
			sb.AppendLine("BillPay Transaction:");
			sb.AppendLine("	BillerName:" + BillerName);
			sb.AppendLine("	AccountNumber: " + AccountNumber);
			sb.AppendLine(string.Format("	Amount: {0:C}", Amount));
			sb.AppendLine(string.Format("	Fee: {0:C}", Fee));
            sb.AppendLine(" ProviderName: " + ProviderId.ToString());
            if (MetaData != null)
            {
                foreach (KeyValuePair<string, object> meta in MetaData)
                {
                    sb.AppendLine(meta.Key + " :" + meta.Value);
                }
            }
			return sb.ToString();
		}
	}
}
