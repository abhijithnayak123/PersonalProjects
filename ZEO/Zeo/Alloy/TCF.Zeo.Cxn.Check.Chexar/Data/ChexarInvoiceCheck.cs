using System;
using System.Xml.Linq;

namespace TCF.Zeo.Cxn.Check.Chexar.Data
{
    public class ChexarInvoiceCheck
	{
		public int Badge = int.MinValue;
		public int DetailId = int.MinValue;
		public int CheckNumber = int.MinValue;
		public decimal CheckAmount = decimal.MinValue;
		public int CheckTypeId = 455;
		public decimal FeeRate = decimal.MinValue;
		public decimal FeeAmount = decimal.MinValue;
		public DateTime DTCheck = DateTime.MinValue;
		public int DeclineId = int.MinValue;
		public string DeclineReason = string.Empty;
		public bool Approved = false;

		public ChexarInvoiceCheck() { }

		public ChexarInvoiceCheck(XElement invoiceHeader, XElement checkDetails)
		{
			Badge = ChexarXMLHelper.GetIntToken(invoiceHeader, "badgeno");
			DetailId = ChexarXMLHelper.GetIntToken(checkDetails, "detailid");
			CheckNumber = ChexarXMLHelper.GetIntToken(checkDetails, "refnumber");
			CheckAmount = ChexarXMLHelper.GetDecimalToken(checkDetails, "refamount");
			CheckTypeId = ChexarXMLHelper.GetIntToken(checkDetails, "productid");

			if (checkDetails.Element("declinereason") == null)
			{
				Approved = true;
				FeeRate = ChexarXMLHelper.GetDecimalToken(checkDetails, "feerate");
				FeeAmount = ChexarXMLHelper.GetDecimalToken(checkDetails, "feeamount");
				DTCheck = ChexarXMLHelper.GetDateTimeToken(checkDetails, "checkdate");
			}
			else
			{
				DeclineId = ChexarXMLHelper.GetIntToken(checkDetails, "declinecodeid");

				if (DeclineId == int.MinValue)
					DeclineId = 0;

				DeclineReason = ChexarXMLHelper.GetXMLValue(checkDetails, "declinereason");
				Approved = ChexarXMLHelper.GetBoolToken(checkDetails, "status");
			}
		}
	}
}
