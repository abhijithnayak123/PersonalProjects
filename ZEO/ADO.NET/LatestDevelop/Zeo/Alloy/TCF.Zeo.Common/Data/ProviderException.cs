using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Common.Data
{
    public class ProviderException : Exception
    {
        public string ProductCode { get; set; }
        public string ProviderCode { get; set; }
        public string ProviderErrorCode { get; set; }

        public ProviderException()
        {

        }

        public ProviderException(string productCode, string providerCode, string providerErrorCode, string providerMessage, Exception innerException)
    : base(providerMessage, innerException)
        {
            this.ProductCode = productCode;
            this.ProviderCode = providerCode;
            this.ProviderErrorCode = providerErrorCode;
        }

        public ProviderException(string productCode, string providerCode, string providerErrorCode, string Message)
    : base(Message)
        {
            this.ProviderCode = providerCode;
            this.ProviderErrorCode = providerErrorCode;
        }

        public ProviderException(string providerErrorCode, string Message, Exception InnerException)
            : this(null, null, providerErrorCode, Message, InnerException)
        {
        }


        public static string PROVIDER_FAULT_ERROR = "5000";
        public static string PROVIDER_ENDPOINTNOTFOUND_ERROR = "5001";
        public static string PROVIDER_COMMUNICATION_ERROR = "5002";
        public static string PROVIDER_TIMEOUT_ERROR = "5003";
    }
}