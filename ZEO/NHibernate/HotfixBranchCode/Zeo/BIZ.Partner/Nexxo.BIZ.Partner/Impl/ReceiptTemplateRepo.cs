using MGI.Biz.FundsEngine.Data;
using MGI.Biz.Partner.Contract;
using MGI.Common.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using PTNRContract = MGI.Core.Partner.Contract;
using PTNRData = MGI.Core.Partner.Data;

namespace MGI.Biz.Partner.Impl
{
	public class ReceiptTemplateRepo
	{
		public string BaseUrl { get; set; }
		public string Database { get; set; }
		public string BucketName { get; set; }
        public NLoggerCommon NLogger { get; set; }

		public ReceiptTemplateRepo(string baseUrl, string dbName, string bucketName)
		{
			this.BaseUrl = baseUrl;
			this.Database = dbName;
			this.BucketName = bucketName;
		}

		public string GetFundsReceiptTemplate(string partner, FundType fundsTrxType, PTNRContract.ProviderIds processor, string state)
		{
			string fundtype = Enum.Parse(typeof(FundType), fundsTrxType.ToString()).ToString();

			string receiptFileNameWithState = string.Format("{0}.Funds.{1}.{2}.{3}.Receipt.docx", partner, fundtype, processor.ToString(), state);
			string receiptFileNameWithOutState = string.Format("{0}.Funds.{1}.{2}.Receipt.docx", partner, fundtype, processor.ToString());
			string receiptFileNameBase = string.Format("{0}.Funds.{1}.Receipt.docx", partner, fundtype);

			List<string> receiptfiles = new List<string>() { receiptFileNameWithState, receiptFileNameWithOutState, receiptFileNameBase };

			return getReceiptFile(receiptfiles);
		}

		public string GetReceiptTemplate(string partner, PTNRData.Transactions.TransactionType product, string productSubType,
												PTNRContract.ProviderIds provider, string state, string providerAttribute)
		{
			string receiptFileComplete = string.Format("{0}.{1}.{2}.{3}.{4}.{5}.Receipt.docx", partner, product, productSubType, provider, state, providerAttribute);
			string receiptFileWithAttribute = string.Format("{0}.{1}.{2}.{3}.{4}.Receipt.docx", partner, product, productSubType, provider, providerAttribute);
			string receiptFileWithState = string.Format("{0}.{1}.{2}.{3}.{4}.Receipt.docx", partner, product, productSubType, provider, state);
			string receiptFileWithSubType = string.Format("{0}.{1}.{2}.{3}.Receipt.docx", partner, product, productSubType, provider);
			string receiptFileBase = string.Format("{0}.{1}.{2}.Receipt.docx", partner, product, provider);

			List<string> receiptfiles = new List<string>()
            {
                receiptFileComplete,
                receiptFileWithState,
                receiptFileWithAttribute,
                receiptFileWithSubType,
                receiptFileBase
            };

			return getReceiptFile(receiptfiles);
		}

		public string GetShoppingCartSummaryReceiptTemplate(string partner, string gpr, string state)
		{
			string scart = "ShoppingCartSummary";
			string receiptFileComplete = string.Format("{0}.{1}.{2}.{3}.Receipt.docx", partner, scart, gpr, state);
			string receiptFileWithProcessor = string.Format("{0}.{1}.{2}.Receipt.docx", partner, scart, gpr);
			string receiptFileBase = string.Format("{0}.{1}.Receipt.docx", partner, scart);

			List<string> receiptfiles = new List<string>() { receiptFileComplete, receiptFileWithProcessor, receiptFileBase };

			return getReceiptFile(receiptfiles);
		}

		public string GetCashDrawerReportTemplate(string partner = "")
		{
			string sreport = "CashDrawer";
			string receiptFileComplete = string.Format("{0}.{1}.Report.docx", partner, sreport);
			string receiptFileBase = string.Format("{0}.Report.docx", sreport);

			List<string> receiptfiles = new List<string>() { receiptFileComplete, receiptFileBase };

			return getReceiptFile(receiptfiles);
		}

		public string GetDoddfrankReportTemplate(string partner = "", bool isFixOnSend = false)
		{
			string sreport = "DoddFrank";
			string receiptFileComplete = "";

			if (isFixOnSend)
				receiptFileComplete = string.Format("{0}.{1}.Fxd.Receipt.docx", partner, sreport);
			else
				receiptFileComplete = string.Format("{0}.{1}.Estd.Receipt.docx", partner, sreport);

			string receiptFileBase = string.Format("{0}.Receipt.docx", sreport);


			string preClosureReport = string.Format("{0}.MoneyTransfer.Disclosure.Receipt.docx", partner);

			List<string> receiptfiles = new List<string>() 
			{ 
				receiptFileComplete, 
				receiptFileBase, 
				preClosureReport 
			};

			return getReceiptFile(receiptfiles);
		}

		private string getReceiptFile(List<string> receiptsfile)
		{
			string receiptfile = string.Empty;

			string files = string.Empty;

			foreach (string file in receiptsfile)
			{
				files += file + ",";
				receiptfile = getReceiptTemplate(file);
				if (!string.IsNullOrWhiteSpace(receiptfile))
					break;
			}

			if (string.IsNullOrWhiteSpace(receiptfile))
				throw new BizReceiptException(BizReceiptException.RECEIPT_TEMPLATE_NOT_FOUND, string.Format("Receipt file not found in repo. Receipt files searched are {0}", files.TrimEnd(',')));

			return receiptfile;
		}


		private string getReceiptTemplate(string filename)
		{
			Console.WriteLine("trying " + filename);
			string response = string.Empty;

			string requestUri = string.Format("{0}gridfs?dbname={1}&bucketname={2}&filename={3}", BaseUrl, Database, BucketName, filename);
			try
			{
				response = NexxoUtil.ExecuteRESTFulServiceAsBase64(requestUri);
			}
			catch (Exception ex)
			{
				NLogger.Error("Error in get Receipt Template. " + ex.Message);
			}

			return response;
		}

	}
}
