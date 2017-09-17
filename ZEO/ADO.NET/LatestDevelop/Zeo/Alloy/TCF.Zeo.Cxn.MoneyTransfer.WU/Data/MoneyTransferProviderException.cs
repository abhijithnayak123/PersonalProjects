using TCF.Zeo.Common.Data;
using TCF.Zeo.Common.Util;
using System;

namespace TCF.Zeo.Cxn.MoneyTransfer.WU.Data
{
    public class MoneyTransferProviderException : ProviderException, IExceptionHelper
    {
        public static string MoneyTransferProductCode = ((int)Helper.ProductCode.MoneyTransfer).ToString();
        public static string WUProviderCode = ((int)Helper.ProviderId.WesternUnion).ToString();

        public MoneyTransferProviderException(string providerErrorCode, string providerMessage, Exception innerException)
            : base(MoneyTransferProductCode, WUProviderCode, providerErrorCode, providerMessage, innerException)
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
