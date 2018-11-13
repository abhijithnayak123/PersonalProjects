using MGI.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace MGI.Channel.DMS.Server.Data
{
	[DataContract]
	public class ChannelPartnerProductProvider
	{
		[DataMember]
		public string ProductName { get; set; }
		[DataMember]
		public string ProcessorName { get; set; }
		[DataMember]
		public long ProcessorId { get; set; }
		[DataMember]
		public int Sequence { get; set; }
		[DataMember]
		public bool IsSSNRequired { get; set; }
		[DataMember]
		public bool IsSWBRequired { get; set; }
		[DataMember]
		public bool IsTnCForcePrintRequired { get; set; }
		[DataMember]
		public bool CanParkReceiveMoney { get; set; }
		[DataMember]
		public CheckEntryTypes CheckEntryType { get; set; }
		[DataMember]
		public int MinimumTransactAge { get; set; }

		override public string ToString()
		{
			string str = string.Empty;
			str = string.Concat(str, "ProductName = ", ProductName, "\r\n");
			str = string.Concat(str, "ProcessorName = ", ProcessorName, "\r\n");
			str = string.Concat(str, "ProcessorId = ", ProcessorId, "\r\n");
			str = string.Concat(str, "Sequence = ", Sequence, "\r\n");
			str = string.Concat(str, "IsSSNRequired = ", IsSSNRequired, "\r\n");
			str = string.Concat(str, "IsSWBRequired = ", IsSWBRequired, "\r\n");
			str = string.Concat(str, "IsTnCForcePrintRequired=", IsTnCForcePrintRequired, "\r\n");
			str = string.Concat(str, "CanParkReceiveMoney=", CanParkReceiveMoney, "\r\n");
			str = string.Concat(str, "CheckEntryType=", CheckEntryType, "\r\n");
			str = string.Concat(str, "MinimumTransactAge", MinimumTransactAge, "\r\n");
			return str;
		}
	}
}
