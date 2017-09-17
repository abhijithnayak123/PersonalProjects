using System.Collections.Generic;

namespace MGI.Peripheral.Printer.Contract
{
	public class PrintData
	{
		public PrintData()
		{
			ReceiptData = new List<string>();
		}

		public string receiptType { get; set; }
		public byte[] logo { get; set; }
		public string kioskID { get; set; }
		public string approverAuthority { get; set; }
		public string kioskAddress { get; set; }

		public List<string> ReceiptData { get; set; }
	}
}
