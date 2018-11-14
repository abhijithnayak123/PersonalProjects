using System;

using MGI.Common.Sys;
using MGI.Common.Util;

namespace MGI.Core.CXE.Data
{
    public class CXEMoneyTransferException : AlloyException
    {

		static string MoneyTransferProductCode = "1005";
        static string AlloyCode = ((int)ProviderId.Alloy).ToString();

		public CXEMoneyTransferException(string alloyErrorCode, Exception innerException)
            : base(MoneyTransferProductCode, AlloyCode, alloyErrorCode, string.Empty, innerException)
        {
        }

        public static readonly string MONEYTRANSFER_GET_FAILED								= "1000";
        public static readonly string MONEYTRANSFER_CREATE_FAILED							= "1001";
        public static readonly string MONEYTRANSFER_UPDATE_FAILED							= "1002";
        public static readonly string MONEYTRANSFER_COMMIT_FAILED							= "1003";
		public static readonly string MONEYTRANSFER_GETSTAGE_FAILED							= "1005";
    }
}
