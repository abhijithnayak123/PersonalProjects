using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MGI.Common.Sys;


namespace MGI.Cxn.Customer.FIS.Contract
{
    public class FISProviderException : ProviderException 
    {
        const int FIS_PROVIDER_ID = 600;

		public FISProviderException(int errorCode, string Message)
			: base(FIS_PROVIDER_ID, errorCode.ToString(), Message)
		{

		}

        public FISProviderException(string Message)
			: base(FIS_PROVIDER_ID, string.Empty, Message)
		{

		}

        public static int PROVIDER_SUB_ERROR = 200;
        public static int PROVIDER_VALIDATECUSTOMER_ERROR = 201;
    }
}
