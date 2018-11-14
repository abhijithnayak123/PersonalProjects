using TCF.Zeo.Common.Data;
using TCF.Zeo.Common.Util;
using System;

namespace TCF.Zeo.Cxn.Check.Chexar.Data.Exceptions
{
	public class ChexarProviderException : ProviderException
	{
		public static string CheckProcessingProductCode = ((int)Helper.ProductCode.CheckProcessing).ToString();
		public static string IngoProviderCode = ((int)Helper.ProviderId.Ingo).ToString();

        public ChexarProviderException(string providerErrorCode, string providerMessage)
            : base(CheckProcessingProductCode, IngoProviderCode, providerErrorCode, providerMessage, null)
        {
        }

        public ChexarProviderException(string providerErrorCode, string providerMessage, Exception innerException)
			: base(CheckProcessingProductCode, IngoProviderCode, providerErrorCode, providerMessage, innerException)
		{
		}

	}
}
