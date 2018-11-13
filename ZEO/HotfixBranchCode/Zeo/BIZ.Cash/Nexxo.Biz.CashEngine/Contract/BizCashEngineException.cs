using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MGI.Common.Sys;

namespace MGI.Biz.CashEngine.Contract
{
    public class BizCashEngineException : NexxoException
    {
        const int CASH_EXCEPTION_MAJOR_CODE = 1000;

		public BizCashEngineException(int MinorCode, string Message)
			: this(MinorCode, Message, null)
		{
		}

		public BizCashEngineException(int MinorCode)
			: this(MinorCode, string.Empty)
		{
		}

		public BizCashEngineException(int MinorCode, Exception innerException)
			: this(MinorCode, string.Empty, innerException)
		{
		}

        public BizCashEngineException(int MinorCode, string Message, Exception innerException)
            : base(CASH_EXCEPTION_MAJOR_CODE, MinorCode, Message, innerException)
		{
		}

        static public int INVALID_SESSION = 6000;
    }
}
