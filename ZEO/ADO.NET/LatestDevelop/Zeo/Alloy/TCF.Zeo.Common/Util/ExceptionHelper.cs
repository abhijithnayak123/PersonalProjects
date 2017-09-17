using TCF.Zeo.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCF.Zeo.Common.Util
{
    public class ExceptionHelper
    {
        public static bool IsExceptionHandled(Exception ex)
        {
            Exception providerException = ex as ProviderException;
            if (providerException != null)
                return true;

            Exception alloyException = ex as ZeoException;
            if (alloyException != null)
                return true;

            return false;
        }

        public static string GetWUErrorCode(string wuErrorMessage)
        {
            string wuErrorCode = "T0000";
            if (!string.IsNullOrWhiteSpace(wuErrorMessage) && wuErrorMessage.Length >= 5)
            {
                string errorCode = wuErrorMessage.Substring(1, 4);
                int errorNumber;
                bool isConvertionSucceed = Int32.TryParse(errorCode, out errorNumber);
                if (isConvertionSucceed)
                    wuErrorCode = wuErrorMessage.Substring(0, 5);
            }
            return wuErrorCode;
        }
    }
}
