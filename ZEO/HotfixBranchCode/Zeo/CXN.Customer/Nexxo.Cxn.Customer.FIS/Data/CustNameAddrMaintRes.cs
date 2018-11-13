using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Cxn.Customer.FIS.Data
{
    public class CustNameAddrMaintRes
    {
        public string CISApplnNumber { get; set; }
        public decimal CISCustomerBankNum { get; set; }
        public string CISElementizedCity { get; set; }
        public string CISFirstMiddleNm { get; set; }
        public string CISPrimaryPhoneNum { get; set; }
        public string CISSecPhoneNum { get; set; }
        public string CISElementizedState { get; set; }
        public string CISSurname { get; set; }
        public string CISElemetizedZipCode { get; set; }
        public string CISCountryCode { get; set; }
    }
}
