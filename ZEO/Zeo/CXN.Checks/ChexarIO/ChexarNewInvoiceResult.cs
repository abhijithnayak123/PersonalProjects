using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;

namespace ChexarIO
{
	public class ChexarNewInvoiceResult
	{
		public int InvoiceNo = int.MinValue;
		public string Status = string.Empty;
		public int TicketNo = int.MinValue;
		public bool OnHold = false;

		public ChexarNewInvoiceResult() { }

		public ChexarNewInvoiceResult(XElement x)
		{
			InvoiceNo = ChexarXMLHelper.GetIntToken(x, "invoiceno");

			// 10/27/2011 - JWA - try/catch needed to workaround occasional Chexar issue with not returning status and hold tags from PostTmp()
			try
			{
				Status = ChexarXMLHelper.GetXMLValue(x, "status");
				OnHold = ChexarXMLHelper.GetBoolToken(x, "masthold");

				if (OnHold)
					TicketNo = ChexarXMLHelper.GetIntToken(x.Element("tickets"), "ticketno");
			}
			catch
			{
				Status = "Pending";
				OnHold = true;
			}
		}
	}
}
