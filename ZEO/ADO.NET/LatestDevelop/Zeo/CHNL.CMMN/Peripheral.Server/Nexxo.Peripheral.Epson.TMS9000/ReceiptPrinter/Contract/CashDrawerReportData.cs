using System;

namespace MGI.Peripheral.Printer.Contract
{
	public class CashDrawerReportData : PrintData
	{
		//Cash Drawer Data
		public String cashDrawerDateTime { get; set; }
		public String cashDrawerTellerID { get; set; }
		public String cashDrawerTellerIDList { get; set; }
		public String cashDrawerCashInAmount { get; set; }
		public String cashDrawerCashOutAmount { get; set; }

	}
}
