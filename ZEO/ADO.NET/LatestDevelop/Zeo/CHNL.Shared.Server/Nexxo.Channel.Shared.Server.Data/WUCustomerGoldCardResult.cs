using System;
using System.Runtime.Serialization;

namespace MGI.Channel.Shared.Server.Data
{
	[DataContract]
	public class WUCustomerGoldCardResult
	{
		public WUCustomerGoldCardResult() { }

		[DataMember]
		public string FullName { get; set; }

		[DataMember]
		public string Address { get; set; }

		[DataMember]
		public string ZipCode { get; set; }

		[DataMember]
		public string PhoneNumber { get; set; }

		[DataMember]
		public string WUGoldCardNumber { get; set; }		

		override public string ToString()
		{
			string str = string.Empty;
			str = string.Concat(str, "FullName = ", FullName, "\r\n");
			str = string.Concat(str, "Address = ", Address, "\r\n");
			str = string.Concat(str, "ZipCode = ", ZipCode, "\r\n");
			str = string.Concat(str, "PhoneNumber = ", PhoneNumber, "\r\n");
			str = string.Concat(str, "WUGoldCardNumber = ", WUGoldCardNumber, "\r\n");			
			return str;
		}
	}
}
