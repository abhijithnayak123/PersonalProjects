using System;

namespace MGI.Common.Sys
{
	public class NexxoException : Exception
	{
		public int MajorCode { get; internal set; }
		public int MinorCode { get; internal set; }
        public int ErrorCode { get; internal set; }
		public NexxoExceptionContext Context { get; internal set; }

        public string ProductCode { get; set; }
        public string AlloyCode { get; set; }
        public string AlloyErrorCode { get; set; }

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

        public NexxoException(int MajorCode, int MinorCode, int ErrorCode, string Message)
            :base(Message)
        {
            this.MajorCode = MajorCode;
            this.MinorCode = MinorCode;
            this.ErrorCode = ErrorCode;
        }

        public NexxoException(int MajorCode, int MinorCode, int ErrorCode, string Message, Exception InnerException)
            : base(Message, InnerException)
        {
            this.MajorCode = MajorCode;
            this.MinorCode = MinorCode;
            this.ErrorCode = ErrorCode;
        }

        public NexxoException(string alloyErrorCode)
        {
            this.AlloyErrorCode = alloyErrorCode;
        }

        public NexxoException(string alloyErrorCode, Exception innerException)
            : base(string.Empty, innerException)
        {
            this.AlloyErrorCode = alloyErrorCode;
        }

		public void AddContext( NexxoExceptionContext context )
		{
			this.Context = context;
		}
	}
}
