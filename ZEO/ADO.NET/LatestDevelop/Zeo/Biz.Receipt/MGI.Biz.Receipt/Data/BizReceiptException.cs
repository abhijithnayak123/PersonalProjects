using System;
using MGI.Common.Sys;
using MGI.Common.Util;

namespace MGI.Biz.Receipt.Data
{
	public class BizReceiptException : AlloyException
	{
		public const string ReceiptProductCode = "1000";
		public static string AlloyCode = ((int)ProviderId.Alloy).ToString();

		public BizReceiptException(string alloyErrorCode)
			: base(ReceiptProductCode, AlloyCode, alloyErrorCode, null)
		{
		}

		public BizReceiptException(string alloyErrorCode, Exception innerException)
			: base(ReceiptProductCode, AlloyCode, alloyErrorCode, innerException)
		{
		}

		//4100-4199
		public static string RECEIPT_TEMPLATE_NOT_FOUND								= "4700";
		public static string CHECK_RECEIPT_TEMPLATE_RETRIVEL_FAILED					= "4703";
		public static string MT_RECEIPT_TEMPLATE_RETRIVEL_FAILED					= "4704";
		public static string FUNDS_RECEIPT_TEMPLATE_RETRIVEL_FAILED					= "4705";
		public static string BILLPAY_RECEIPT_TEMPLATE_RETRIVEL_FAILED				= "4706";
		public static string MONEYORDER_RECEIPT_TEMPLATE_RETRIVEL_FAILED			= "4707";
		public static string SUMMARY_RECEIPT_TEMPLATE_RETRIVEL_FAILED				= "4708";
		public static string CHECKDECLINED_RECEIPT_TEMPLATE_RETRIVEL_FAILED			= "4709";
		public static string COUPON_RECEIPT_TEMPLATE_RETRIVEL_FAILED				= "4710";
		public static string DODFRANK_RECEIPT_RETRIVEL_FAILED						= "4711";
		public static string RECEIPT_EXCEPTION										= "4712";
	}
}
