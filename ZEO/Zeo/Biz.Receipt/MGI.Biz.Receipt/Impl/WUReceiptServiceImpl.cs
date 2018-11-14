using AutoMapper;
using MGI.Common.Util;
using MGI.Core.Partner.Contract;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using CXNMoneyTransferContract = MGI.Cxn.MoneyTransfer.Contract;
using CXNMoneyTransferData = MGI.Cxn.MoneyTransfer.Data;
using CXNBillPayData = MGI.Cxn.BillPay.Data;
using PTNRData = MGI.Core.Partner.Data;
using CXECustomerData = MGI.Core.CXE.Data;
using System.Text.RegularExpressions;
using MGI.Common.TransactionalLogging.Data;
using MGI.Biz.Receipt.Data;

namespace MGI.Biz.Receipt.Impl
{
	public class WUReceiptServiceImpl : BaseReceiptServiceImpl
	{

		#region IReceiptService Members

		public override List<Data.Receipt> GetBillPayReceipt(long agentSessionId, long customerSessionId, long transactionId, bool isReprint, MGIContext mgiContext)
		{
			try
			{

				#region AL-1014 Transactional Log User Story
				List<string> details = new List<string>();
				details.Add("Transaction Id:" + Convert.ToString(transactionId));
				details.Add("Is Reprint:" + Convert.ToString(isReprint));
				MongoDBLogger.ListInfo<string>(customerSessionId, details, "GetBillPayReceipt", AlloyLayerName.BIZ,
					ModuleName.BillPayment, "Begin GetBillPayReceipt - MGI.Biz.Receipt.Impl.WUReceiptServiceImpl",
					mgiContext);
				#endregion
				PTNRData.Transactions.BillPay billPay = _ptnrBillPaySvc.Lookup(transactionId);

				ProviderIds providerId = (ProviderIds)billPay.Account.ProviderId;

				string channelPartnerName = GetChannelPartnerName(billPay.CustomerSession.Customer.ChannelPartnerId);

				GetUpdatedContext(billPay.CustomerSession, ref mgiContext);

				CXNBillPayData.BillPayTransaction cxntran = _GetBillPayProcessor(channelPartnerName, providerId).
					GetTransaction(billPay.CXNId);
				//AL-491 For first print updating the cardPoints in database else getting from database cardpoints and printing it on receipts
				if (!isReprint && !string.IsNullOrEmpty(cxntran.Account.CardNumber))
				{
					CXNBillPayData.CardInfo cxnCardInfo = _GetBillPayProcessor(channelPartnerName, providerId)
						.GetCardInfo(cxntran.Account.CardNumber, mgiContext);

					if (cxnCardInfo != null && !string.IsNullOrEmpty(cxnCardInfo.TotalPointsEarned))
					{
						cxntran.MetaData.AddOrUpdate("WuCardTotalPointsEarned", cxnCardInfo.TotalPointsEarned);
						//Updating the cardpoints in billpaytransaction
						_GetBillPayProcessor(channelPartnerName, providerId).UpdateGoldCardPoints(billPay.CXNId, cxnCardInfo.TotalPointsEarned, mgiContext);
					}
				}
				List<Data.Receipt> receipts = GetBillpayReceiptTemplate(channelPartnerName,
					PTNRData.Transactions.TransactionType.BillPay, providerId, isReprint);

				string receiptContentstags = GetBillPayTrxtags(billPay, cxntran, billPay.CustomerSession);

				string partnerContentstags = GetPartnerTags(agentSessionId, billPay, billPay.CustomerSession, mgiContext);

				receiptContentstags = receiptContentstags + "|" + partnerContentstags;
				foreach (Data.Receipt receipt in receipts)
				{
					if (!string.IsNullOrEmpty(receipt.PrintData))
						receipt.PrintData = receiptContentstags + "|{data}|" + receipt.PrintData;
				}

				#region AL-1014 Transactional Log User Story
				MongoDBLogger.Info<string>(customerSessionId, string.Empty, "GetBillPayReceipt", AlloyLayerName.BIZ,
					ModuleName.BillPayment, "End GetBillPayReceipt - MGI.Biz.Receipt.Impl.WUReceiptServiceImpl",
					mgiContext);
				#endregion
				return receipts;
			}
			catch (Exception ex)
			{
				MongoDBLogger.Error<string>(Convert.ToString(customerSessionId), "GetBillPayReceipt", AlloyLayerName.BIZ, ModuleName.Receipt,
									  "GetBillPayReceipt-MGI.Biz.Receipt.Impl.WUReceiptServiceImpl", ex.Message, ex.StackTrace);

				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new BizReceiptException(BizReceiptException.BILLPAY_RECEIPT_TEMPLATE_RETRIVEL_FAILED, ex);
			}
		}

		public override List<Data.Receipt> GetMoneyTransferReceipt(long agentSessionId, long customerSessionId, long transactionId, bool isReprint, MGIContext mgiContext)
		{
			try
			{
				#region AL-3370 Transactional Log User Story
				List<string> details = new List<string>();
				details.Add("Transaction Id:" + Convert.ToString(transactionId));
				details.Add("Is Reprint:" + Convert.ToString(isReprint));
				MongoDBLogger.ListInfo<string>(customerSessionId, details, "GetMoneyTransferReceipt", AlloyLayerName.BIZ,
					ModuleName.MoneyTransfer, "Begin GetMoneyTransferReceipt - MGI.Biz.Receipt.Impl.WUReceiptServiceImpl",
					mgiContext);
				#endregion
				PTNRData.Transactions.MoneyTransfer xfer = _ptnrXferSvc.Lookup(transactionId);

				string channelPartnerName = GetChannelPartnerName(xfer.CustomerSession.Customer.ChannelPartnerId);

				GetUpdatedContext(xfer.CustomerSession, ref mgiContext);

				CXNMoneyTransferData.TransactionRequest request = new CXNMoneyTransferData.TransactionRequest()
				{
					TransactionId = xfer.CXNId
				};

				MGI.Cxn.MoneyTransfer.Data.Transaction cxntran = _GetMoneyTransferProcessor(channelPartnerName).GetTransaction(request, mgiContext);

				//AL-491 For first print updating the cardPoints in database else getting from database cardpoints and printing it on receipts
				if (!isReprint && !string.IsNullOrEmpty(cxntran.Account.LoyaltyCardNumber))
				{
					CXNMoneyTransferData.CardInfo cxnCardInfo = _GetMoneyTransferProcessor(channelPartnerName).GetCardInfo(cxntran.Account.LoyaltyCardNumber, mgiContext);

					if (cxnCardInfo != null && !string.IsNullOrEmpty(cxnCardInfo.TotalPointsEarned))
					{
						cxntran.MetaData.AddOrUpdate("WuCardTotalPointsEarned", cxnCardInfo.TotalPointsEarned);
						//Updating the cardpoints in wutransaction
						_GetMoneyTransferProcessor(channelPartnerName).UpdateGoldCardPoints(xfer.CXNId, cxnCardInfo.TotalPointsEarned, mgiContext);
					}
				}


				string productSubType = string.IsNullOrEmpty(cxntran.TransactionSubType) ?
							((CXNMoneyTransferData.MoneyTransferType)xfer.TransferType).ToString()
							: CXNMoneyTransferData.TransactionSubType.Modify.ToString();

				string providerAttribute = string.Empty;

				if (!cxntran.IsDomesticTransfer)
				{
					if (Convert.ToBoolean(NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "IsFixOnSend").ToNullSafeString()))
						providerAttribute = CXNMoneyTransferData.SendType.Fxd.ToString();
					else
						providerAttribute = CXNMoneyTransferData.SendType.Estd.ToString();
				}

				List<Data.Receipt> receipts = GetMoneyTransferReceiptTemplate(channelPartnerName, PTNRData.Transactions.TransactionType.MoneyTransfer,
																 productSubType, (ProviderIds)cxntran.ProviderId, providerAttribute, isReprint);


				string receiptContentTagsForMT = string.Empty;

				if (xfer.TransferType == (int)CXNMoneyTransferData.MoneyTransferType.Send)
				{
					receiptContentTagsForMT = GetMoneyTransferTrxTags(xfer, cxntran, xfer.CustomerSession, channelPartnerName);
				}
				else
				{
					receiptContentTagsForMT = GetReceiveMoneyTrxTags(xfer, cxntran, xfer.CustomerSession, channelPartnerName);
				}

				string partnerContentstags = GetPartnerTags(agentSessionId, xfer, xfer.CustomerSession, mgiContext);

				receiptContentTagsForMT = receiptContentTagsForMT + "|" + partnerContentstags;
				foreach (Data.Receipt receipt in receipts)
				{
					if (!string.IsNullOrEmpty(receipt.PrintData))
						receipt.PrintData = receiptContentTagsForMT + "|{data}|" + receipt.PrintData;
				}
				#region AL-3370 Transactional Log User Story
				MongoDBLogger.Info<string>(customerSessionId, string.Empty, "GetMoneyTransferReceipt", AlloyLayerName.BIZ,
					ModuleName.MoneyTransfer, "Begin GetMoneyTransferReceipt - MGI.Biz.Receipt.Impl.WUReceiptServiceImpl",
					mgiContext);
				#endregion
				return receipts;
			}
			catch (Exception ex)
			{
				MongoDBLogger.Error<string>(Convert.ToString(customerSessionId), "GetMoneyTransferReceipt", AlloyLayerName.BIZ, ModuleName.Receipt,
									  "GetMoneyTransferReceipt-MGI.Biz.Receipt.Impl.WUReceiptServiceImpl", ex.Message, ex.StackTrace);

				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new BizReceiptException(BizReceiptException.MT_RECEIPT_TEMPLATE_RETRIVEL_FAILED, ex);
			}
		}

		public override List<Data.Receipt> GetDoddFrankReceipt(long agentSessionId, long customerSessionId, long transactionId, MGIContext mgiContext)
		{
			try
			{
				#region AL-3370 Transactional Log User Story
				MongoDBLogger.Info<string>(customerSessionId, Convert.ToString(transactionId), "GetDoddFrankReceipt", AlloyLayerName.BIZ,
					ModuleName.MoneyTransfer, "Begin GetDoddFrankReceipt - MGI.Biz.Receipt.Impl.WUReceiptServiceImpl",
					mgiContext);
				#endregion
				PTNRData.Transactions.MoneyTransfer xfer = _ptnrXferSvc.Lookup(transactionId);

				string channelPartnerName = GetChannelPartnerName(xfer.CustomerSession.Customer.ChannelPartnerId);

				CXNMoneyTransferData.TransactionRequest request = new CXNMoneyTransferData.TransactionRequest()
				{
					TransactionId = xfer.CXNId
				};

				MGI.Cxn.MoneyTransfer.Data.Transaction cxntran = _GetMoneyTransferProcessor(channelPartnerName).GetTransaction(request, mgiContext);

				bool isFixOnSend = Convert.ToBoolean(NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "IsFixOnSend").ToNullSafeString());

				string receiptContents = _receiptRepo.GetDoddfrankReportTemplate(channelPartnerName, isFixOnSend);

				string transactionTags = GetMoneyTransferTrxTags(xfer, cxntran, xfer.CustomerSession, channelPartnerName);

				string partnerContentstags = GetPartnerTags(agentSessionId, xfer, xfer.CustomerSession, mgiContext);

				transactionTags = transactionTags + "|" + partnerContentstags;
				#region AL-3370 Transactional Log User Story
				MongoDBLogger.Info<string>(customerSessionId, string.Empty, "GetDoddFrankReceipt", AlloyLayerName.BIZ,
					ModuleName.MoneyTransfer, "End GetDoddFrankReceipt - MGI.Biz.Receipt.Impl.WUReceiptServiceImpl",
					mgiContext);
				#endregion
				return GetReceipt(receiptContents, transactionTags, "DoddFrank Receipt");
			}
			catch (Exception ex)
			{
				MongoDBLogger.Error<string>(Convert.ToString(customerSessionId), "GetDoddFrankReceipt", AlloyLayerName.BIZ, ModuleName.Receipt,
									  "GetDoddFrankReceipt-MGI.Biz.Receipt.Impl.WUReceiptServiceImpl", ex.Message, ex.StackTrace);

				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new BizReceiptException(BizReceiptException.DODFRANK_RECEIPT_RETRIVEL_FAILED, ex);
			}

		}

		#endregion

		#region Private Methods

		private List<Data.Receipt> GetMoneyTransferReceiptTemplate(string partner, PTNRData.Transactions.TransactionType product, string productSubType,
			ProviderIds provider, string providerAttribute, bool isReprint)
		{
			PTNRData.ProductProcessor productProcessor = null;
			//Below this line to get the Values from ReprintCopies and ReceiptReprintCopies from Core Partner database based on to retrive the Product and Process level.
			MGI.Core.Partner.Data.ChannelPartner channelPartner = _channelPartnerSvc.ChannelPartnerConfig(partner);

			if (productSubType == "Send" || productSubType == "Modify")
			{
				productProcessor = channelPartner.Providers.Where(c => c.ProductProcessor.Code == (int)provider && c.ProductProcessor.Product.Name == Product.MoneyTransfer.ToString()).FirstOrDefault().ProductProcessor;
			}
			else
			{
				productProcessor = channelPartner.Providers.Where(c => c.ProductProcessor.Code == (int)provider && c.ProductProcessor.Product.Name == Product.ReceiveMoney.ToString()).FirstOrDefault().ProductProcessor;
			}


			List<string> receiptfileNames = new List<string>();

			if (!string.IsNullOrEmpty(providerAttribute))
				receiptfileNames.Add(string.Format("{0}.{1}.{2}.{3}.{4}.Receipt.docx", partner, product,
														   productSubType, provider, providerAttribute));
			receiptfileNames.Add(string.Format("{0}.{1}.{2}.{3}.Receipt.docx",
									 partner, product, productSubType, provider));
			receiptfileNames.Add(string.Format("{0}.{1}.{2}.Receipt.docx",
									 partner, product, provider));
			return new List<Data.Receipt>
                       {
                           new Data.Receipt()
                               {
                                   PrintData = _receiptRepo.GetReceiptTemplates(receiptfileNames),
                                   Name = "",
                                   NumberOfCopies = isReprint ? productProcessor.ReceiptReprintCopies : productProcessor.ReceiptCopies
                               }
                       };
		}

		private List<Data.Receipt> GetBillpayReceiptTemplate(string partner, PTNRData.Transactions.TransactionType product, ProviderIds provider, bool isReprint)
		{
			MGI.Core.Partner.Data.ChannelPartner channelPartner = _channelPartnerSvc.ChannelPartnerConfig(partner);

			var productProcessor = channelPartner.Providers.Where(c => c.ProductProcessor.Code == (int)provider && c.ProductProcessor.Product.Name == Product.BillPayment.ToString()).FirstOrDefault().ProductProcessor;

			string receiptfileName = string.Format("{0}.{1}.{2}.Receipt.docx", partner, product, provider);
			return new List<Data.Receipt>
                       {
                           new Data.Receipt()
                               {
                                   PrintData = _receiptRepo.GetReceiptTemplates(new List<string> {receiptfileName}),
                                   Name = "",
                                   NumberOfCopies = isReprint ? productProcessor.ReceiptReprintCopies : productProcessor.ReceiptCopies

                               }
                       };
		}

		protected override string GetPayOutCountry(CXNMoneyTransferContract.IMoneyTransfer moneyTransferProcessor, MGI.Cxn.MoneyTransfer.Data.Transaction cxntran)
		{
			string language = "es";
			string translatedName = string.Empty;
			string payOutCountry = string.Empty;
			string payOutCountryName = string.Empty;
			var receiveCountry = moneyTransferProcessor.GetCountries().FirstOrDefault(c => c.Code.ToLower() == cxntran.DestinationCountryCode.ToLower());
			if (receiveCountry != null)
			{
				payOutCountryName = receiveCountry.Name;
				translatedName = moneyTransferProcessor.GetCountryTransalation(cxntran.DestinationCountryCode, language);
				payOutCountry = string.Format("{0} / {1}", payOutCountryName, translatedName);
			}

			return payOutCountry;
		}

		protected string ReceiverCountry(CXNMoneyTransferContract.IMoneyTransfer moneyTransferProcessor, MGI.Cxn.MoneyTransfer.Data.Transaction cxntran)
		{
			string receiverCountry = string.Empty;
			var receiveCountry = moneyTransferProcessor.GetCountries().FirstOrDefault(c => c.Code.ToLower() == cxntran.DestinationCountryCode.ToLower());
			if (receiveCountry != null)
			{
				receiverCountry = receiveCountry.Name;
			}
			return receiverCountry;
		}

		private string GetBillPayTrxtags(PTNRData.Transactions.BillPay billpay, CXNBillPayData.BillPayTransaction cxntran,
			PTNRData.CustomerSession customerSession)
		{

			StringBuilder billPayReceiptTrxTags = new StringBuilder();

			CXECustomerData.Customer customer = CxeCustomerSvc.Lookup(customerSession.Customer.CXEId);

			CultureInfo cultureinfo = new CultureInfo("es-ES");
			string timezone = customerSession.AgentSession.Terminal.Location.TimezoneID;

			DateTime trxDate = billpay.DTTerminalLastModified == null ? billpay.DTTerminalCreate : billpay.DTTerminalLastModified.Value; //AL-7079
			if (trxDate == null)
			{
				trxDate = MGI.TimeStamp.Clock.DateTimeWithTimeZone(timezone);
			}
			string mobileNumber = string.Empty;

			if (customer.Phone1Type == "Cell")
				mobileNumber = customer.Phone1;
			else if (!string.IsNullOrEmpty(customer.Phone2Type) && customer.Phone2Type.Equals("Cell"))
				mobileNumber = customer.Phone2;

			string timezoneFmt = TimeZones.Where(y => y.Key == timezone).FirstOrDefault().Value;
			billPayReceiptTrxTags.Append("|{TxrDate}|" + (trxDate.ToString("MMMM dd yyyy").ToUpper() + string.Format(" / {0}", cultureinfo.TextInfo.ToTitleCase(trxDate.ToString("MMMM dd yyyy", cultureinfo).ToUpper()))));
			billPayReceiptTrxTags.Append("|{TxrTime}|" + string.Format("{0} {1}", trxDate.ToString("hh:mm tt"), timezoneFmt));
			billPayReceiptTrxTags.Append("|{SenderName}|" + string.Format("{0} {1}", string.IsNullOrEmpty(customer.FirstName) ? "" : customer.FirstName.ToUpper(), string.IsNullOrEmpty(customer.LastName) ? "" : customer.LastName.ToUpper()));
			billPayReceiptTrxTags.Append("|{SenderAddress}|" + string.Format("{0} {1}", string.IsNullOrEmpty(customer.Address1) ? "" : customer.Address1.ToUpper(), string.IsNullOrEmpty(customer.Address2) ? "" : customer.Address2.ToUpper()));
			billPayReceiptTrxTags.Append("|{SenderCity}|" + (string.IsNullOrEmpty(customer.City) ? "" : string.Format("{0},", customer.City.ToUpper())));
			billPayReceiptTrxTags.Append("|{SenderState}|" + (string.IsNullOrEmpty(customer.State) ? "" : customer.State.ToUpper()));
			billPayReceiptTrxTags.Append("|{SenderZip}|" + customer.ZipCode);
			billPayReceiptTrxTags.Append("|{SenderPhoneNumber}|" + (string.IsNullOrEmpty(customer.Phone1) ? removeLine : customer.Phone1));
			billPayReceiptTrxTags.Append("|{SenderMobileNumber}|" + (string.IsNullOrEmpty(mobileNumber) ? removeLine : mobileNumber));
			billPayReceiptTrxTags.Append("|{ReceiverName}|" + (string.IsNullOrEmpty(cxntran.BillerName) ? "" : cxntran.BillerName.ToUpper()));
			billPayReceiptTrxTags.Append("|{Account}|" + (string.IsNullOrEmpty(cxntran.AccountNumber) ? "" : cxntran.AccountNumber.Substring(cxntran.AccountNumber.Length - 4, 4)));
			billPayReceiptTrxTags.Append("|{TransferAmmount}|" + billpay.Amount.ToString("#.00"));
			billPayReceiptTrxTags.Append("|{TransferFee}|" + cxntran.MetaData["UnDiscountedFee"].ToString());
			billPayReceiptTrxTags.Append("|{CurrencyCode}|" + "USD");
			billPayReceiptTrxTags.Append("|{AdditionalFee}|" + "0.00");
			billPayReceiptTrxTags.Append("|{PrmDiscount}|" + cxntran.MetaData["DiscountedFee"].ToString());
			billPayReceiptTrxTags.Append("|{NetAmount}|" + (billpay.Amount + billpay.Fee).ToString("#.00"));
			billPayReceiptTrxTags.Append("|{TransferTaxes}|" + "0.00");
			billPayReceiptTrxTags.Append("|{MTCN}|" + FormatMTCN(Convert.ToString(cxntran.MetaData["MTCN"])));
			billPayReceiptTrxTags.Append("|{GCNumber}|" + cxntran.Account.CardNumber);
			billPayReceiptTrxTags.Append("|{ServiceType}|" + Convert.ToString(cxntran.MetaData["DeliveryService"]).ToUpper());
			billPayReceiptTrxTags.Append("|{CardPoints}|" + Convert.ToString(cxntran.MetaData["WuCardTotalPointsEarned"]));
			billPayReceiptTrxTags.Append("|{ReceiptDate}|" + trxDate.ToString("MMMM dd yyyy") + " / " + string.Format("{0} {1}", trxDate.ToString("hh:mm tt"), timezoneFmt));
			if (billPayReceiptTrxTags.ToString().Substring(0, 1) == "|")
				billPayReceiptTrxTags.Remove(0, 1).ToString();

			string messageArea = NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "MessageArea").ToString();
			billPayReceiptTrxTags.Append("|{MessageArea}|" + messageArea);

			return billPayReceiptTrxTags.ToString();
		}

		private string GetMoneyTransferTrxTags(PTNRData.Transactions.Transaction trx, CXNMoneyTransferData.Transaction cxntran,
			PTNRData.CustomerSession customerSession, string channelPartnerName)
		{
			// replace other transaction-specific receipt tags // with processor router 
			CXNMoneyTransferContract.IMoneyTransfer moneyTransferProcessor = _GetMoneyTransferProcessor(channelPartnerName);

			string stateCode = customerSession.AgentSession.Terminal.Location.State;

			StringBuilder receiptBuilder = new StringBuilder();
			CultureInfo cultureinfo = new CultureInfo("es-ES");

			string timezone = customerSession.AgentSession.Terminal.Location.TimezoneID;
			
			DateTime trxDate = trx.DTTerminalLastModified == null ? trx.DTTerminalCreate : trx.DTTerminalLastModified.Value;
			if (trxDate == null)
			{
				trxDate = MGI.TimeStamp.Clock.DateTimeWithTimeZone(timezone);
			}

			string payOutCountry = GetPayOutCountry(moneyTransferProcessor, cxntran);
			string receiverCountry = ReceiverCountry(moneyTransferProcessor, cxntran);


			string timezoneFmt = TimeZones.Where(y => y.Key == timezone).FirstOrDefault().Value;

			receiptBuilder.Append("|{TxrDate}|" + (trxDate.ToString("MMMM dd yyyy").ToUpper() + string.Format(" / {0}", cultureinfo.TextInfo.ToTitleCase(trxDate.ToString("MMMM dd yyyy", cultureinfo).ToUpper()))));
			receiptBuilder.Append("|{TxrTime}|" + string.Format("{0} {1}", trxDate.ToString("hh:mm tt"), timezoneFmt));
			receiptBuilder.Append("|{Date}|" + (trxDate.ToString("M/dd/yy").ToUpper()));
			receiptBuilder.Append("|{Time}|" + string.Format("{0} {1}", trxDate.ToString("hh:mm tt"), timezoneFmt));
			if (!string.IsNullOrWhiteSpace(cxntran.Account.SecondLastName))
			{
				receiptBuilder.Append("|{SenderName}|" + string.Format("{0} {1} {2}", string.IsNullOrEmpty(cxntran.Account.FirstName) ? "" : cxntran.Account.FirstName.ToUpper(), string.IsNullOrEmpty(cxntran.Account.LastName) ? "" : cxntran.Account.LastName.ToUpper(), string.IsNullOrEmpty(cxntran.Account.SecondLastName) ? "" : cxntran.Account.SecondLastName.ToUpper()));
			}
			else
			{
				receiptBuilder.Append("|{SenderName}|" + string.Format("{0} {1} {2}", string.IsNullOrEmpty(cxntran.Account.FirstName) ? "" : cxntran.Account.FirstName.ToUpper(), string.IsNullOrEmpty(cxntran.Account.MiddleName) ? "" : cxntran.Account.MiddleName.ToUpper(), string.IsNullOrEmpty(cxntran.Account.LastName) ? "" : cxntran.Account.LastName.ToUpper()));
			}
			receiptBuilder.Append("|{SenderAddress}|" + (string.IsNullOrEmpty(cxntran.Account.Address) ? "" : cxntran.Account.Address.ToUpper()));
			receiptBuilder.Append("|{SenderCity}|" + (string.IsNullOrEmpty(cxntran.Account.City) ? "" : string.Format("{0},", cxntran.Account.City.ToUpper())));
			receiptBuilder.Append("|{SenderState}|" + (string.IsNullOrEmpty(cxntran.Account.State) ? "" : cxntran.Account.State.ToUpper()));
			receiptBuilder.Append("|{SenderZip}|" + cxntran.Account.PostalCode);
			receiptBuilder.Append("|{SenderPhoneNumber}|" + (string.IsNullOrEmpty(cxntran.Account.ContactPhone) ? removeLine : cxntran.Account.ContactPhone));
			receiptBuilder.Append("|{SenderMobileNumber}|" + (string.IsNullOrEmpty(cxntran.Account.MobilePhone) ? removeLine : cxntran.Account.MobilePhone));

			string receiverFirstName = cxntran.ReceiverFirstName;
			string receiverLastName = cxntran.ReceiverLastName;
			string receiverSecondLastName = cxntran.ReceiverSecondLastName;


			string receiverName = string.Format("{0} {1} {2}", string.IsNullOrEmpty(receiverFirstName) ? "" : receiverFirstName.ToUpper(), string.IsNullOrEmpty(receiverLastName) ? "" : receiverLastName.ToUpper(), string.IsNullOrEmpty(receiverSecondLastName) ? "" : receiverSecondLastName.ToUpper());

			receiptBuilder.Append("|{ReceiverName}|" + receiverName);
			receiptBuilder.Append("|{ReceiverAddress}|" + (string.IsNullOrEmpty(cxntran.Receiver.Address) ? "" : cxntran.Receiver.Address.ToUpper()));
			receiptBuilder.Append("|{ReceiverCity}|" + (string.IsNullOrEmpty(cxntran.Receiver.City) ? "" : string.Format("{0},", cxntran.Receiver.City.ToUpper())));
			receiptBuilder.Append("|{ReceiverState}|" + (string.IsNullOrEmpty(cxntran.Receiver.State_Province) ? "" : cxntran.Receiver.State_Province.ToUpper()));
			receiptBuilder.Append("|{ReceiverZip}|" + cxntran.Receiver.ZipCode);
			receiptBuilder.Append("|{ReceiverPhoneNumber}|" + (string.IsNullOrEmpty(cxntran.Receiver.PhoneNumber) ? removeLine : cxntran.Receiver.PhoneNumber));
			receiptBuilder.Append("|{ReceiverMobileNumber}|" + removeLine);
			receiptBuilder.Append("|{ReceiverCountry}|" + receiverCountry);

			receiptBuilder.Append("|{PayoutCountry}|" + payOutCountry);
			receiptBuilder.Append("|{PayoutState}|" + (cxntran.DestinationState == null ? "" : string.Format("{0},", cxntran.DestinationState.ToUpper())));
			receiptBuilder.Append("|{TransferAmount}|" + cxntran.TransactionAmount);
			receiptBuilder.Append("|{CurrencyCode}|" + cxntran.OriginatingCurrencyCode);

			//If PromoCode is Present the we are adding Promo discount to transfer fee for getting the net total. 
			decimal fee = cxntran.Fee + cxntran.PromotionDiscount;
			receiptBuilder.Append("|{TransferFee}|" + fee);
			decimal transferTax = NexxoUtil.GetDecimalDictionaryValueIfExists(cxntran.MetaData, "TransferTax");
			receiptBuilder.Append("|{TransferTaxes}|" + transferTax.ToString("0.00"));
			receiptBuilder.Append("|{ReceiveAgentName}|" + customerSession.AgentSession.Terminal.Location.LocationName);
			receiptBuilder.Append("|{AgentPhoneNumber}|" + customerSession.AgentSession.Terminal.Location.PhoneNumber);
			receiptBuilder.Append("|{AgentAddressLine}|" + customerSession.AgentSession.Terminal.Location.Address1);

			//On receiveFeeDisclosureText being true "asterisk" is added for Other Fees and Other Taxes
			bool receiveFeeDisclosure = Convert.ToBoolean(NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "ReceiveFeeDisclosureText").ToNullSafeString());
			//On validCurrencyIndicator being false Estimated is added for Exchange Rate,Transfer Amount,Other Fees,Other Taxes and Total to Recipient
			bool validCurrencyIndicator = Convert.ToBoolean(NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "ValidCurrencyIndicator").ToNullSafeString());
			receiptBuilder.Append("|{Asterisk}|" + ((receiveFeeDisclosure) ? "*" : string.Empty));
			receiptBuilder.Append("|{Estimated}|" + ((!validCurrencyIndicator) ? "Estimated " : string.Empty));

			decimal additionalCharges = NexxoUtil.GetDecimalDictionaryValueIfExists(cxntran.MetaData, "AdditionalCharges");
			decimal messageCharge = NexxoUtil.GetDecimalDictionaryValueIfExists(cxntran.MetaData, "MessageCharge");
			decimal paysideCharges = NexxoUtil.GetDecimalDictionaryValueIfExists(cxntran.MetaData, "PaySideCharges");
			string expectedPayoutCity = NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "ExpectedPayoutCity").ToNullSafeString();

			string accountNumberLastFour = NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "AccountNumberLastFour");
			string accountNickname = NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "AccountNickName");
			string payoutCurrency = NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "PayoutCurrency");
			string customerReceiveNumber = NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "CustomerReceiveNumber");
			string partnerConfirmationNumber = NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "PartnerConfirmationNumber");
			string disclosureText = NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "DisclosureTextPrimaryLanguage");
			// Need to identify the logic to get PIN number and account balance
			string pinNumber = string.Empty;
			string accountBalance = string.Empty;

			receiptBuilder.Append("|{Pin}|" + (string.IsNullOrEmpty(pinNumber) ? removeLine : (string.Format("Your PIN is {0}", pinNumber))));
			receiptBuilder.Append("|{AccountBalance}|" + (string.IsNullOrEmpty(accountBalance) ? removeLine : (string.Format("Your account balance is ${0}", accountBalance))));

			receiptBuilder.Append("|{AccountLastFour}|" + (string.IsNullOrEmpty(accountNumberLastFour) ? removeLine : accountNumberLastFour));
			receiptBuilder.Append("|{AccountNickName}|" + (string.IsNullOrEmpty(accountNickname) ? removeLine : accountNickname));
			receiptBuilder.Append("|{PayoutCurrency}|" + (string.IsNullOrEmpty(payoutCurrency) ? removeLine : (string.Format(" - {0}", cxntran.OriginatingCurrencyCode))));
			receiptBuilder.Append("|{CustomerReceiveNumber}|" + (string.IsNullOrEmpty(customerReceiveNumber) ? removeLine : customerReceiveNumber));
			receiptBuilder.Append("|{PartnerConfirmationNumber}|" + (string.IsNullOrEmpty(partnerConfirmationNumber) ? removeLine : partnerConfirmationNumber));
			receiptBuilder.Append("|{AdditionalFee}|" + additionalCharges);
			receiptBuilder.Append("|{PromoCode}|" + (string.IsNullOrEmpty(cxntran.PromotionsCode) ? removeLine : cxntran.PromotionsCode));
			receiptBuilder.Append("|{PrmDiscount}|" + (cxntran.PromotionDiscount <= 0 ? removeLine : cxntran.PromotionDiscount.ToString()));
			receiptBuilder.Append("|{OtherFee}|" + paysideCharges.ToString("0.00"));

			decimal otherFees = NexxoUtil.GetDecimalDictionaryValueIfExists(cxntran.MetaData, "OtherFees");
			receiptBuilder.Append("|{OtherFees}|" + otherFees.ToString("0.00"));

			decimal otherTaxes = NexxoUtil.GetDecimalDictionaryValueIfExists(cxntran.MetaData, "OtherTaxes");
			receiptBuilder.Append("|{OtherTaxes}|" + otherTaxes.ToString("0.00"));

			receiptBuilder.Append("|{ExchangeRate}|" + cxntran.ExchangeRate.ToString("0.0000"));
			receiptBuilder.Append("|{DstnTransferAmount}|" + cxntran.DestinationPrincipalAmount);
			receiptBuilder.Append("|{DstnCurrencyCode}|" + cxntran.DestinationCurrencyCode);
			receiptBuilder.Append("|{MTCN}|" + FormatMTCN(cxntran.ConfirmationNumber));
			receiptBuilder.Append("|{GCNumber}|" + cxntran.LoyaltyCardNumber);
			receiptBuilder.Append("|{CardPoints}|" + cxntran.LoyaltyCardPoints);
			receiptBuilder.Append("|{PayoutCity}|" + (string.IsNullOrWhiteSpace(expectedPayoutCity) ? "" : expectedPayoutCity.ToUpper()));
			receiptBuilder.Append("|{EstTransferAmount}|" + cxntran.DestinationPrincipalAmount);
			receiptBuilder.Append("|{EstOtherFee}|" + paysideCharges);
			//receiptBuilder.Append("|{EstTotalToReceiver}|" + (cxntran.DestinationPrincipalAmount - (paysideCharges == null ? 0 : paysideCharges)));

			receiptBuilder.Append("|{EstTotalToReceiver}|" + (cxntran.AmountToReceiver));

			string deliveryOption = NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "DeliveryOption").ToNullSafeString();
			string deliveryOptionDesc = NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "DeliveryOptionDesc").ToNullSafeString();
			string serviceType = string.IsNullOrEmpty(deliveryOptionDesc) ? cxntran.DeliveryServiceDesc : deliveryOptionDesc;
			string transalatedDeliveryServiceName = NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "TransalatedDeliveryServiceName").ToNullSafeString();
			if (!string.IsNullOrWhiteSpace(transalatedDeliveryServiceName))
			{
				serviceType = string.Format("{0} / {1}", serviceType, transalatedDeliveryServiceName);
			}
			else if (string.IsNullOrWhiteSpace(serviceType))
			{
				serviceType = deliveryOption;
			}
			receiptBuilder.Append("|{ServiceType}|" + serviceType);


			if (!string.IsNullOrWhiteSpace(cxntran.DTAvailableForPickup.ToNullSafeString()))
			{
				DateTime dtEstimatedDate = Convert.ToDateTime(cxntran.DTAvailableForPickup);
				receiptBuilder.Append("|{DateAvailableInReceiverCountry}|" + (dtEstimatedDate.ToString("MMMM dd yyyy").ToUpper() + string.Format(" / {0}", cultureinfo.TextInfo.ToTitleCase(dtEstimatedDate.ToString("MMMM dd yyyy", cultureinfo).ToUpper()))));
				receiptBuilder.Append("|{DateInReceiverCountry}|" + (dtEstimatedDate.ToString("M/dd/yy").ToUpper()));
			}
			else
			{
				receiptBuilder.Append("|{DateAvailableInReceiverCountry}|" + removeLine);
				receiptBuilder.Append("|{DateInReceiverCountry}|" + removeLine);
			}

			decimal netamt = 0;
			decimal otherCharges = NexxoUtil.GetDecimalDictionaryValueIfExists(cxntran.MetaData, "OtherCharges");
			if (cxntran.ExchangeRate != 0)
			{
				netamt = cxntran.TransactionAmount + fee + cxntran.TaxAmount + otherCharges + additionalCharges - cxntran.PromotionDiscount;
			}

			receiptBuilder.Append("|{NetAmount}|" + netamt);

			decimal total = (cxntran.TransactionAmount * cxntran.ExchangeRate) -
				  ((cxntran.Fee * cxntran.ExchangeRate) + (otherCharges * cxntran.ExchangeRate) + (cxntran.TaxAmount * cxntran.ExchangeRate) - (cxntran.PromotionDiscount * cxntran.ExchangeRate));

			//appendreceipttags.Append("|{TotalToReceiver}|" + total);
			receiptBuilder.Append("|{TotalToReceiver}|" + (cxntran.AmountToReceiver));

			receiptBuilder.Append("|{MTCN}|" + FormatMTCN(trx.ConfirmationNumber));

			string agencyName = NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "AgencyName").ToNullSafeString();
			string phoneNumber = NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "PhoneNumber").ToNullSafeString();
			string url = NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "Url").ToNullSafeString();
			string tollFreePhoneNumber = NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "TollFreePhoneNumber").ToNullSafeString();
			string freePhoneCallPIN = NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "FreePhoneCallPIN").ToNullSafeString();

			if (!string.IsNullOrWhiteSpace(agencyName))
			{
				agencyName = agencyName.Replace("&", "and");
				receiptBuilder.Append("|{StateRegulatoryAgency}|" + agencyName);
			}
			else
			{
				receiptBuilder.Append("|{StateRegulatoryAgency}|" + removeLine);
			}

			receiptBuilder.Append("|{SRAPhoneNumber}|" + (string.IsNullOrEmpty(phoneNumber) ? removeLine : phoneNumber));
			receiptBuilder.Append("|{SRAUrl}|" + (string.IsNullOrEmpty(url) ? removeLine : url));
			receiptBuilder.Append("|{TestQuestion}|" + (string.IsNullOrEmpty(cxntran.TestQuestion) ? removeLine : "YES / Si"));
			// MGI specific tags
			receiptBuilder.Append("|{TestAnswer}|" + (string.IsNullOrEmpty(cxntran.TestAnswer) ? removeLine : ""));
			receiptBuilder.Append("|{FREEPhoneCall}|" + (string.IsNullOrEmpty(tollFreePhoneNumber) ? removeLine : "FREE Phone Call"));
			receiptBuilder.Append("|{TollFreePhoneNumber}|" + (string.IsNullOrEmpty(tollFreePhoneNumber) ? removeLine : tollFreePhoneNumber));
			receiptBuilder.Append("|{FreePhoneCallPIN}|" + (string.IsNullOrEmpty(freePhoneCallPIN) ? removeLine : freePhoneCallPIN));
			receiptBuilder.Append("|{ReferenceNumber}|" + cxntran.ConfirmationNumber);

			if (!string.IsNullOrWhiteSpace(cxntran.PersonalMessage))
			{
				List<string> messages = cxntran.PersonalMessage.Split(40).ToList();

				if (messages.Count > 0)
				{
					string message1 = messages[0].Replace("&","and");
					receiptBuilder.Append("|{Message1}|" + (string.IsNullOrEmpty(messages[0]) ? removeLine : message1));
					if (messages.Count > 1)
					{
						message1 = messages[1].Replace("&", "and");
						receiptBuilder.Append("|{Message2}|" + (string.IsNullOrEmpty(messages[1]) ? removeLine : message1));
					}
					else
						receiptBuilder.Append("|{Message2}|" + string.Empty);
				}
			}
			else
			{
				receiptBuilder.Append("|{Message1}|" + removeLine);
				receiptBuilder.Append("|{Message2}|" + removeLine);
			}
			string messageArea = NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "MessageArea").ToString();
			receiptBuilder.Append("|{MessageArea}|" + messageArea);

			decimal paySideTax = NexxoUtil.GetDecimalDictionaryValueIfExists(cxntran.MetaData, "PaySideTax");

			receiptBuilder.Append("|{PaySideTaxes}|" + (paySideTax == 0 ? removeLine : paySideTax.ToString(("0.00"))));
			receiptBuilder.Append("|{EstPaySideTaxes}|" + (paySideTax == 0 ? removeLine : paySideTax.ToString(("0.00"))));

			receiptBuilder.Append("|{DisclosureText}|" + (string.IsNullOrEmpty(disclosureText) ? removeLine : disclosureText));
			receiptBuilder.Append("|{ReceiptDate}|" + trxDate.ToString("MMMM dd yyyy") + " / " + string.Format("{0} {1}", trxDate.ToString("hh:mm tt"), timezoneFmt));
			if (receiptBuilder.ToString().Substring(0, 1) == "|")
				receiptBuilder.Remove(0, 1).ToString();

			return receiptBuilder.ToString();
		}

		private string GetReceiveMoneyTrxTags(PTNRData.Transactions.Transaction ptnrTrx,
			 CXNMoneyTransferData.Transaction cxntran, PTNRData.CustomerSession customerSession, string channelPartnerName)
		{
			StringBuilder appendreceipttags = new StringBuilder();

			CultureInfo cultureinfo = new CultureInfo("es-ES");
			string timezone = customerSession.AgentSession.Terminal.Location.TimezoneID;

			DateTime trxDate = ptnrTrx.DTTerminalLastModified == null ? ptnrTrx.DTTerminalCreate : ptnrTrx.DTTerminalLastModified.Value;
			if (trxDate == null)
			{
				trxDate = MGI.TimeStamp.Clock.DateTimeWithTimeZone(timezone);
			}
			string timezoneFmt = TimeZones.Where(y => y.Key == timezone).FirstOrDefault().Value;
			appendreceipttags.Append("|{TxrDate}|" + (trxDate.ToString("MMMM dd yyyy").ToUpper() + string.Format(" / {0}", cultureinfo.TextInfo.ToTitleCase(trxDate.ToString("MMMM dd yyyy", cultureinfo).ToUpper()))));
			appendreceipttags.Append("|{TxrTime}|" + string.Format("{0} {1}", trxDate.ToString("hh:mm tt"), timezoneFmt));
			appendreceipttags.Append("|{Date}|" + (trxDate.ToString("M/dd/yy").ToUpper()));
			appendreceipttags.Append("|{Time}|" + string.Format("{0} {1}", trxDate.ToString("hh:mm tt"), timezoneFmt));
			appendreceipttags.Append("|{SenderName}|" + string.Format("{0}", string.IsNullOrEmpty(cxntran.SenderName) ? "" : cxntran.SenderName.ToUpper()));
			appendreceipttags.Append("|{ReceiveAgentName}|" + customerSession.AgentSession.Terminal.Location.LocationName);
			appendreceipttags.Append("|{AgentPhoneNumber}|" + customerSession.AgentSession.Terminal.Location.PhoneNumber);

			string ReceiverFirstName = cxntran.ReceiverFirstName;
			string ReceiverLastName = cxntran.ReceiverLastName;
			string ReceiverSecondLastName = cxntran.ReceiverSecondLastName;

			string receiverName = string.Format("{0} {1} {2}", string.IsNullOrEmpty(ReceiverFirstName) ? "" : ReceiverFirstName.ToUpper(), string.IsNullOrEmpty(ReceiverLastName) ? "" : ReceiverLastName.ToUpper(), string.IsNullOrEmpty(ReceiverSecondLastName) ? "" : ReceiverSecondLastName.ToUpper());
			appendreceipttags.Append("|{ReceiverName}|" + receiverName);
			appendreceipttags.Append("|{ReceiverAddress}|" + NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "ReceiverAddress").ToNullSafeString());

			if (cxntran.Receiver != null)
			{
				appendreceipttags.Append("|{ReceiverCity}|" + cxntran.Receiver.City);
				appendreceipttags.Append("|{ReceiverState}|" + cxntran.Receiver.State_Province);
				appendreceipttags.Append("|{ReceiverZip}|" + cxntran.Receiver.ZipCode);
				appendreceipttags.Append("|{ReceiverPhoneNumber}|" + cxntran.Receiver.PhoneNumber);
				appendreceipttags.Append("|{ReceiverMobileNumber}|" + removeLine);
			}

			appendreceipttags.Append("|{ReceiverDOB}|" + NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "ReceiverDOB"));
			appendreceipttags.Append("|{ReceiverOccupation}|" + NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "ReceiverOccupation").ToNullSafeString());
			appendreceipttags.Append("|{OriginatingCountry}|" + cxntran.OriginatingCountryCode);
			appendreceipttags.Append("|{ReceiveCurrency}|" + cxntran.DestinationCurrencyCode);
			appendreceipttags.Append("|{Currency}|" + cxntran.OriginatingCurrencyCode);
			appendreceipttags.Append("|{Taxes}|" + cxntran.TaxAmount);
			string amountToReceiver = cxntran.AmountToReceiver.ToNullSafeString();
			appendreceipttags.Append("|{TotalAmtPaid}|" + amountToReceiver);
			appendreceipttags.Append("|{Total}|" + amountToReceiver);
			appendreceipttags.Append("|{MTCN}|" + FormatMTCN(cxntran.ConfirmationNumber));
			appendreceipttags.Append("|{ReferenceNumber}|" + cxntran.ConfirmationNumber);
			string personalMessage = cxntran.PersonalMessage.Replace("&", "and");
			appendreceipttags.Append("|{PersonalMessage}|" + personalMessage);
			appendreceipttags.Append("|{ReceiverPhotoIdType}|" + NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "ReceiverPhotoIdType").ToNullSafeString());
			appendreceipttags.Append("|{ReceiverPhotoIdNumber}|" + NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "ReceiverPhotoIdNumber").ToNullSafeString());
			appendreceipttags.Append("|{ReceiverPhotoIdState}|" + NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "ReceiverPhotoIdState").ToNullSafeString());
			appendreceipttags.Append("|{ReceiverPhotoIdCountry}|" + NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "ReceiverPhotoIdCountry").ToNullSafeString());
			appendreceipttags.Append("|{ReceiverLegalIdType}|" + NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "ReceiverLegalIdType").ToNullSafeString());
			appendreceipttags.Append("|{ReceiverLegalIdNumber}|" + NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "ReceiverLegalIdNumber").ToNullSafeString());
			appendreceipttags.Append("|{TextTranslationPrimary}|" + NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "TextTranslationPrimary").ToNullSafeString());
			appendreceipttags.Append("|{TextTranslationSecondary}|" + NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "TextTranslationSecondary").ToNullSafeString());
			appendreceipttags.Append("|{ReceiptDate}|" + trxDate.ToString("MMMM dd yyyy") + " / " + string.Format("{0} {1}", trxDate.ToString("hh:mm tt"), timezoneFmt));
			appendreceipttags.Append("|{CurrencyCode}|" + "$");
			if (appendreceipttags.ToString().Substring(0, 1) == "|")
				appendreceipttags.Remove(0, 1).ToString();

			string messageArea = NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "MessageArea").ToString();
			appendreceipttags.Append("|{MessageArea}|" + messageArea);

			return appendreceipttags.ToString();
		}

		private void GetUpdatedContext(PTNRData.CustomerSession customerSession, ref MGIContext mgiContext)
		{
			//mgiContext.WUCounterId = customerSession.CustomerSessionCounter.CounterId;
			mgiContext.AgentId = Convert.ToInt32(customerSession.AgentSession.AgentId);
			mgiContext.AgentFirstName = customerSession.AgentSession.Agent.FirstName;
			mgiContext.AgentLastName = customerSession.AgentSession.Agent.LastName;
		}
		#endregion

	}
}
