using MGI.Common.Sys;
using Spring.Aop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Cxn.Partner.TCF.Data
{
	public class PartnerException : NexxoException
	{
		const int Partner_EXCEPTION_MAJOR_CODE = 1010;
		public static int TCIS_PreFlush;
		public static int TCIS_PostFlush;

		public PartnerException(int MinorCode, string Message, Exception innerException)
            : base(Partner_EXCEPTION_MAJOR_CODE, MinorCode, Message, innerException)
		{

		}

		public PartnerException(int MinorCode, Exception innerException)
            : base(Partner_EXCEPTION_MAJOR_CODE, MinorCode, innerException)
		{

		}

		public PartnerException(int MinorCode, string Message)
            : base(Partner_EXCEPTION_MAJOR_CODE, MinorCode, Message)
        {

        }

        public static int PROVIDER_ERROR = 3700;
		public static int NO_RESPONSE_CASHIN = 3701;
        
	}
}
