
using MGI.Common.Sys;
using MGI.Common.Util;
using System;

namespace MGI.Biz.FundsEngine.Data
{

	/// <summary>
	/// TODO: Update summary.
	/// </summary>
	public class BizFundsException : AlloyException
	{
		public const string FundsProductCode = "1003";
		public static string FundsProviderCode = ((int)ProviderId.Alloy).ToString();

		public BizFundsException(string alloyErrorCode, Exception innerException)
			: base(FundsProductCode, FundsProviderCode, alloyErrorCode, innerException)
		{
		}

		public BizFundsException(string alloyErrorCode)
			: base(FundsProductCode, FundsProviderCode, alloyErrorCode, null)
		{
		}

		public static readonly string INVALID_SESSION = "6000";
		public static readonly string UNKNOWN = "6001"; // this should be in the base-class. Adding here to use in all cases. Fine-grained exception handling needed.
		public static readonly string PROVIDER_ERROR = "6002";
		public static readonly string ACCOUNT_NOT_FOUND = "6003";
		public static readonly string INVALID_TRANSACTION_REQUEST = "6004";
		public static readonly string STAGE_TRANSACTION_FAILED = "6005";
		public static readonly string FEE_CHANGED = "6010";

		//AL-6773 CHANGES
		public static readonly string FUNDS_EXCEPTION = "6011";
		public static readonly string ADD_FUNDS_FAILED = "6012";
		public static readonly string FUNDS_AUTHENTICATECARD_FAILED = "6013";
		public static readonly string FUNDS_BALANCE_RETRIVEL_FAILED = "6014";
		public static readonly string FUNDS_TRANSACTION_RETRIVEL_FAILED = "6015";
		public static readonly string FUNDS_FEE_FAILED = "6016";
		public static readonly string FUNDS_UPDATE_FAILED = "6017";
		public static readonly string FUNDS_GETMINIMUM_FAILED = "6018";
		public static readonly string FUNDS_CANCEL_TRANSACTION_FAILED = "6019";
		public static readonly string FUNDS_TRANSACTION_HISTORY_RETRIVEL_FAILED = "6020";
		public static readonly string FUNDS_ACCOUNT_CLOSURE_FAILED = "6021";
		public static readonly string FUNDS_UPDATE_STATUS_FAILED = "6022";
		public static readonly string FUNDS_CARD_REPLACE_FAILED = "6023";
		public static readonly string FUNDS_SHIPPING_TYPE_RETRIVEL_FAILED = "6024";
		public static readonly string FUNDS_SHIPPING_FEE_RETRIVEL_FAILED = "6025";
		public static readonly string FUNDS_CARD_ASSOCIATION_FAILED = "6026";
		public static readonly string FUNDS_CARDMAINTENANCE_FEE_RETRIVEL_FAILED = "6027";
		public static readonly string FUNDS_WITHDRAW_FAILED = "6028";
		public static readonly string FUNDS_ACTIVATE_FAILED = "6029";
		public static readonly string FUNDS_LOAD_FAILED = "6030";
		public static readonly string FUNDS_ORDER_COMPANION_FAILED = "6031";
	}
}
