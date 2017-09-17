using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Cxn.WU.Common.Data
{
    public class WUEnrollmentRequest
    {
        public string expectedPayoutLoc_StateCode { get; set; }
        public string expectedPayoutLoc_City { get; set; }
        public CountryCurrencyInfo destination_country_currency { get; set; }
        public CountryCurrencyInfo originating_country_currency { get; set; }
        public CountryCurrencyInfo recording_country_currency { get; set; }
        public WUEnums.Transaction_type transaction_type { set; get; }
        public WUEnums.Payment_type Payment_type { get; set; }
        public bool Transaction_TypeSpecified { get; set; }
        public bool Payment_typeSpecified { get; set; }
        public WUEnums.yes_no Fix_on_send { get; set; }
        public double Exchange_Rate { get; set; }
        public string duplicate_detection_flag { get; set; }
        public string Originating_city { get; set; }
        public string Originating_state { get; set; }
        public WUEnums.name_type NameType { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string AddressAddrLine1 { get; set; }
        public string AddressAddrLine2 { get; set; }
        public string AddressCity { get; set; }
        public string AddressState { get; set; }
        public string AddressPostalCode { get; set; }
        public string AddressStreet { get; set; }
        public string AddressStateZip { get; set; }
        public string PreferredCustomerAccountNumber { get; set; }
        public string PreferredCustomerLevelCode { get; set; }
        public string Email { get; set; }
        public string ContactPhone { get; set; }
        public string MobilePhone { get; set; }
        public string SmsNotificationFlag { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
        public string CurrencyCode { get; set; }
        public string SenderComplianceDetailsComplianceDataBuffer { get; set; }
        public string SMSNotificationFlag { get; set; }
        public string PreferredCustomerPermanentChange { get; set; }
    }
}
