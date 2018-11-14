using System;
using System.Collections.Generic;
using TCF.Zeo.Biz.Receipt.Contract;
using commonData = TCF.Zeo.Common.Data;
using BizData = TCF.Zeo.Biz.Receipt.Data;
using CoreData = TCF.Zeo.Core;
using System.Text;
using PTNRData = TCF.Zeo.Core.Data;
using TCF.Zeo.Biz.Common.Data.Exceptions;
using TCF.Zeo.Common.Util;

namespace TCF.Zeo.Biz.Receipt.Impl
{
    public class BaseReceiptServiceImpl : IReceiptService
    {
        protected CoreData.Contract.IReceiptService _receiptServiceRepo = new CoreData.Impl.ReceiptServiceImpl();

        protected string removeLine = "{RemoveLine}";

        #region IReceiptService methods
        public virtual BizData.Receipt GetBillPayReceipt(long transactionId, bool isReprint, commonData.ZeoContext context)
        {
            throw new NotImplementedException();
        }


        public virtual BizData.Receipt GetCheckReceipt(long transactionId, bool isReprint, commonData.ZeoContext context)
        {
            try
            {
                CoreData.Data.ProcessCheckReceiptData receiptData = _receiptServiceRepo.GetCheckReceiptData(transactionId, context);
                BizData.Receipt receipt = GetCheckReceiptTemplate(receiptData.ClientName, receiptData.NumberOfCopies, Helper.TransactionType.ProcessCheck, Helper.ProviderId.Ingo, string.Empty);
                string receiptContentstags = GetCheckTrxTags(receiptData);
                if (!string.IsNullOrEmpty(receipt.PrintData))
                    receipt.PrintData = EncodeTransactionTags(receiptContentstags) + "|{data}|" + receipt.PrintData;

                return receipt;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new ReceiptException(ReceiptException.CHECK_RECEIPT_TEMPLATE_RETRIVEL_FAILED, ex);
            }
        }

        public virtual BizData.Receipt GetCouponReceipt(long customerSessionId, commonData.ZeoContext context)
        {
            try
            {
                CoreData.Data.CouponReceiptData receiptData = _receiptServiceRepo.GetCouponReceiptData(customerSessionId, context);
                BizData.Receipt receipt = GetCouponCodeReceiptTemplate(receiptData.ClientName);
                string receiptContentstags = GetCouponTrxTags(receiptData);

                if (!string.IsNullOrEmpty(receipt.PrintData))
                    receipt.PrintData = EncodeTransactionTags(receiptContentstags) + "|{data}|" + receipt.PrintData;

                return receipt;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new ReceiptException(ReceiptException.COUPON_RECEIPT_TEMPLATE_RETRIVEL_FAILED, ex);
            }
        }

        public virtual BizData.Receipt GetDoddFrankReceipt(long transactionId, commonData.ZeoContext context)
        {
            throw new NotImplementedException();
        }

        public virtual BizData.Receipt GetFundsReceipt(long transactionId, bool isReprint, commonData.ZeoContext context)
        {
            try
            {
                CoreData.Data.FundReceiptData receiptData = _receiptServiceRepo.GetFundReceiptData(transactionId, context);
                BizData.Receipt receipt = GetFundReceiptTemplate(receiptData.ClientName, (Helper.FundType)receiptData.FundType, Helper.ProviderId.Visa, string.Empty);
                string receiptContentstags = GetFundTrxTags(receiptData);

                if (!string.IsNullOrEmpty(receipt.PrintData))
                    receipt.PrintData = EncodeTransactionTags(receiptContentstags) + "|{data}|" + receipt.PrintData;

                return receipt;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new ReceiptException(ReceiptException.FUNDS_RECEIPT_TEMPLATE_RETRIVEL_FAILED, ex);
            }
        }

        public virtual BizData.Receipt GetMoneyOrderReceipt(long transactionId, bool isReprint, commonData.ZeoContext context)
        {
            try
            {
                PTNRData.MoneyOrderReceiptData receiptData = _receiptServiceRepo.GetMoneyOrderReceiptData(transactionId, context);
                BizData.Receipt receipt = GetMoneyOrderReceiptTemplate(receiptData.ClientName, Helper.TransactionType.MoneyOrder, Helper.ProviderId.Continental, string.Empty);

                string receiptContentstags = GetMoneyOrderTrxTags(receiptData);

                if (!string.IsNullOrEmpty(receipt.PrintData))
                    receipt.PrintData = EncodeTransactionTags(receiptContentstags) + "|{data}|" + receipt.PrintData;

                return receipt;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new ReceiptException(ReceiptException.MONEYORDER_RECEIPT_TEMPLATE_RETRIVEL_FAILED, ex);
            }
        }

        public virtual BizData.Receipt GetMoneyTransferReceipt(long transactionId, bool isReprint, commonData.ZeoContext context)
        {
            throw new NotImplementedException();
        }

        public virtual BizData.Receipt GetSummaryReceipt(long customerSessionId, commonData.ZeoContext context)
        {
            try
            {
                PTNRData.ShoppingCartSummeryReceiptData receiptData = _receiptServiceRepo.GetShoppingCartReceiptData(customerSessionId, context);
                BizData.Receipt receipt = GetShoppingCartSummaryReceiptTemplate(receiptData.ClientName, receiptData.Gpr, string.Empty);
                string receiptContentstags = GetShoppingCartSummeryTrxTags(receiptData);

                if (!string.IsNullOrEmpty(receipt.PrintData))
                    receipt.PrintData = EncodeTransactionTags(receiptContentstags) + "|{data}|" + receipt.PrintData;

                return receipt;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new ReceiptException(ReceiptException.SUMMARY_RECEIPT_TEMPLATE_RETRIVEL_FAILED, ex);
            }

        }
        public virtual BizData.CashDrawerReceipt GetCashDrawerReceipt(long agentId, long locationId, commonData.ZeoContext context)
        {
            try
            {
                PTNRData.CashDrawerReceiptData receiptData = _receiptServiceRepo.GetCashDrawerReceiptData(agentId, locationId, context);
                BizData.CashDrawerReceipt receipt = new BizData.CashDrawerReceipt();
                if (receiptData != null)
                {
                    receipt.CashIn = receiptData.CashIn;
                    receipt.CashOut = receiptData.CashOut;
                    receipt.ReportingDate = receiptData.ReportingDate;
                    receipt.ReportTemplate = GetCashDrawerReportTemplate(context);
                }
                return receipt;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new ReceiptException(ReceiptException.CASHDRAWERRECEIPT_FAILED, ex);
            }
        }

        private string GetCashDrawerReportTemplate(commonData.ZeoContext context, string partner = "")
        {

            string sreport = "CashDrawer";
            string receiptFileComplete = string.Format("{0}.{1}.Report.docx", partner, sreport);
            string receiptFileBase = string.Format("{0}.Report.docx", sreport);

            List<string> receiptfiles = new List<string>() { receiptFileComplete, receiptFileBase };

            return GetReceiptTemplates(receiptfiles);

        }
        protected string FormatMTCN(string mtcn)
        {
            return string.IsNullOrWhiteSpace(mtcn) ? string.Empty : mtcn.Insert(3, "-").Insert(7, "-");
        }
        #endregion

        #region Private Methods
        private Data.Receipt GetCouponCodeReceiptTemplate(string partner)
        {
            string type = "CouponCode";
            List<string> receiptfileNames = new List<string>();
            receiptfileNames.Add(string.Format("{0}.{1}.Receipt.docx", partner, type));
            return new Data.Receipt()
            {
                PrintData = GetReceiptTemplates(receiptfileNames),
                Name = "",
                NumberOfCopies = 1
            };
        }

        private Data.Receipt GetMoneyOrderReceiptTemplate(string partner, Helper.TransactionType product, Helper.ProviderId provider, string state)
        {
            List<string> receiptfiles = new List<string>();
            receiptfiles.Add(string.Format("{0}.{1}.{2}.{3}.Receipt.docx", partner, product.ToString(), provider.ToString(), state));
            receiptfiles.Add(string.Format("{0}.{1}.{2}.Receipt.docx", partner, product.ToString(), provider.ToString()));
            receiptfiles.Add(string.Format("{0}.{1}.Receipt.docx", partner, product.ToString()));
            return new Data.Receipt()
            {
                PrintData = GetReceiptTemplates(receiptfiles),
                Name = "",
                NumberOfCopies = 1
            };
        }

        private Data.Receipt GetFundReceiptTemplate(string partner, Helper.FundType fundType, Helper.ProviderId provider, string state)
        {
            List<string> receiptfiles = new List<string>();
            string fundtype = Enum.Parse(typeof(Helper.FundType), fundType.ToString()).ToString();

            if (fundtype == "Activation")
            {
                fundtype = "None";
            }

            receiptfiles.Add(string.Format("{0}.Funds.{1}.{2}.{3}.Receipt.docx", partner, fundtype, provider.ToString(), state));
            receiptfiles.Add(string.Format("{0}.Funds.{1}.{2}.Receipt.docx", partner, fundtype, provider.ToString()));
            receiptfiles.Add(string.Format("{0}.Funds.{1}.Receipt.docx", partner, fundtype));

            return new Data.Receipt()
            {
                PrintData = GetReceiptTemplates(receiptfiles),
                Name = "",
                NumberOfCopies = 1
            };
        }

        private Data.Receipt GetShoppingCartSummaryReceiptTemplate(string partner, string gpr, string state)
        {
            string scart = "ShoppingCartSummary";
            List<string> receiptfiles = new List<string>();
            receiptfiles.Add(string.Format("{0}.{1}.{2}.{3}.Receipt.docx", partner, scart, gpr, state));
            receiptfiles.Add(string.Format("{0}.{1}.{2}.Receipt.docx", partner, scart, gpr));
            receiptfiles.Add(string.Format("{0}.{1}.Receipt.docx", partner, scart));

            return new Data.Receipt()
            {
                PrintData = GetReceiptTemplates(receiptfiles),
                Name = "",
                NumberOfCopies = 1
            };
        }

        private Data.Receipt GetCheckReceiptTemplate(string partner, int NumberOfCopies, Helper.TransactionType checkType, Helper.ProviderId provider, string state)
        {
            List<string> receiptfiles = new List<string>();
            string checktype = Enum.Parse(typeof(Helper.TransactionType), checkType.ToString()).ToString();

            receiptfiles.Add(string.Format("{0}.{1}.{2}.{3}.Receipt.docx", partner, "Check", provider.ToString(), state));
            receiptfiles.Add(string.Format("{0}.{1}.{2}.Receipt.docx", partner, "Check", provider.ToString()));
            receiptfiles.Add(string.Format("{0}.{1}.Receipt.docx", partner, "Check"));

            return new Data.Receipt()
            {
                PrintData = GetReceiptTemplates(receiptfiles),
                Name = "",
                NumberOfCopies = 1
            };
        }

        public string GetReceiptTemplates(List<string> receiptNames)
        {
            string receiptTemplate = _receiptServiceRepo.GetReceiptTemplate(receiptNames);
            if (string.IsNullOrEmpty(receiptTemplate))
                throw new ReceiptException(ReceiptException.RECEIPT_TEMPLATE_NOT_FOUND);

            return receiptTemplate;
        }

        private string GetCouponTrxTags(CoreData.Data.CouponReceiptData receiptData)
        {
            StringBuilder couponReceiptTrxTags = new StringBuilder();

            couponReceiptTrxTags.Append("|{CouponCode}|" + receiptData.CustomerId.ToString().Substring(receiptData.CustomerId.ToString().Length - 8));
            couponReceiptTrxTags.Append("|{PromoName}|" + receiptData.PromoName);
            couponReceiptTrxTags.Append("|{PromoDescriptionOfReferee}|" + receiptData.PromoDescription);
            couponReceiptTrxTags.Append("|{PromoDescriptionOfReferrer}|" + receiptData.PromoDescription);
            couponReceiptTrxTags.Append(GetPartnerTags(receiptData));

            if (couponReceiptTrxTags.ToString().Substring(0, 1) == "|")
                couponReceiptTrxTags = couponReceiptTrxTags.Remove(0, 1);

            return couponReceiptTrxTags.ToString();
        }

        private string GetFundTrxTags(CoreData.Data.FundReceiptData receiptData)
        {
            StringBuilder receiptTrxTags = new StringBuilder();

            Helper.FundType trxType = (Helper.FundType)receiptData.FundType;
            decimal latestCardBalance = 0;
            decimal netamount = 0;

            switch (trxType)
            {
                case Helper.FundType.Debit:
                    latestCardBalance = (decimal)(receiptData.PreviousCardBalance - receiptData.Amount);
                    receiptTrxTags.Append("|{DiscountName}|" + (receiptData.DiscountApplied > 0 ? "(" + receiptData.DiscountName + " Withdraw Discount)" : removeLine));
                    netamount = receiptData.Amount;
                    break;
                case Helper.FundType.Credit:
                    latestCardBalance = (decimal)(receiptData.PreviousCardBalance + receiptData.Amount);
                    receiptTrxTags.Append("|{DiscountName}|" + (receiptData.DiscountApplied > 0 ? "(" + receiptData.DiscountName + " Load Discount)" : removeLine));
                    netamount = receiptData.Amount;
                    break;
                case Helper.FundType.Activation:
                    receiptTrxTags.Append("|{DiscountName}|" + (receiptData.DiscountApplied > 0 ? "(" + receiptData.DiscountName + " Activate Discount)" : removeLine));
                    netamount = receiptData.Amount + receiptData.Fee;
                    break;
            }
            receiptTrxTags.Append("|{PrevCardBalance}|" + Convert.ToDecimal(receiptData.PreviousCardBalance).ToString("0.00"));
            receiptTrxTags.Append("|{Amount}|" + receiptData.Amount.ToString("0.00"));
            receiptTrxTags.Append("|{CurrentBalance}|" + latestCardBalance.ToString("0.00"));

            if (trxType != Helper.FundType.Activation)
                receiptTrxTags.Append("|{Fee}|" + receiptData.Fee.ToString("0.00"));
            else
                receiptTrxTags.Append("|{ActivationFee}|" + receiptData.Fee.ToString("0.00"));

            receiptTrxTags.Append("|{Discount}|" + (receiptData.DiscountApplied > 0 ? (receiptData.DiscountApplied * -1).ToString("0.00") : removeLine));
            receiptTrxTags.Append("|{Fee}|" + receiptData.BaseFee.ToString("0.00"));
            receiptTrxTags.Append("|{NetAmount}|" + netamount.ToString("0.00"));
            receiptTrxTags.Append("|{CustomerName}|" + string.Format("{0}", receiptData.CustomerName));
            receiptTrxTags.Append("|{CardNumber}|" + (receiptData.CardNumber.Length > 4 ? receiptData.CardNumber.Substring(receiptData.CardNumber.Length - 4) : receiptData.CardNumber));
            receiptTrxTags.Append("|{ConfirmationNo}|" + receiptData.ConfirmationNo);
            receiptTrxTags.Append("|{CompanionName}|" + string.Format("{0}", receiptData.CompanionName));
            receiptTrxTags.Append("|{ActivationFee}|" + receiptData.Fee.ToString("0.00"));
            receiptTrxTags.Append("|{TransactionId}|" + receiptData.TransactionId);
            receiptTrxTags.Append(GetPartnerTags(receiptData));
            if (receiptTrxTags.ToString().Substring(0, 1) == "|")
                receiptTrxTags = receiptTrxTags.Remove(0, 1);

            return receiptTrxTags.ToString();
        }

        private string GetMoneyOrderTrxTags(CoreData.Data.MoneyOrderReceiptData receiptDatra)
        {
            StringBuilder transactionTags = new StringBuilder();
            transactionTags.Append("|{MONumber}|" + receiptDatra.MONumber);
            transactionTags.Append("|{Amount}|" + receiptDatra.Amount.ToString("0.00"));
            transactionTags.Append("|{Discount}|" + (Math.Abs(receiptDatra.Discount) > 0 ? Math.Abs(receiptDatra.Discount).ToString("0.00") : removeLine));
            transactionTags.Append("|{DiscountName}|" + (Math.Abs(receiptDatra.Discount) > 0 ? "(" + receiptDatra.DiscountName + " Check Discount)" : removeLine));
            transactionTags.Append("|{Fee}|" + receiptDatra.Fee.ToString("0.00"));
            transactionTags.Append("|{NetAmount}|" + (receiptDatra.NetAmount).ToString("0.00"));
            transactionTags.Append("|{TransactionId}|" + receiptDatra.TransactionId);
            transactionTags.Append(GetPartnerTags(receiptDatra));
            if (transactionTags.ToString().Substring(0, 1) == "|")
                transactionTags = transactionTags.Remove(0, 1);

            return transactionTags.ToString();
        }

        private string GetCheckTrxTags(CoreData.Data.ProcessCheckReceiptData receiptData)
        {
            StringBuilder transactionTags = new StringBuilder();
            transactionTags.Append("|{NetAmount}|" + (receiptData.NetAmount).ToString("0.00"));
            transactionTags.Append("|{ConfirmationNo}|" + AlloyUtil.TrimString(receiptData.ConfirmationNumber, 6));
            transactionTags.Append("|{CheckType}|" + AlloyUtil.TrimString(receiptData.ReturnType.ToString(), 20));
            transactionTags.Append("|{Discount}|" + (receiptData.Discount != 0 ? receiptData.Discount.ToString("0.00") : removeLine));
            transactionTags.Append("|{DiscountName}|" + (receiptData.Discount != 0 ? "(" + receiptData.DiscountName + " Check Discount)" : removeLine));
            transactionTags.Append("|{Fee}|" + receiptData.Fee.ToString("0.00"));
            transactionTags.Append("|{Amount}|" + receiptData.Amount.ToString("0.00"));
            transactionTags.Append("|{TransactionId}|" + receiptData.TransactionId);

            transactionTags.Append(GetPartnerTags(receiptData));
            if (transactionTags.ToString().Substring(0, 1) == "|")
                transactionTags = transactionTags.Remove(0, 1);

            return transactionTags.ToString();
        }

        protected string GetPartnerTags(CoreData.Data.BaseReceiptData receiptData)
        {
            StringBuilder appendreceipttags = new StringBuilder();
            appendreceipttags.Append("|{ClientName}|" + receiptData.ClientName);
            appendreceipttags.Append("|{LogoUrl}|" + receiptData.LogoUrl);
            appendreceipttags.Append("|{LocationAddress}|" + receiptData.LocationAddress);
            appendreceipttags.Append("|{City}|" + receiptData.LocationCity);
            appendreceipttags.Append("|{Phonenumber}|" + "");
            appendreceipttags.Append("|{State}|" + receiptData.LocationState);
            appendreceipttags.Append("|{Zip}|" + receiptData.LocationZip);
            appendreceipttags.Append("|{BranchId}|" + receiptData.BranchId);
            appendreceipttags.Append("|{BankId}|" + receiptData.BankId);
            appendreceipttags.Append("|{TerminalID}|" + receiptData.TerminalID);
            appendreceipttags.Append("|{TellerName}|" + receiptData.TellerName);
            appendreceipttags.Append("|{SessionlID}|" + receiptData.SessionlID);
            appendreceipttags.Append("|{TerminalDate}|" + receiptData.CustomerSessionDate.ToLongDateString());
            appendreceipttags.Append("|{ReceiptDate}|" + (receiptData.ReceiptDate.ToString("MMMM dd, yyyy")));
            appendreceipttags.Append("|{Currency}|" + "$");
            appendreceipttags.Append("|{TellerNumber}|" + receiptData.TellerNumber);
            appendreceipttags.Append("|{CustomerName}|" + receiptData.CustomerName);
            appendreceipttags.Append("|{LocationName}|" + receiptData.LocationName);
            appendreceipttags.Append("|{LocationPhoneNumber}|" + receiptData.LocationPhoneNumber);
            return appendreceipttags.ToString();
        }
        private string GetShoppingCartSummeryTrxTags(CoreData.Data.ShoppingCartSummeryReceiptData receiptDatra)
        {
            StringBuilder transactionTags = new StringBuilder();
            transactionTags.Append("|{CheckCount}|" + receiptDatra.CheckCount.ToString());
            transactionTags.Append("|{MOCount}|" + receiptDatra.MOCount.ToString());
            transactionTags.Append("|{BPCount}|" + receiptDatra.BPCount.ToString());
            transactionTags.Append("|{SMCount}|" + receiptDatra.SMCount.ToString());
            transactionTags.Append("|{RMCount}|" + receiptDatra.RMCount.ToString());
            transactionTags.Append("|{CheckTotal}|" + (receiptDatra.CheckTotal > 0 ? receiptDatra.CheckTotal.ToString("0.00") : removeLine));
            transactionTags.Append("|{GPRWithDraw}|" + (receiptDatra.GPRWithDraw > 0 ? receiptDatra.GPRWithDraw.ToString("0.00") : removeLine));
            transactionTags.Append("|{FundsGeneratingTotal}|" + receiptDatra.FundsGeneratingTotal.ToString("0.00"));
            transactionTags.Append("|{GPRLoad}|" + (receiptDatra.GPRLoad > 0 ? receiptDatra.GPRLoad.ToString("-0.00") : removeLine));
            transactionTags.Append("|{GPRActivate}|" + (receiptDatra.GPRActivate > 0 ? receiptDatra.GPRActivate.ToString("-0.00") : removeLine));
            transactionTags.Append("|{MoneyOrder}|" + (receiptDatra.MoneyOrder > 0 ? receiptDatra.MoneyOrder.ToString("-0.00") : removeLine));
            transactionTags.Append("|{FundsDepletingTotal}|" + receiptDatra.FundsDepletingTotal.ToString("-0.00"));
            transactionTags.Append("|{GPRCompanion}|" + (receiptDatra.GPRCompanion > 0 ? "0.00" : removeLine));
            transactionTags.Append("|{NetAmount}|" + Math.Abs(receiptDatra.NetAmount).ToString("0.00"));
            transactionTags.Append("|{TotalMsg}|" + (receiptDatra.NetAmount > 0 ? "TOTAL DUE TO CUSTOMER" : "TOTAL AMOUNT DUE "));
            transactionTags.Append("|{CashCollected}|" + receiptDatra.CashCollected.ToString("0.00"));
            transactionTags.Append("|{CashToCustomer}|" + receiptDatra.CashToCustomer.ToString("0.00"));
            transactionTags.Append("|{Currency}|" + "$");
            transactionTags.Append("|{CardNumber}|" + (receiptDatra.CardNumber.Length > 4 ? receiptDatra.CardNumber.Substring(receiptDatra.CardNumber.Length - 4) : receiptDatra.CardNumber));
            transactionTags.Append("|{MoneyTransferSend}|" + (receiptDatra.MoneyTransferSend > 0 ? receiptDatra.MoneyTransferSend.ToString("-0.00") : removeLine));
            transactionTags.Append("|{BillPay}|" + (receiptDatra.BillPay > 0 ? receiptDatra.BillPay.ToString("-0.00") : removeLine));
            transactionTags.Append("|{MoneyTransferReceive}|" + (receiptDatra.MoneyTransferReceive > 0 ? receiptDatra.MoneyTransferReceive.ToString("0.00") : removeLine));
            transactionTags.Append("|{MoneyTransferModified}|" + (receiptDatra.MoneyTransferModified > 0 ? receiptDatra.MoneyTransferModified.ToString("-0.00") : removeLine));
            transactionTags.Append("|{MoneyTransferCancelled}|" + (receiptDatra.MoneyTransferCancelled > 0 ? receiptDatra.MoneyTransferCancelled.ToString("0.00") : removeLine));
            transactionTags.Append("|{MoneyTransferRefund}|" + (receiptDatra.MoneyTransferRefund > 0 ? receiptDatra.MoneyTransferRefund.ToString("0.00") : removeLine));
            transactionTags.Append(GetPartnerTags(receiptDatra));
            if (transactionTags.ToString().Substring(0, 1) == "|")
                transactionTags = transactionTags.Remove(0, 1);

            return transactionTags.ToString();
        }


        protected string EncodeTransactionTags(string trxData)
        {
             return Convert.ToBase64String(System.Text.Encoding.Unicode.GetBytes(trxData));
        }
        #endregion
    }
}
