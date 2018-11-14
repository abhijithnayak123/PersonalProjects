using TCF.Zeo.Common.Data;
using TCF.Zeo.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Cxn.Customer.TCF.Data.Exceptions
{
    public class PartnerException : ProviderException
    {
        public static string ProductCode = ((int)Helper.ProductCode.Partner).ToString();
        public static string ProviderCode = ((int)Helper.ProviderId.TCISCustomer).ToString();

        public PartnerException(string MinorCode, string Message, Exception innerException)
            : base(ProductCode, ProviderCode, MinorCode, Message, innerException)
        {
        }

        public PartnerException(string MinorCode, string Message)
            : base(ProductCode, MinorCode, Message, null)
        {
        }

        public static string PROVIDER_ERROR = "3700";
        public static string TCIS_PREFLUSH_ERROR = "3701";
        public static string TCIS_PREFLUSH_NORESPONSE = "3702";

        public static string TELLER_MIDDLETIER_NORESPONSE = "3703";
        public static string TELLER_MAINFRAME_ERROR = "3704";
    }
}
