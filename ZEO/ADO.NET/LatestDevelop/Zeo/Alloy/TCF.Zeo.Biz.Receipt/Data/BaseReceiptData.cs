﻿using System;

namespace TCF.Zeo.Biz.Receipt.Data
{
    public class BaseReceiptData
    {
        public string ClientName { get; set; }
        public string LogoUrl { get; set; }
        public string LocationName { get; set; }
        public string LocationPhoneNumber { get; set; }
        public string BankId { get; set; }
        public string BranchId { get; set; }
        public string LocationAddress { get; set; }
        public string LocationCity { get; set; }
        public string LocationState { get; set; }
        public string LocationZip { get; set; }
        public string TellerName { get; set; }
        public string TerminalID { get; set; }
        public string TellerNumber { get; set; }
        public string CustomerName { get; set; }
        public long SessionlID { get; set; }
        public DateTime CustomerSessionDate { get; set; }
        public DateTime ReceiptDate { get; set; }
        
    }
}
