using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Common.Data
{
    public class ApplicationException : ZeoException
    {
        public ApplicationException(string productCode, string alloyCode, string errorCode, Exception innerException)
             : base(productCode, alloyCode, errorCode, innerException) { }
        public ApplicationException(string productCode, string alloyCode, string errorCode, string message, Exception innerException)
            : base(productCode, alloyCode, errorCode, innerException) { }
    }
}
