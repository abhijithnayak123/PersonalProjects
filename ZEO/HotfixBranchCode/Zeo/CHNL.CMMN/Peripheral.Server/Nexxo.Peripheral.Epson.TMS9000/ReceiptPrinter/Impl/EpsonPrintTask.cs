using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MGI.Peripheral.Printer.Contract;

namespace MGI.Peripheral.Printer.EpsonTMS9000.Impl
{
	public partial class TMS9000Base
	{
		private const String NEW_LINE = "\r\n";
		public struct PrintDefinition
		{
			public String type;
			public int fontType;         // 0 Not Bold, 1 Bold
			public int alignment;       // 0 = Left, 1 = Center, 2 = Right
			public int spacersRequired; // 0 or 1
			public int printIfNull;
			public String text1;
			public String text2;

			public PrintDefinition(String type, int fontType, int alignment, int spacersRequired, int printIfNull, String text1, String text2)
			{
				this.type = type;
				this.fontType = fontType;
				this.alignment = alignment;
				this.spacersRequired = spacersRequired;
				this.printIfNull = printIfNull;
				this.text1 = text1;
				this.text2 = text2;
			}
		};

		public IList<PrintDefinition> headerPrintList = new ReadOnlyCollection<PrintDefinition>
	   (new[]{
            new PrintDefinition ("LOGO",   0, 1, 0 , 1, String.Empty, String.Empty),
            new PrintDefinition ("STATIC", 0, 1, 0 , 1, NEW_LINE, String.Empty),
            new PrintDefinition ("TEXT",   1, 1, 0 , 1, "NEXXO KIOSK", "KIOSK_ID"),
            new PrintDefinition ("STATIC", 0, 1, 0 , 1, NEW_LINE, String.Empty),
            new PrintDefinition ("TEXT",   0, 1, 0 , 1, "NEXXO - ", "APPROVER_AUTHORITY"),
            new PrintDefinition ("STATIC", 0, 1, 0 , 1, NEW_LINE, String.Empty),
            new PrintDefinition ("TEXT",   0, 1, 0 , 0, String.Empty, "KIOSK_ADDRESS"),
            new PrintDefinition ("STATIC", 0, 1, 0 , 1, NEW_LINE, String.Empty),
        });

		public IList<PrintDefinition> footerPrintList = new ReadOnlyCollection<PrintDefinition>
	   (new[]{
            new PrintDefinition ("STATIC", 0, 1, 0 , 1, NEW_LINE, String.Empty),
            new PrintDefinition ("STATIC", 0, 1, 0 , 1, NEW_LINE, String.Empty),
            new PrintDefinition ("STATIC", 1, 1, 0 , 1, "Nexxo Customer Service", String.Empty),
            new PrintDefinition ("STATIC", 0, 1, 0 , 1, NEW_LINE, String.Empty),
            new PrintDefinition ("STATIC", 1, 1, 0 , 1, "Toll Free 877-887-0682", String.Empty),
            new PrintDefinition ("STATIC", 0, 1, 0 , 1, NEW_LINE, String.Empty),
            new PrintDefinition ("STATIC", 0, 1, 0 , 1, NEW_LINE, String.Empty),
            new PrintDefinition ("TEXT",   0, 1, 0 , 1, "", "CURRENT_TIME"),
            new PrintDefinition ("STATIC", 0, 1, 0 , 1, NEW_LINE, String.Empty),
        });


		public IList<PrintDefinition> cashCheckPrintList = new ReadOnlyCollection<PrintDefinition>
	   (new[]{
            new PrintDefinition ("HEADER", 0, 1, 0 , 1, String.Empty, String.Empty),
            new PrintDefinition ("STATIC", 0, 1, 0 , 1, NEW_LINE, String.Empty),
            new PrintDefinition ("STATIC", 1, 1, 0 , 1, "Card Load from Check", String.Empty),
            new PrintDefinition ("STATIC", 0, 1, 0 , 1, NEW_LINE, String.Empty),
            new PrintDefinition ("STATIC", 0, 1, 0 , 1, NEW_LINE, String.Empty),
            new PrintDefinition ("STATIC", 0, 1, 0 , 1, NEW_LINE, String.Empty),
            new PrintDefinition ("TEXT",   0, 0, 1 , 1, "Transaction Date : ", "TRANSACTION_DATE"),
            new PrintDefinition ("STATIC", 0, 1, 0 , 1, NEW_LINE, String.Empty),
            new PrintDefinition ("TEXT",   0, 0, 1 , 1, "Check Amount : ", "CHECK_AMOUNT"),
            new PrintDefinition ("STATIC", 0, 1, 0 , 1, NEW_LINE, String.Empty),
            new PrintDefinition ("TEXT",   0, 0, 1 , 1, "Fee Amount : ", "FEE_AMOUNT"),
            new PrintDefinition ("STATIC", 0, 1, 0 , 1, NEW_LINE, String.Empty),
            new PrintDefinition ("TEXT",   0, 0, 1 , 1, "Total : ", "TOTAL"),
            new PrintDefinition ("STATIC", 0, 1, 0 , 1, NEW_LINE, String.Empty),
            new PrintDefinition ("STATIC", 0, 1, 0 , 1, NEW_LINE, String.Empty),
            new PrintDefinition ("TEXT",   0, 0, 1 , 1, "Card Holder : ", "CARD_HOLDER"),
            new PrintDefinition ("STATIC", 0, 1, 0 , 1, NEW_LINE, String.Empty),
            new PrintDefinition ("TEXT",   0, 0, 1 , 1, "Balance : ", "BALANCE"),
            new PrintDefinition ("STATIC", 0, 1, 0 , 1, NEW_LINE, String.Empty),
            new PrintDefinition ("TEXT",   0, 0, 1 , 1, "Card Number : ", "CARD_NUMBER"),
            new PrintDefinition ("STATIC", 0, 1, 0 , 1, NEW_LINE, String.Empty),
            new PrintDefinition ("STATIC", 0, 1, 0 , 1, NEW_LINE, String.Empty),
            new PrintDefinition ("FOOTER", 0, 1, 0 , 1, String.Empty, String.Empty),
            new PrintDefinition ("STATIC", 0, 1, 0 , 1, NEW_LINE, String.Empty),
            new PrintDefinition ("TEXT",   0, 1, 0 , 1, "Receipt Number : ", "RECEIPT_NUMBER"),
            new PrintDefinition ("STATIC", 0, 1, 0 , 1, NEW_LINE, String.Empty),
            new PrintDefinition ("STATIC", 0, 1, 0 , 1, NEW_LINE, String.Empty),
            new PrintDefinition ("STATIC", 0, 1, 0 , 1, NEW_LINE, String.Empty),
        });


		public IList<PrintDefinition> cashDrawerPrintList = new ReadOnlyCollection<PrintDefinition>
	   (new[]{
            new PrintDefinition ("HEADER", 0, 1, 0 , 1, String.Empty, String.Empty),
            new PrintDefinition ("STATIC", 0, 1, 0 , 1, NEW_LINE, String.Empty),
            new PrintDefinition ("STATIC", 1, 1, 0 , 1, "Cash Drawer Report", String.Empty),
            new PrintDefinition ("STATIC", 0, 1, 0 , 1, NEW_LINE, String.Empty),
            new PrintDefinition ("STATIC", 0, 1, 0 , 1, NEW_LINE, String.Empty),
            new PrintDefinition ("STATIC", 0, 1, 0 , 1, NEW_LINE, String.Empty),
            new PrintDefinition ("TEXT",   0, 0, 1 , 1, "Date Time of Report : ", "CASHDRAWER_REPORT_DATE"),
            new PrintDefinition ("STATIC", 0, 1, 0 , 1, NEW_LINE, String.Empty),
            new PrintDefinition ("TEXT",   0, 0, 1 , 1, "Teller ID generating report:", "TELLER_ID"),
            new PrintDefinition ("STATIC", 0, 1, 0 , 1, NEW_LINE, String.Empty),
            new PrintDefinition ("STATIC", 0, 0, 0 , 1, "Teller ID's logged in since last report:", String.Empty),
            new PrintDefinition ("STATIC", 0, 1, 0 , 1, NEW_LINE, String.Empty),
            new PrintDefinition ("TEXT",   0, 0, 0 , 0, String.Empty, "TELLER_ID_LIST"),
            new PrintDefinition ("STATIC", 0, 1, 0 , 1, NEW_LINE, String.Empty),
            new PrintDefinition ("STATIC", 0, 1, 0 , 1, NEW_LINE, String.Empty),
            new PrintDefinition ("STATIC", 1, 0, 0 , 1, "Sum amount of all Cash-in transactions: ", String.Empty),
            new PrintDefinition ("STATIC", 0, 1, 0 , 1, NEW_LINE, String.Empty),
            new PrintDefinition ("TEXT",   0, 2, 0 , 0, String.Empty, "CASHDRAWER_CASHIN"),
            new PrintDefinition ("STATIC", 0, 1, 0 , 0, NEW_LINE, String.Empty),
            new PrintDefinition ("STATIC", 0, 1, 0 , 0, NEW_LINE, String.Empty),
            new PrintDefinition ("STATIC", 1, 0, 0 , 0, "Sum amount of all Cash-out transactions: ", String.Empty),
            new PrintDefinition ("STATIC", 0, 1, 0 , 1, NEW_LINE, String.Empty),
            new PrintDefinition ("TEXT",   0, 0, 1 , 0, "", "CASHDRAWER_CASHOUT"),
            new PrintDefinition ("STATIC", 0, 1, 0 , 1, NEW_LINE, String.Empty),
            new PrintDefinition ("FOOTER", 0, 1, 0 , 1, String.Empty, String.Empty),
            new PrintDefinition ("STATIC", 0, 1, 0 , 1, NEW_LINE, String.Empty),
            new PrintDefinition ("STATIC", 0, 1, 0 , 1, NEW_LINE, String.Empty),
            new PrintDefinition ("STATIC", 0, 1, 0 , 1, NEW_LINE, String.Empty),
            new PrintDefinition ("STATIC", 0, 1, 0 , 1, NEW_LINE, String.Empty),
        });

		public String GetData(String label)
		{
			String str = String.Empty;

			//common items
			if (label == "KIOSK_ID")
				return objData.kioskID ?? str;
			else if (label == "APPROVER_AUTHORITY")
				return objData.approverAuthority ?? str;
			else if (label == "KIOSK_ADDRESS")
				return objData.kioskAddress ?? str;
			else if (label == "CURRENT_TIME")
				return DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");

			//cash check items
			if (label == "TRANSACTION_DATE" && objData.receiptType == "CashCheckReceipt")
				return ((CashCheckReceiptData)objData).transactionDateTime ?? str;
			if (label == "CHECK_AMOUNT" && objData.receiptType == "CashCheckReceipt")
				return ((CashCheckReceiptData)objData).checkAmount ?? str;
			if (label == "FEE_AMOUNT" && objData.receiptType == "CashCheckReceipt")
				return ((CashCheckReceiptData)objData).feeAmount ?? str;
			if (label == "TOTAL" && objData.receiptType == "CashCheckReceipt")
				return ((CashCheckReceiptData)objData).totalAmount ?? str;
			if (label == "CARD_HOLDER" && objData.receiptType == "CashCheckReceipt")
				return ((CashCheckReceiptData)objData).cardHolderName ?? str;
			if (label == "BALANCE" && objData.receiptType == "CashCheckReceipt")
				return ((CashCheckReceiptData)objData).cardBalance ?? str;
			if (label == "CARD_NUMBER" && objData.receiptType == "CashCheckReceipt")
				return ((CashCheckReceiptData)objData).cardNumber ?? str;
			if (label == "RECEIPT_NUMBER" && objData.receiptType == "CashCheckReceipt")
				return ((CashCheckReceiptData)objData).receiptNumber ?? str;

			//cash drawer report items
			if (label == "CASHDRAWER_REPORT_DATE" && objData.receiptType == "CashDrawerReport")
				return ((CashDrawerReportData)objData).cashDrawerDateTime ?? str;
			if (label == "TELLER_ID" && objData.receiptType == "CashDrawerReport")
				return ((CashDrawerReportData)objData).cashDrawerTellerID ?? str;
			if (label == "TELLER_ID_LIST" && objData.receiptType == "CashDrawerReport")
				return ((CashDrawerReportData)objData).cashDrawerTellerIDList ?? str;
			if (label == "CASHDRAWER_CASHIN" && objData.receiptType == "CashDrawerReport")
				return ((CashDrawerReportData)objData).cashDrawerCashInAmount ?? str;
			if (label == "CASHDRAWER_CASHOUT" && objData.receiptType == "CashDrawerReport")
				return ((CashDrawerReportData)objData).cashDrawerCashOutAmount ?? str;

			return str;
		}
	}
}
