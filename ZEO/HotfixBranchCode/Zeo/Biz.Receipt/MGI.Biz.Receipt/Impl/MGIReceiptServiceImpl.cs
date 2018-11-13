using System.Globalization;
using AutoMapper;
using MGI.Common.Util;
using MGI.Core.Partner.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreCXEContract = MGI.Core.CXE.Contract;
using CoreCXEData = MGI.Core.CXE.Data;
using CorePtnrData = MGI.Core.Partner.Data;
using CXNCheckContract = MGI.Cxn.Check.Contract;
using CXNFundsContract = MGI.Cxn.Fund.Contract;
using CXNCheckData = MGI.Cxn.Check.Data;
using CXNFundData = MGI.Cxn.Fund.Data;
using CXNMoneyTransferContract = MGI.Cxn.MoneyTransfer.Contract;
using CXNMoneyTransferData = MGI.Cxn.MoneyTransfer.Data;
using CXNBillPayContract = MGI.Cxn.BillPay.Contract;
using CXNBillPayData = MGI.Cxn.BillPay.Data;
using PTNRContract = MGI.Core.Partner.Contract;
using PTNRData = MGI.Core.Partner.Data;
using BIZReceiptData = MGI.Biz.Receipt.Data;
using BizPtrnData = MGI.Biz.Partner.Data;
using MGI.Common.TransactionalLogging.Data;

namespace MGI.Biz.Receipt.Impl
{
    public class MGIReceiptServiceImpl : BaseReceiptServiceImpl
    {

        #region IReceiptService Members

		public override List<Data.Receipt> GetBillPayReceipt(long agentSessionId, long customerSessionId, long transactionId, bool isReprint, MGIContext mgiContext)
		{
			#region AL-1014 Transactional Log User Story
			List<string> details = new List<string>();
			details.Add("Transaction Id:"+Convert.ToString(transactionId));
			details.Add("Is Reprint:"+Convert.ToString(isReprint));
			MongoDBLogger.ListInfo<string>(customerSessionId, details, "GetBillPayReceipt", AlloyLayerName.BIZ,
				ModuleName.BillPayment, "Begin GetBillPayReceipt - MGI.Biz.Receipt.Impl.MGIReceiptServiceImpl",
				mgiContext);
			#endregion
			PTNRData.Transactions.BillPay billPay = _ptnrBillPaySvc.Lookup(transactionId);

            ProviderIds providerId = (ProviderIds)billPay.Account.ProviderId;

            CXNBillPayData.BillPayTransaction cxntran = _GetBillPayProcessor(mgiContext.ChannelPartnerName, providerId).GetTransaction(billPay.CXNId);

            string senderState = "";

            if (cxntran.MetaData.ContainsKey("SenderState"))
                senderState = NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "SenderState").ToNullSafeString();

            List<Data.Receipt> receipts = GetBillpayReceiptTemplate(mgiContext.ChannelPartnerName,
                PTNRData.Transactions.TransactionType.BillPay, providerId, senderState);

			string receiptContentstags = GetPartnerTags(agentSessionId, billPay, billPay.CustomerSession, mgiContext);

            receiptContentstags += GetBillPayTrxTags(billPay, cxntran, billPay.CustomerSession);
            foreach (Data.Receipt receipt in receipts)
            {
                if (!string.IsNullOrEmpty(receipt.PrintData))
					receipt.PrintData = receiptContentstags + "|{data}|" + receipt.PrintData;
			}

			#region AL-1014 Transactional Log User Story
			MongoDBLogger.Info<string>(customerSessionId, string.Empty, "GetBillPayReceipt", AlloyLayerName.BIZ,
				ModuleName.BillPayment, "End GetBillPayReceipt - MGI.Biz.Receipt.Impl.MGIReceiptServiceImpl",
				mgiContext);
			#endregion
			return receipts;

        }

		public override List<Data.Receipt> GetMoneyTransferReceipt(long agentSessionId, long customerSessionId, long transactionId, bool isReprint, MGIContext mgiContext)
		{
			#region AL-3370 Transactional Log User Story
			List<string> details = new List<string>();
			details.Add("Transaction Id:" + Convert.ToString(transactionId));
			details.Add("Is Reprint:" + Convert.ToString(isReprint));
			MongoDBLogger.ListInfo<string>(customerSessionId, details, "GetMoneyTransferReceipt", AlloyLayerName.BIZ,
				ModuleName.BillPayment, "Begin GetMoneyTransferReceipt - MGI.Biz.Receipt.Impl.MGIReceiptServiceImpl",
				mgiContext);
			#endregion
			PTNRData.Transactions.MoneyTransfer xfer = _ptnrXferSvc.Lookup(transactionId);

            if (!string.IsNullOrEmpty(mgiContext.RegulatorInfoStateCode))
                mgiContext.RegulatorInfoStateCode = xfer.CustomerSession.AgentSession.Terminal.Location.State;

            CXNMoneyTransferData.TransactionRequest request = new CXNMoneyTransferData.TransactionRequest()
            {
                TransactionId = xfer.CXNId
            };

			MGI.Cxn.MoneyTransfer.Data.Transaction cxntran = _GetMoneyTransferProcessor(mgiContext.ChannelPartnerName).GetTransaction(request, mgiContext);

            string productSubType = string.IsNullOrEmpty(cxntran.TransactionSubType) ?
                ((CXNMoneyTransferData.MoneyTransferType)xfer.TransferType).ToString() : CXNMoneyTransferData.TransactionSubType.Modify.ToString();

            List<Data.Receipt> receipts = GetSendMoneyReceiptTemplate(mgiContext.ChannelPartnerName, PTNRData.Transactions.TransactionType.MoneyTransfer,
                                                    productSubType, (ProviderIds)cxntran.ProviderId, cxntran.SenderState);

			string receiptContentTagsForMT = GetPartnerTags(agentSessionId, xfer, xfer.CustomerSession, mgiContext);

            if (xfer.TransferType == (int)CXNMoneyTransferData.MoneyTransferType.Send)
                receiptContentTagsForMT += GetMoneyTransferTrxTags(xfer, cxntran, xfer.CustomerSession, mgiContext.ChannelPartnerName);
            else if (xfer.TransferType == (int)CXNMoneyTransferData.MoneyTransferType.Receive)
                receiptContentTagsForMT += GetReceiveMoneyTrxTags(xfer, cxntran, xfer.CustomerSession, mgiContext.ChannelPartnerName);
            else if (xfer.TransferType == (int)CXNMoneyTransferData.MoneyTransferType.Refund)
                receiptContentTagsForMT += GetRefundSendMoneyTrxTags(xfer, cxntran, xfer.CustomerSession, mgiContext.ChannelPartnerName);
            foreach (Data.Receipt receipt in receipts)
            {
                if (!string.IsNullOrEmpty(receipt.PrintData))
					receipt.PrintData = receiptContentTagsForMT + "|{data}|" + receipt.PrintData;
            }
			#region AL-3370 Transactional Log User Story
			MongoDBLogger.Info<string>(customerSessionId, string.Empty, "GetMoneyTransferReceipt", AlloyLayerName.BIZ,
				ModuleName.BillPayment, "End GetMoneyTransferReceipt - MGI.Biz.Receipt.Impl.MGIReceiptServiceImpl",
				mgiContext);
			#endregion
            return receipts;
        }

		public override List<Data.Receipt> GetDoddFrankReceipt(long agentSessionId, long customerSessionId, long transactionId, MGIContext mgiContext)
        {
			#region AL-3370 Transactional Log User Story
			MongoDBLogger.Info<string>(customerSessionId, Convert.ToString(transactionId), "GetDoddFrankReceipt", AlloyLayerName.BIZ,
				ModuleName.BillPayment, "Begin GetDoddFrankReceipt - MGI.Biz.Receipt.Impl.MGIReceiptServiceImpl",
				mgiContext);
			#endregion
            PTNRData.Transactions.MoneyTransfer xfer = _ptnrXferSvc.Lookup(transactionId);

            CXNMoneyTransferData.TransactionRequest request = new CXNMoneyTransferData.TransactionRequest()
            {
                TransactionId = xfer.CXNId
            };

			MGI.Cxn.MoneyTransfer.Data.Transaction cxntran = _GetMoneyTransferProcessor(mgiContext.ChannelPartnerName).GetTransaction(request, mgiContext);

            List<Data.Receipt> receipts = GetDoddFrankReceiptTemplate(mgiContext.ChannelPartnerName,
                PTNRData.Transactions.TransactionType.MoneyTransfer);

			string receiptContentTagsForMt = GetPartnerTags(agentSessionId, xfer, xfer.CustomerSession, mgiContext);

            receiptContentTagsForMt += GetMoneyTransferTrxTags(xfer, cxntran, xfer.CustomerSession, mgiContext.ChannelPartnerName);
            foreach (Data.Receipt receipt in receipts)
            {
                if (!string.IsNullOrEmpty(receipt.PrintData))
					receipt.PrintData = receiptContentTagsForMt + "|{data}|" + receipt.PrintData;
            }
			#region AL-3370 Transactional Log User Story
			MongoDBLogger.Info<string>(customerSessionId, string.Empty, "GetDoddFrankReceipt", AlloyLayerName.BIZ,
				ModuleName.BillPayment, "End GetDoddFrankReceipt - MGI.Biz.Receipt.Impl.MGIReceiptServiceImpl",
				mgiContext);
			#endregion
            return receipts;

        }

        #endregion

        #region Private Methods

        private List<Data.Receipt> GetSendMoneyReceiptTemplate(string partner, PTNRData.Transactions.TransactionType product,
            string productSubType, ProviderIds provider, string state)
        {
            List<string> receiptfileNames = new List<string>();

            List<Data.Receipt> receipts = new List<Data.Receipt>();

            receiptfileNames.Add(string.Format("{0}.{1}.{2}.{3}.Agent.Receipt.docx", partner, product, productSubType, provider));
            receipts.Add(new Data.Receipt()
                                 {
                                     PrintData = _receiptRepo.GetReceiptTemplates(receiptfileNames),
                                     Name = "Agent Copy",
                                     NumberOfCopies = 1
                                 });

            receiptfileNames.Clear();

            receiptfileNames.Add(string.Format("{0}.{1}.{2}.{3}.{4}.Receipt.docx",
                partner, product, productSubType, provider, state));
            receiptfileNames.Add(string.Format("{0}.{1}.{2}.{3}.Receipt.docx",
                partner, product, productSubType, provider));
            receipts.Add(new Data.Receipt()
                                 {
                                     PrintData = _receiptRepo.GetReceiptTemplates(receiptfileNames),
                                     Name = (productSubType == "Refund" || productSubType == "Receive") ? "Customer Copy" : "Customer Copy ENG",
                                     NumberOfCopies = 1
                                 });

            receiptfileNames.Clear();

            if (productSubType != "Refund" && productSubType != "Receive")
            {
                receiptfileNames.Add(string.Format("{0}.{1}.{2}.{3}.{4}.Spanish.Receipt.docx",
                                                   partner, product, productSubType, provider, state));
                receiptfileNames.Add(string.Format("{0}.{1}.{2}.{3}.Spanish.Receipt.docx",
                                                   partner, product, productSubType, provider));
                receipts.Add(new Data.Receipt()
                                 {
                                     PrintData = _receiptRepo.GetReceiptTemplates(receiptfileNames),
                                     Name = "Customer Copy ESP",
                                     NumberOfCopies = 1
                                 });
            }
            return receipts;
        }

        private List<Data.Receipt> GetBillpayReceiptTemplate(string partner, PTNRData.Transactions.TransactionType product,
           ProviderIds provider, string state)
        {

            List<string> receiptfileNames = new List<string>();

            List<Data.Receipt> receipts = new List<Data.Receipt>();

            receiptfileNames.Add(string.Format("{0}.{1}.{2}.Agent.Receipt.docx",
                                     partner, product, provider));
            receipts.Add(new Data.Receipt()
            {
                PrintData = _receiptRepo.GetReceiptTemplates(receiptfileNames),
                Name = "Agent Copy",
                NumberOfCopies = 1
            });

            receiptfileNames.Clear();

            receiptfileNames.Add(string.Format("{0}.{1}.{2}.{3}.Receipt.docx",
                                     partner, product, provider, state));
            receiptfileNames.Add(string.Format("{0}.{1}.{2}.Receipt.docx",
                                     partner, product, provider));
            receipts.Add(new Data.Receipt()
            {
                PrintData = _receiptRepo.GetReceiptTemplates(receiptfileNames),
                Name = "Customer Copy",
                NumberOfCopies = 1
            });
            return receipts;
        }

        private List<Data.Receipt> GetDoddFrankReceiptTemplate(string partner, PTNRData.Transactions.TransactionType product)
        {
            List<string> receiptfileNames = new List<string>();

            List<Data.Receipt> receipts = new List<Data.Receipt>();
            receiptfileNames.Add(string.Format("{0}.{1}.Disclosure.Spanish.Receipt.docx", partner, product));
            receipts.Add(new Data.Receipt()
            {
                PrintData = _receiptRepo.GetReceiptTemplates(receiptfileNames),
                Name = "Disclosure ESP",
                NumberOfCopies = 1
            });

            receiptfileNames.Clear();
            receiptfileNames.Add(string.Format("{0}.{1}.Disclosure.Receipt.docx", partner, product));
            receipts.Add(new Data.Receipt()
            {
                PrintData = _receiptRepo.GetReceiptTemplates(receiptfileNames),
                Name = "Disclosure ENG",
                NumberOfCopies = 1
            });
            return receipts;
        }
        private string GetBillPayTrxTags(PTNRData.Transactions.BillPay billpay, CXNBillPayData.BillPayTransaction cxntran, PTNRData.CustomerSession customerSession)
        {
            string timezone = customerSession.AgentSession.Terminal.Location.TimezoneID;
            DateTime trxDate = MGI.TimeStamp.Clock.DateTimeWithTimeZone(timezone);
            StringBuilder receiptTrxTags = new StringBuilder();
            trxDate = billpay.DTTerminalCreate == null ? trxDate : billpay.DTTerminalCreate;
            string timezoneFmt = TimeZones.Where(y => y.Key == timezone).FirstOrDefault().Value;

            receiptTrxTags.Append("|{TxrDate}|" + (trxDate.ToString("M/dd/yy").ToUpper()));
            receiptTrxTags.Append("|{TxrTime}|" + string.Format("{0} {1}", trxDate.ToString("hh:mm tt"), timezoneFmt));
            receiptTrxTags.Append("|{ProductVariant}|" + Convert.ToString(cxntran.MetaData["ProductVariant"]));
            receiptTrxTags.Append("|{SenderName}|" + string.Format("{0} {1} {2} {3}"
                , string.IsNullOrEmpty(Convert.ToString(cxntran.MetaData["SenderFirstName"])) ? "" : cxntran.MetaData["SenderFirstName"].ToString().ToUpper()
                , string.IsNullOrEmpty(Convert.ToString(cxntran.MetaData["SenderMiddleName"])) ? "" : cxntran.MetaData["SenderMiddleName"].ToString().ToUpper()
                , string.IsNullOrEmpty(Convert.ToString(cxntran.MetaData["SenderLastName"])) ? "" : cxntran.MetaData["SenderLastName"].ToString().ToUpper()
                , string.IsNullOrEmpty(Convert.ToString(cxntran.MetaData["SenderLastName2"])) ? "" : cxntran.MetaData["SenderLastName2"].ToString().ToUpper()));
            receiptTrxTags.Append("|{SenderAddress}|" + Convert.ToString(cxntran.MetaData["SenderAddress"]));
            receiptTrxTags.Append("|{SenderCity}|" + Convert.ToString(cxntran.MetaData["SenderCity"]));
            receiptTrxTags.Append("|{SenderState}|" + Convert.ToString(cxntran.MetaData["SenderState"]));
            receiptTrxTags.Append("|{SenderZip}|" + Convert.ToString(cxntran.MetaData["SenderZipCode"]));
            receiptTrxTags.Append("|{SenderHomePhone}|" + Convert.ToString(cxntran.MetaData["SenderHomePhone"]));
            receiptTrxTags.Append("|{ReceiverName}|" + cxntran.BillerName);
            receiptTrxTags.Append("|{ReceiveCode}|" + cxntran.MetaData["BillerCode"]);
            receiptTrxTags.Append("|{BillerWebsite}|" + cxntran.MetaData["BillerWebsite"]);
            receiptTrxTags.Append("|{BillerPhone}|" + cxntran.MetaData["BillerPhone"]);
            receiptTrxTags.Append("|{Account}|" + (string.IsNullOrEmpty(cxntran.AccountNumber) ? "" : cxntran.AccountNumber.Substring(cxntran.AccountNumber.Length - 4, 4)));
            receiptTrxTags.Append("|{Attention}|" + Convert.ToString(cxntran.MetaData["Attention"]));
            receiptTrxTags.Append("|{Message}|" + Convert.ToString(cxntran.MetaData["Message"]));
            receiptTrxTags.Append("|{TextTranslation}|" + Convert.ToString(cxntran.MetaData["TextTranslation"]));
            receiptTrxTags.Append("|{ReferenceNumber}|" + cxntran.ConfirmationNumber);
            receiptTrxTags.Append("|{TransferAmount}|" + string.Format("{0} {1}", cxntran.Amount.ToString("0.00"), Convert.ToString(cxntran.MetaData["SendCurrency"])));
            receiptTrxTags.Append("|{TransferFee}|" + string.Format("{0} {1}", cxntran.Fee.ToString("0.00"), Convert.ToString(cxntran.MetaData["SendCurrency"])));
            receiptTrxTags.Append("|{NetAmount}|" + string.Format("{0} {1}", ((decimal)cxntran.MetaData["TotalAmountToCollect"]).ToString("0.00"), Convert.ToString(cxntran.MetaData["SendCurrency"])));
            receiptTrxTags.Append("|{PrmDiscount}|" + string.Format("{0} {1}", ((decimal)cxntran.MetaData["TotalDiscountAmount"]).ToString("0.00"), Convert.ToString(cxntran.MetaData["SendCurrency"])));
            receiptTrxTags.Append("|{TransferTaxes}|" + string.Format("{0} {1}", ((decimal)cxntran.MetaData["TotalSendTaxes"]).ToString("0.00"), Convert.ToString(cxntran.MetaData["SendCurrency"])));
            receiptTrxTags.Append("|{CurrencyCode}|" + Convert.ToString(cxntran.MetaData["SendCurrency"]));
            receiptTrxTags.Append("|{BillerCutoffTime}|" + (string.IsNullOrEmpty(NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "BillerCutoffTime").ToNullSafeString()) ? "11.59PM CST" : NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "BillerCutoffTime").ToNullSafeString()));
            receiptTrxTags.Append("|{CustomerTipTextTranslation}|" + NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "CustomerTipTextTranslation").ToNullSafeString());

            string ExpectedPostingTimeFrame = NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "ExpectedPostingTimeFrame").ToNullSafeString();

            if (NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "ExpectedPostingTimeFrameSecondary").ToNullSafeString() != "")
                ExpectedPostingTimeFrame = ExpectedPostingTimeFrame + " / " + NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "ExpectedPostingTimeFrameSecondary").ToNullSafeString();

            receiptTrxTags.Append("|{ExpectedPostingTimeFrame}|" + ExpectedPostingTimeFrame);

            receiptTrxTags.Append("|{ReceiveAgentName}|" + customerSession.AgentSession.Terminal.Location.LocationName);
            receiptTrxTags.Append("|{AgentPhoneNumber}|" + customerSession.AgentSession.Terminal.Location.PhoneNumber);
            return receiptTrxTags.ToString();
        }

        private string GetMoneyTransferTrxTags(PTNRData.Transactions.Transaction trx, CXNMoneyTransferData.Transaction cxntran,
            PTNRData.CustomerSession customerSession, string channelPartnerName)
        {
            // replace other transaction-specific receipt tags // with processor router 
            CXNMoneyTransferContract.IMoneyTransfer moneyTransferProcessor = _GetMoneyTransferProcessor(channelPartnerName);

            StringBuilder receiptBuilder = new StringBuilder();
            CultureInfo cultureinfo = new CultureInfo("es-ES");

            string timezone = customerSession.AgentSession.Terminal.Location.TimezoneID;
            DateTime trxDate = MGI.TimeStamp.Clock.DateTimeWithTimeZone(timezone);
            trxDate = trx.DTTerminalCreate == null ? trxDate : trx.DTTerminalCreate;

            string filingDate = NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "FilingDate").ToNullSafeString();
            string filingTime = NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "FilingTime").ToNullSafeString();

            if (!string.IsNullOrWhiteSpace(filingDate) && !string.IsNullOrWhiteSpace(filingTime))
            {
                string mmddyy = String.Format("{0}-{1}", filingDate, Convert.ToString(trx == null ? trxDate.Year : trx.DTTerminalCreate.Year));
                var sourcetimezone = filingTime.Split(' ')[1];
                var time = filingTime.Split(' ')[0];
                trxDate = MGI.TimeStamp.Clock.ConvertDateTimeWithTimeZone(timezone, sourcetimezone, mmddyy, time);
            }

            //string payOutCountry = Convert.ToString(PTNRDataStructureService.GetCountry(cxntran.DestinationCountryCode)).ToUpper();
            string payOutCountry = GetPayOutCountry(moneyTransferProcessor, cxntran);

            string timezoneFmt = TimeZones.Where(y => y.Key == timezone).FirstOrDefault().Value;

            receiptBuilder.Append("|{TxrDate}|" + (trxDate.ToString("MMMM dd yyyy").ToUpper() + string.Format(" / {0}", cultureinfo.TextInfo.ToTitleCase(trxDate.ToString("MMMM dd yyyy", cultureinfo).ToUpper()))));
            receiptBuilder.Append("|{TxrTime}|" + string.Format("{0} {1}", trxDate.ToString("hh:mm tt"), timezoneFmt));
            receiptBuilder.Append("|{Date}|" + (trxDate.ToString("M/dd/yy").ToUpper()));
            receiptBuilder.Append("|{Time}|" + string.Format("{0} {1}", trxDate.ToString("hh:mm tt"), timezoneFmt));

            string senderMiddleName = NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "SenderMiddleName").ToNullSafeString();
            string senderLastName2 = NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "SenderLastName2").ToNullSafeString();
            string senderName = string.Format("{0}{1}{2}{3}", string.IsNullOrEmpty(cxntran.Account.FirstName) ? "" : cxntran.Account.FirstName.ToUpper(), string.IsNullOrEmpty(senderMiddleName) ? "" : " " + senderMiddleName.ToUpper(), string.IsNullOrEmpty(cxntran.Account.LastName) ? "" : " " + cxntran.Account.LastName.ToUpper(), string.IsNullOrEmpty(senderLastName2) ? "" : " " + senderLastName2.ToUpper());
            receiptBuilder.Append("|{SenderName}|" + senderName);
            receiptBuilder.Append("|{SenderAddress}|" + (string.IsNullOrEmpty(cxntran.Account.Address) ? "" : cxntran.Account.Address.ToUpper()));
            receiptBuilder.Append("|{SenderCity}|" + (string.IsNullOrEmpty(cxntran.Account.City) ? "" : string.Format("{0},", cxntran.Account.City.ToUpper())));
            receiptBuilder.Append("|{SenderState}|" + (string.IsNullOrEmpty(cxntran.Account.State) ? "" : cxntran.Account.State.ToUpper()));
            receiptBuilder.Append("|{SenderZip}|" + cxntran.Account.PostalCode);
            receiptBuilder.Append("|{SenderPhoneNumber}|" + (string.IsNullOrEmpty(cxntran.Account.ContactPhone) ? removeLine : cxntran.Account.ContactPhone));
            receiptBuilder.Append("|{SenderMobileNumber}|" + (string.IsNullOrEmpty(cxntran.Account.MobilePhone) ? removeLine : cxntran.Account.MobilePhone));

            string receiverName = string.Format("{0}{1}{2}{3}", string.IsNullOrEmpty(cxntran.ReceiverFirstName) ? "" : cxntran.ReceiverFirstName.ToUpper(), string.IsNullOrEmpty(cxntran.Receiver.MiddleName) ? "" : " " + cxntran.Receiver.MiddleName.ToUpper(), string.IsNullOrEmpty(cxntran.ReceiverLastName) ? "" : " " + cxntran.ReceiverLastName.ToUpper(), string.IsNullOrEmpty(cxntran.ReceiverSecondLastName) ? "" : " " + cxntran.ReceiverSecondLastName.ToUpper());
            receiptBuilder.Append("|{ReceiverName}|" + receiverName);

            receiptBuilder.Append("|{ReceiverAddress}|" + (string.IsNullOrEmpty(cxntran.Receiver.Address) ? "" : cxntran.Receiver.Address.ToUpper()));
            receiptBuilder.Append("|{ReceiverCity}|" + (string.IsNullOrEmpty(cxntran.Receiver.City) ? "" : string.Format("{0},", cxntran.Receiver.City.ToUpper())));
            receiptBuilder.Append("|{ReceiverState}|" + (string.IsNullOrEmpty(cxntran.Receiver.State_Province) ? "" : cxntran.Receiver.State_Province.ToUpper()));
            receiptBuilder.Append("|{ReceiverZip}|" + cxntran.Receiver.ZipCode);
            receiptBuilder.Append("|{ReceiverPhoneNumber}|" + (string.IsNullOrEmpty(cxntran.Receiver.PhoneNumber) ? removeLine : cxntran.Receiver.PhoneNumber));

            receiptBuilder.Append("|{PayoutCountry}|" + payOutCountry);
            receiptBuilder.Append("|{PayoutState}|" + (cxntran.DestinationState == null ? "" : string.Format("{0},", cxntran.DestinationState.ToUpper())));
            receiptBuilder.Append("|{TransferAmount}|" + cxntran.TransactionAmount);
            receiptBuilder.Append("|{CurrencyCode}|" + cxntran.OriginatingCurrencyCode);

            //If PromoCode is Present the we are adding Promo discount to transfer fee for getting the net total. 
            decimal fee = cxntran.Fee + cxntran.PromotionDiscount;
            receiptBuilder.Append("|{TransferFee}|" + fee);
            receiptBuilder.Append("|{TransferTaxes}|" + (cxntran.TaxAmount <= 0 ? "0.00" : cxntran.TaxAmount.ToString()));
            receiptBuilder.Append("|{ReceiveAgentName}|" + customerSession.AgentSession.Terminal.Location.LocationName);
            receiptBuilder.Append("|{AgentPhoneNumber}|" + customerSession.AgentSession.Terminal.Location.PhoneNumber);
            receiptBuilder.Append("|{AgentAddressLine}|" + customerSession.AgentSession.Terminal.Location.Address1);
            receiptBuilder.Append("|{AgentAddressLine2}|" + (string.IsNullOrEmpty(customerSession.AgentSession.Terminal.Location.Address2) ? removeLine : customerSession.AgentSession.Terminal.Location.Address2));
            receiptBuilder.Append("|{AgentCity}|" + (string.IsNullOrEmpty(customerSession.AgentSession.Terminal.Location.City) ? removeLine : customerSession.AgentSession.Terminal.Location.City));

            //On receiveFeeDisclosureText being true "asterisk" is added for Other Fees and Other Taxes
            bool receiveFeeDisclosure = Convert.ToBoolean(NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "ReceiveFeeDisclosureText").ToNullSafeString());

            //On validCurrencyIndicator being false Estimated is added for Exchange Rate,Transfer Amount,Other Fees,Other Taxes and Total to Recipient
            bool validCurrencyIndicator = Convert.ToBoolean(NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "ValidCurrencyIndicator").ToNullSafeString());
            bool receiveTaxesAreEstimated = Convert.ToBoolean(NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "ReceiveTaxesAreEstimated").ToNullSafeString());
            bool receiveFeesAreEstimated = Convert.ToBoolean(NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "ReceiveFeesAreEstimated").ToNullSafeString());
            receiptBuilder.Append("|{Asterisk}|" + ((receiveFeeDisclosure) ? "*" : string.Empty));

            if (!validCurrencyIndicator)
            {
                receiptBuilder.Append("|{Estimated}|" + "Estimated ");
                receiptBuilder.Append("|{EstimatedTax}|" + "Estimated ");
                receiptBuilder.Append("|{EstimatedFees}|" + "Estimated ");
            }
            else
            {
                receiptBuilder.Append("|{Estimated}|" + string.Empty);
                receiptBuilder.Append("|{EstimatedTax}|" + ((receiveTaxesAreEstimated) ? "Estimated " : string.Empty));
                receiptBuilder.Append("|{EstimatedFees}|" + ((receiveFeesAreEstimated) ? "Estimated " : string.Empty));
            }

            receiptBuilder.Append("|{EstimatedTotal}|" + ((!validCurrencyIndicator || receiveTaxesAreEstimated || receiveFeesAreEstimated) ? "Estimated " : string.Empty));

            decimal additionalCharges = NexxoUtil.GetDecimalDictionaryValueIfExists(cxntran.MetaData, "AdditionalCharges");
            decimal messageCharge = NexxoUtil.GetDecimalDictionaryValueIfExists(cxntran.MetaData, "MessageCharge");
            decimal paysideCharges = NexxoUtil.GetDecimalDictionaryValueIfExists(cxntran.MetaData, "PaySideCharges");
            string expectedPayoutCity = NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "ExpectedPayoutCity").ToNullSafeString();

            string accountNumberLastFour = NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "AccountNumberLastFour");
            string accountNickname = NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "AccountNickName");
            string payoutCurrency = NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "PayoutCurrency");
            string receiveAgentAbbr = NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "ReceiveAgentAbbreviation");
            string customerReceiveNumber = NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "CustomerReceiveNumber");
            string partnerConfirmationNumber = NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "PartnerConfirmationNumber");
            string disclosureText = NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "DisclosureTextPrimaryLanguage");
            // Need to identify the logic to get PIN number and account balance
            string pinNumber = string.Empty;
            string accountBalance = string.Empty;

            receiptBuilder.Append("|{Pin}|" + (string.IsNullOrEmpty(pinNumber) ? removeLine : (string.Format("Your PIN is {0}", pinNumber))));
            receiptBuilder.Append("|{AccountBalance}|" + (string.IsNullOrEmpty(accountBalance) ? removeLine : (string.Format("Your account balance is ${0}", accountBalance))));

            decimal additionalFee = additionalCharges + messageCharge;
            receiptBuilder.Append("|{AccountLastFour}|" + (string.IsNullOrEmpty(accountNumberLastFour) ? removeLine : accountNumberLastFour));
            receiptBuilder.Append("|{AccountNickName}|" + (string.IsNullOrEmpty(accountNickname) ? removeLine : accountNickname));
            receiptBuilder.Append("|{PayoutCurrency}|" + (string.IsNullOrEmpty(payoutCurrency) ? removeLine : (string.Format(" - {0}", payoutCurrency))));
            receiptBuilder.Append("|{ReceiveAgentAbbr}|" + (string.IsNullOrEmpty(receiveAgentAbbr) ? "" : (string.Format(" - {0}", receiveAgentAbbr))));
            receiptBuilder.Append("|{CustomerReceiveNumber}|" + (string.IsNullOrEmpty(customerReceiveNumber) ? removeLine : customerReceiveNumber));
            receiptBuilder.Append("|{PartnerConfirmationNumber}|" + (string.IsNullOrEmpty(partnerConfirmationNumber) ? removeLine : partnerConfirmationNumber));
            receiptBuilder.Append("|{AdditionalFee}|" + additionalFee);
            receiptBuilder.Append("|{PromoCode}|" + (string.IsNullOrEmpty(cxntran.PromotionsCode) ? removeLine : cxntran.PromotionsCode));
            receiptBuilder.Append("|{PrmDiscount}|" + (cxntran.PromotionDiscount <= 0 ? removeLine : (string.Format("-{0} {1}", cxntran.PromotionDiscount.ToString(), cxntran.OriginatingCurrencyCode))));
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
            receiptBuilder.Append("|{EstTotalToReceiver}|" + (cxntran.DestinationPrincipalAmount - (paysideCharges == 0 ? 0 : paysideCharges)));

            string deliveryOption = NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "DeliveryOption").ToNullSafeString();
            string deliveryOptionDesc = NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "DeliveryOptionDesc").ToNullSafeString();
            string serviceType = string.IsNullOrEmpty(deliveryOptionDesc) ? cxntran.DeliveryServiceDesc : deliveryOptionDesc;
            if (string.IsNullOrWhiteSpace(serviceType))
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
            receiptBuilder.Append("|{TotalToReceiver}|" + (cxntran.DestinationPrincipalAmount - (paysideCharges <= 0 ? 0 : paysideCharges)
                - (otherFees <= 0 ? 0 : otherFees) - (otherTaxes <= 0 ? 0 : otherTaxes)));
            receiptBuilder.Append("|{MTCN}|" + FormatMTCN(trx.ConfirmationNumber));

            string agencyName = NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "AgencyName").ToNullSafeString();
            string phoneNumber = NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "PhoneNumber").ToNullSafeString();
            string url = NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "Url").ToNullSafeString();
            string tollFreePhoneNumber = NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "TollFreePhoneNumber").ToNullSafeString();

            if (tollFreePhoneNumber.Length == 11)
                tollFreePhoneNumber = string.Format("{0}-{1}-{2}-{3}", tollFreePhoneNumber.Substring(0, 1), tollFreePhoneNumber.Substring(1, 3), tollFreePhoneNumber.Substring(4, 3), tollFreePhoneNumber.Substring(7, 4));

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
            receiptBuilder.Append("|{FREEPhoneCall}|" + (string.IsNullOrEmpty(tollFreePhoneNumber) ? removeLine : ""));
            receiptBuilder.Append("|{TollFreePhoneNumber}|" + (string.IsNullOrEmpty(tollFreePhoneNumber) ? removeLine : tollFreePhoneNumber));
            receiptBuilder.Append("|{FreePhoneCallPIN}|" + (string.IsNullOrEmpty(freePhoneCallPIN) ? removeLine : freePhoneCallPIN));
            receiptBuilder.Append("|{ReferenceNumber}|" + cxntran.ConfirmationNumber);

            if (!string.IsNullOrWhiteSpace(cxntran.PersonalMessage))
            {
                List<string> messages = cxntran.PersonalMessage.Split(40).ToList();

                if (messages.Count > 0)
                {
                    receiptBuilder.Append("|{Message1}|" + (string.IsNullOrEmpty(messages[0]) ? removeLine : messages[0]));
                    if (messages.Count > 1)
                    {
                        receiptBuilder.Append("|{Message2}|" + (string.IsNullOrEmpty(messages[1]) ? removeLine : messages[1]));
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

            receiptBuilder.Append("|{StateRegulatorName}|" + NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "StateRegulatorName").ToNullSafeString());
            receiptBuilder.Append("|{StateRegualtorPhone}|" + NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "StateRegulatorPhone").ToNullSafeString());
            receiptBuilder.Append("|{StateRegulatroURL}|" + NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "StateRegulatorURL").ToNullSafeString());

            receiptBuilder.Append("|{DisclosureText}|" + (string.IsNullOrEmpty(disclosureText) ? removeLine : disclosureText));

            string CashToAccountTrans = NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "CashToAccountTrans").ToNullSafeString();
            receiptBuilder.Append("|{CashToAccountTran}|" + (CashToAccountTrans == "False" ? removeLine : ""));

            //AGENT RECEIPT DETAILS
            receiptBuilder.Append("|{SenderPhotoIdType}|" + NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "SenderPhotoIdType").ToNullSafeString());
            receiptBuilder.Append("|{SenderPhotoIdNumber}|" + NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "SenderPhotoIdNumber").ToNullSafeString());
            receiptBuilder.Append("|{SenderPhotoIdState}|" + NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "SenderPhotoIdState").ToNullSafeString());
            receiptBuilder.Append("|{SenderPhotoIdCountry}|" + NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "SenderPhotoIdCountry").ToNullSafeString());

            receiptBuilder.Append("|{SenderLegalIdType}|" + NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "SenderLegalIdType").ToNullSafeString());
            receiptBuilder.Append("|{SenderLegalIdNumber}|" + NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "SenderLegalIdNumber").ToNullSafeString());

            DateTime dobDate = Convert.ToDateTime(NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "SenderDOB"));
            receiptBuilder.Append("|{SenderDOB}|" + (dobDate.ToString("M/dd/yy").ToUpper()));
            receiptBuilder.Append("|{SenderOccupation}|" + NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "SenderOccupation").ToNullSafeString());

            receiptBuilder.Append("|{TextTranslationPrimary}|" + NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "TextTranslationPrimary").ToNullSafeString());
            receiptBuilder.Append("|{TextTranslationSecondary}|" + NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "TextTranslationSecondary").ToNullSafeString());

            return receiptBuilder.ToString();
        }

        private string GetReceiveMoneyTrxTags(PTNRData.Transactions.Transaction ptnrTrx,
             CXNMoneyTransferData.Transaction cxntran, PTNRData.CustomerSession customerSession, string channelPartnerName)
        {
            StringBuilder appendreceipttags = new StringBuilder();

            CultureInfo cultureinfo = new CultureInfo("es-ES");
            string timezone = customerSession.AgentSession.Terminal.Location.TimezoneID;
            DateTime trxDate = MGI.TimeStamp.Clock.DateTimeWithTimeZone(timezone);
            trxDate = ptnrTrx.DTTerminalCreate == null ? trxDate : ptnrTrx.DTTerminalCreate;

            string paidDateTime = NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "PaidDateTime").ToNullSafeString();

            if (!string.IsNullOrWhiteSpace(paidDateTime))
            {
                string mmddyy = paidDateTime.Split(' ')[0];
                string sourcetimezone = "EST";
                string t = paidDateTime.Split(' ')[1];
                string time = t.Split(':')[0] + t.Split(':')[1] + (t.Split(':')[0].Equals("P") ? "P" : "A");
                trxDate = MGI.TimeStamp.Clock.ConvertDateTimeWithTimeZone(timezone, sourcetimezone, mmddyy, time);
            }

            string timezoneFmt = TimeZones.Where(y => y.Key == timezone).FirstOrDefault().Value;
            appendreceipttags.Append("|{TxrDate}|" + (trxDate.ToString("MMMM dd yyyy").ToUpper() + string.Format(" / {0}", cultureinfo.TextInfo.ToTitleCase(trxDate.ToString("MMMM dd yyyy", cultureinfo).ToUpper()))));
            appendreceipttags.Append("|{TxrTime}|" + string.Format("{0} {1}", trxDate.ToString("hh:mm tt"), timezoneFmt));
            appendreceipttags.Append("|{Date}|" + (trxDate.ToString("M/dd/yy").ToUpper()));
            appendreceipttags.Append("|{Time}|" + string.Format("{0} {1}", trxDate.ToString("hh:mm tt"), timezoneFmt));
            string senderMiddleName = NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "SenderMiddleName").ToNullSafeString();
            string senderLastName2 = NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "SenderLastName2").ToNullSafeString();
            string senderName = string.Format("{0}{1}{2}{3}", string.IsNullOrEmpty(cxntran.Account.FirstName) ? "" : cxntran.Account.FirstName.ToUpper(), string.IsNullOrEmpty(senderMiddleName) ? "" : " " + senderMiddleName.ToUpper(), string.IsNullOrEmpty(cxntran.Account.LastName) ? "" : " " + cxntran.Account.LastName.ToUpper(), string.IsNullOrEmpty(senderLastName2) ? "" : " " + senderLastName2.ToUpper());
            appendreceipttags.Append("|{SenderName}|" + senderName);
            appendreceipttags.Append("|{ReceiveAgentName}|" + customerSession.AgentSession.Terminal.Location.LocationName);
            appendreceipttags.Append("|{AgentPhoneNumber}|" + customerSession.AgentSession.Terminal.Location.PhoneNumber);

            string receiverName = string.Format("{0} {1} {2} {3}", string.IsNullOrEmpty(cxntran.ReceiverFirstName) ? "" : cxntran.ReceiverFirstName.ToUpper(), string.IsNullOrEmpty(cxntran.Receiver.MiddleName) ? "" : cxntran.Receiver.MiddleName.ToUpper(), string.IsNullOrEmpty(cxntran.ReceiverLastName) ? "" : cxntran.ReceiverLastName.ToUpper(), string.IsNullOrEmpty(cxntran.ReceiverSecondLastName) ? "" : cxntran.ReceiverSecondLastName.ToUpper());
            appendreceipttags.Append("|{ReceiverName}|" + receiverName);

            appendreceipttags.Append("|{ReceiverAddress}|" + NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "ReceiverAddress").ToNullSafeString());

            if (cxntran.Receiver != null)
            {
                appendreceipttags.Append("|{ReceiverCity}|" + cxntran.Receiver.City);
                appendreceipttags.Append("|{ReceiverState}|" + cxntran.Receiver.State_Province);
                appendreceipttags.Append("|{ReceiverZip}|" + cxntran.Receiver.ZipCode);
                appendreceipttags.Append("|{ReceiverPhoneNumber}|" + cxntran.Receiver.PhoneNumber);
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
            appendreceipttags.Append("|{PersonalMessage}|" + cxntran.PersonalMessage);
            appendreceipttags.Append("|{ReceiverPhotoIdType}|" + NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "ReceiverPhotoIdType").ToNullSafeString());
            appendreceipttags.Append("|{ReceiverPhotoIdNumber}|" + NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "ReceiverPhotoIdNumber").ToNullSafeString());
            appendreceipttags.Append("|{ReceiverPhotoIdState}|" + NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "ReceiverPhotoIdState").ToNullSafeString());
            appendreceipttags.Append("|{ReceiverPhotoIdCountry}|" + NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "ReceiverPhotoIdCountry").ToNullSafeString());
            appendreceipttags.Append("|{ReceiverLegalIdType}|" + NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "ReceiverLegalIdType").ToNullSafeString());
            appendreceipttags.Append("|{ReceiverLegalIdNumber}|" + NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "ReceiverLegalIdNumber").ToNullSafeString());
            appendreceipttags.Append("|{TextTranslationPrimary}|" + NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "TextTranslationPrimary").ToNullSafeString());
            appendreceipttags.Append("|{TextTranslationSecondary}|" + NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "TextTranslationSecondary").ToNullSafeString());
            return appendreceipttags.ToString();
        }

        private string GetRefundSendMoneyTrxTags(PTNRData.Transactions.Transaction ptnrTrx,
             CXNMoneyTransferData.Transaction cxntran, PTNRData.CustomerSession customerSession, string channelPartnerName)
        {
            StringBuilder receiptBuilder = new StringBuilder();

            string timezone = customerSession.AgentSession.Terminal.Location.TimezoneID;
            DateTime trxDate = MGI.TimeStamp.Clock.DateTimeWithTimeZone(timezone);
            trxDate = ptnrTrx.DTTerminalCreate == null ? trxDate : ptnrTrx.DTTerminalCreate;

            string filingDate = NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "FilingDate").ToNullSafeString();
            string filingTime = NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "FilingTime").ToNullSafeString();

            if (!string.IsNullOrWhiteSpace(filingDate) && !string.IsNullOrWhiteSpace(filingTime))
            {
                string mmddyy = String.Format("{0}-{1}", filingDate, Convert.ToString(ptnrTrx == null ? trxDate.Year : ptnrTrx.DTTerminalCreate.Year));
                var sourcetimezone = filingTime.Split(' ')[1];
                var time = filingTime.Split(' ')[0];
                trxDate = MGI.TimeStamp.Clock.ConvertDateTimeWithTimeZone(timezone, sourcetimezone, mmddyy, time);
            }

            receiptBuilder.Append("|{ReceiveAgentName}|" + customerSession.AgentSession.Terminal.Location.LocationName);
            receiptBuilder.Append("|{AgentPhoneNumber}|" + customerSession.AgentSession.Terminal.Location.PhoneNumber);

            string timezoneFmt = TimeZones.Where(y => y.Key == timezone).FirstOrDefault().Value;

            DateTime tranferDate = Convert.ToDateTime(NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "DateTimeSent"));

            receiptBuilder.Append("|{TransferDate}|" + (tranferDate.ToString("M/dd/yy").ToUpper()));
            receiptBuilder.Append("|{TransferTime}|" + string.Format("{0} {1}", tranferDate.ToString("hh:mm tt"), timezoneFmt));

            receiptBuilder.Append("|{Date}|" + (trxDate.ToString("M/dd/yy").ToUpper()));
            receiptBuilder.Append("|{Time}|" + string.Format("{0} {1}", trxDate.ToString("hh:mm tt"), timezoneFmt));

            receiptBuilder.Append("|{ReferenceNumber}|" + cxntran.ConfirmationNumber);

            string senderMiddleName = NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "SenderMiddleName").ToNullSafeString();
            string senderLastName2 = NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "SenderLastName2").ToNullSafeString();
            string senderName = string.Format("{0}{1}{2}{3}", string.IsNullOrEmpty(cxntran.Account.FirstName) ? "" : cxntran.Account.FirstName.ToUpper(), string.IsNullOrEmpty(senderMiddleName) ? "" : " " + senderMiddleName.ToUpper(), string.IsNullOrEmpty(cxntran.Account.LastName) ? "" : " " + cxntran.Account.LastName.ToUpper(), string.IsNullOrEmpty(senderLastName2) ? "" : " " + senderLastName2.ToUpper());
            receiptBuilder.Append("|{SenderName}|" + senderName);

            string receiverName = string.Format("{0} {1} {2} {3}", string.IsNullOrEmpty(cxntran.ReceiverFirstName) ? "" : cxntran.ReceiverFirstName.ToUpper(), string.IsNullOrEmpty(cxntran.Receiver.MiddleName) ? "" : cxntran.Receiver.MiddleName.ToUpper(), string.IsNullOrEmpty(cxntran.ReceiverLastName) ? "" : cxntran.ReceiverLastName.ToUpper(), string.IsNullOrEmpty(cxntran.ReceiverSecondLastName) ? "" : cxntran.ReceiverSecondLastName.ToUpper());
            receiptBuilder.Append("|{ReceiverName}|" + receiverName);

            receiptBuilder.Append("|{SendReversalReason}|" + NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "SendReversalReason"));
            receiptBuilder.Append("|{OperatorName}|" + NexxoUtil.GetDictionaryValueIfExists(cxntran.MetaData, "OperatorName"));

            decimal TransferAmount = NexxoUtil.GetDecimalDictionaryValueIfExists(cxntran.MetaData, "RefundFaceAmount");
            decimal TransferFee = NexxoUtil.GetDecimalDictionaryValueIfExists(cxntran.MetaData, "RefundFeeAmount");
            decimal NetAmount = NexxoUtil.GetDecimalDictionaryValueIfExists(cxntran.MetaData, "RefundTotalAmount");

            receiptBuilder.Append("|{TransferAmount}|" + TransferAmount.ToString("0.00"));
            receiptBuilder.Append("|{CurrencyCode}|" + cxntran.OriginatingCurrencyCode);
            receiptBuilder.Append("|{TransferFee}|" + TransferFee.ToString("0.00"));

            receiptBuilder.Append("|{NetAmount}|" + NetAmount.ToString("0.00"));

            return receiptBuilder.ToString();
        }

        #endregion
    }
}
