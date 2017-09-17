using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Channel.Shared.Server.Data
{
    public class CardLookupDetails
    {
        public string sessionid { get; set; }
        public string emea { get; set; }
        public string firstname { get; set; }
        public string midname { get; set; }
        public string lastname { get; set; }
        public Account[] Sender { get; set; }
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
    }
}
