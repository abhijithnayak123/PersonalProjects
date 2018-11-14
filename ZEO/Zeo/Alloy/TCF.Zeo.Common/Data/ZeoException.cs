using System;
using TCF.Zeo.Common.Util;

namespace TCF.Zeo.Common.Data
{
    public class ZeoException : Exception
    {
        public string ProductCode { get; set; }
        public string ProviderCode { get; set; }
        public string ZeoErrorCode { get; set; }

        public ZeoException(string productCode, string zeoCode, string zeoErrorCode, Exception innerException)
            : base(string.Empty, innerException)
        {
            this.ProductCode = productCode;
            this.ProviderCode = zeoCode;
            this.ZeoErrorCode = zeoErrorCode;
        }

        public ZeoException(string productCode, string zeoCode, string zeoErrorCode, string message, Exception innerException)
            : base(message, innerException)
        {
            this.ProductCode = productCode;
            this.ProviderCode = zeoCode;
            this.ZeoErrorCode = zeoErrorCode;
        }
    }
}