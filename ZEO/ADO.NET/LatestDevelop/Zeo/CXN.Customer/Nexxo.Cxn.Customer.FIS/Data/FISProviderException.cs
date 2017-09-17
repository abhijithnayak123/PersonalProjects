using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MGI.Common.Sys;


namespace MGI.Cxn.Customer.FIS.Data
{
    public class FISProviderException : ProviderException, IExceptionHelper 
    {
        public const string ProductCode = "1001";
        public const string ProviderCode = "600";

        public FISProviderException()
        {}

         public FISProviderException(string providerErrorCode, string providerMessage, Exception innerException)
            : base(ProductCode, ProviderCode, providerErrorCode, providerMessage, innerException)
        {}

        public string GetProviderErrorCode(string message)
        {
            string fisErrorCode = "CI00";
            if (!string.IsNullOrWhiteSpace(message) && message.Length >= 6)
            {
                string errorCode = message.Substring(2, 4);
                int errorNumber;
                bool isConvertionSucceed = Int32.TryParse(errorCode, out errorNumber);
                if (isConvertionSucceed)
                    fisErrorCode = message.Substring(2, 5);
            }
            return fisErrorCode;
        }

        public bool IsExceptionHandled(Exception ex)
        {
            Exception providerException = ex as ProviderException;
            if (providerException != null)
                return true;

            Exception alloyException = ex as AlloyException;
            if (alloyException != null)
                return true;

            return false;
        }
    }
}
