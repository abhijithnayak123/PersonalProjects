using TCF.Zeo.Common.Data;
using TCF.Zeo.Common.Util;
using System;

namespace TCF.Zeo.Cxn.Check.TCF.Data.Exceptions
{
	public class TCFProviderException : ProviderException
	{
		public static string CheckProcessingProductCode = ((int)Helper.ProductCode.CheckProcessing).ToString();
		public static string TCFCheckProviderCode = ((int)Helper.ProviderId.TCFCheck).ToString();

        public TCFProviderException(string providerErrorCode, string providerMessage)
            : base(CheckProcessingProductCode, TCFCheckProviderCode, providerErrorCode, providerMessage, null)
        {
        }

        public TCFProviderException(string providerErrorCode, string providerMessage, Exception innerException)
			: base(CheckProcessingProductCode, TCFCheckProviderCode, providerErrorCode, providerMessage, innerException)
		{
		}

	}
}
