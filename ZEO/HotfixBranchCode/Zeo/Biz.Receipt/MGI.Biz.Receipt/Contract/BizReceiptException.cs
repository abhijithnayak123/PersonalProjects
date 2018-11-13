using System;
using MGI.Common.Sys;

namespace MGI.Biz.Receipt.Contract
{
    public class BizReceiptException : NexxoException
    {
        const int PARTNER_EXCEPTION_MAJOR_CODE = 1010;


        public BizReceiptException(int MinorCode, string Message)
            : this(MinorCode, Message, null)
        {
        }

        public BizReceiptException(int MinorCode)
            : this(MinorCode, string.Empty)
        {
        }

        public BizReceiptException(int MinorCode, Exception innerException)
            : this(MinorCode, string.Empty, innerException)
        {
        }

        public BizReceiptException(int MinorCode, string Message, Exception innerException)
            : base(PARTNER_EXCEPTION_MAJOR_CODE, MinorCode, Message, innerException)
        {
        }
        //4100-4199
        public static int RECEIPT_TEMPLATE_NOT_FOUND = 4100;
        public static int LOCATION_NOT_SET = 4101;
        public static int PROCESSOR_NOT_SET = 4102;
    }
}
