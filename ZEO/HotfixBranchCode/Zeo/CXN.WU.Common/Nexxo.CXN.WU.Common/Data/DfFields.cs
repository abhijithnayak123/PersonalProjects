using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Cxn.WU.Common.Data
{
	public class DfFields
	{
		public WUEnums.yes_no Ds_Required_Flag;
		public WUEnums.yes_no Pds_Required_Flag;
		public bool Pds_Required_flagSpecified;
		public WUEnums.yes_no Df_Transaction_flag;
		public bool Df_transaction_flagSpecified;
		public WUEnums.yes_no Reprint_more_flag;
		public bool Reprint_more_flagSpecified;
		public string[] Partner_marketing_languages;
		public string Customer_preferrred_language;		
		public double Pay_side_tax;
		public bool Pay_side_taxSpecified;
		public double Pay_side_charges;
		public bool Pay_side_chargesSpecified;
		public double Amount_to_receiver;
		public bool Amount_to_receiverSpecified;
		public string Available_for_pickup;
		public string Delivery_service_name;
		public string Available_for_pickup_est;
		public string Delay_hours;
		public string Time_available;
		public string Country_timezone;
		public string Qcc_consumer_fee;
		public string Qcc_transfer_tax;
		public string Qcc_total_amount;
		public string Qcc_attention;
		public string Reference_number;
        public string AgencyName;
        public string Url;
        public string PhoneNumber;
	}
}
