using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Alloy.CXN.Customer.FIS.Data
{
    public class CustTaxNbrSrchRes
    { 
        public string SrchCity { get; set; }
        public string SrchState { get; set; }
        public string SrchStreetNm { get; set; }
        public string SrchZipCode { get; set; }
        public string SrchCustNum { get; set; }
        public string SrchPhoneNum { get; set; }
        public string SrchSSN { get; set; }
        public string CustNumbStartSearch { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public int CustBankNum { get; set; }
        public string CustNum { get; set; }
        public string CustSSN { get; set; }
        public string CustPhoneNum { get; set; }
        public string CustLastNm { get; set; }
        public string CustFirstNm { get; set; }
        public string CustMiddleNm { get; set; }
        public string CustElementizedStreet { get; set;}       
        public string CustElememtizedState { get; set; }
        public string CustelementizedZipCode { get; set; }
        public string CustDOB { get; set; }
        public string CustDriverLicence { get; set; }
        public string CustEmploymentschool { get; set; }
        public string CustMothersMaidenNm { get; set; }
        public string CustRecordTypeInd { get; set; }
        public string CustSecondPhoneNum { get; set; }
        public string  CustTypeInd { get; set; }
    }
}