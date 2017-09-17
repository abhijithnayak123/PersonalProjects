using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MGI.Common.Sys;
namespace MGI.Core.CXE.Contract
{
    public class CXEMoneyOrderException: NexxoException
    {

        const int MONEYORDER_EXCEPTION_MAJOR_CODE = 1006;

		public CXEMoneyOrderException(int MinorCode, string Message, Exception innerException)
			: base(MONEYORDER_EXCEPTION_MAJOR_CODE, MinorCode, Message, innerException)
		{
		}

		public CXEMoneyOrderException(int MinorCode, Exception innerException)
			: base(MONEYORDER_EXCEPTION_MAJOR_CODE, MinorCode, innerException)
		{
		}

		public CXEMoneyOrderException(int MinorCode, string Message)
			: base(MONEYORDER_EXCEPTION_MAJOR_CODE, MinorCode, Message)
		{
		}

		public static int MONEYORDER_CREATE_FAILED = 1000;
		public static int MONEYORDER_UPDATE_FAILED = 1001;
        public static int MONEYORDER_COMMIT_FAILED = 1002;
        public static int MONEYORDER_NOT_FOUND = 1003;
    }
}
