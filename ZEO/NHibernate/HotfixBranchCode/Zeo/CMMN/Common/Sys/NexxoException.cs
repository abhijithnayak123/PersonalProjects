using System;

namespace MGI.Common.Sys
{
	public class NexxoException : Exception
	{
		public int MajorCode { get; internal set; }
		public int MinorCode { get; internal set; }
		public NexxoExceptionContext Context { get; internal set; }

		public NexxoException(int MajorCode, int MinorCode)
			: this(MajorCode, MinorCode, string.Empty)
		{
		}

		public NexxoException( int MajorCode, int MinorCode, string Message )
			: base( Message )
		{
			this.MajorCode = MajorCode;
			this.MinorCode = MinorCode;
		}

		public NexxoException( int MajorCode, int MinorCode, Exception innerException )
			: this( MajorCode, MinorCode, string.Empty, innerException )
		{
		}

		public NexxoException(int MajorCode, int MinorCode, string Message, Exception innerException)
			: base(Message, innerException)
		{
			this.MajorCode = MajorCode;
			this.MinorCode = MinorCode;
		}

		public void AddContext( NexxoExceptionContext context )
		{
			this.Context = context;
		}
	}
}
