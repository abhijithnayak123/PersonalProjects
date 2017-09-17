using MGI.Common.Sys;
using MGI.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Biz.Partner.Data
{
	public class BizTransactionHistoryException : AlloyException
	{
		public static string TransactionHistoryProductCode = "1000";
		public static string AlloyCode = ((int)ProviderId.Alloy).ToString();

		public BizTransactionHistoryException(string alloyErrorCode, Exception innerException)
			: base(TransactionHistoryProductCode, AlloyCode, alloyErrorCode, innerException)
        {
        }
		public static readonly string GET_CUSTOMER_TRANSACTION_HISTORY_FAILED					= "4900";
		public static readonly string GET_AGENT_TRANSACTION_HISTORY_FAILED						= "4901";
		public static readonly string GET_TRANSACTION_FAILED									= "4902";
		public static readonly string GET_PAST_TRANSACTION_FAILED								= "4903";
		public static readonly string GET_CASH_TRANSACTION_FAILED								= "4904";
		public static readonly string GET_FUND_TRANSACTION_FAILED								= "4905";
		public static readonly string GET_CHECK_TRANSACTION_FAILED								= "4906";
		public static readonly string GET_MONEYTRANSFER_TRANSACTION_FAILED						= "4907";
		public static readonly string GET_MONEYORDER_TRANSACTION_FAILED							= "4908";
		public static readonly string GET_BILLPAY_TRANSACTION_FAILED							= "4909";
	}
}
