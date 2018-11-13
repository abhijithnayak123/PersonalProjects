// -----------------------------------------------------------------------
// <copyright file="FundsException.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MGI.Biz.FundsEngine.Data
{
    //using MGI.Common.Sys;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class FundsException : System.ApplicationException
    {
        private int Minor;
        private System.Exception Inner; // this should be NexxoProviderException with ProviderId and ProviderError code plus Message.
        
        public static readonly int INVALID_SESSION = 101;
        public static readonly int UNKNOWN = 100; // this should be in the base-class. Adding here to use in all cases. Fine-grained exception handling needed.
        public static readonly int PROVIDER_ERROR = 102;
        public static readonly int ACCOUNT_NOT_FOUND = 103;
        public static readonly int INVALID_TRANSACTION_REQUEST = 4004;
        public static readonly int STAGE_TRANSACTION_FAILED = 4005;
        public static readonly int COMMIT_TRANSACTION_FAILED = 4006;

        public FundsException(int errorCode) {
            this.Minor = errorCode;
        }

        public FundsException(int errorCode, System.Exception inner) : this(errorCode) {
            this.Inner = inner;
        }
    }
}
