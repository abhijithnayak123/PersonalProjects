using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MGI.Common.Sys;

namespace MGI.Biz.MoneyTransfer.Contract
{
    public class BizMoneyTransferException : NexxoException
    {
        const int MAJOR_CODE = 1005;  
		public BizMoneyTransferException(int MinorCode, string Message)
			: this(MinorCode, Message, null)
		{
		}

		public BizMoneyTransferException(int MinorCode)
			: this(MinorCode, string.Empty)
		{
		}

		public BizMoneyTransferException(int MinorCode, Exception innerException)
			: this(MinorCode, string.Empty, innerException)
		{
		}

        public BizMoneyTransferException(int MinorCode, string Message, Exception innerException)
			: base(MAJOR_CODE, MinorCode, Message, innerException)
		{
		}

        public static readonly int LOCATION_NOT_SET = 6000;
        public static readonly int PROCESSOR_NOT_SET = 6001;
        public static readonly int ACCOUNT_NOT_FOUND = 6002;
		public static readonly int TRANSACTION_EXISTED = 6003;
    }
}
