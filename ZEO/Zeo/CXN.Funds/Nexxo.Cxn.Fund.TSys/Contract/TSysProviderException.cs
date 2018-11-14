using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MGI.Common.Sys;

namespace MGI.Cxn.Fund.TSys.Contract
{
	public class TSysProviderException : ProviderException
	{
		const int TSYS_PROVIDER_ID = 102;

		public TSysProviderException(string errorCode, string Message)
			: base(TSYS_PROVIDER_ID, errorCode, Message)
		{
		}
	}
}
