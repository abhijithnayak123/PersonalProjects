using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Biz.MoneyTransfer.Data
{
    public class TransferException : System.ApplicationException  // This is from Funds Exception
    {
        private int Minor;
        private System.Exception Inner; 

        public static readonly int INVALID_SESSION = 101;
        public static readonly int UNKNOWN = 100; 
        public static readonly int PROVIDER_ERROR = 102;
        public static readonly int ACCOUNT_NOT_FOUND = 103;

        public TransferException(int errorCode)
        {
            this.Minor = errorCode;
        }

        public TransferException(int errorCode, System.Exception inner)
            : this(errorCode)
        {
            this.Inner = inner;
        }
    }
}

