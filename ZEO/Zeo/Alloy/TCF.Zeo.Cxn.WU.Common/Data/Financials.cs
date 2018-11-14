using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCF.Zeo.Cxn.WU.Common.Data
{
	public class Financials
	{
		public long originators_principal_amount { get; set; }
		public bool originators_principal_amountSpecified { set; get; }
		public long destination_principal_amount { get; set; }
		public bool destination_principal_amountSpecified { set; get; }
		public long principal_amount { get; set; }
		public long gross_total_amount { set; get; }
		public long plus_charges_amount { set; get; }
		public long pay_amount { set; get; }
		public long charges { set; get; }
		public long tolls { set; get; }
		public string originating_currency_principal { set; get; }
		public long canadian_dollar_exchange_fee { set; get; }
		public decimal message_charge { set; get; }
		public string promo_code_description { set; get; }
		public string promo_sequence_no { set; get; }
		public string promo_name { set; get; }
		public long promo_discount_amount { set; get; }
		public double exchange_rate { set; get; }
		public string base_message_charge { set; get; }
		public string base_message_limit { set; get; }
		public string incremental_message_charge { set; get; }
		public string incremental_message_limit { set; get; }
		public long Total_undiscounted_charges;
		public long Total_discount;
		public long Total_discounted_charges;
        public Taxes taxes { get; set; }
        public string PersonalMessage { get; set; } 

	}
}
