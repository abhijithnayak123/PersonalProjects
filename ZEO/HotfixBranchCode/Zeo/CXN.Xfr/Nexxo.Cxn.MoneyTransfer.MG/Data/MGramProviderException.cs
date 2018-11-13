using MGI.Common.Sys;

namespace MGI.Cxn.MoneyTransfer.MG.Data
{
	public class MGramProviderException : ProviderException
	{
		const int MGRAM_PROVIDER_ID = 302;

		public MGramProviderException(string errorCode, string message)
			: base(MGRAM_PROVIDER_ID, errorCode, message)
		{
		}
	}
}
