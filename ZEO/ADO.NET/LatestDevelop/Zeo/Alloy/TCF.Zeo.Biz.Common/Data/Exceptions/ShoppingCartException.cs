using TCF.Zeo.Common.Data;
using TCF.Zeo.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Biz.Common.Data.Exceptions
{
    public class ShoppingCartException : ZeoException
    {
        static string PARTNER_EXCEPTION_PRODUCT_CODE = ((int)Helper.ProductCode.Alloy).ToString();
        static string AlloyProviderCode = ((int)Helper.ProviderId.Alloy).ToString();

        public ShoppingCartException(string errorCode, Exception innerException)
            : base(PARTNER_EXCEPTION_PRODUCT_CODE, AlloyProviderCode, errorCode, innerException)
        {
        }

        public const string ADD_SHOPPINGCART_TRANSACTION_FAILED = "4800";
        public const string PARK_SHOPPINGCART_TRANSACTION_FAILED = "4801";
        public const string REMOVE_SHOPPINGCART_TRANSACTION_FAILED = "4802";
        public const string GET_SHOPPINGCART_FAILED = "4803";
        public const string SHOPPINGCART_CHECKOUT_FAILED = "4805";
        public const string ISSHOPPINGCARTEMPTY_FAILED = "4806";
        public const string CANCLOSECUSTOMERSESSION_FAILED = "4807";
        public const string GET_ALLPARKEDTRANSACTION_FAILED = "4808";
        public const string GENERATERECEIPTSFORSHOPPINGCART_FAILED = "4809";
        public const string GETRESUBMITTRANSACTIONS_FAILED = "4810";
    }
}
