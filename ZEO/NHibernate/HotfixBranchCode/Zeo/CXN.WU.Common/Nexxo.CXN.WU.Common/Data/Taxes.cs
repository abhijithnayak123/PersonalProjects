using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Cxn.WU.Common.Data
{
	public class Taxes
	{
		public long tax_amount;

		public bool tax_amountSpecified;

		public long tax_rate;

		public bool tax_rateSpecified;

		public long municipal_tax;

		public bool municipal_taxSpecified;

		public long state_tax;

		public bool state_taxSpecified;

		public long county_tax;

		public bool county_taxSpecified;

		public string tax_worksheet;

		public long client_tax;

		public bool client_taxSpecified;

	}
}
