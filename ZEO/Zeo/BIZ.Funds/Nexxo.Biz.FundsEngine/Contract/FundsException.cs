// -----------------------------------------------------------------------
// <copyright file="FundsException.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
using MGI.Common.Sys;
using System;

namespace MGI.Biz.FundsEngine.Contract
{  

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BizFundsException : NexxoException
    {
        public const int MAJOR_CODE = 1003;  
        
        public static readonly int INVALID_SESSION = 6000;
        public static readonly int UNKNOWN = 6001; // this should be in the base-class. Adding here to use in all cases. Fine-grained exception handling needed.
        public static readonly int PROVIDER_ERROR = 6002;
        public static readonly int ACCOUNT_NOT_FOUND = 6003;
        public static readonly int INVALID_TRANSACTION_REQUEST = 6004;
        public static readonly int STAGE_TRANSACTION_FAILED = 6005;
        public static readonly int COMMIT_TRANSACTION_FAILED = 6006;
        public static readonly int MINIMUM_LIMIT_BREACHED = 6007;
        public static readonly int LOCATION_NOT_SET = 6008;
        public static readonly int PROCESSOR_NOT_SET = 6009;
		public static readonly int FEE_CHANGED = 6010;

		public BizFundsException(int MinorCode, string Message)
			: this(MinorCode, Message, null)
		{
		}

		public BizFundsException(int MinorCode)
			: this(MinorCode, string.Empty)
		{
		}

		public BizFundsException(int MinorCode, Exception innerException)
			: this(MinorCode, string.Empty, innerException)
		{
		}

        public BizFundsException(int MinorCode, string Message, Exception innerException)
			: base(MAJOR_CODE, MinorCode, Message, innerException)
		{
		}
    }
}
