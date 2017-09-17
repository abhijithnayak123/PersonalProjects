using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Cxn.WU.Common.Data
{
	public class Promotions
	{
		public string promo_code_description { set; get; }
		public string promo_sequence_no { set; get; }
		public string promo_name { set; get; }
		public long promo_discount_amount { set; get; }
		public string coupons_promotions;
		public string sender_promo_code { get; set; }
		public string promotion_error { get; set; }
		public string promo_message { get; set; }
	}
}
