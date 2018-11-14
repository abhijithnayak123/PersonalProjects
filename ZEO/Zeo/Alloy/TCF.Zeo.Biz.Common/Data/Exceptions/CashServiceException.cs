using TCF.Zeo.Common.Data;
using TCF.Zeo.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Biz.Common.Data.Exceptions
{
    public class CashServiceException : ZeoException
    {
        public static string CashServiceProductCode = ((int)Helper.ProductCode.Alloy).ToString();
        public static string AlloyCode = ((int)Helper.ProviderId.Alloy).ToString();

        public CashServiceException(string alloyErrorCode, Exception innerException)
			: base(CashServiceProductCode, AlloyCode, alloyErrorCode, innerException)
		{
        }

        public static readonly string CASHIN_FAILED = "6000";
        public static readonly string UPDATE_OR_CANCEL_CASHIN_FAILED = "6001";
        public static readonly string GET_CASHTRANSACTION_FAILED = "6002";
        public static readonly string REMOVE_CASHIN_FAILED = "6003";
    }
}
