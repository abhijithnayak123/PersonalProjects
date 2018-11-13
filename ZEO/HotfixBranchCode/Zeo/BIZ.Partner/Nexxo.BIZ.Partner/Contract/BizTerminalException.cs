using System;

using MGI.Common.Sys;

namespace MGI.Biz.Partner.Contract
{
    public class BizTerminalException : NexxoException
    {
        const int PARTNER_EXCEPTION_MAJOR_CODE = 1010;

        public BizTerminalException(int MinorCode, string Message)
            : this(MinorCode, Message, null)
        {
        }

        public BizTerminalException(int MinorCode)
            : this(MinorCode, string.Empty)
        {
        }

        public BizTerminalException(int MinorCode, Exception innerException)
            : this(MinorCode, string.Empty, innerException)
        {
        }

        public BizTerminalException(int MinorCode, string Message, Exception innerException)
            : base(PARTNER_EXCEPTION_MAJOR_CODE, MinorCode, Message, innerException)
        {
        }
    }
}