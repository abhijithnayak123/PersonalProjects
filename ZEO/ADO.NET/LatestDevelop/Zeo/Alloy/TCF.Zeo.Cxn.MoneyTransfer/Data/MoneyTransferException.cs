//using TCF.Zeo.Common.Data;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using static TCF.Zeo.Common.Util.Helper;

//namespace TCF.Zeo.Cxn.MoneyTransfer.Data
//{
//    public class MoneyTransferException : AlloyException
//    {
//        static string MoneyTransferProductCode = "1005";
//        static string AlloyCode = ((int)ProviderId.Alloy).ToString();

//        public MoneyTransferException(string errorCode)
//            : base(MoneyTransferProductCode, AlloyCode, errorCode, null)
//        {
//        }

//        public MoneyTransferException(string errorCode, Exception innerException)
//            : base(MoneyTransferProductCode, AlloyCode, errorCode, innerException)
//        {
//        }


//        public static string RECEIVER_ALREADY_EXISTED                                   = "2000";//DESC : Receiver already existed with first name & last name combination.
//        public static string DESTINATION_COUNTRY_CODE_NOT_FOUND                         = "2077";
//        public static string DELIVERYSERVICE_NOT_FOUND                                  = "2078";
//        public static string CUSTOMER_NAME_NOT_MATCH                                    = "2010"; //US1784 - WU Gold Card Name Matching
//        public static string MODIFY_TRANSACTION_NOT_ALLOWED                             = "2011"; //US1865 WU Modify Transaction
//        public static string TRANSACTION_ALREADY_PAID                                   = "2012";      //US1865 WU Modify Transaction
//        public static string COUNTERID_NOT_FOUND                                        = "2023";
//        public static string MONEYTRANSFER_VALIDATE_FAILED                              = "2075";
//        //public static string MONEYTRANSFER_GETRECEIVER_FAILED = "2076";
//        //public static string MONEYTRANSFER_GETACTIVERECEIVER_FAILED = "2026";
//        public static string MONEYTRANSFER_GETFREQUENTRECEIVERS_FAILED                  = "2027";
//        //public static string MONEYTRANSFER_GETRECEIVERS_FAILED = "2028";
//        public static string MONEYTRANSFER_SAVERECEIVER_FAILED                          = "2029";
//        public static string MONEYTRANSFER_COMMIT_FAILED                                = "2030";
//        public static string MONEYTRANSFER_SENDMONEYSTORE_FAILED                        = "2031";
//        public static string MONEYTRANSFER_SEARCH_FAILED                                = "2032";
//        public static string MONEYTRANSFER_REFUND_FAILED                                = "2033";
//        public static string MONEYTRANSFER_GETPAYSTATUS_FAILED                          = "2034";
//        public static string MONEYTRANSFER_MODIFYSEARCH_FAILED                          = "2035";
//        public static string MONEYTRANSFER_MODIFY_FAILED                                = "2036";
//        public static string MONEYTRANSFER_GETDELIVERYSERVICES_FAILED                   = "2037";
//        public static string MONEYTRANSFER_FEEINQUIRY_FAILED                            = "2038";
//        public static string MONEYTRANSFER_SEARCHRECEIVEMONEY_FAILED                    = "2039";
//        public static string MONEYTRANSFER_RECEIVEMONEYPAY_FAILED                       = "2040";
//        public static string MONEYTRANSFER_GETREFUNDREASONS_FAILED                      = "2041";
//        public static string MONEYTRANSFER_EXECUTEDASINQUIRY_FAILED                     = "2042";
//        //public static string MONEYTRANSFER_UPDATEGOLDCARDPOINTS_FAILED = "2043";
//        //public static string MONEYTRANSFER_GETACCOUNT_FAILED = "2044";
//        //public static string MONEYTRANSFER_UPDATEACCOUNT_FAILED = "2045";
//        public static string MONEYTRANSFER_GETBANNERMSGS_FAILED                         = "2046";
//        //public static string MONEYTRANSFER_GETCARDINFO_FAILED = "2047";
//        //public static string MONEYTRANSFER_GET_FAILED = "2048";
//        public static string MONEYTRANSFER_GETTRANSACTION_FAILED                        = "2049";
//        public static string MONEYTRANSFER_GETPASTRECEIVERS_FAILED                      = "2050";
//        public static string MONEYTRANSFER_USEGOLDCARD_FAILED                           = "2051";
//        public static string MONEYTRANSFER_WUCARDENROLLMENT_FAILED                      = "2052";
//        public static string MONEYTRANSFER_WUCARDLOOKUP_FAILED                          = "2053";
//        //public static string MONEYTRANSFER_GETWUCARDACCOUNT_FAILED = "2054";
//        //public static string MONEYTRANSFER_DISPLAYWUCARDACCOUNTINFO_FAILED = "2055";
//        public static string MONEYTRANSFER_STAGEMODIFY_FAILED                           = "2056";
//        public static string MONEYTRANSFER_GETSTATUS_FAILED                             = "2057";
//        public static string MONEYTRANSFER_DELETEFAVORITERECEIVER_FAILED                = "2058";
//        public static string MONEYTRANSFER_GETFEE_FAILED                                = "2059";
//        //public static string MONEYTRANSFER_GETCOUNTRYTRANSALATION_FAILED = "2060";
//        //public static string MONEYTRANSFER_GETDELIVERYSERVICETRANSALATION_FAILED = "2061";
//        //public static string MONEYTRANSFER_ISSWBSTATE_FAILED = "2062";
//        //public static string CERIFICATE_NOTFOUND = "2002";
//        //public static string MONEYTRANSFER_SEARCHMODIFY_FAILED = "2063";
//        public static string MONEYTRANSFER_SEARCHREFUND_FAILED                          = "2064";
//        //public static string MONEYTRANSFER_CREATESENDMONEYTRANSACTION_FAILED = "2065";
//        //public static string MONEYTRANSFER_CREATESENDMONEYREFUNDTRANSACTION_FAILED = "2066";
//        //public static string MONEYTRANSFER_UPDATESENDMONEYREFUNDTRANSACTION_FAILED = "2067";
//        //public static string MONEYTRANSFER_CREATERECEIVEMONEYTRANSACTION_FAILED = "2068";
//        //public static string MONEYTRANSFER_UPDATETRX_FAILED = "2069";
//        //public static string TIME_ZONE_NOT_PROVIDE = "2070";
//        public static string DESTINATION_STATE_NOT_FOUND                                = "2071";
//        public static string DESTINATION_STATE_CODE_NOT_FOUND                           = "2072";
//        public static string DESTINATION_CITY_NOT_FOUND                                 = "2073";
//        public static string MONEYTRANSFER_ADDACCOUNT_FAILED                            = "2074";
//        public static string MONEYTRANSFER_GETCOUNTRIES_FAILED                          = "2079";
//        public static string MONEYTRANSFER_GETSTATES_FAILED                             = "2080";
//        public static string MONEYTRANSFER_GETCITIES_FAILED                             = "2081";
//        public static string MONEYTRANSFER_GETCURRENCYCODE_FAILED                       = "2082";
//        public static string MONEYTRANSFER_GETCURRENCYCODELIST_FAILED                   = "2083";
//        public static string MONEYTRANSFER_ADDRECEIVER_FAILED                           = "2029";
//        public static string MONEYTRANSFER_UPDATERECEIVER_FAILED                        = "2084";
//        public static string MONEYTRANSFER_UPDATEWUCARD_FAILED                          = "2085";
//        public static string MONEYTRANSFER_ISSENDMONEYMODIFYREFUNDAVAILABLE_FAILED      = "2086";
//        public static string MONEYTRANSFER_GETRECEIVEMONEY_FAILED                       = "2087";
//    }
//}
