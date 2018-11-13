using System.Runtime.Serialization;
using System;
using MGI.Common.Util;

namespace MGI.Channel.Shared.Server.Data
{
	[DataContract]
	public class CustomerAuthentication
	{
		public CustomerAuthentication() { }

		[DataMember]
		public long AlloyID { get; set; }
		[DataMember]
		public string CardNumber { get; set; }
		[DataMember]
		public string PhoneNumber { get; set; }
		[DataMember]
		public string Track2 { get; set; }
		[DataMember]
		public string PINBlock { get; set; }

		override public string ToString()
		{
			string str = string.Empty;
			str = string.Concat(str, "AlloyID = ", AlloyID.ToString().Substring(0, 6) + "XXXXXX" + AlloyID.ToString().Substring(AlloyID.ToString().Length - 4, 4), "\r\n");
			str = string.Concat(str, "Cardnumber = ", NexxoUtil.cardLastFour(CardNumber), "\r\n");
			str = string.Concat(str, "PhoneNumber = ", PhoneNumber, "\r\n");
			//str = string.Concat(str, "Track2 = ", Track2, "\r\n");
            str = string.Concat(str, "Track2 After Masking :{0} ", "*****", "\r\n");
			//str = string.Concat(str, "PINBlock = ", PINBlock, "\r\n");
            str = string.Concat(str, "PINBlock After Masking :{0}","****","\r\n");
			return str;
           
		}
	}
}
