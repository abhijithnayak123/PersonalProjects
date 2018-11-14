using MGI.Common.Sys;
using System;

namespace MGI.Cxn.Customer.TCIS.Data
{
    public class TCISProviderException : ProviderException, IExceptionHelper 
	{
        public const string ProductCode = "1001";
        public const string ProviderCode = "602";

        public TCISProviderException()
        {}

        public TCISProviderException(string providerErrorCode, string providerMessage, Exception innerException)
            : base(ProductCode, ProviderCode, providerErrorCode, providerMessage, innerException)
        {}

        public string GetProviderErrorCode(string message)
        {
            throw new NotImplementedException();
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