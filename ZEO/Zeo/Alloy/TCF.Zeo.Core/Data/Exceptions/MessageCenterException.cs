using TCF.Zeo.Common.Data;
using TCF.Zeo.Common.Util;
using System;
using static TCF.Zeo.Common.Util.Helper;

namespace TCF.Zeo.Core.Data.Exceptions
{    
    public class MessageCenterException : ZeoException
    {
        static string ProductCode = ((int)Helper.ProductCode.Alloy).ToString();
        static string AlloyCode = ((int)ProviderId.Alloy).ToString();

        public MessageCenterException(string alloyErrorCode, Exception innerException)
            : base(ProductCode, AlloyCode, alloyErrorCode, innerException)
        {
        }

        public static string AGENT_MESSAGE_DELETE_FAILED = "3402";
        public static string AGENT_MESSAGE_GET_FAILED = "3403";

    }
}
