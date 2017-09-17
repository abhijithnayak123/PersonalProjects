using TCF.Zeo.Core.Contract;
using TCF.Zeo.Core.Impl;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using BizData = TCF.Zeo.Biz.Receipt.Data;
using PTNRData = TCF.Zeo.Core.Data;
using TCF.Zeo.Common.Util;
using commonData = TCF.Zeo.Common.Data;
using TCF.Zeo.Biz.Common.Data.Exceptions;
using TCF.Zeo.Processor;

namespace TCF.Zeo.Biz.Receipt.Impl
{
    public class WUReceiptServiceImpl : BaseReceiptServiceImpl
    {
        #region IReceiptService methods
        public override BizData.Receipt GetBillPayReceipt(long transactionId, bool isReprint, commonData.ZeoContext context)
        {
            try
            {
                if (!isReprint && !string.IsNullOrWhiteSpace(context.WUCardNumber))
                {
                    ProcessorRouter processorRouter = new ProcessorRouter();
                    processorRouter.GetBillPayCXNProcessor(context.ChannelPartnerName).UpdateBillPayGoldCardPoints(transactionId, context.WUCardNumber, context);
                }

                PTNRData.BillpayReceiptData receiptData = _receiptServiceRepo.GetBillpayReceiptData(transactionId, Helper.TransactionType.BillPayment.ToString(), (int)Helper.ProviderId.WesternUnionBillPay, isReprint, context);
                BizData.Receipt receipt = GetBillpayReceiptTemplate(receiptData.ClientName, receiptData.NumberOfCopies, Helper.TransactionType.BillPayment, Helper.ProviderId.WesternUnionBillPay, isReprint);
                string receiptContentstags = GetBillPayTrxtags(receiptData);

                if (!string.IsNullOrEmpty(receipt.PrintData))
                    receipt.PrintData = receiptContentstags + "|{data}|" + receipt.PrintData;

                return receipt;
            }
            catch (Exception ex)
            {

                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new ReceiptException(ReceiptException.BILLPAY_RECEIPT_TEMPLATE_RETRIVEL_FAILED, ex);
            }
        }
        public override BizData.Receipt GetMoneyTransferReceipt(long transactionId, bool isReprint, commonData.ZeoContext context)
        {
            try
            {
                if (!isReprint && !string.IsNullOrWhiteSpace(context.WUCardNumber))
                {
                    ProcessorRouter processorRouter = new ProcessorRouter();
                    processorRouter.GetCXNMoneyTransferProcessor(context.ChannelPartnerName).UpdateMoneyTransferGoldCardPoints(transactionId, context.WUCardNumber, context);
                }

                PTNRData.MoneyTransferReceiptData receiptData = _receiptServiceRepo.GetMoneyTransferReceiptData(transactionId, Helper.TransactionType.MoneyTransfer.ToString(), (int)Helper.ProviderId.WesternUnion, context, isReprint);

                string providerAttribute = string.Empty;
                if (!receiptData.IsDomesticTransfer)
                {
                    providerAttribute = receiptData.IsFixOnSend ? Helper.SendType.Fxd.ToString() : Helper.SendType.Estd.ToString();
                }
                string moneyTransferType = receiptData.TranascationType;
                string moneyTransferSubType = string.IsNullOrWhiteSpace(receiptData.TransactionSubType) ? ((Helper.MoneyTransferType)Convert.ToInt32(moneyTransferType)).ToString() : ((Helper.TransactionSubType.Modify).ToString());

                BizData.Receipt receipt = GetMoneyTransferReceiptTemplate(receiptData.ClientName, receiptData.NumberOfCopies, Helper.TransactionType.MoneyTransfer, moneyTransferSubType, Helper.ProviderId.WesternUnion, providerAttribute, isReprint);

                string receiptContentstags = string.Empty;
                if (moneyTransferType == "1")//send money
                    receiptContentstags = GetMoneyTransferTrxTags(receiptData);
                else // receive money
                    receiptContentstags = GetReceiveMoneyTrxTags(receiptData);

                if (!string.IsNullOrEmpty(receipt.PrintData))
                    receipt.PrintData = receiptContentstags + "|{data}|" + receipt.PrintData;

                return receipt;
            }
            catch (Exception ex)
            {

                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new ReceiptException(ReceiptException.MT_RECEIPT_TEMPLATE_RETRIVEL_FAILED, ex);
            }
        }
        public override BizData.Receipt GetDoddFrankReceipt(long transactionId, commonData.ZeoContext context)
        {
            try
            {
                PTNRData.MoneyTransferReceiptData receiptData = _receiptServiceRepo.GetMoneyTransferReceiptData(transactionId, Helper.TransactionType.MoneyTransfer.ToString(), (int)Helper.ProviderId.WesternUnion, null);

                BizData.Receipt receipt = GetDoddfrankReportTemplate(receiptData.ClientName, receiptData.IsFixOnSend);

                string receiptContentstags = GetMoneyTransferTrxTags(receiptData);

                if (!string.IsNullOrEmpty(receipt.PrintData))
                    receipt.PrintData = receiptContentstags + "|{data}|" + receipt.PrintData;

                return receipt;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new ReceiptException(ReceiptException.DODFRANK_RECEIPT_RETRIVEL_FAILED, ex);
            }
        }

        #endregion

        #region Private Methods
        private Data.Receipt GetBillpayReceiptTemplate(string partner,int NumberOfCopies, Helper.TransactionType product, Helper.ProviderId provider, bool isReprint)
        {
            string receiptfileName = string.Format("{0}.{1}.{2}.Receipt.docx", partner, "BillPay", provider);
            return new Data.Receipt()
            {
                PrintData = GetReceiptTemplates(new List<string> { receiptfileName }),
                Name = "",
                NumberOfCopies = NumberOfCopies
            };
        }

        private Data.Receipt GetDoddfrankReportTemplate(string partner, bool isFixOnSend = false)
        {
            string sreport = "DoddFrank";
            List<string> receiptfileNames = new List<string>();

            if (isFixOnSend)
                receiptfileNames.Add(string.Format("{0}.{1}.Fxd.Receipt.docx", partner, sreport));
            else
                receiptfileNames.Add(string.Format("{0}.{1}.Estd.Receipt.docx", partner, sreport));

            receiptfileNames.Add(string.Format("{0}.MoneyTransfer.Disclosure.Receipt.docx", partner));

            return new Data.Receipt()
            {
                PrintData = GetReceiptTemplates(receiptfileNames),
                Name = "",
                NumberOfCopies = 1
            };
        }
        
        private Data.Receipt GetMoneyTransferReceiptTemplate(string partner,int NumberOfCopies, Helper.TransactionType product, string productSubType,
            Helper.ProviderId provider, string providerAttribute, bool isReprint)
        {
            List<string> receiptfileNames = new List<string>();

            if (!string.IsNullOrEmpty(providerAttribute))
                receiptfileNames.Add(string.Format("{0}.{1}.{2}.{3}.{4}.Receipt.docx", partner, product,
                                                           productSubType, provider, providerAttribute));
            receiptfileNames.Add(string.Format("{0}.{1}.{2}.{3}.Receipt.docx",
                                     partner, product, productSubType, provider));
            receiptfileNames.Add(string.Format("{0}.{1}.{2}.Receipt.docx",
                                     partner, product, provider));
            return new Data.Receipt()
            {
                PrintData = GetReceiptTemplates(receiptfileNames),
                Name = "",
                NumberOfCopies = NumberOfCopies
            };
        }

        private string GetBillPayTrxtags(PTNRData.BillpayReceiptData receiptData)
        {
            StringBuilder billPayReceiptTrxTags = new StringBuilder();
            
            CultureInfo cultureinfo = new CultureInfo("es-ES");
            
            billPayReceiptTrxTags.Append("|{TxrDate}|" + (receiptData.TxrDate.ToString("MMMM dd yyyy").ToUpper() + string.Format(" / {0}", cultureinfo.TextInfo.ToTitleCase(receiptData.TxrDate.ToString("MMMM dd yyyy", cultureinfo).ToUpper()))));
            billPayReceiptTrxTags.Append("|{TxrTime}|" + string.Format("{0} {1}", receiptData.TxrDate.ToString("hh:mm tt"), receiptData.TimezoneId));
            billPayReceiptTrxTags.Append("|{SenderName}|" + receiptData.SenderName.ToUpper());
            billPayReceiptTrxTags.Append("|{SenderAddress}|" + receiptData.SenderAddress.ToUpper());
            billPayReceiptTrxTags.Append("|{SenderCity}|" + receiptData.SenderCity.ToUpper());
            billPayReceiptTrxTags.Append("|{SenderState}|" + receiptData.SenderState.ToUpper());
            billPayReceiptTrxTags.Append("|{SenderZip}|" + receiptData.SenderZip);
            billPayReceiptTrxTags.Append("|{SenderPhoneNumber}|" + (string.IsNullOrEmpty(receiptData.SenderPhoneNumber) ? removeLine : receiptData.SenderPhoneNumber));
            billPayReceiptTrxTags.Append("|{SenderMobileNumber}|" + (string.IsNullOrEmpty(receiptData.SenderMobileNumber) ? removeLine : receiptData.SenderMobileNumber));
            billPayReceiptTrxTags.Append("|{ReceiverName}|" + (string.IsNullOrEmpty(receiptData.ReceiverName) ? "" : receiptData.ReceiverName.ToUpper()));
            billPayReceiptTrxTags.Append("|{Account}|" + (string.IsNullOrEmpty(receiptData.AccountNumber) ? "" : receiptData.AccountNumber.Substring(receiptData.AccountNumber.Length - 4, 4)));
            billPayReceiptTrxTags.Append("|{TransferAmmount}|" + receiptData.TransferAmmount.ToString("#.00"));
            billPayReceiptTrxTags.Append("|{Amount}|" + receiptData.TransferAmmount.ToString("0.00"));
            billPayReceiptTrxTags.Append("|{ConfirmationId}|" + receiptData.ConfirmationId);
            billPayReceiptTrxTags.Append("|{TransactionId}|" + receiptData.TransactionId);
            billPayReceiptTrxTags.Append("|{TransferFee}|" + receiptData.UnDiscountedFee.ToString());
            billPayReceiptTrxTags.Append("|{CurrencyCode}|" + "USD");
            billPayReceiptTrxTags.Append("|{AdditionalFee}|" + "0.00");
            billPayReceiptTrxTags.Append("|{PrmDiscount}|" + receiptData.PrmDiscount.ToString());
            billPayReceiptTrxTags.Append("|{NetAmount}|" + (receiptData.TransferAmmount + receiptData.UnDiscountedFee - receiptData.PrmDiscount).ToString("#.00"));
            billPayReceiptTrxTags.Append("|{TransferTaxes}|" + "0.00");
            billPayReceiptTrxTags.Append("|{MTCN}|" + FormatMTCN(Convert.ToString(receiptData.MTCN)));
            billPayReceiptTrxTags.Append("|{GCNumber}|" + receiptData.GCNumber);
            billPayReceiptTrxTags.Append("|{ServiceType}|" + Convert.ToString(receiptData.DeliveryService).ToUpper());
            billPayReceiptTrxTags.Append("|{CardPoints}|" + Convert.ToString(receiptData.WuCardTotalPointsEarned));
            billPayReceiptTrxTags.Append("|{ReceiptDate}|" + receiptData.ReceiptDate.ToString("MMMM dd yyyy") + " / " + string.Format("{0} {1}", receiptData.ReceiptDate.ToString("hh:mm tt"), receiptData.TimezoneId));
            if (billPayReceiptTrxTags.ToString().Substring(0, 1) == "|")
                billPayReceiptTrxTags.Remove(0, 1).ToString();
            
            billPayReceiptTrxTags.Append("|{MessageArea}|" + receiptData.MessageArea.ToString());

            billPayReceiptTrxTags.Append(GetPartnerTags(receiptData));
            return billPayReceiptTrxTags.ToString();
        }

        private string GetMoneyTransferTrxTags(PTNRData.MoneyTransferReceiptData receiptData)
        {
            StringBuilder receiptBuilder = new StringBuilder();
            CultureInfo cultureinfo = new CultureInfo("es-ES");
            
            string timezoneFmt = receiptData.TimezoneId;

            receiptBuilder.Append("|{TxrDate}|" + string.Format(" {0}/ {1}", receiptData.TrxDateTime.ToString("MMMM dd yyyy"), cultureinfo.TextInfo.ToTitleCase(receiptData.TrxDateTime.ToString("MMMM dd yyyy", cultureinfo))).ToUpper());
            receiptBuilder.Append("|{TxrTime}|" + string.Format("{0} {1}", receiptData.TrxDateTime.ToString("hh:mm tt"), timezoneFmt));
            receiptBuilder.Append("|{TransactionId}|" + receiptData.TransactionId);
            receiptBuilder.Append("|{Date}|" + (receiptData.TrxDateTime.ToString("M/dd/yy").ToUpper()));
            receiptBuilder.Append("|{Time}|" + string.Format("{0} {1}", receiptData.TrxDateTime.ToString("hh:mm tt"), timezoneFmt));
            receiptBuilder.Append("|{SenderName}|" + receiptData.SenderName.ToUpper());
            receiptBuilder.Append("|{SenderAddress}|" + (receiptData.SenderAddress.ToUpper()));
            receiptBuilder.Append("|{SenderCity}|" + (receiptData.SenderCity.ToUpper()));
            receiptBuilder.Append("|{SenderState}|" + (receiptData.SenderState.ToUpper()));
            receiptBuilder.Append("|{SenderZip}|" + receiptData.SenderZip);
            receiptBuilder.Append("|{SenderPhoneNumber}|" + (string.IsNullOrEmpty(receiptData.SenderPhoneNumber) ? removeLine : receiptData.SenderPhoneNumber));
            receiptBuilder.Append("|{SenderMobileNumber}|" + (string.IsNullOrEmpty(receiptData.SenderMobileNumber) ? removeLine : receiptData.SenderMobileNumber));
            
            receiptBuilder.Append("|{ReceiverName}|" + receiptData.ReceiverName);
           if(receiptData.ReceiverAddress.Contains("#"))
            {
                receiptData.ReceiverAddress = receiptData.ReceiverAddress.Replace('#',' ');
            }
            receiptBuilder.Append("|{ReceiverAddress}|" + (receiptData.ReceiverAddress.ToUpper()));
            receiptBuilder.Append("|{ReceiverCity}|" + (receiptData.ReceiverCity.ToUpper()));
            receiptBuilder.Append("|{ReceiverState}|" + (receiptData.ReceiverState.ToUpper()));
            receiptBuilder.Append("|{ReceiverZip}|" + receiptData.ReceiverZip);
            receiptBuilder.Append("|{ReceiverPhoneNumber}|" + (string.IsNullOrWhiteSpace(receiptData.ReceiverPhoneNumber) ? removeLine : receiptData.ReceiverPhoneNumber));
            receiptBuilder.Append("|{ReceiverMobileNumber}|" + removeLine);
            receiptBuilder.Append("|{ReceiverCountry}|" + receiptData.ReceiverCountry);

            receiptBuilder.Append("|{PayoutCountry}|" + receiptData.PayoutCountry);
            receiptBuilder.Append("|{PayoutState}|" + (receiptData.PayoutState.ToUpper()));
            receiptBuilder.Append("|{TransferAmount}|" + receiptData.TransferAmount);
            receiptBuilder.Append("|{CurrencyCode}|" + receiptData.CurrencyCode);
            receiptBuilder.Append("|{TransferFee}|" + receiptData.TransferFee);
            receiptBuilder.Append("|{TransferTaxes}|" + receiptData.TransferTaxes.ToString("0.00"));
                        
            // Need to identify the logic to get PIN number and account balance
            string pinNumber = string.Empty;
            string accountBalance = string.Empty;
            receiptBuilder.Append("|{Pin}|" + (string.IsNullOrEmpty(pinNumber) ? removeLine : (string.Format("Your PIN is {0}", pinNumber))));
            receiptBuilder.Append("|{AccountBalance}|" + (string.IsNullOrEmpty(accountBalance) ? removeLine : (string.Format("Your account balance is ${0}", accountBalance))));

            receiptBuilder.Append("|{AdditionalFee}|" + receiptData.AdditionalCharges);
            receiptBuilder.Append("|{PromoCode}|" + (string.IsNullOrEmpty(receiptData.PromoCode) ? removeLine : receiptData.PromoCode));
            receiptBuilder.Append("|{PrmDiscount}|" + (receiptData.PromotionDiscount <= 0 ? removeLine : receiptData.PromotionDiscount.ToString()));
            receiptBuilder.Append("|{OtherFee}|" + receiptData.PaySideCharges.ToString("0.00"));
            receiptBuilder.Append("|{OtherFees}|" + receiptData.OtherCharges.ToString("0.00"));
            receiptBuilder.Append("|{ExchangeRate}|" + receiptData.ExchangeRate.ToString("0.0000"));
            receiptBuilder.Append("|{DstnTransferAmount}|" + receiptData.DstnTransferAmount);
            receiptBuilder.Append("|{DstnCurrencyCode}|" + receiptData.DstnCurrencyCode);
            receiptBuilder.Append("|{MTCN}|" + FormatMTCN(receiptData.MTCN));
            receiptBuilder.Append("|{GCNumber}|" + receiptData.GCNumber);
            receiptBuilder.Append("|{CardPoints}|" + receiptData.CardPoints);
            receiptBuilder.Append("|{PayoutCity}|" + (receiptData.PayoutCity.ToUpper()));
            receiptBuilder.Append("|{EstTransferAmount}|" + receiptData.EstTransferAmount);
            receiptBuilder.Append("|{EstOtherFee}|" + receiptData.PaySideCharges);
            receiptBuilder.Append("|{EstTotalToReceiver}|" + (receiptData.EstTotalToReceiver));

            string deliveryOption = receiptData.DeliveryOption;
            string deliveryOptionDesc = receiptData.DeliveryOptionDesc;
            string serviceType = string.IsNullOrEmpty(deliveryOptionDesc) ? receiptData.DeliveryServiceDesc : deliveryOptionDesc;
            string transalatedDeliveryServiceName = receiptData.TransalatedDeliveryServiceName;
            if (!string.IsNullOrWhiteSpace(transalatedDeliveryServiceName))
            {
                serviceType = string.Format("{0} / {1}", serviceType, transalatedDeliveryServiceName);
            }
            else if (string.IsNullOrWhiteSpace(serviceType))
            {
                serviceType = deliveryOption;
            }
            receiptBuilder.Append("|{ServiceType}|" + serviceType);


            if (receiptData.DTAvailableForPickup != DateTime.MinValue)
            {
                DateTime dtEstimatedDate = Convert.ToDateTime(receiptData.DTAvailableForPickup);
                receiptBuilder.Append("|{DateAvailableInReceiverCountry}|" + (dtEstimatedDate.ToString("MMMM dd yyyy").ToUpper() + string.Format(" / {0}", cultureinfo.TextInfo.ToTitleCase(dtEstimatedDate.ToString("MMMM dd yyyy", cultureinfo).ToUpper()))));
                receiptBuilder.Append("|{DateInReceiverCountry}|" + (dtEstimatedDate.ToString("M/dd/yy").ToUpper()));
            }
            else
            {
                receiptBuilder.Append("|{DateAvailableInReceiverCountry}|" + removeLine);
                receiptBuilder.Append("|{DateInReceiverCountry}|" + removeLine);
            }

            decimal netamt = 0;
            if (receiptData.ExchangeRate != 0)
            {
                netamt = receiptData.TransferAmount + receiptData.TransferFee + receiptData.Taxes + receiptData.OtherCharges + receiptData.AdditionalCharges - receiptData.PromotionDiscount;
            }

            receiptBuilder.Append("|{NetAmount}|" + netamt);

            decimal total = (receiptData.TransferFee * receiptData.ExchangeRate) -
                  ((receiptData.TransferFee * receiptData.ExchangeRate) + (receiptData.OtherCharges * receiptData.ExchangeRate) + (receiptData.Taxes * receiptData.ExchangeRate) - (receiptData.PromotionDiscount * receiptData.ExchangeRate));
            
            receiptBuilder.Append("|{TotalToReceiver}|" + (receiptData.TotalToReceiver));

            receiptBuilder.Append("|{MTCN}|" + FormatMTCN(receiptData.MTCN));

            string agencyName = receiptData.AgencyName;
            string phoneNumber = receiptData.PhoneNumber;
            string url = receiptData.Url;

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
            receiptBuilder.Append("|{TestQuestion}|" + (string.IsNullOrEmpty(receiptData.TestQuestion) ? removeLine : "YES / Si"));
            // MGI specific tags
            receiptBuilder.Append("|{TestAnswer}|" + (string.IsNullOrEmpty(receiptData.TestAnswer) ? removeLine : ""));
            receiptBuilder.Append("|{ReferenceNumber}|" + receiptData.MTCN);

            if (!string.IsNullOrWhiteSpace(receiptData.PersonalMessage))
            {
                List<string> messages = receiptData.PersonalMessage.Split(40).ToList();

                if (messages.Count > 0)
                {
                    string message1 = messages[0].Replace("&", "and");
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
            receiptBuilder.Append("|{MessageArea}|" + receiptData.MessageArea);

            decimal paySideTax = receiptData.PaySideTaxes;

            receiptBuilder.Append("|{PaySideTaxes}|" + (paySideTax == 0 ? removeLine : paySideTax.ToString(("0.00"))));
            receiptBuilder.Append("|{EstPaySideTaxes}|" + (paySideTax == 0 ? removeLine : paySideTax.ToString(("0.00"))));
            
            receiptBuilder.Append("|{ReceiptDate}|" + receiptData.TrxDateTime.ToString("MMMM dd yyyy") + " / " + string.Format("{0} {1}", receiptData.TrxDateTime.ToString("hh:mm tt"), timezoneFmt));
            if (receiptBuilder.ToString().Substring(0, 1) == "|")
                receiptBuilder.Remove(0, 1).ToString();

            receiptBuilder.Append(GetPartnerTags(receiptData));

            return receiptBuilder.ToString();
        }

        private string GetReceiveMoneyTrxTags(PTNRData.MoneyTransferReceiptData receiptData)
        {
            StringBuilder appendreceipttags = new StringBuilder();
            CultureInfo cultureinfo = new CultureInfo("es-ES");
            
            string timezoneFmt = receiptData.TimezoneId;
            appendreceipttags.Append("|{TxrDate}|" + (receiptData.TrxDateTime.ToString("MMMM dd yyyy").ToUpper() + string.Format(" / {0}", cultureinfo.TextInfo.ToTitleCase(receiptData.TrxDateTime.ToString("MMMM dd yyyy", cultureinfo).ToUpper()))));
            appendreceipttags.Append("|{TxrTime}|" + string.Format("{0} {1}", receiptData.TrxDateTime.ToString("hh:mm tt"), timezoneFmt));
            appendreceipttags.Append("|{Date}|" + (receiptData.TrxDateTime.ToString("M/dd/yy").ToUpper()));
            appendreceipttags.Append("|{Time}|" + string.Format("{0} {1}", receiptData.TrxDateTime.ToString("hh:mm tt"), timezoneFmt));
            appendreceipttags.Append("|{SenderName}|" + receiptData.SenderName.ToUpper());
            appendreceipttags.Append("|{ReceiverName}|" + receiptData.ReceiverName);
            appendreceipttags.Append("|{ReceiverAddress}|" + receiptData.ReceiverAddress);
            appendreceipttags.Append("|{ReceiverCity}|" + receiptData.ReceiverCity);
            appendreceipttags.Append("|{ReceiverState}|" + receiptData.ReceiverState);
            appendreceipttags.Append("|{ReceiverZip}|" + receiptData.ReceiverZip);
            appendreceipttags.Append("|{ReceiverPhoneNumber}|" + receiptData.ReceiverPhoneNumber);
            appendreceipttags.Append("|{ReceiverMobileNumber}|" + removeLine);
            appendreceipttags.Append("|{ReceiverDOB}|" + receiptData.ReceiverDOB);
            appendreceipttags.Append("|{ReceiverOccupation}|" + receiptData.ReceiverOccupation);
            appendreceipttags.Append("|{OriginatingCountry}|" + receiptData.OriginatingCountry);
            appendreceipttags.Append("|{ReceiveCurrency}|" + receiptData.DstnCurrencyCode);
            appendreceipttags.Append("|{Currency}|" + receiptData.CurrencyCode);
            appendreceipttags.Append("|{Taxes}|" + receiptData.Taxes);
            appendreceipttags.Append("|{TotalAmtPaid}|" + receiptData.TotalToReceiver);
            appendreceipttags.Append("|{Total}|" + receiptData.TotalToReceiver);
            appendreceipttags.Append("|{MTCN}|" + FormatMTCN(receiptData.MTCN));
            appendreceipttags.Append("|{ReferenceNumber}|" + receiptData.MTCN);
            string personalMessage = receiptData.PersonalMessage.Replace("&", "and");
            appendreceipttags.Append("|{PersonalMessage}|" + personalMessage);
            appendreceipttags.Append("|{ReceiptDate}|" + receiptData.TrxDateTime.ToString("MMMM dd yyyy") + " / " + string.Format("{0} {1}", receiptData.TrxDateTime.ToString("hh:mm tt"), timezoneFmt));
            appendreceipttags.Append("|{CurrencyCode}|" + "$");
            appendreceipttags.Append("|{MessageArea}|" + receiptData.MessageArea);
            appendreceipttags.Append("|{TransactionId}|" + receiptData.TransactionId);

            if (appendreceipttags.ToString().Substring(0, 1) == "|")
                appendreceipttags.Remove(0, 1).ToString();

            appendreceipttags.Append(GetPartnerTags(receiptData));
            return appendreceipttags.ToString();
        }
        #endregion
    }
}
