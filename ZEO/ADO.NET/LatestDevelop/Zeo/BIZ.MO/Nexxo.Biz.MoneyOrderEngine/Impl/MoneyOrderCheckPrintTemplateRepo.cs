using System;
using MGI.Common.Util;
using PTNRContract = MGI.Core.Partner.Contract;
using PTNRData = MGI.Core.Partner.Data;
using System.Collections.Generic;
using System.Diagnostics;
using MGI.Biz.Common.Contract;
using MGI.Biz.MoneyOrderEngine.Data;

namespace MGI.Biz.MoneyOrderEngine.Impl
{
    public class MoneyOrderCheckPrintTemplateRepo
    {
        public string BaseUrl { get; set; }
        public string Database { get; set; }
		public string ReceiptBucket { get; set; }
		public IPrintTemplate PrintTemplateRepo { get; set; }

        //  public TLoggerCommon tLogger { get; set; }

        public TLoggerCommon tLogger { get; set; }

        public MoneyOrderCheckPrintTemplateRepo(string baseUrl, string dbName, string receiptBucket)
        {
				this.BaseUrl = baseUrl;
				this.Database = dbName;
				this.ReceiptBucket = receiptBucket;
        }

        public string GetCheckPrintTemplate(string partner, PTNRData.Transactions.TransactionType product, PTNRContract.ProviderIds processor, string state)
        {
			try
			{
				string checkPrintFileComplete = string.Format("{0}.{1}.{2}.{3}.CheckPrint.txt", partner, product.ToString(), processor.ToString(), state);

				string checkPrintFileWithProcessor = string.Format("{0}.{1}.{2}.CheckPrint.txt", partner, product.ToString(), processor.ToString());

				string checkPrintFileBase = string.Format("{0}.{1}.CheckPrint.txt", partner, product.ToString());

				List<string> checkPrintfiles = new List<string>() { checkPrintFileComplete, checkPrintFileWithProcessor, checkPrintFileBase };

				return GetCheckPrintFile(checkPrintfiles);
			}
			catch (Exception ex)
			{
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new BizMoneyOrderEngineException(BizMoneyOrderEngineException.CHECKPRINT_TEMPLATE_NOT_FOUND, ex);
			}
        }

		public string GetMoneyOrderDiagnosticsTemplate()
		{
			try
			{
				List<string> moneyOrderTemplates = new List<string>() { "MoneyOrder.CheckPrint.Test.txt" };

				return GetCheckPrintFile(moneyOrderTemplates);
			}
			catch (Exception ex)
			{
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new BizMoneyOrderEngineException(BizMoneyOrderEngineException.MONEYORDER_DIAGONOSTIC_FAILED, ex);
			}
		}

        private string GetCheckPrintFile(List<string> checkPrintsfile)
        {
            string checkPrintfile = string.Empty;

            string files = string.Empty;

            foreach (string file in checkPrintsfile)
            {
                files += file + ",";
                checkPrintfile = GetCheckPrintTemplate(file);
                if (!string.IsNullOrWhiteSpace(checkPrintfile))
                    break;
            }

            if (string.IsNullOrWhiteSpace(checkPrintfile))
				throw new BizMoneyOrderEngineException(BizMoneyOrderEngineException.MONEYORDER_CHECK_PRINT_FAILED);

            return checkPrintfile;
        }

        private string GetCheckPrintTemplate(string filename)
        {
            Console.WriteLine("trying " + filename);
            string response = string.Empty;
            string requestUri = string.Format("{0}gridfs?dbname={1}&bucketname={2}&filename={3}", BaseUrl, Database, ReceiptBucket, filename);
			
			response = PrintTemplateRepo.GetPrintTemplate(requestUri);

            return response;
        }
    }
}
