using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;

namespace ChexarIO
{
	public class ChexarPendingTransaction
	{
		public int InvoiceNo = int.MinValue;
		public int Badge = int.MinValue;
		public int CustomerId = int.MinValue;
		public string CustomerName = string.Empty;
		public bool OnHold = false;
		public bool Complete = false;
		public int EmployeeId = int.MinValue;

		public ChexarPendingTransaction(XElement x)
		{
			InvoiceNo = ChexarXMLHelper.GetIntToken(x, "invoiceno");
			OnHold = ChexarXMLHelper.GetBoolToken(x, "onhold");
			Badge = ChexarXMLHelper.GetIntToken(x, "badgeno");
			CustomerId = ChexarXMLHelper.GetIntToken(x, "customerid");
			CustomerName = ChexarXMLHelper.GetXMLValue(x, "name");
			EmployeeId = ChexarXMLHelper.GetIntToken(x, "employeeid");
			Complete = ChexarXMLHelper.GetBoolToken(x, "printed");
		}
	}
}
