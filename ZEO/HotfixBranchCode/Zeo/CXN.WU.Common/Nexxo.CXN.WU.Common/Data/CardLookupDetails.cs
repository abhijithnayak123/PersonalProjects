using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Cxn.WU.Common.Data
{
    public class CardLookupDetails
    {
        // public PaymentDetails paymentDetails { get; set; }
        public string sessionid { get; set; }
        public string emea { get; set; }
        public string firstname { get; set; }
        public string midname { get; set; }
        public string lastname { get; set; }
        public Sender[] Sender { get; set; }

        // This is for User Story # US1645. (RECEIVERS AND BILLERS).
        public Receiver[] Receiver { get; set; }


        public string AccountNumber { get; set; }
        public string postalcode { get; set; }
        public string countrycode { get; set; }
        public string currencycode { get; set; }
        public string levelcode { get; set; }
        public string type { get; set; }
        public string originalcardtype { get; set; }
        public string ForiegnSystemId { get; set; }
        public string ForiegnRefNum { get; set; }
        public string CounterId { get; set; }
        public string card_lookup_search_type { get; set; }
        public string receiver_index_number { get; set; }
        public string convenience_search_type { get; set; }
        public string convenience_search_key { get; set; }
        public string wu_card_lookup_context { get; set; }
        public string save_key { get; set; }
        public string WuCardTotalPointsEarned { get; set; }
    }
}
