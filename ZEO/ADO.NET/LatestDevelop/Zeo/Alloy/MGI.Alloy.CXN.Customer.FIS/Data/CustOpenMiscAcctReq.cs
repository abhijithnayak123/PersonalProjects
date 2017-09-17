using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Alloy.CXN.Customer.FIS.Data
{
    public class CustOpenMiscAcctReq : Request
    {        
        public string CISRelationPrimarySecInd { get; set; }
        public string CISAcctCurrentAddrLineCode1 { get; set; }
        public string CISAcctCurrentAddrLineCode2   { get; set; }        
        public string CISAcctCurrentAddrLineCode3   { get; set; }
        public string CIStAccountCurrentNmAddrLine1 { get; set; }
        public string CISAccountCurrentNmAddrLine2 { get; set; }
        public string CISAccountCurrentNmAddrLine3 { get; set; }   
        public string CISAcctApplCode { get;set;}
        public string CISAcctNum { get; set; }  
    }
}
