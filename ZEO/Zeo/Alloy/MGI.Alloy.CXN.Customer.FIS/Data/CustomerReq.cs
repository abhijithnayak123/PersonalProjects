using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Alloy.CXN.Customer.FIS.Data
{
    public class CustomerReq : Request
    {
        public string CustNameAddressLine1{ get; set; }
        public string CustNameAddressLine2{ get; set; }
        public string CustNameAddressLine3{ get; set; }
        public string CustNameAddressLineCode1{ get; set; }
        public string CustNameAddressLineCode2{ get; set; }
        public string CustNameAddressLineCode3{ get; set; }  
    }
}
