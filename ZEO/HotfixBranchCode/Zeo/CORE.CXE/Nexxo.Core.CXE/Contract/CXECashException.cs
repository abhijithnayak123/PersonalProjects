using System;

using MGI.Common.Sys;

namespace MGI.Core.CXE.Contract
{
    public class CXECashException : NexxoException 
    {
        const int CASH_EXCEPTION_MAJOR_CODE = 1000;

		public CXECashException(int MinorCode, string Message, Exception innerException)
            : base(CASH_EXCEPTION_MAJOR_CODE, MinorCode, Message, innerException)
		{
		}

		public CXECashException(int MinorCode, Exception innerException)
            : base(CASH_EXCEPTION_MAJOR_CODE, MinorCode, innerException)
		{
		}

        public CXECashException(int MinorCode, string Message)
            : base(CASH_EXCEPTION_MAJOR_CODE, MinorCode, Message)
		{
		}

		public static int CASH_NOT_FOUND = 1000;		
		public static int CASH_UPDATE_FAILED = 1001;
		public static int CASH_COMMIT_FAILED = 1002;
        public static int CASH_CREATE_FAILED = 1003;

    }
}
