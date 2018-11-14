using TCF.Zeo.Common.Data;
using TCF.Zeo.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Cxn.Customer.TCF.Data.Exceptions
{
    public class CustomerProviderException : ProviderException
    {
        public static string ProductCode = ((int)Helper.ProductCode.Customer).ToString();
        public static string ProviderCode = ((int)Helper.ProviderId.TCISCustomer).ToString();

        public CustomerProviderException(string providerErrorCode, string providerMessage, Exception innerException)
            : base(ProductCode, ProviderCode, providerErrorCode, providerMessage, innerException)
        { }

        public static string PROVIDER_ERROR = "2000";
        public static string RCIF_CUSTOMER_REG_ERROR = "1"; //SoftStop
    }
}
