using System;

using MGI.Common.Sys;


namespace MGI.Biz.Partner.Contract
{
    public class BizPartnerException : NexxoException 
    {
        const int PARTNER_EXCEPTION_MAJOR_CODE = 1010;

		public BizPartnerException( int MinorCode, string Message )
			: this( MinorCode, Message, null )
		{
		}

		public BizPartnerException( int MinorCode )
			: this( MinorCode, string.Empty )
		{
		}

		public BizPartnerException( int MinorCode, Exception innerException )
			: this( MinorCode, string.Empty, innerException )
		{
		}

        public BizPartnerException(int MinorCode, string Message, Exception innerException)
            : base(PARTNER_EXCEPTION_MAJOR_CODE, MinorCode, Message, innerException)
		{
		}

		public static int CUSTOMERSESSION_NOT_FOUND = 4100;
       
    }
}
