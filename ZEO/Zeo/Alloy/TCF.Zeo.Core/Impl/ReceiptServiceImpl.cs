using TCF.Zeo.Common.Data;
using TCF.Zeo.Core.Contract;
using TCF.Zeo.Core.Data;
using TCF.Zeo.Core.Data.Exceptions;
using P3Net.Data;
using P3Net.Data.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TCF.Zeo.Core.Impl
{
    public class ReceiptServiceImpl : IReceiptService
    {
        #region IReceiptService methods
        public BillpayReceiptData GetBillpayReceiptData(long transactionId, string transactionType, int provider, bool isReprint, ZeoContext context)
        {
            try
            {
                BillpayReceiptData receiptData = null;

                StoredProcedure spGetBillpayReceiptData = new StoredProcedure("usp_GetBillpayReceiptData");
                spGetBillpayReceiptData.WithParameters(InputParameter.Named("TransactionID").WithValue(transactionId));
                spGetBillpayReceiptData.WithParameters(InputParameter.Named("TransactionType").WithValue(transactionType));
                spGetBillpayReceiptData.WithParameters(InputParameter.Named("Provider").WithValue(provider));
                spGetBillpayReceiptData.WithParameters(InputParameter.Named("IsReprint").WithValue(isReprint));

                using (IDataReader datareader = DataHelper.GetConnectionManager().ExecuteReader(spGetBillpayReceiptData))
                {
                    while (datareader.Read())
                    {
                        receiptData = new BillpayReceiptData();
                        receiptData.AccountNumber = datareader.GetStringOrDefault("AccountNumber");
                        receiptData.BankId = datareader.GetStringOrDefault("BankId");
                        receiptData.BranchId = datareader.GetStringOrDefault("BranchId");
                        receiptData.LocationCity = datareader.GetStringOrDefault("LocationCity"); ;
                        receiptData.ClientName = datareader.GetStringOrDefault("ClientName");
                        receiptData.ConfirmationId = datareader.GetStringOrDefault("ConfirmationId");
                        receiptData.CustomerName = datareader.GetStringOrDefault("CustomerName");
                        receiptData.DeliveryService = datareader.GetStringOrDefault("DeliveryService");
                        receiptData.GCNumber = datareader.GetStringOrDefault("GCNumber");
                        receiptData.LocationAddress = datareader.GetStringOrDefault("LocationAddress");
                        receiptData.LocationName = datareader.GetStringOrDefault("LocationName");
                        receiptData.LocationPhoneNumber = datareader.GetStringOrDefault("LocationPhoneNumber");
                        receiptData.Timezone = datareader.GetStringOrDefault("Timezone");
                        receiptData.TimezoneId = datareader.GetStringOrDefault("TimezoneId");
                        receiptData.LogoUrl = datareader.GetStringOrDefault("LogoUrl");
                        receiptData.MessageArea = datareader.GetStringOrDefault("MessageArea");
                        receiptData.MTCN = datareader.GetStringOrDefault("MTCN");
                        receiptData.NetAmount = datareader.GetDecimalOrDefault("NetAmount");
                        receiptData.PrmDiscount = datareader.GetDecimalOrDefault("PrmDiscount");
                        receiptData.ReceiptDate = datareader.GetDateTimeOrDefault("ReceiptDate");
                        receiptData.ReceiverName = datareader.GetStringOrDefault("ReceiverName");
                        receiptData.SenderName = datareader.GetStringOrDefault("SenderName");
                        receiptData.SenderAddress = datareader.GetStringOrDefault("SenderAddress");
                        receiptData.SenderCity = datareader.GetStringOrDefault("SenderCity");
                        receiptData.SenderState = datareader.GetStringOrDefault("SenderState");
                        receiptData.SenderZip = datareader.GetStringOrDefault("SenderZip");
                        receiptData.SenderMobileNumber = datareader.GetStringOrDefault("SenderMobileNumber");
                        receiptData.SenderPhoneNumber = datareader.GetStringOrDefault("SenderPhoneNumber");
                        receiptData.SessionlID = datareader.GetInt64OrDefault("SessionlID");
                        receiptData.CustomerSessionDate = datareader.GetDateTimeOrDefault("CustomerSessionDate");
                        receiptData.LocationState = datareader.GetStringOrDefault("LocationState");
                        receiptData.TellerName = datareader.GetStringOrDefault("TellerName");
                        receiptData.TellerNumber = datareader.GetStringOrDefault("TellerNumber");
                        receiptData.TerminalID = datareader.GetStringOrDefault("TerminalID");
                        receiptData.TransactionId = datareader.GetInt64OrDefault("TransactionId");
                        receiptData.TransferAmmount = datareader.GetDecimalOrDefault("TransferAmmount");
                        receiptData.TxrDate = datareader.GetDateTimeOrDefault("TxrDate");
                        receiptData.UnDiscountedFee = datareader.GetDecimalOrDefault("UnDiscountedFee");
                        receiptData.WuCardTotalPointsEarned = datareader.GetStringOrDefault("WuCardTotalPointsEarned");
                        receiptData.LocationZip = datareader.GetStringOrDefault("LocationZip");
                        receiptData.NumberOfCopies = datareader.GetInt32OrDefault("NoOfCopies");
                    }
                }

                return receiptData;
            }
            catch (Exception ex)
            {
                throw new ReceiptException(ReceiptException.GET_BILLPAY_RECEIPT_DATA_FAILED, ex);
            }
        }
        public ProcessCheckReceiptData GetCheckReceiptData(long transactionId, ZeoContext context)
        {
            try
            {
                ProcessCheckReceiptData receiptData = null;

                StoredProcedure spGetProcessCheckReceiptData = new StoredProcedure("usp_GetProcessCheckReceiptData");
                spGetProcessCheckReceiptData.WithParameters(InputParameter.Named("TransactionID").WithValue(transactionId));

                using (IDataReader datareader = DataHelper.GetConnectionManager().ExecuteReader(spGetProcessCheckReceiptData))
                {
                    while (datareader.Read())
                    {
                        receiptData = new ProcessCheckReceiptData();
                        receiptData.Amount = datareader.GetDecimalOrDefault("Amount");
                        receiptData.BankId = datareader.GetStringOrDefault("BankId");
                        receiptData.BranchId = datareader.GetStringOrDefault("BranchId");
                        receiptData.ClientName = datareader.GetStringOrDefault("ClientName"); ;
                        receiptData.ConfirmationNumber = datareader.GetStringOrDefault("ConfirmationNumber");
                        receiptData.CustomerName = datareader.GetStringOrDefault("CustomerName");
                        receiptData.CustomerSessionDate = datareader.GetDateTimeOrDefault("CustomerSessionDate");
                        receiptData.Discount = datareader.GetDecimalOrDefault("Discount");
                        receiptData.DiscountName = datareader.GetStringOrDefault("DiscountName");
                        receiptData.Fee = datareader.GetDecimalOrDefault("Fee");
                        receiptData.LocationAddress = datareader.GetStringOrDefault("LocationAddress");
                        receiptData.LocationCity = datareader.GetStringOrDefault("LocationCity");
                        receiptData.LocationName = datareader.GetStringOrDefault("LocationName");
                        receiptData.LocationPhoneNumber = datareader.GetStringOrDefault("LocationPhoneNumber");
                        receiptData.LocationState = datareader.GetStringOrDefault("LocationState");
                        receiptData.LocationZip = datareader.GetStringOrDefault("LocationZip");
                        receiptData.LogoUrl = datareader.GetStringOrDefault("LogoUrl");
                        receiptData.NetAmount = datareader.GetDecimalOrDefault("NetAmount");
                        receiptData.ReceiptDate = datareader.GetDateTimeOrDefault("ReceiptDate");
                        receiptData.ReturnType = datareader.GetStringOrDefault("ReturnType");
                        receiptData.SessionlID = datareader.GetInt64OrDefault("SessionlID");
                        receiptData.TellerName = datareader.GetStringOrDefault("TellerName");
                        receiptData.TellerNumber = datareader.GetStringOrDefault("TellerNumber");
                        receiptData.TerminalID = datareader.GetStringOrDefault("TerminalID");
                        receiptData.TransactionId = datareader.GetInt64OrDefault("TransactionId");

                    }
                }

                return receiptData;
            }
            catch (Exception ex)
            {
                throw new ReceiptException(ReceiptException.GET_CHECK_RECEIPT_DATA_FAILED, ex);
            }
        }
        public CouponReceiptData GetCouponReceiptData(long customerSessionId, ZeoContext context)
        {
            try
            {
                CouponReceiptData receiptData = null;

                StoredProcedure spGetCouponReceiptData = new StoredProcedure("usp_GetCouponReceiptData");
                spGetCouponReceiptData.WithParameters(InputParameter.Named("CustomerSessionId").WithValue(customerSessionId));

                using (IDataReader datareader = DataHelper.GetConnectionManager().ExecuteReader(spGetCouponReceiptData))
                {
                    while (datareader.Read())
                    {
                        receiptData = new CouponReceiptData();
                        receiptData.BankId = datareader.GetStringOrDefault("BankId");
                        receiptData.BranchId = datareader.GetStringOrDefault("BranchId");
                        receiptData.ClientName = datareader.GetStringOrDefault("ClientName"); ;
                        receiptData.CustomerId = datareader.GetInt64OrDefault("CustomerId");
                        receiptData.CustomerName = datareader.GetStringOrDefault("CustomerName");
                        receiptData.CustomerSessionDate = datareader.GetDateTimeOrDefault("CustomerSessionDate");
                        receiptData.LocationAddress = datareader.GetStringOrDefault("LocationAddress");
                        receiptData.LocationCity = datareader.GetStringOrDefault("LocationCity");
                        receiptData.LocationName = datareader.GetStringOrDefault("LocationName");
                        receiptData.LocationPhoneNumber = datareader.GetStringOrDefault("LocationPhoneNumber");
                        receiptData.LocationState = datareader.GetStringOrDefault("LocationState");
                        receiptData.LocationZip = datareader.GetStringOrDefault("LocationZip");
                        receiptData.LogoUrl = datareader.GetStringOrDefault("LogoUrl");
                        receiptData.PromoDescription = datareader.GetStringOrDefault("PromoDescription");
                        receiptData.PromoName = datareader.GetStringOrDefault("PromoName");
                        receiptData.ReceiptDate = datareader.GetDateTimeOrDefault("ReceiptDate");
                        receiptData.SessionlID = datareader.GetInt64OrDefault("SessionlID");
                        receiptData.TellerName = datareader.GetStringOrDefault("TellerName");
                        receiptData.TellerNumber = datareader.GetStringOrDefault("TellerNumber");
                        receiptData.TerminalID = datareader.GetStringOrDefault("TerminalID");
                    }
                }

                return receiptData;
            }
            catch (Exception ex)
            {
                throw new ReceiptException(ReceiptException.GET_COUPON_RECEIPT_DATA_FAILED, ex);
            }
        }
        public FundReceiptData GetFundReceiptData(long transactionId, ZeoContext context)
        {
            try
            {
                FundReceiptData receiptData = null;

                StoredProcedure spGetCouponReceiptData = new StoredProcedure("usp_GetFundReceiptData");
                spGetCouponReceiptData.WithParameters(InputParameter.Named("transactionID").WithValue(transactionId));

                using (IDataReader datareader = DataHelper.GetConnectionManager().ExecuteReader(spGetCouponReceiptData))
                {
                    while (datareader.Read())
                    {
                        receiptData = new FundReceiptData();
                        receiptData.Amount = datareader.GetDecimalOrDefault("Amount");
                        receiptData.BankId = datareader.GetStringOrDefault("BankId");
                        receiptData.BaseFee = datareader.GetDecimalOrDefault("BaseFee");
                        receiptData.BranchId = datareader.GetStringOrDefault("BranchId");
                        receiptData.CardNumber = datareader.GetStringOrDefault("CardNumber");
                        receiptData.ClientName = datareader.GetStringOrDefault("ClientName");
                        receiptData.ConfirmationNo = datareader.GetStringOrDefault("ConfirmationNo");
                        receiptData.CustomerName = datareader.GetStringOrDefault("CustomerName");
                        receiptData.CustomerSessionDate = datareader.GetDateTimeOrDefault("CustomerSessionDate");
                        receiptData.DiscountApplied = datareader.GetDecimalOrDefault("DiscountApplied");
                        receiptData.Fee = datareader.GetDecimalOrDefault("Fee");
                        receiptData.FundType = datareader.GetInt32OrDefault("FundType");
                        receiptData.LocationAddress = datareader.GetStringOrDefault("LocationAddress");
                        receiptData.LocationCity = datareader.GetStringOrDefault("LocationCity");
                        receiptData.LocationName = datareader.GetStringOrDefault("LocationName");
                        receiptData.LocationPhoneNumber = datareader.GetStringOrDefault("LocationPhoneNumber");
                        receiptData.LocationState = datareader.GetStringOrDefault("LocationState");
                        receiptData.LocationZip = datareader.GetStringOrDefault("LocationZip");
                        receiptData.LogoUrl = datareader.GetStringOrDefault("LogoUrl");
                        receiptData.PreviousCardBalance = datareader.GetDecimalOrDefault("PreviousCardBalance");
                        receiptData.ReceiptDate = datareader.GetDateTimeOrDefault("ReceiptDate");
                        receiptData.SessionlID = datareader.GetInt64OrDefault("SessionlID");
                        receiptData.TellerName = datareader.GetStringOrDefault("TellerName");
                        receiptData.TellerNumber = datareader.GetStringOrDefault("TellerNumber");
                        receiptData.TerminalID = datareader.GetStringOrDefault("TerminalID");
                        receiptData.Timezone = datareader.GetStringOrDefault("Timezone");
                        receiptData.TimezoneId = datareader.GetStringOrDefault("TimezoneId");
                        receiptData.DiscountName = datareader.GetStringOrDefault("DiscountName");
                        receiptData.CompanionName = datareader.GetStringOrDefault("CompanionName");
                        receiptData.TransactionId = datareader.GetInt64OrDefault("TransactionId");
                    }
                }

                return receiptData;
            }
            catch (Exception ex)
            {
                throw new ReceiptException(ReceiptException.GET_FUND_RECEIPT_DATA_FAILED, ex);
            }
        }
        public MoneyOrderReceiptData GetMoneyOrderReceiptData(long transactionId, ZeoContext context)
        {
            try
            {
                MoneyOrderReceiptData receiptData = null;

                StoredProcedure spGetCouponReceiptData = new StoredProcedure("usp_GetMoneyOrderReceiptData");
                spGetCouponReceiptData.WithParameters(InputParameter.Named("TransactionID").WithValue(transactionId));

                using (IDataReader datareader = DataHelper.GetConnectionManager().ExecuteReader(spGetCouponReceiptData))
                {
                    while (datareader.Read())
                    {
                        receiptData = new MoneyOrderReceiptData();
                        receiptData.Amount = datareader.GetDecimalOrDefault("Amount");
                        receiptData.BankId = datareader.GetStringOrDefault("BankId");
                        receiptData.BranchId = datareader.GetStringOrDefault("BranchId");
                        receiptData.ClientName = datareader.GetStringOrDefault("ClientName");
                        receiptData.CustomerName = datareader.GetStringOrDefault("CustomerName");
                        receiptData.CustomerSessionDate = datareader.GetDateTimeOrDefault("CustomerSessionDate");
                        receiptData.Discount = datareader.GetDecimalOrDefault("Discount");
                        receiptData.DiscountName = datareader.GetStringOrDefault("DiscountName");
                        receiptData.Fee = datareader.GetDecimalOrDefault("Fee");
                        receiptData.LocationAddress = datareader.GetStringOrDefault("LocationAddress");
                        receiptData.LocationCity = datareader.GetStringOrDefault("LocationCity");
                        receiptData.LocationName = datareader.GetStringOrDefault("LocationName");
                        receiptData.LocationPhoneNumber = datareader.GetStringOrDefault("LocationPhoneNumber");
                        receiptData.LocationState = datareader.GetStringOrDefault("LocationState");
                        receiptData.LocationZip = datareader.GetStringOrDefault("LocationZip");
                        receiptData.LogoUrl = datareader.GetStringOrDefault("LogoUrl");
                        receiptData.MONumber = datareader.GetStringOrDefault("MONumber");
                        receiptData.NetAmount = datareader.GetDecimalOrDefault("NetAmount");
                        receiptData.ReceiptDate = datareader.GetDateTimeOrDefault("ReceiptDate");
                        receiptData.SessionlID = datareader.GetInt64OrDefault("SessionlID");
                        receiptData.TellerName = datareader.GetStringOrDefault("TellerName");
                        receiptData.TellerNumber = datareader.GetStringOrDefault("TellerNumber");
                        receiptData.TerminalID = datareader.GetStringOrDefault("TerminalID");
                        receiptData.TransactionId = datareader.GetInt64OrDefault("TransactionId");
                    }
                }

                return receiptData;
            }
            catch (Exception ex)
            {
                throw new ReceiptException(ReceiptException.GET_MONEYORDER_RECEIPT_DATA_FAILED, ex);
            }
        }
        public MoneyTransferReceiptData GetMoneyTransferReceiptData(long transactionId, string transactionType, int provider, ZeoContext context, bool? isReprint = null)
        {
            try
            {
                MoneyTransferReceiptData receiptData = null;

                StoredProcedure spGetBillpayReceiptData = new StoredProcedure("usp_GetMoneyTransferReceiptData");
                spGetBillpayReceiptData.WithParameters(InputParameter.Named("TransactionID").WithValue(transactionId));
                spGetBillpayReceiptData.WithParameters(InputParameter.Named("TransactionType").WithValue(transactionType));
                spGetBillpayReceiptData.WithParameters(InputParameter.Named("Provider").WithValue(provider));
                spGetBillpayReceiptData.WithParameters(InputParameter.Named("IsReprint").WithValue(isReprint));


                using (IDataReader datareader = DataHelper.GetConnectionManager().ExecuteReader(spGetBillpayReceiptData))
                {
                    while (datareader.Read())
                    {
                        receiptData = new MoneyTransferReceiptData();
                        receiptData.AgencyName = datareader.GetStringOrDefault("AgencyName");
                        receiptData.BankId = datareader.GetStringOrDefault("BankId");
                        receiptData.BranchId = datareader.GetStringOrDefault("BranchId");
                        receiptData.CardPoints = datareader.GetStringOrDefault("CardPoints"); ;
                        receiptData.ClientName = datareader.GetStringOrDefault("ClientName");
                        receiptData.CurrencyCode = datareader.GetStringOrDefault("CurrencyCode");
                        receiptData.CustomerName = datareader.GetStringOrDefault("CustomerName");
                        receiptData.CustomerSessionDate = datareader.GetDateTimeOrDefault("CustomerSessionDate");
                        receiptData.DeliveryOption = datareader.GetStringOrDefault("DeliveryOption");
                        receiptData.DeliveryOptionDesc = datareader.GetStringOrDefault("DeliveryOptionDesc");
                        receiptData.DeliveryServiceDesc = datareader.GetStringOrDefault("DeliveryServiceDesc");
                        receiptData.DestinationCountryCode = datareader.GetStringOrDefault("DestinationCountryCode");
                        receiptData.DstnCurrencyCode = datareader.GetStringOrDefault("DstnCurrencyCode");
                        receiptData.DstnTransferAmount = datareader.GetDecimalOrDefault("DstnTransferAmount");
                        receiptData.DTAvailableForPickup = datareader.GetDateTimeOrDefault("DTAvailableForPickup");
                        receiptData.EstOtherFee = datareader.GetDecimalOrDefault("EstOtherFee");
                        receiptData.EstTotalToReceiver = datareader.GetDecimalOrDefault("EstTotalToReceiver");
                        receiptData.EstTransferAmount = datareader.GetDecimalOrDefault("EstTransferAmount");
                        receiptData.ExchangeRate = datareader.GetDecimalOrDefault("ExchangeRate");
                        receiptData.GCNumber = datareader.GetStringOrDefault("GCNumber");
                        receiptData.LocationAddress = datareader.GetStringOrDefault("LocationAddress");
                        receiptData.LocationCity = datareader.GetStringOrDefault("LocationCity");
                        receiptData.LocationName = datareader.GetStringOrDefault("LocationName");
                        receiptData.LocationPhoneNumber = datareader.GetStringOrDefault("LocationPhoneNumber");
                        receiptData.LocationState = datareader.GetStringOrDefault("LocationState");
                        receiptData.LocationZip = datareader.GetStringOrDefault("LocationZip");
                        receiptData.LogoUrl = datareader.GetStringOrDefault("LogoUrl");
                        receiptData.MessageArea = datareader.GetStringOrDefault("MessageArea");
                        receiptData.MTCN = datareader.GetStringOrDefault("MTCN");
                        receiptData.OriginatingCountry = datareader.GetStringOrDefault("OriginatingCountry");
                        receiptData.OtherCharges = datareader.GetDecimalOrDefault("OtherCharges");
                        receiptData.PayoutCity = datareader.GetStringOrDefault("PayoutCity");
                        receiptData.PayoutState = datareader.GetStringOrDefault("PayoutState");
                        receiptData.PaySideTaxes = datareader.GetDecimalOrDefault("PaySideTaxes");
                        receiptData.PersonalMessage = datareader.GetStringOrDefault("PersonalMessage");
                        receiptData.PhoneNumber = datareader.GetStringOrDefault("PhoneNumber");
                        receiptData.ReceiptDate = datareader.GetDateTimeOrDefault("ReceiptDate");
                        receiptData.ReceiverAddress = datareader.GetStringOrDefault("ReceiverAddress");
                        receiptData.ReceiverCity = datareader.GetStringOrDefault("ReceiverCity");
                        receiptData.ReceiverCountry = datareader.GetStringOrDefault("ReceiverCountry");
                        receiptData.PayoutCountry = datareader.GetStringOrDefault("PayoutCountry");
                        receiptData.ReceiverDOB = datareader.GetStringOrDefault("ReceiverDOB");
                        receiptData.ReceiverName = datareader.GetStringOrDefault("ReceiverName");
                        receiptData.ReceiverOccupation = datareader.GetStringOrDefault("ReceiverOccupation");
                        receiptData.ReceiverPhoneNumber = datareader.GetStringOrDefault("ReceiverPhoneNumber");
                        receiptData.ReceiverState = datareader.GetStringOrDefault("ReceiverState");
                        receiptData.ReceiverZip = datareader.GetStringOrDefault("ReceiverZip");
                        receiptData.SenderAddress = datareader.GetStringOrDefault("SenderAddress");
                        receiptData.SenderCity = datareader.GetStringOrDefault("SenderCity");
                        receiptData.SenderMobileNumber = datareader.GetStringOrDefault("SenderMobileNumber");
                        receiptData.SenderName = datareader.GetStringOrDefault("SenderName");
                        receiptData.SenderPhoneNumber = datareader.GetStringOrDefault("SenderPhoneNumber");
                        receiptData.SenderState = datareader.GetStringOrDefault("SenderState");
                        receiptData.SenderZip = datareader.GetStringOrDefault("SenderZip");
                        receiptData.SessionlID = datareader.GetInt64OrDefault("SessionlID");
                        receiptData.Taxes = datareader.GetDecimalOrDefault("Taxes");
                        receiptData.TellerName = datareader.GetStringOrDefault("TellerName");
                        receiptData.TellerNumber = datareader.GetStringOrDefault("TellerNumber");
                        receiptData.TerminalID = datareader.GetStringOrDefault("TerminalID");
                        receiptData.TestAnswer = datareader.GetStringOrDefault("TestAnswer");
                        receiptData.TestQuestion = datareader.GetStringOrDefault("TestQuestion");
                        receiptData.Timezone = datareader.GetStringOrDefault("Timezone");
                        receiptData.TimezoneId = datareader.GetStringOrDefault("TimezoneId");
                        receiptData.TotalToReceiver = datareader.GetDecimalOrDefault("TotalToReceiver");
                        receiptData.TransalatedDeliveryServiceName = datareader.GetStringOrDefault("TransalatedDeliveryServiceName");
                        receiptData.TransferAmount = datareader.GetDecimalOrDefault("TransferAmount");
                        receiptData.TransferFee = datareader.GetDecimalOrDefault("TransferFee");
                        receiptData.TransferTaxes = datareader.GetDecimalOrDefault("TransferTaxes");
                        receiptData.TrxDateTime = datareader.GetDateTimeOrDefault("TrxDateTime");
                        receiptData.Url = datareader.GetStringOrDefault("Url");
                        receiptData.AdditionalCharges = datareader.GetDecimalOrDefault("AdditionalCharges");
                        receiptData.MessageCharge = datareader.GetDecimalOrDefault("MessageCharge");
                        receiptData.PaySideCharges = datareader.GetDecimalOrDefault("PaySideCharges");
                        receiptData.IsFixOnSend = datareader.GetBooleanOrDefault("IsFixOnSend");
                        receiptData.PlusChargesAmount = datareader.GetDecimalOrDefault("PlusChargesAmount");
                        receiptData.FilingDate = datareader.GetStringOrDefault("FilingDate");
                        receiptData.FilingTime = datareader.GetStringOrDefault("FilingTime");
                        receiptData.PaidDateTime = datareader.GetStringOrDefault("PaidDateTime");
                        receiptData.ExpectedPayoutCity = datareader.GetStringOrDefault("ExpectedPayoutCity");
                        receiptData.PromoCode = datareader.GetStringOrDefault("PromoCode");
                        receiptData.PromotionDiscount = datareader.GetDecimalOrDefault("PromotionDiscount");
                        receiptData.IsDomesticTransfer = datareader.GetBooleanOrDefault("IsDomesticTransfer");
                        receiptData.TransactionId = datareader.GetInt64OrDefault("TransactionId");
                        receiptData.TransactionSubType = datareader.GetStringOrDefault("TransactionSubType");
                        receiptData.TranascationType = datareader.GetStringOrDefault("TranascationType");
                        receiptData.NumberOfCopies = datareader.GetInt32OrDefault("NoOfCopies");
                    }
                }

                return receiptData;
            }
            catch (Exception ex)
            {
                throw new ReceiptException(ReceiptException.GET_MONEYTRANSFER_RECEIPT_DATA_FAILED, ex);
            }
        }
        public ShoppingCartSummeryReceiptData GetShoppingCartReceiptData(long customerSessionId, ZeoContext context)
        {
            try
            {
                ShoppingCartSummeryReceiptData receiptData = null;
                StoredProcedure spGetSummaryReceiptData = new StoredProcedure("usp_GetSummaryReceiptData");
                spGetSummaryReceiptData.WithParameters(InputParameter.Named("CustomerSessionId").WithValue(customerSessionId));

                using (IDataReader datareader = DataHelper.GetConnectionManager().ExecuteReader(spGetSummaryReceiptData))
                {
                    while (datareader.Read())
                    {
                        receiptData = new ShoppingCartSummeryReceiptData();
                        receiptData.CheckCount = datareader.GetInt32OrDefault("CheckCount");
                        receiptData.MOCount = datareader.GetInt32OrDefault("MOCount");
                        receiptData.BPCount = datareader.GetInt32OrDefault("BPCount");
                        receiptData.SMCount = datareader.GetInt32OrDefault("SMCount");
                        receiptData.RMCount = datareader.GetInt32OrDefault("RMCount");
                        receiptData.CheckTotal = datareader.GetDecimalOrDefault("CheckTotal");
                        receiptData.MoneyOrder = datareader.GetDecimalOrDefault("MoneyOrderTotal");
                        receiptData.BillPay = datareader.GetDecimalOrDefault("BillpayTotal");
                        receiptData.CashCollected = datareader.GetDecimalOrDefault("CashCollected");
                        receiptData.CashToCustomer = datareader.GetDecimalOrDefault("CashToCustomer");
                        receiptData.FundsGeneratingTotal = datareader.GetDecimalOrDefault("FundsGeneratingTotal");
                        receiptData.FundsDepletingTotal = datareader.GetDecimalOrDefault("FundsDepletingTotal");
                        receiptData.GPRWithDraw = datareader.GetDecimalOrDefault("GPRWithDraw");
                        receiptData.GPRLoad = datareader.GetDecimalOrDefault("GPRLoad");
                        receiptData.GPRActivate = datareader.GetDecimalOrDefault("GPRActivate");
                        receiptData.MoneyTransferSend = datareader.GetDecimalOrDefault("MoneyTransferSend");
                        receiptData.MoneyTransferReceive = datareader.GetDecimalOrDefault("MoneyTransferReceive");
                        receiptData.MoneyTransferModified = datareader.GetDecimalOrDefault("MoneyTransferModified");
                        receiptData.MoneyTransferCancelled = datareader.GetDecimalOrDefault("MoneyTransferCancelled");
                        receiptData.MoneyTransferRefund = datareader.GetDecimalOrDefault("MoneyTransferRefund");
                        receiptData.NetAmount = datareader.GetDecimalOrDefault("NetAmount");
                        receiptData.CardNumber = datareader.GetStringOrDefault("CardNumber");
                        receiptData.TotalMsg = datareader.GetStringOrDefault("TotalMsg");
                        receiptData.GPRCompanion = datareader.GetUInt32OrDefault("IsAddOn");
                        receiptData.Gpr = datareader.GetStringOrDefault("Gpr");
                        receiptData.ClientName = datareader.GetStringOrDefault("ClientName");
                        receiptData.LogoUrl = datareader.GetStringOrDefault("LogoUrl");
                        receiptData.LocationAddress = datareader.GetStringOrDefault("LocationAddress");
                        receiptData.LocationCity = datareader.GetStringOrDefault("LocationCity");
                        receiptData.LocationPhoneNumber = datareader.GetStringOrDefault("LocationPhoneNumber");
                        receiptData.LocationState = datareader.GetStringOrDefault("LocationState");
                        receiptData.LocationZip = datareader.GetStringOrDefault("LocationZip");
                        receiptData.BranchId = datareader.GetStringOrDefault("BranchId");
                        receiptData.BankId = datareader.GetStringOrDefault("BankId");
                        receiptData.TerminalID = datareader.GetStringOrDefault("TerminalID");
                        receiptData.TellerName = datareader.GetStringOrDefault("TellerName");
                        receiptData.SessionlID = datareader.GetInt64OrDefault("SessionlID");
                        receiptData.CustomerSessionDate = datareader.GetDateTimeOrDefault("CustomerSessionDate");
                        receiptData.ReceiptDate = datareader.GetDateTimeOrDefault("ReceiptDate");
                        receiptData.TellerNumber = datareader.GetStringOrDefault("TellerNumber");
                        receiptData.CustomerName = datareader.GetStringOrDefault("CustomerName");
                        receiptData.LocationName = datareader.GetStringOrDefault("LocationName");
                    }
                }

                return receiptData;
            }
            catch (Exception ex)
            {
                throw new ReceiptException(ReceiptException.GET_SHOPPING_CART_RECEIPT_DATA_FAILED, ex);
            }
        }
        public CashDrawerReceiptData GetCashDrawerReceiptData(long agentId, long locationId, ZeoContext context)
        {
            try
            {
                CashDrawerReceiptData cashDrawerData = null;
                StoredProcedure spGetCashDrawerReceiptData = new StoredProcedure("usp_GetCashDrawerReceiptData");
                spGetCashDrawerReceiptData.WithParameters(InputParameter.Named("AgentId").WithValue(agentId));
                spGetCashDrawerReceiptData.WithParameters(InputParameter.Named("LocationId").WithValue(locationId));

                using (IDataReader datareader = DataHelper.GetConnectionManager().ExecuteReader(spGetCashDrawerReceiptData))
                {
                    while (datareader.Read())
                    {
                        cashDrawerData = new CashDrawerReceiptData();
                        //cashDrawerData.TellerName = datareader.GetStringOrDefault("TellerName");
                        cashDrawerData.CashIn = datareader.GetDecimalOrDefault("CashIn");
                        cashDrawerData.CashOut = datareader.GetDecimalOrDefault("CashOut");
                        cashDrawerData.ReportingDate = DateTime.Now;
                    }
                }

                return cashDrawerData;
            }
            catch (Exception ex)
            {
                throw new ReceiptException(ReceiptException.GET_CASHDRAWER_RECEIPT_DATA_FAILED, ex);
            }
        }
        public string GetReceiptTemplate(List<string> receiptNames)
        {
            string receiptTemplate = string.Empty;
            StoredProcedure getReceiptTemplate = new StoredProcedure("usp_GetReceiptTemplate");
            StringWriter writer = GetReceiptTemplateXML(receiptNames);            
            DataParameter[] dataParameters = new DataParameter[]
               {
                    new DataParameter("Receipt", DbType.Xml)
                    {
                        Value = writer.ToString()
                    }
               };
            getReceiptTemplate.WithParameters(dataParameters);
            IDataReader datareader = DataHelper.GetConnectionManager().ExecuteReader(getReceiptTemplate);
            while (datareader.Read())
            {
                string templateName = datareader.GetStringOrDefault("TemplateName");
                byte[] recpt = (byte[])datareader.GetValue(datareader.GetOrdinal("ReceiptData"));
                receiptTemplate = Convert.ToBase64String(recpt);
            }
            return receiptTemplate;
        }
        public string GetStringReceiptTemplate(List<string> receiptNames)
        {
            string receiptTemplate = string.Empty;
            StoredProcedure getReceiptTemplate = new StoredProcedure("usp_GetReceiptTemplate");
            StringWriter writer = GetReceiptTemplateXML(receiptNames);
            DataParameter[] dataParameters = new DataParameter[]
               {
                    new DataParameter("Receipt", DbType.Xml)
                    {
                        Value = writer.ToString()
                    }
               };
            getReceiptTemplate.WithParameters(dataParameters);
            IDataReader datareader = DataHelper.GetConnectionManager().ExecuteReader(getReceiptTemplate);
            while (datareader.Read())
            {
                string templateName = datareader.GetStringOrDefault("TemplateName");              
                byte[] recpt = (byte[])datareader.GetValue(datareader.GetOrdinal("ReceiptData"));
                receiptTemplate = System.Text.Encoding.UTF8.GetString(recpt);
                
            }
            return receiptTemplate;
        }

        #endregion

        #region Private Methods
        private StringWriter GetReceiptTemplateXML(List<string> receiptNames)
        {
            DataTable dataTable = new DataTable();
            DataRow row;
            dataTable.Columns.Add("Name", typeof(string));
            dataTable.Columns.Add("Order", typeof(int));
            for (int i = 0; i < receiptNames.Count(); i++)
            {
                row = dataTable.NewRow();
                row["Name"] = receiptNames[i];
                row["Order"] = i + 1;
                dataTable.Rows.Add(row);
            }
            StringWriter writer = new StringWriter();
            dataTable.TableName = "Receipt";
            dataTable.WriteXml(writer);
            return writer;
        }

        #endregion
    }
}
