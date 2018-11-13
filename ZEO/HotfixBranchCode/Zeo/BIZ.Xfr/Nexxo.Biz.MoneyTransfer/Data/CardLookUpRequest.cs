using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Biz.MoneyTransfer.Data
{
    public class CardLookUpRequest
    {
        public Account sender { get; set; }
        public string receiver_index_number { get; set; }
        public string convenience_search_type { get; set; }
        public string convenience_search_key { get; set; }
        public string wu_card_lookup_context { get; set; }
        public string card_lookup_search_type { get; set; }
        public string save_key { get; set; }
    }
}
