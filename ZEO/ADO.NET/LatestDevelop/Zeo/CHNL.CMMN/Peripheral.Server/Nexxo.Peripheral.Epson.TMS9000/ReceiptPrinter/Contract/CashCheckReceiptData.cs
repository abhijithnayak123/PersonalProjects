using System;

namespace MGI.Peripheral.Printer.Contract
{
	public class CashCheckReceiptData : PrintData
	{
		//Cash Check Data
		public string transactionDateTime { get; set; }
		public string checkAmount { get; set; }
		public string amountPaid { get; set; }
		public string feeAmount { get; set; }
		public string totalAmount { get; set; }
		public string cardNumber { get; set; }
		public string cardHolderName { get; set; }
		public string cardBalance { get; set; }
		public string receiptNumber { get; set; }
	}
}
