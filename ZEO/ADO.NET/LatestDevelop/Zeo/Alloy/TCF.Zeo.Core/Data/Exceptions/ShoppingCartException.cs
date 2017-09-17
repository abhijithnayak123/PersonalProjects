using TCF.Zeo.Common.Data;
using TCF.Zeo.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Core.Data.Exceptions
{
    public class ShoppingCartException : ZeoException
    {
        public static string ShoppingCartProductCode = ((int)Helper.ProductCode.Alloy).ToString();
        public static string AlloyCode = ((int)Helper.ProviderId.Alloy).ToString();

        public ShoppingCartException(string alloyErrorCode, Exception innerException)
            : base(ShoppingCartProductCode, AlloyCode, alloyErrorCode, innerException)
        {
        }

        public static string GET_SHOPPINGCART_FAILED = "3800";
        public static string SHOPPINGCART_STATUS_UPDATE_FAILED = "3801";
        public static string SHOPPINGCART_REFERRAL_UPDATE_FAILED = "3802";
        public static string GET_SHOPPINGCARTBYID_FAILED = "3803";
        public static string ADD_TRANSACTION_TO_CART_FAILED = "3804";
        public static string REMOVE_ITEM_FROM_CART_FAILED = "3805";
        public static string PARK_TRANSACTION_FAILED = "3806";
        public static string GET_SHOPPINGCART_PARKED_ITEM_FAILED = "3807";
        public static string GET_SHOPPINGCART_CHECKOUT_DETAILS_FAILED = "3808";
        public static string IS_SHOPPINGCART_EMPTY_FAILED = "3809";
        public static string CAN_CLOSE_CUSTOMER_SESSION_FAILED = "3810";
        public static string GET_RESUBMIT_TRANSACTIONS_FAILED = "3811";
    }
}
