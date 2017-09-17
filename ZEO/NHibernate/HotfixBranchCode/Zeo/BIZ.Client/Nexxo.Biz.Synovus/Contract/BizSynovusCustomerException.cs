using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MGI.Common.Sys;

namespace MGI.Biz.Synovus.Contract
{
    public class BizSynovusCustomerException : NexxoException
    {
        const int SYNOVUS_CUSTOMER_EXCEPTION_MAJOR_CODE = 1013;

		public BizSynovusCustomerException( int MinorCode, string Message )
			: this( MinorCode, Message, null )
		{
		}

		public BizSynovusCustomerException( int MinorCode )
			: this( MinorCode, string.Empty )
		{
		}

		public BizSynovusCustomerException( int MinorCode, Exception innerException )
			: this( MinorCode, string.Empty, innerException )
		{
		}

        public BizSynovusCustomerException(int MinorCode, string Message, Exception innerException)
            : base(SYNOVUS_CUSTOMER_EXCEPTION_MAJOR_CODE, MinorCode, Message, innerException)
		{
		}
    }
}
