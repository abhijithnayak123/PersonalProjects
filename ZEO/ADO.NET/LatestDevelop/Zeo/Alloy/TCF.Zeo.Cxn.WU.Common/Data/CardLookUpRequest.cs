using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCF.Zeo.Cxn.WU.Common.Data
{
    public class CardLookUpRequest
    {
        //Added for US # 1646
        public Sender sender { get; set; }
        public ConvenienceSearch conveniencesearch { get; set; }

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
        public string Firstname { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string AccountNumber { get; set; }
        public string PostalCode { get; set; }
        public string CountryCode { get; set; }
        public string CurrencyCode { get; set; }
        public string LevelCode { get; set; }
        public string Type { get; set; }

    } 
}
