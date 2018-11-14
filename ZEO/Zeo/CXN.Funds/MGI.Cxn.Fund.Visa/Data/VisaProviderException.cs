using MGI.Common.Sys;
using System;

namespace MGI.Cxn.Fund.Visa.Data
{
	public class VisaProviderException : ProviderException
	{
		const string VisaProviderId = "103";
		public const string ProductCode = "1003";

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
