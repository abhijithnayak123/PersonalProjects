using MGI.Common.Sys;

namespace MGI.CXN.MG.Common.Data
{
	public class MGramProviderException : ProviderException
	{
		const int MGRAM_PROVIDER_ID = 302;

		public MGramProviderException(string errorCode, string Message)
			: base(MGRAM_PROVIDER_ID, errorCode, Message)
		{
		}
	}
}
