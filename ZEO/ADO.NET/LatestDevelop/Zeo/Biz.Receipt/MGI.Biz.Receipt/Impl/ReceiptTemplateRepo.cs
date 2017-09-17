using MGI.Biz.FundsEngine.Data;
using MGI.Common.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using PTNRContract = MGI.Core.Partner.Contract;
using PTNRData = MGI.Core.Partner.Data;
using MGI.Biz.Receipt.Contract;
using MGI.Biz.Receipt.Data;

namespace MGI.Biz.Receipt.Impl
{
	public class ReceiptTemplateRepo
	{
		public string BaseUrl { get; set; }
		public string Database { get; set; }
		public string BucketName { get; set; }
		public NLoggerCommon NLogger = new NLoggerCommon();

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

			return GetReceiptFile(receiptfiles);
		}

		public string GetReceiptTemplate(string partner, PTNRData.Transactions.TransactionType product, PTNRContract.ProviderIds processor, string state)
		{
			string receiptFileComplete = string.Format("{0}.{1}.{2}.{3}.Receipt.docx", partner, product.ToString(), processor.ToString(), state);
			string receiptFileWithProcessor = string.Format("{0}.{1}.{2}.Receipt.docx", partner, product.ToString(), processor.ToString());
			string receiptFileBase = string.Format("{0}.{1}.Receipt.docx", partner, product.ToString());

			List<string> receiptfiles = new List<string>() { receiptFileComplete, receiptFileWithProcessor, receiptFileBase };

			return GetReceiptFile(receiptfiles);
		}

		public string GetShoppingCartSummaryReceiptTemplate(string partner, string gpr, string state)
		{
			string scart = "ShoppingCartSummary";
			string receiptFileComplete = string.Format("{0}.{1}.{2}.{3}.Receipt.docx", partner, scart, gpr, state);
			string receiptFileWithProcessor = string.Format("{0}.{1}.{2}.Receipt.docx", partner, scart, gpr);
			string receiptFileBase = string.Format("{0}.{1}.Receipt.docx", partner, scart);

			List<string> receiptfiles = new List<string>() { receiptFileComplete, receiptFileWithProcessor, receiptFileBase };

			return GetReceiptFile(receiptfiles);
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

			return GetReceiptFile(receiptfiles);
		}

		public string GetCouponCodeReceiptTemplate(string partner = "")
		{
			string type = "CouponCode";
			string receiptFileComplete = string.Format("{0}.{1}.Receipt.docx", partner, type);
			string receiptFileBase = string.Format("{0}.Receipt.docx", type);
			List<string> receiptfiles = new List<string>() { receiptFileComplete, receiptFileBase };
			return GetReceiptFile(receiptfiles);
		}

		private string GetReceiptFile(List<string> receiptsfile)
		{
			string receiptfile = string.Empty;

			string files = string.Empty;

			foreach (string file in receiptsfile)
			{
				files += file + ",";
				receiptfile = GetReceiptTemplateContent(file);
				if (!string.IsNullOrWhiteSpace(receiptfile))
					break;
			}

			if (string.IsNullOrWhiteSpace(receiptfile))
				throw new BizReceiptException(BizReceiptException.RECEIPT_TEMPLATE_NOT_FOUND);

			return receiptfile;
		}

		public string GetReceiptTemplates(List<string> receiptNames)
		{
			string receiptContent = string.Empty;
			foreach (string receipName in receiptNames)
			{
				receiptContent = GetReceiptTemplateContent(receipName);
				if (!string.IsNullOrWhiteSpace(receiptContent))
					return receiptContent;
			}
			return string.Empty;
		}

		private string GetReceiptTemplateContent(string filename)
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
				NLogger.Error(string.Format("Error in get Receipt Template: {0} \n Stack Trace: {1} ", ex.Message, !string.IsNullOrWhiteSpace(ex.StackTrace) ? ex.StackTrace : "No stack trace available"));
			}

			return response;
		}

		public string GetDeclinedReceiptTemplate(string partner, PTNRData.Transactions.TransactionType product, PTNRContract.ProviderIds processor, string state)
		{
			string receiptFileWithProcessor = string.Format("{0}.{1}.{2}.Declined.Receipt.docx", partner, product, processor);

			List<string> receiptfiles = new List<string>() { receiptFileWithProcessor };

			return GetReceiptFile(receiptfiles);
		}
	}
}
