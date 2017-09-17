using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;

namespace ChexarIO
{
	public class ChexarTicketStatus
	{
		public int TicketNo = int.MinValue;
		public string WaitTime = string.Empty;
		public string Status = string.Empty;

		public ChexarTicketStatus() { }

		public ChexarTicketStatus(XElement ticketInfo)
		{
			TicketNo = ChexarXMLHelper.GetIntToken(ticketInfo, "ticketno");
			WaitTime = ChexarXMLHelper.GetXMLValue(ticketInfo, "waittime");
			Status = ChexarXMLHelper.GetXMLValue(ticketInfo, "status");
		}
	}
}
