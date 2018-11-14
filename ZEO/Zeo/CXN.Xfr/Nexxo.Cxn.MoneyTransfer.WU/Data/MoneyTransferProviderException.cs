using MGI.Common.Sys;
using MGI.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Cxn.MoneyTransfer.WU.Data
{
	public class MoneyTransferProviderException : ProviderException, IExceptionHelper
	{
		public static string MoneyTransferProductCode = ((int)MGI.Common.Util.ProductCode.MONEY_TRANSFER_PRODUCTCODE).ToString();
		public static string WUProviderCode = ((int)MGI.Common.Util.ProviderId.WesternUnion).ToString();

		public MoneyTransferProviderException()
        {
        }

		public MoneyTransferProviderException(string providerErrorCode, string providerMessage, Exception innerException)
            : base(MoneyTransferProductCode, WUProviderCode, providerErrorCode, providerMessage, innerException)
        {
        }

		public string GetProviderErrorCode(string message)
		{
			string wuErrorCode = "T0000";
			if (!string.IsNullOrWhiteSpace(message) && message.Length >= 5)
			{
				string errorCode = message.Substring(1, 4);
				int errorNumber;
				bool isConvertionSucceed = Int32.TryParse(errorCode, out errorNumber);
				if (isConvertionSucceed)
					wuErrorCode = message.Substring(0, 5);
			}
			return wuErrorCode;
		}

		public bool IsExceptionHandled(Exception ex)
		{
			Exception providerException = ex as ProviderException;
            if (providerException != null)
                return true;

            Exception alloyException = ex as AlloyException;
            if (alloyException != null)
                return true;

            return false;
		}
	}
}
