using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Channel.DMS.Server.Data
{
   public class CardLookUpRequest
    {
        //public Sender sender { get; set; }
        public string receiver_index_number { get; set; }
        public string convenience_search_type { get; set; }
        public string convenience_search_key { get; set; }
        public string wu_card_lookup_context { get; set; }
        public string card_lookup_search_type { get; set; }
        public string save_key { get; set; }
        public string ForiegnSystemId { get; set; }
        public string ForiegnRefNum { get; set; }
        public string CounterId { get; set; }
        public string sessionid { get; set; }
        public string emea { get; set; }
        public string firstname { get; set; }
        public string midname { get; set; }
        public string lastname { get; set; }
        public string AccountNumber { get; set; }
        public string postalcode { get; set; }
        public string countrycode { get; set; }
        public string currencycode { get; set; }
        public string levelcode { get; set; }
        public string type { get; set; }
    }
}
