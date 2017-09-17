using TCF.Zeo.Common.Data;
using TCF.Zeo.Common.Util;
using System;

namespace TCF.Zeo.Cxn.BillPay.WU.Data.Exceptions
{
    public class BillpayProviderException : ProviderException, IExceptionHelper
    {
        public static string BillPayProductCode = ((int)Helper.ProductCode.BillPay).ToString();
        public static string WUProviderCode = ((int)Helper.ProviderId.WesternUnionBillPay).ToString();

        public BillpayProviderException()
        {
                
        }
        public BillpayProviderException(string providerErrorCode, string providerMessage, Exception innerException)
            : base(BillPayProductCode, WUProviderCode, providerErrorCode, providerMessage, innerException)
        {
        }

        public string GetProviderErrorCode(string message)
        {
            string wuErrorCode = "T0000";
            if (!string.IsNullOrWhiteSpace(message) && message.Length >= 5)
            {
                string errorCode = message.Substring(1, 4);
                int errorNumber;
                bool isConvertionSucceed = Int32.TryParse(errorCode, out errorNumber);
                if (isConvertionSucceed)
                    wuErrorCode = message.Substring(0, 5);
            }
            return wuErrorCode;
        }

        public bool IsExceptionHandled(Exception ex)
        {
            Exception providerException = ex as ProviderException;
            if (providerException != null)
                return true;

            Exception alloyException = ex as ZeoException;
            if (alloyException != null)
                return true;

            return false;
        }
    }
}
