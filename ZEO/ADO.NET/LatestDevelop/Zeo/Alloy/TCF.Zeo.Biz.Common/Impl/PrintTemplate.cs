using TCF.Zeo.Biz.Common.Contract;
using TCF.Zeo.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCF.Zeo.Core.Contract;
using TCF.Zeo.Core.Impl;

namespace TCF.Zeo.Biz.Common.Impl
{
	public class PrintTemplate : IPrintTemplate
	{
        IReceiptService ReceiptService;
        public string GetPrintTemplate(List<string> fileNames)
		{
            string response = string.Empty;
            ReceiptService = new ReceiptServiceImpl();
            response = ReceiptService.GetStringReceiptTemplate(fileNames);
            return response;
		}
	}
}
