using TCF.Zeo.Common.Data;
using TCF.Zeo.Common.Util;
using System;
using static TCF.Zeo.Common.Util.Helper;

namespace TCF.Zeo.Biz.Common.Data.Exceptions
{
    public class MessageCenterException : ZeoException
    {
        public static string ProductCode = ((int)Helper.ProductCode.Alloy).ToString();
        static string AlloyCode = ((int)ProviderId.Alloy).ToString();

        public MessageCenterException(string alloyErrorCode, Exception innerException)
            : base(ProductCode, AlloyCode, alloyErrorCode, innerException)
        {
        }
        static public string AGENT_MESSAGE_GET_FAILED = "4401";
    }

}
