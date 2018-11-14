using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Channel.Shared.Server.Data
{
   public class PaymentDetails
    {
        public string expectedPayoutLoc_StateCode { get; set; }
        public string expectedPayoutLoc_City { get; set; }
        public string destination_country_currency { get; set; }
        public string originating_country_currency { get; set; }
        public string recording_country_currency { get; set; }
        public string transaction_type { set; get; }
        public string Payment_type { get; set; }
        public bool Transaction_TypeSpecified;
        public bool Payment_typeSpecified;
        public double Exchange_Rate;
        public string duplicate_detection_flag;
        public string Originating_city;
        public string Originating_state;
		public bool IsFixedOnSend { get; set; }

    }
}


