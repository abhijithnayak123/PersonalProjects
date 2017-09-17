using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MGI.Common.Sys;

namespace MGI.Cxn.Check.Certegy.Data
{
	public class CertegyProviderException : ProviderException
	{
		const int Certegy_PROVIDER_ID = 201;

		public CertegyProviderException(int errorCode, string Message)
			: base(Certegy_PROVIDER_ID, errorCode.ToString(), Message)
		{
		}

		public CertegyProviderException(string errorCode, string message)
			: base(Certegy_PROVIDER_ID, errorCode, message)
		{
		}
		public CertegyProviderException(string Message)
			: base(Certegy_PROVIDER_ID, string.Empty, Message)
		{
		}

		//public static int PROVIDER_SUB_ERROR = 201;
	}
}
