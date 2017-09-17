using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCF.Zeo.Common.Data;
using TCF.Zeo.Common.Util;

namespace TCF.Zeo.Cxn.Fund.Data.Exceptions
{
	public class FundException : ZeoException
	{
		static string ProductCode = ((int)Helper.ProductCode.Funds).ToString();
		static string ProviderCode = ((int)Helper.ProviderId.Alloy).ToString();


		public FundException(string errorCode)
			: base(ProductCode, ProviderCode, errorCode, null)
		{
		}

		public FundException(string errorCode, Exception innerException)
			: base(ProductCode, ProviderCode, errorCode, innerException)
		{
		}

		public static string ACCOUNT_NOT_FOUND = "2001";
		public static string TRANSACTION_NOT_FOUND = "2003";
		public static string CARD_ACTIVATION_FAILED = "2010";
		public static string INVALID_CUSTOMER_DETAILS = "2011";
		public static string CARD_ALREADY_REGISTERED = "2013";
		public static string ERROR_OCCURRED_WHILE_SAVING = "2041";
		public static string CAN_NOT_UPDATE_TRANSACTION = "2054";
		public static string INVALID_EXPIRATION_DATE = "2063";
		public static string PAN_NUMBER_MISMATCH = "2090";
		public static string PSEUDO_DDA_MISMATCH = "2106";
		public static string CARD_ALREADY_ISSUED = "2109";
		public static string CARD_ASSOCIATION_ERROR = "2110";
		public static string CARD_MAPPING_ERROR = "2111";

		//Added as part of AL-6773
		public static string CARD_INFORMATION_RETRIEVAL_ERROR = "2113";
		public static string CARD_BALANCE_RETRIEVAL_ERROR = "2114";
		public static string CARD_LOAD_ERROR = "2115";
		public static string CARD_WITHDRAW_ERROR = "2116";
		public static string CARD_HISTORY_RETRIVEL_ERROR = "2117";
		public static string ACCOUNT_CLOSURE_ERROR = "2118";
		public static string CARD_STATUS_UPDATE_ERROR = "2119";
		public static string COMPANION_CARD_ORDER_ERROR = "2120";
		public static string CARD_REPLACEMENT_ERROR = "2121";
		public static string ACCOUNT_CREATE_ERROR = "2122";
		public static string FUND_REQUEST_NOTFOUND = "2123";
		public static string ACCOUNT_RETRIVEL_FAILED = "2124";
		public static string GET_CARDNUMBER_FAILED = "2125";
		public static string ACCOUNT_CLOSE_FAILED = "2126";
		public static string SHIPPING_RETRIVEL_FAILED = "2127";
		public static string FEE_RETRIVEL_FAILED = "2128";
		public static string COMMIT_FAILED = "2129";
		public static string STAGE_TRANSACTION_FAILED = "2130";
        public static string CARD_ACCOUNT_HISTORY_RETRIEVAL_ERROR = "2131";

    }
}
