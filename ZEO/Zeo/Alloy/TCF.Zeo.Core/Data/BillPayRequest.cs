using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Core.Data
{
    public class BillPayRequest
    {
        public string CustomerFirstName { get; set; }
        public string CustomerMiddleName { get; set; }
        public string CustomerLastName { get; set; }
        public string CustomerLastName2 { get; set; }
        public string CustomerAddress1 { get; set; }
        public string CustomerAddress2 { get; set; }
        public string CustomerCity { get; set; }
        public string CustomerState { get; set; }
        public string CustomerStreet { get; set; }
        public string CustomerZip { get; set; }
        public string SSN { get; set; }
        public string CustomerPhoneNumber { get; set; }
        public System.DateTime CustomerDateOfBirth { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerMobileNumber { get; set; }
        public string PrimaryIdType { get; set; }
        public string PrimaryIdNumber { get; set; }
        public string PrimaryIdCountryOfIssue { get; set; }
        public string PrimaryIdPlaceOfIssue { get; set; }
        public string PrimaryCountryOfIssue { get; set; }
        public string SecondIdType { get; set; }
        public string SecondIdNumber { get; set; }
        public string SecondIdCountryOfIssue { get; set; }
        public string Occupation { get; set; }
        public string CountryOfBirth { get; set; }
        public string CountryOfBirthAbbr3 { get; set; }
        public string PrimaryIdCountryOfIssueCode { get; set; }
        public string PrimaryIdPlaceOfIssueCode { get; set; }
        public decimal Amount { get; set; }
        public string ProductName { get; set; }
        public long ProductId { get; set; }
        public decimal Fee { get; set; }
        public string PromoCode { get; set; }
        public string CouponCode { get; set; }
        public string AccountNumber { get; set; }
    }
}
