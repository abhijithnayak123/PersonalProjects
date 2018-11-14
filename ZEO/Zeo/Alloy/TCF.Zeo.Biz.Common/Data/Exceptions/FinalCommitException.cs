using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCF.Zeo.Common.Data;
using TCF.Zeo.Common.Util;

namespace TCF.Zeo.Biz.Common.Data.Exceptions
{
    public class FinalCommitException : ZeoException
    {
        public static string ProductCode = ((int)Helper.ProductCode.Partner).ToString();
        public static string AlloyCode = ((int)Helper.ProviderId.Alloy).ToString();

        public FinalCommitException(string alloyErrorCode, Exception innerException)
            : base(ProductCode, AlloyCode, alloyErrorCode, innerException)
        {
        }

        public FinalCommitException(string alloyErrorCode)
            : base(ProductCode, AlloyCode, alloyErrorCode, null)
        {
        }
        public static readonly string FINAL_COMMIT_FAILED = "6001";
    }
}
