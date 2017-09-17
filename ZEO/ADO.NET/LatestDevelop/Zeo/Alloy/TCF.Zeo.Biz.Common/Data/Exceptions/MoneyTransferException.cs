using TCF.Zeo.Common.Data;
using TCF.Zeo.Common.Util;
using System;

namespace TCF.Zeo.Biz.Common.Data.Exceptions
{
    public class MoneyTransferException : ZeoException
    {
        public static string MoneyTransferProductCode = ((int)Helper.ProductCode.MoneyTransfer).ToString();
        public static string AlloyCode = ((int)Helper.ProviderId.Alloy).ToString();

        public MoneyTransferException(string alloyErrorCode)
            : base(MoneyTransferProductCode, AlloyCode, alloyErrorCode, null)
        {
        }

        public MoneyTransferException(string alloyErrorCode, Exception innerException)
            : base(MoneyTransferProductCode, AlloyCode, alloyErrorCode, innerException)
        {
        }

        public MoneyTransferException(string moneyTransferProductCode, string alloyErrorCode, Exception innerException)
            : base(moneyTransferProductCode, AlloyCode, alloyErrorCode, innerException)
        {
        }

        public static readonly string MONEYTRANSFER_ISSWBSTATE_FAILED                               = "6004";
        public static readonly string MONEYTRANSFER_GETFEE_FAILED                                   = "6005";
        public static readonly string MONEYTRANSFER_VALIDATE_FAILED                                 = "6006";
        public static readonly string MONEYTRANSFER_COMMIT_FAILED                                   = "6007";
        public static readonly string MONEYTRANSFER_ADDRECEIVER_FAILED                              = "6008";
        public static readonly string MONEYTRANSFER_GETRECEIVER_FAILED                              = "6010";
        public static readonly string MONEYTRANSFER_GETFREQUENTRECEIVERS_FAILED                     = "6013";
        public static readonly string MONEYTRANSFER_DELETEFAVORITERECEIVER_FAILED                   = "6014";
        public static readonly string MONEYTRANSFER_GET_FAILED                                      = "6015";
        public static readonly string MONEYTRANSFER_GETRECEIVERLASTTRANSACTION_FAILED               = "6016";
        public static readonly string MONEYTRANSFER_UPDATEACCOUNT_FAILED                            = "6017";
        public static readonly string MONEYTRANSFER_WUCARDENROLLMENT_FAILED                         = "6018";
        public static readonly string MONEYTRANSFER_WUCARDLOOKUP_FAILED                             = "6019";
        public static readonly string MONEYTRANSFER_CANCEL_FAILED                                   = "6022";
        public static readonly string MONEYTRANSFER_ADDPASTRECEIVERS_FAILED                         = "6023";
        public static readonly string MONEYTRANSFER_GETSTATUS_FAILED                                = "6025";
        public static readonly string MONEYTRANSFER_SEARCH_FAILED                                   = "6026";
        public static readonly string MONEYTRANSFER_STAGEMODIFY_FAILED                              = "6027";
        public static readonly string MONEYTRANSFER_AUTHORIZEMODIFY_FAILED                          = "6029";
        public static readonly string MONEYTRANSFER_MODIFY_FAILED                                   = "6030";
        public static readonly string MONEYTRANSFER_REFUND_FAILED                                   = "6031";
        public static readonly string MONEYTRANSFER_GETDELIVERYSERVICES_FAILED                      = "6032";
        public static readonly string MONEYTRANSFER_GETCOUNTRIES_FAILED                             = "6035";
        public static readonly string MONEYTRANSFER_GETSTATES_FAILED                                = "6036";
        public static readonly string MONEYTRANSFER_GETCITIES_FAILED                                = "6037";
        public static readonly string MONEYTRANSFER_GETBANNERMSGS_FAILED                            = "6038";
        public static readonly string MONEYTRANSFER_GETCURRENCYCODE_FAILED                          = "6039";
        public static readonly string MONEYTRANSFER_GETCURRENCYCODELIST_FAILED                      = "6040";
        public static readonly string MONEYTRANSFER_GETREFUNDREASONS_FAILED                         = "6041";
        public static readonly string MONEYTRANSFER_ISSENDMONEYMODIFYREFUNDAVAILABLE_FAILED         = "6042";
    }
}
