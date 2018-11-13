using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MGI.Common.Sys;

namespace MGI.Core.CXE.Contract
{
	public class CXECheckException : NexxoException
	{
		const int CHECK_EXCEPTION_MAJOR_CODE = 1002;

		public CXECheckException(int MinorCode, string Message, Exception innerException)
			: base(CHECK_EXCEPTION_MAJOR_CODE, MinorCode, Message, innerException)
		{
		}

		public CXECheckException(int MinorCode, Exception innerException)
			: base(CHECK_EXCEPTION_MAJOR_CODE, MinorCode, innerException)
		{
		}

		public CXECheckException(int MinorCode, string Message)
			: base(CHECK_EXCEPTION_MAJOR_CODE, MinorCode, Message)
		{
		}

		public static int CHECK_NOT_FOUND = 1000;
		public static int CHECK_CREATE_FAILED = 1001;
		public static int CHECK_UPDATE_FAILED = 1002;
		public static int CHECK_COMMIT_FAILED = 1003;
	}
}
