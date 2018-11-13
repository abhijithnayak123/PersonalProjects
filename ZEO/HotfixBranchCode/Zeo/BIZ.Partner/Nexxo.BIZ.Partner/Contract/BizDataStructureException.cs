using System;

using MGI.Common.Sys;


namespace MGI.Biz.Partner.Contract
{
    public class BizDataStructureException : NexxoException
    {
        const int PARTNER_EXCEPTION_MAJOR_CODE = 1010;

        public BizDataStructureException(int MinorCode, string Message)
            : this(MinorCode, Message, null)
        {
        }

        public BizDataStructureException(int MinorCode)
            : this(MinorCode, string.Empty)
        {
        }

        public BizDataStructureException(int MinorCode, Exception innerException)
            : this(MinorCode, string.Empty, innerException)
        {
        }

        public BizDataStructureException(int MinorCode, string Message, Exception innerException)
            : base(PARTNER_EXCEPTION_MAJOR_CODE, MinorCode, Message, innerException)
        {
        }

    }
}
