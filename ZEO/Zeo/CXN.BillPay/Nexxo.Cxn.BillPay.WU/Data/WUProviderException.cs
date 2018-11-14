using MGI.Common.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Cxn.BillPay.WU.Data
{
    public class WUProviderException : ProviderException
    {
        const int MAJOR_CODE = 1004;
        const int MINOR_CODE = 401;

        public WUProviderException(int ERROR_CODE )
            :base(MAJOR_CODE, MINOR_CODE, ERROR_CODE)
        {

        }

        public WUProviderException(int ERROR_CODE, string Message)
            : base(MAJOR_CODE, MINOR_CODE, ERROR_CODE, Message)
        {

        }

        public WUProviderException(int ERROR_CODE, Exception InnerException)
            : base(MAJOR_CODE, MINOR_CODE, ERROR_CODE, string.Empty, InnerException)
        {

        }

        public WUProviderException(int ERROR_CODE, string Message, Exception InnerException)
            : base(MAJOR_CODE, MINOR_CODE, ERROR_CODE, Message, InnerException)
        {

        }

        public static int BILLPAY_VALIDATE_FAILED           = 2401;
        public static int BILLPAY_COMMIT_FAILED             = 2402;
        public static int LOCATION_RETRIEVAL_FAILED         = 2403;
        public static int DELIVERY_METHODS_RETRIEVAL_FAILED = 2404;
        public static int BILLER_MESSAGE_RETRIEVAL_FAILED   = 2405;
        public static int BILLER_FIELDS_RETRIEVAL_FAILED    = 2406;
        public static int BILLER_RETRIEVAL_FAILED           = 2407;

        public static int INVALID_SOCIAL_SECURITY_NUMBER = 6008;
        public static int REQUIRES_SSN_ITIN = 0749;
        public static int MISSING_SSN_ITIN = 0505;
        public static int TRANSACTION_REQUIRES_ADDITIONAL_CUSTOMER_INFORMATION = 0425;
        public static int DO_TRANSACTION_REQUIRES_ADDITIONAL_CUSTOMER_INFORMATION = 0415;
    }
}
