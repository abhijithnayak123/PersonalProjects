using MGI.Common.Sys;

namespace MGI.Cxn.Fund.Visa.Data
{
	public class VisaProviderException : ProviderException
	{
		const int VisaProviderId = 103;

		public VisaProviderException(string errorCode, string message)
			: base(VisaProviderId, errorCode, message)
		{
		}
	
		public static string CARD_REGISTRATION_ERROR = "6601";		

	}
}
