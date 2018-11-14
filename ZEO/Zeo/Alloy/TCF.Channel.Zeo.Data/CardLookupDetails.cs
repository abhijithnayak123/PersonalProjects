using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Channel.Zeo.Data
{
    public class CardLookupDetails
    {
        public string SessionId { get; set; }
        public string Emea { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public Account[] Sender { get; set; }
        public string AccountNumber { get; set; }
        public string PostalCode { get; set; }
        public string CountryCode { get; set; }
        public string CurrencyCode { get; set; }
        public string LevelCode { get; set; }
        public string Type { get; set; }
        public string OriginalCardType { get; set; }
        public string ForiegnSystemId { get; set; }
        public string ForiegnRefNum { get; set; }
        public string CounterId { get; set; }
        public string AddressAddrLine1 { get; set; }
        public string AddressCity { get; set; }
        public string AddressPostalCode { get; set; }
        public string AddressState { get; set; }
        public string ContactPhone { get; set; }
    }
}
