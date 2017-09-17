using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace MGI.Channel.DMS.Server.Data
{
	[DataContract]
	public class CardMaintenanceInfo
	{
		[DataMember]
		public string CardStatus { get; set; }

		[DataMember]
		public string ShippingType { get; set; }

		[DataMember]
		public string CardNumber { get; set; }

		[DataMember]
		public string SelectedCardStatus { get; set; }

		public override string ToString()
		{
			string str = string.Empty;
			str = string.Concat(str, "CardStatus = ", CardStatus, "\r\n");
			str = string.Concat(str, "ShippingType = ", ShippingType, "\r\n");
			str = string.Concat(str, "CardNumber = ", CardNumber, "\r\n");
			str = string.Concat(str, "SelectedCardStatus = ", SelectedCardStatus, "\r\n");
			return str;
		}
	}
}
