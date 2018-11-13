using MGI.Common.Sys;

namespace MGI.Cxn.Customer.TCIS.Contract
{
	class TCIFProviderException : ProviderException
	{
		const int TCIS_PROVIDER_ID = 700;

		public TCIFProviderException(string errorCode, string Message)
			: base(TCIS_PROVIDER_ID, errorCode.ToString(), Message)
		{

		}

		public TCIFProviderException(string Message)
			: base(TCIS_PROVIDER_ID, string.Empty, Message)
		{

		}

	}
}