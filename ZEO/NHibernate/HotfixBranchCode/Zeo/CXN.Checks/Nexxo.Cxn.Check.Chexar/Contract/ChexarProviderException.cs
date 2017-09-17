using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MGI.Common.Sys;

namespace MGI.Cxn.Check.Chexar.Contract
{
	public class ChexarProviderException : ProviderException
	{
		const int CHEXAR_PROVIDER_ID = 200;

		public ChexarProviderException(int errorCode, string Message)
			: base(CHEXAR_PROVIDER_ID, errorCode.ToString(), Message)
		{
		}

		public ChexarProviderException(string Message)
			: base(CHEXAR_PROVIDER_ID, string.Empty, Message)
		{
		}

        public static int PROVIDER_SUB_ERROR = 200;
	}
}
