using System.Collections.Generic;

namespace MGI.Cxn.BillPay.Data
{
    public class BillPayRequest
    {
        //Customer personal details 
        public string CustomerFirstName { get; set; }
        public string CustomerMiddleName { get; set; }
        public string CustomerLastName { get; set; }
        public string CustomerLastName2 { get; set; }
        public string CustomerAddress1 { get; set; }
        public string CustomerAddress2 { get; set; }
        public string CustomerCity { get; set; }
        public string CustomerState { get; set; }
        public string CustomerStreet { get; set; }//street is not ther in cxe,why still its there? 
        public string CustomerZip { get; set; }
        public string SSN { get; set; }
        public string CustomerPhoneNumber { get; set; }
        public System.DateTime CustomerDateOfBirth { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerMobileNumber { get; set; }

        //customer government details and other details
        public string PrimaryIdType { get; set; }
        public string PrimaryIdNumber { get; set; }
        public string PrimaryIdCountryOfIssue { get; set; }
        public string PrimaryIdPlaceOfIssue { get; set; }
        public string SecondIdType { get; set; }
        public string SecondIdNumber { get; set; }
        public string SecondIdCountryOfIssue { get; set; }
        public string Occupation { get; set; }
        public string CountryOfBirth { get; set; }
        public string CountryOfBirthAbbr3 { get; set; }

        public string PrimaryIdCountryOfIssueCode { get; set; }
        public string PrimaryIdPlaceOfIssueCode { get; set; }

        //Gateway specific
        public string CardNumber { get; set; }

        //Trx specific details
        public decimal Amount { get; set; } //TODO: Shouldnt this be decimal?
        public string ProductName { get; set; }
        public long ProductId { get; set; }
        public decimal Fee { get; set; }
        public string PromoCode { get; set; }//not used?
        public string CouponCode { get; set; }
        public string AccountNumber { get; set; }//renamed CustomerAccount to AccountNumber

        //provider specific
        public Dictionary<string, object> MetaData { get; set; }
    }
}
