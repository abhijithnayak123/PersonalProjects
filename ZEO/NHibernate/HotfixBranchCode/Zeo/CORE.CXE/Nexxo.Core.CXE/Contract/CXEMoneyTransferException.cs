using System;

using MGI.Common.Sys;

namespace MGI.Core.CXE.Contract
{
    public class CXEMoneyTransferException : NexxoException
    {
        const int MONEYTRANSFER_EXCEPTION_MAJOR_CODE = 1005;

        public CXEMoneyTransferException(int MinorCode, string Message, Exception innerException)
            : base(MONEYTRANSFER_EXCEPTION_MAJOR_CODE, MinorCode, Message, innerException)
        {
        }

        public CXEMoneyTransferException(int MinorCode, Exception innerException)
            : base(MONEYTRANSFER_EXCEPTION_MAJOR_CODE, MinorCode, innerException)
        {
        }

        public CXEMoneyTransferException(int MinorCode, string Message)
            : base(MONEYTRANSFER_EXCEPTION_MAJOR_CODE, MinorCode, Message)
        {
        }

        public static int MONEYTRANSFER_NOT_FOUND = 1000;
        public static int MONEYTRANSFER_CREATE_FAILED = 1001;
        public static int MONEYTRANSFER_UPDATE_FAILED = 1002;
        public static int MONEYTRANSFER_COMMIT_FAILED = 1003;
    }
}
