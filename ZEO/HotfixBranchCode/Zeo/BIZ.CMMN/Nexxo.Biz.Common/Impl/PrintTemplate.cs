using MGI.Biz.Common.Contract;
using MGI.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Biz.Common.Impl
{
	public class PrintTemplate : IPrintTemplate
	{
		public NLoggerCommon NLogger { get; set; }
		public TLoggerCommon MongoDBLogger { get; set; }

		public string GetPrintTemplate(string requestUri)
		{
			string response = string.Empty;
			try
			{
				response = NexxoUtil.ExecuteRESTFulService(requestUri);
			}
			catch (Exception ex)
			{
				NLogger.Error("Error in get CheckPrint Template. " + ex.Message);
			}

			return response;
		}
	}
}
