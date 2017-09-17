using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MGI.Common.Sys;

namespace MGI.Core.Partner.Contract
{
    public class TransactionServiceException : AlloyException
    {
        const string ProductCode = "1000";
        const string AlloyCode   = "100";

        public TransactionServiceException(string ErrorCode)
            : base(ProductCode, AlloyCode, ErrorCode, null)
        {
        }

        public TransactionServiceException(string ErrorCode, Exception ex)
            : base(ProductCode, AlloyCode, ErrorCode, ex)
        {
        }

        static public string CHECKTYPE_NOT_FOUND   = "3001";
        static public string CHANNEL_PARTNER_BILLPAY_FEE_NOT_FOUND  = "3010";
        static public string PROMOCODE_INVALID                      = "3017";
        static public string TRANSACTION_CREATE_FAILED              = "3600";
        static public string TRANSACTION_NOT_FOUND                  = "3601";
        static public string TRANSACTION_UPDATE_STATES_FAILED       = "3602";
        static public string TRANSACTION_UPDATE_CXE_STATE_FAILED    = "3603";
        static public string TRANSACTION_UPDATE_CXN_STATE_FAILED    = "3604";
        static public string TRANSACTION_UPDATE_FEE_FAILED          = "3605";
        static public string TRANSACTION_UPDATE_AMOUNT_FAILED       = "3607";
        static public string TRANSACTION_UPDATE_TRANSACTIONDETAILS_FAILED = "3608";
		static public string TRANSACTION_UPDATE_FAILED              = "3609";
		static public string TRANSACTION_UPDATE_MONEYORDERIMAGE_FAILED = "3610";
    }
}
