using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MGI.Common.Sys;

namespace MGI.Biz.MoneyOrderEngine.Contract
{
    public class BizMoneyOrderEngineException : NexxoException
    {
        const int MAJOR_CODE = 1006;

        public static readonly int LOCATION_NOT_SET = 6000;
        public static readonly int CHECKPRINT_TEMPLATE_NOT_FOUND = 6001;
		public static readonly int FEE_CHANGED = 6002;
        public static readonly int MONEYORDER_COMMIT_ALREADY_EXIST = 6003;

        public BizMoneyOrderEngineException(int MinorCode, string Message)
            : this(MinorCode, Message, null)
        {
        }

        public BizMoneyOrderEngineException(int MinorCode)
            : this(MinorCode, string.Empty)
        {
        }

        public BizMoneyOrderEngineException(int MinorCode, Exception innerException)
            : this(MinorCode, string.Empty, innerException)
        {
        }

        public BizMoneyOrderEngineException(int MinorCode, string Message, Exception innerException)
            : base(MAJOR_CODE, MinorCode, Message, innerException)
        {
        }
    }
}