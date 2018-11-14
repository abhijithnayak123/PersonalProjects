using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace MGI.Channel.DMS.Server.Data
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
        //Todo:Commented By Sakhatech 
        //[DataMember]
        //public string MTCN { get; set; }
        [DataMember]
        public string ProviderName { get; set; }
        

		override public string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("BillPay Transaction:");
			sb.AppendLine("	BillerName:" + BillerName);
            sb.Append(string.Concat(sb, "AccountNumber = ", FormatCardNumber(AccountNumber), "\r\n"));
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
            //Todo:Commented By Sakhatech 
            //sb.AppendLine(" Location: " + MTCN);
		}

        private string FormatCardNumber(string str)
        {
            if (str != null && str.Length > 15)
            {
                return string.Format("{0} **** **** {1}", AccountNumber.Substring(0, 4), AccountNumber.Substring(12, 4));
            }
            return str;
        }
	}
}
