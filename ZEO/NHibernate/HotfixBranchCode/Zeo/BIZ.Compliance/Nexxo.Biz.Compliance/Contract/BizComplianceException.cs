using System;

using MGI.Common.Sys;

namespace MGI.Biz.Compliance.Contract
{
	public class BizComplianceException: NexxoException
	{
		const int COMPLIANCE_EXCEPTION_MAJOR_CODE = 1008;
		static public int COMPLIANCE_PROGRAM_NOT_FOUND = 5004;

		public BizComplianceException( int MinorCode, string Message )
			: this( MinorCode, Message, null )
		{
		}

		public BizComplianceException( int MinorCode )
			: this( MinorCode, string.Empty )
		{
		}

		public BizComplianceException( int MinorCode, Exception innerException )
			: this( MinorCode, string.Empty, innerException )
		{
		}

		public BizComplianceException( int MinorCode, string Message, Exception innerException )
			: base( COMPLIANCE_EXCEPTION_MAJOR_CODE, MinorCode, Message, innerException )
		{
		}
	}
}
