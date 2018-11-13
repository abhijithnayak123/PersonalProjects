using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Cxn.WU.Common.Data
{
	public class QpCompany
	{
		public string account_holder_name { set; get; }
		public string debtor_account_number { set; get; }
		public string account_no { set; get; }
		public string addr_line1 { set; get; }
		public string addr_line2 { set; get; }
		public string address_type { set; get; }
		public string available_balance { set; get; }
		public string city { set; get; }
		public string city_code { set; get; }
		public string company_name { set; get; }
		public general_name contact_name { set; get; }
	}
	public class general_name
	{
		public string name_raw { set; get; }
		public WUEnums.name_prefix name_prefix { get; set; }
		public WUEnums.name_suffix name_suffix { get; set; }
		public string secondary_given_name { set; get; }
		public string secondary_paternal_name { set; get; }
		public string secondary_maternal_name { set; get; }
	}
}
