using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Cxn.Customer.FIS.Data
{
    public class ConnectsDBTaxNbrSrchRes
    {
        public string CustNum { get; set; }
        public string PrimarPhoneNum { get; set; }
        public string SecondaryPhoneNum { get; set; }
        public int CustTax { get; set; }
        public int TaxCode { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string AddressStreet{ get; set; }
        public string AddrState{ get; set; }
        public int ZipCode{ get; set; }
		public int DateOfBirth { get; set; }
        public string DriversLic{ get; set; }
        public string MothersMaidenNm{ get; set; }
        public string RecordType  { get; set; } 
        public int ExtrernalKey { get; set; }
        public int MetBankNumber { get; set; }
    }
}
