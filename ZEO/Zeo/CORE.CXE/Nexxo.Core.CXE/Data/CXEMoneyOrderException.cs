using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MGI.Common.Sys;
using MGI.Common.Util;
namespace MGI.Core.CXE.Data
{
	public class CXEMoneyOrderException : AlloyException
    {

		public static string MOProductCode = "1006";
		public static string MOProviderCode = ((int)ProviderId.Alloy).ToString();

		public CXEMoneyOrderException(string alloyErrorCode, Exception innerException)
			: base(MOProductCode, MOProviderCode, alloyErrorCode, string.Empty, innerException)
        {

        }


		public static string MONEYORDER_CREATE_FAILED = "1000";
		public static string MONEYORDER_UPDATE_FAILED = "1001";
        public static string MONEYORDER_COMMIT_FAILED = "1002";
		public static string MONEYORDER_GET_FAILED	  = "1003";
    }
}
