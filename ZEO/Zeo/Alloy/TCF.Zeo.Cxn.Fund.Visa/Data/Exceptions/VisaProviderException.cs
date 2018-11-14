using TCF.Zeo.Common.Data;
using TCF.Zeo.Common.Util;
using System;

namespace TCF.Zeo.Cxn.Fund.Visa.Data.Exceptions
{
	public class VisaProviderException : ProviderException
	{
		public static string VisaProviderId = ((int)Helper.ProviderId.Visa).ToString();
		public static string ProductCode = ((int)Helper.ProductCode.Funds).ToString();

		public VisaProviderException(string errorCode, string message)
			: base(ProductCode, VisaProviderId, errorCode, message, null)
		{
		}

		public VisaProviderException(string errorCode, string message, Exception innerException)
			: base(ProductCode, VisaProviderId, errorCode, message, innerException)
		{
		}

		public static string CARD_REGISTRATION_FAILED = "6601";
	}
}
