using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MGI.Common.Sys;
using MGI.Common.Util;

namespace MGI.Core.CXE.Contract
{
	public class CXECheckException : AlloyException
	{
		const string ProductCode = "1002";
		static string AlloyCode = ((int)ProviderId.Alloy).ToString(); 
	
		public CXECheckException(string alloyErrorCode, Exception innerException)
            : base(ProductCode, AlloyCode, alloyErrorCode, string.Empty, innerException)
        {

        }
		public static string CHECK_GET_FAILED	 = "1000";
		public static string CHECK_CREATE_FAILED = "1001";
		public static string CHECK_UPDATE_FAILED = "1002";
		public static string CHECK_COMMIT_FAILED = "1003";
	}
}
