using System;
using System.Runtime.Serialization;

namespace MGI.Peripheral.Server.Data
{
	public class CashDrawerPrintRequest : PrintRequest
	{
		[DataMember]
		public string CashDrawerDateTime { get; set; }
		[DataMember]
		public string CashDrawerTellerID { get; set; }
		[DataMember]
		public String CashDrawerTellerIDList { get; set; }
		[DataMember]
		public string CashDrawerCashInAmount { get; set; }
		[DataMember]
		public string CashDrawerCashOutAmount { get; set; }
	}
}
