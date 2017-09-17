using System;

using MGI.Common.Sys;


namespace MGI.Biz.Partner.Contract
{
    public class BizAgentException : NexxoException 
    {
        const int PARTNER_EXCEPTION_MAJOR_CODE = 1010;

        public BizAgentException( int MinorCode, string Message )
			: this( MinorCode, Message, null )
		{
		}

		public BizAgentException( int MinorCode )
			: this( MinorCode, string.Empty )
		{
		}

		public BizAgentException( int MinorCode, Exception innerException )
			: this( MinorCode, string.Empty, innerException )
		{
		}

        public BizAgentException(int MinorCode, string Message, Exception innerException)
            : base(PARTNER_EXCEPTION_MAJOR_CODE, MinorCode, Message, innerException)
		{
		}
        // 4000-4099
        public static int AGENTSESSION_NOT_CREATED = 4000;
        public static int AGENTSESSION_NOT_FOUND = 4001;
		public static int AGENTSESSION_NOT_UPDATED = 4002;
    }
}
