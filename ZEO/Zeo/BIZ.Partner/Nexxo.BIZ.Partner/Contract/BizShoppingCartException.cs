using System;
using MGI.Common.Sys;


namespace MGI.Biz.Partner.Contract
{
    public class BizShoppingCartException : AlloyException
    {
        const string PARTNER_EXCEPTION_PRODUCT_CODE = "1000";
		const string AlloyProviderCode = "100"; 

        public BizShoppingCartException(string errorCode, Exception innerException)
            : base(PARTNER_EXCEPTION_PRODUCT_CODE, AlloyProviderCode, errorCode, innerException)
        {
        }

		public const string ADD_CHECK_SHOPPINGCART_FAILED				= "4800";
		public const string REMOVE_CHECK_SHOPPINGCART_FAILED			= "4801";
		public const string PARK_CHECK_SHOPPINGCART_FAILED				= "4802";
		public const string ADD_BILLPAY_SHOPPINGCART_FAILED				= "4803";
		public const string REMOVE_BILLPAY_SHOPPINGCART_FAILED			= "4804";
		public const string PARK_BILLPAY_SHOPPINGCART_FAILED			= "4805";
		public const string ADD_MONEYTRANSFER_SHOPPINGCART_FAILED		= "4806";
		public const string REMOVE_MONEYTRANSFER_SHOPPINGCART_FAILED	= "4807";
		public const string PARK_MONEYTRANSFER_SHOPPINGCART_FAILED		= "4808";
		public const string ADD_MONEYORDER_SHOPPINGCART_FAILED			= "4809";
		public const string REMOVE_MONEYORDER_SHOPPINGCART_FAILED		= "4810";
		public const string PARK_MONEYORDER_SHOPPINGCART_FAILED			= "4811";
		public const string ADD_FUNDS_SHOPPINGCART_FAILED				= "4812";
		public const string REMOVE_FUNDS_SHOPPINGCART_FAILED			= "4813";
		public const string PARK_FUNDS_SHOPPINGCART_FAILED				= "4814";
		public const string ADD_CASH_SHOPPINGCART_FAILED				= "4815";
		public const string REMOVE_CASH_SHOPPINGCART_FAILED				= "4816";
		public const string GET_SHOPPINGCART_FAILED						= "4817";
		public const string CLOSE_SHOPPINGCART_FAILED					= "4818";
		public const string CHECKOUT_SHOPPINGCART_FAILED				= "4819";
    }
}
