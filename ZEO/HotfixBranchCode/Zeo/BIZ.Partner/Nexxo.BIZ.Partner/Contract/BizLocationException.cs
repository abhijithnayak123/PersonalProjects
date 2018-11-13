using System;

using MGI.Common.Sys;

namespace MGI.Biz.Partner.Contract
{
    public class BizLocationException : NexxoException
    {
        const int PARTNER_EXCEPTION_MAJOR_CODE = 1010;

        public BizLocationException(int MinorCode, string Message)
            : this(MinorCode, Message, null)
        {
        }

        public BizLocationException(int MinorCode)
            : this(MinorCode, string.Empty)
        {
        }

        public BizLocationException(int MinorCode, Exception innerException)
            : this(MinorCode, string.Empty, innerException)
        {
        }

        public BizLocationException(int MinorCode, string Message, Exception innerException)
            : base(PARTNER_EXCEPTION_MAJOR_CODE, MinorCode, Message, innerException)
        {
        }

    }
}
