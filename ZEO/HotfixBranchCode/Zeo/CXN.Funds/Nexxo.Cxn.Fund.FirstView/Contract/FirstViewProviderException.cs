using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MGI.Common.Sys;

namespace MGI.Cxn.Fund.Contract
{
    public class FirstViewProviderException : ProviderException
    {
        const int FIRSTVIEW_PROVIDER_ID = 101;

        public FirstViewProviderException(string errorCode, string Message)
            : base(FIRSTVIEW_PROVIDER_ID, errorCode, Message)
		{
		}
    }
}