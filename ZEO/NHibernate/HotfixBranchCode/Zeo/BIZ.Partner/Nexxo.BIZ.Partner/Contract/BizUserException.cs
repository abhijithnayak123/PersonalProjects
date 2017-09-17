using System;

using MGI.Common.Sys;

namespace MGI.Biz.Partner.Contract
{
    public class BizUserException : NexxoException 
    { 
        const int PARTNER_EXCEPTION_MAJOR_CODE = 1010;

        public BizUserException(int MinorCode, string Message)
            : this(MinorCode, Message, null)
        {
        }

        public BizUserException(int MinorCode)
            : this(MinorCode, string.Empty)
        {
        }

        public BizUserException(int MinorCode, Exception innerException)
            : this(MinorCode, string.Empty, innerException)
        {
        }

        public BizUserException(int MinorCode, string Message, Exception innerException)
            : base(PARTNER_EXCEPTION_MAJOR_CODE, MinorCode, Message, innerException)
        {
        }
        //4200 - 4299
        public static int USER_CREATE_FAILED = 4200;
        public static int USER_USERNAME_ALREADY_EXISTS = 4201;
        public static int USER_AUTHENTICATION_SAVE_FAILED = 4202;
        public static int USER_NEW_AND_OLD_PASSWORD_SAME = 4203;
    }
}