using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MGI.Common.Sys;

namespace ChexarIO
{
	public class ChexarProviderException : ProviderException
	{
		public static string CheckProcessingProductCode = ((int)MGI.Common.Util.ProductCode.CHECK_PROCESSING_PRODUCTCODE).ToString();
		public static string IngoProviderCode = ((int)MGI.Common.Util.ProviderId.Ingo).ToString();

		public ChexarProviderException(string providerErrorCode, string providerMessage)
			: base(CheckProcessingProductCode, IngoProviderCode, providerErrorCode, providerMessage)
		{
		}

		public ChexarProviderException(string providerErrorCode, string providerMessage, Exception innerException)
			: base(CheckProcessingProductCode, IngoProviderCode, providerErrorCode, providerMessage, innerException)
		{
		}

	}
}
