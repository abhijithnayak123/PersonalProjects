using MGI.Common.Sys;
using MGI.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Core.Partner.Data
{
	public class TransactionHistoryException : AlloyException
	{
		public static string TransactionServiceProductCode = "1000";
		public static string AlloyCode = ((int)ProviderId.Alloy).ToString();

		public TransactionHistoryException(string errorCode, Exception innerException)
			: base(TransactionServiceProductCode, AlloyCode, errorCode, innerException)
        {
        }

		public static readonly string GET_TRANSACTION_FAILED						= "3902";
		public static readonly string GET_PAST_TRANSACTION_FAILED					= "3903";

	}
}
