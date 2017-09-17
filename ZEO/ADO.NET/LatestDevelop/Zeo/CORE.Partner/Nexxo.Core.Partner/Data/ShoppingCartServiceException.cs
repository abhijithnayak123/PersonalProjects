using MGI.Common.Sys;
using MGI.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Core.Partner.Data
{
    class ShoppingCartServiceException : AlloyException
    {
        public static string ProductCode = "1000";
        public static string ProviderCode = ((int)ProviderId.Alloy).ToString();

        public ShoppingCartServiceException(string alloyErrorCode, Exception innerException)
            : base(ProductCode, ProviderCode, alloyErrorCode, innerException)
        {
        }

        public static readonly string SHOPPINGCART_LOOKUP_FAILED                 = "3500";
        public static readonly string SHOPPINGCART_STATUS_UPDATE_FAILED          = "3501";
        public static readonly string SHOPPINGCART_REFERRAL_UPDATE_FAILED        = "3502";
        public static readonly string SHOPPINGCART_LOOKUP_CUSTOMER_FAILED        = "3503";
        public static readonly string SHOPPINGCART_GET_PARKEDITEMS_FAILED        = "3504";
        public static readonly string ADD_TRANSACTION_TO_CART_FAILED             = "3505";
        public static readonly string REMOVE_ITEM_FROM_CART_FAILED               = "3506";
        public static readonly string PARK_TRANSACTION_FAILED                    = "3507";
        public static readonly string GET_SHOPPINGCART_PARKED_ITEM_FAILED        = "3508";

    }
}
