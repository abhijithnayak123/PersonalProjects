using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MGI.Common.Sys;

namespace MGI.Biz.Partner.Contract
{
    public class BizCheckPrintException: NexxoException
    {
         const int PARTNER_EXCEPTION_MAJOR_CODE = 1010;


        public BizCheckPrintException(int MinorCode, string Message)
            : this(MinorCode, Message, null)
        {
        }

        public BizCheckPrintException(int MinorCode)
            : this(MinorCode, string.Empty)
        {
        }

        public BizCheckPrintException(int MinorCode, Exception innerException)
            : this(MinorCode, string.Empty, innerException)
        {
        }

        public BizCheckPrintException(int MinorCode, string Message, Exception innerException)
            : base(PARTNER_EXCEPTION_MAJOR_CODE, MinorCode, Message, innerException)
        {
        }
        //4200-4299
        public static int CHECKPRINT_TEMPLATE_NOT_FOUND = 4200;
    }
}
