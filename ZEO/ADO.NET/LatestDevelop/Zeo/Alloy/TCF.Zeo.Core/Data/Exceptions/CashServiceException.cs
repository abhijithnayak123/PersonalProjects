using TCF.Zeo.Common.Data;
using TCF.Zeo.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Core.Data.Exceptions
{
    public class CashServiceException : ZeoException
    {
        public static string CashProductCode = ((int)Helper.ProductCode.Cash).ToString();
        public static string AlloyCode = ((int)Helper.ProviderId.Alloy).ToString();

        public CashServiceException(string alloyErrorCode, Exception innerException)
           : base(CashProductCode, AlloyCode, alloyErrorCode, innerException)
        {
        }

        public static string CASHIN_FAILED = "1000";
        public static string GETCASHTRANSACTION_FAILED = "1001";
        public static string REMOVECASHIN_FAILED = "1002";
        public static string UPDATEORCANCELCASH_FAILED = "1003";
    }
}
