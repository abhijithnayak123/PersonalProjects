﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Biz.MoneyTransfer.Data
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
        public string AddressAddrLine1 { get; set; }
        public string AddressCity { get; set; }
        public string AddressPostalCode { get; set; }
        public string AddressState { get; set; }
        public string ContactPhone { get; set; }
        public string PreferredCustomerAccountNumber { get; set; }
    }
}
