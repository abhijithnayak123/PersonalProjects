using System;

using MGI.Common.Sys;


namespace MGI.Biz.Partner.Contract
{
    public class BizShoppingCartException : NexxoException
    {
        const int PARTNER_EXCEPTION_MAJOR_CODE = 1010;

        public BizShoppingCartException(int MinorCode, string Message)
            : this(MinorCode, Message, null)
        {
        }

        public BizShoppingCartException(int MinorCode)
            : this(MinorCode, string.Empty)
        {
        }

        public BizShoppingCartException(int MinorCode, Exception innerException)
            : this(MinorCode, string.Empty, innerException)
        {
        }

        public BizShoppingCartException(int MinorCode, string Message, Exception innerException)
            : base(PARTNER_EXCEPTION_MAJOR_CODE, MinorCode, Message, innerException)
        {
        }
    }
}
