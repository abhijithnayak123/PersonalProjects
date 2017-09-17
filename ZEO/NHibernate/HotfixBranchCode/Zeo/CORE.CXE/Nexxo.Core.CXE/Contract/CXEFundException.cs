using System;

using MGI.Common.Sys;

namespace MGI.Core.CXE.Contract
{
    public class CXEFundException : NexxoException 
    {
        const int FUND_EXCEPTION_MAJOR_CODE = 1003;

		public CXEFundException(int MinorCode, string Message, Exception innerException)
			: base(FUND_EXCEPTION_MAJOR_CODE, MinorCode, Message, innerException)
		{
		}

		public CXEFundException(int MinorCode, Exception innerException)
			: base(FUND_EXCEPTION_MAJOR_CODE, MinorCode, innerException)
		{
		}

        public CXEFundException(int MinorCode, string Message)
			: base(FUND_EXCEPTION_MAJOR_CODE, MinorCode, Message)
		{
		}

		public static int FUND_NOT_FOUND = 1000;
		public static int FUND_CREATE_FAILED = 1001;
		public static int FUND_UPDATE_FAILED = 1002;
		public static int FUND_COMMIT_FAILED = 1003;
    }
}
