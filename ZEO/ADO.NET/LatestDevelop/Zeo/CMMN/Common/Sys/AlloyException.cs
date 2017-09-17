using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Common.Sys
{
    public class AlloyException : Exception
    {
        public string ProductCode { get; set; }
        public string ProviderCode { get; set; }
        public string AlloyErrorCode { get; set; }

        public AlloyException(string productCode, string alloyCode, string alloyErrorCode, Exception innerException)
            : base(string.Empty, innerException)
        {
            this.ProductCode = productCode;
            this.ProviderCode = alloyCode;
            this.AlloyErrorCode = alloyErrorCode;
        }

        public AlloyException(string productCode, string alloyCode, string alloyErrorCode, string message, Exception innerException)
            : base(message, innerException)
        {
            this.ProductCode = productCode;
            this.ProviderCode = alloyCode;
            this.AlloyErrorCode = alloyErrorCode;
        }
    }
}
