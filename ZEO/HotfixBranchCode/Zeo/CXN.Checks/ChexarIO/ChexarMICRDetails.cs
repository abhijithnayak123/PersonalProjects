using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;

namespace ChexarIO
{
	public class ChexarMICRDetails
	{
		public int CheckTypeId = 455;
		public string ABARoutingNumber = string.Empty;
		public string AccountNumber = string.Empty;
		public string CheckNumber = string.Empty;
		public decimal CheckAmount = decimal.MinValue;
		public decimal FeeAmount = decimal.MinValue;

		public ChexarMICRDetails() { }

		public ChexarMICRDetails(XElement checkDetails)
		{
			CheckTypeId = ChexarXMLHelper.GetIntToken(checkDetails, "productid");
			ABARoutingNumber = ChexarXMLHelper.GetXMLValue(checkDetails, "aba");
			AccountNumber = ChexarXMLHelper.GetXMLValue(checkDetails, "acctnumber");
			CheckNumber = ChexarXMLHelper.GetXMLValue(checkDetails, "refnumber");
			CheckAmount = ChexarXMLHelper.GetDecimalToken(checkDetails, "refamount");

			try
			{
				FeeAmount = ChexarXMLHelper.GetDecimalToken(checkDetails, "feeamount");
			}
			catch { }
		}
	}
}
