using MGI.Common.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Cxn.WU.Common.Data
{
	public class WUCommonProviderException : ProviderException, IExceptionHelper
	{
		public WUCommonProviderException()
		{
		}

		public WUCommonProviderException(string wuCommonProductCode, string wuCommonProvideCode, string providerErrorCode, string providerMessage, Exception InnerException)
			: base(wuCommonProductCode, wuCommonProvideCode, providerErrorCode, providerMessage, InnerException)
		{
		}

        public string GetProviderErrorCode(string errorMessage)
        {
            string wuErrorCode = "T0000";
            if (!string.IsNullOrWhiteSpace(errorMessage) && errorMessage.Length >= 5)
            {
                string errorCode = errorMessage.Substring(1, 4);
                int errorNumber;
                bool isConvertionSucceed = Int32.TryParse(errorCode, out errorNumber);
                if (isConvertionSucceed)
                    wuErrorCode = errorMessage.Substring(0, 5);
            }
            return wuErrorCode;
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
