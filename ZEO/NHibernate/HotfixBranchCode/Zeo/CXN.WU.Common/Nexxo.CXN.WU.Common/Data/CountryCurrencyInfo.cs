using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Cxn.WU.Common.Data
{
	public class CountryCurrencyInfo
	{
		public string cpc_code { get; set; }
		public string country_code { set; get; }
		public string currency_code { set; get; }
		public string country_name { get; set; }
	}
}
