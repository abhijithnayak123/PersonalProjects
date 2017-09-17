using TCF.Zeo.Cxn.BillPay.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCF.Zeo.Cxn.BillPay.WU.Data;
using TCF.Zeo.Cxn.BillPay.WU.FusionGoShopping;
using TCF.Zeo.Common.Data;
using TCF.Zeo.Common.Util;
using P3Net.Data.Common;
using P3Net.Data;
using System.Data;
using System.IO;
using System.Xml.Serialization;
using TCF.Zeo.Cxn.Common;

namespace TCF.Zeo.Cxn.BillPay.WU.Impl
{
    public class ProcessorDAL
    {
        public long GetWUBillPayAccount(long customerSessionId, string timeZone)
        {
            long accountId = 0;

            StoredProcedure getBillPayAccount = new StoredProcedure("usp_GetWUBillPayAccount");
            getBillPayAccount.WithParameters(InputParameter.Named("customerSessionId").WithValue(customerSessionId));
            getBillPayAccount.WithParameters(InputParameter.Named("dtServerDate ").WithValue(DateTime.Now));
            getBillPayAccount.WithParameters(InputParameter.Named("dtTerminalDate ").WithValue(Helper.GetTimeZoneTime(timeZone)));

            using (IDataReader dr = DataConnectionHelper.GetConnectionManager().ExecuteReader(getBillPayAccount))
            {
                while (dr.Read())
                {
                    accountId = dr.GetInt64OrDefault("WUBillPayAccountId");
                }
            }

            return accountId;
        }

        public BillPayTransaction GetTransactionById(long wuTrxId)
        {
            StoredProcedure getBillPayTransaction = new StoredProcedure("usp_GetWUBillPayTransaction");

            getBillPayTransaction.WithParameters(InputParameter.Named("WUTransactionId").WithValue(wuTrxId));

            BillPayTransaction transaction = DataConnectionHelper.GetConnectionManager().ExecuteQueryWithResult<BillPayTransaction>(getBillPayTransaction, b => new BillPayTransaction
            {

                Amount = b.GetDecimalOrDefault("Amount"),
                Fee = b.GetDecimalOrDefault("Fee"),
                BillerName = b.GetStringOrDefault("BillerName"),
                AccountNumber = b.GetStringOrDefault("AccountNumber"),
                MetaData = new Dictionary<string, object>()
                                                {
                                                       {"MTCN",b.GetStringOrDefault("MTCN")},
                                                       {"WuCardTotalPointsEarned", b.GetStringOrDefault("WUCard_TotalPointsEarned")},
                                                       {"UnDiscountedFee",b.GetDecimalOrDefault("Financials_UndiscountedCharges")},
                                                       {"DiscountedFee", b.GetDecimalOrDefault("Financials_TotalDiscount")},
                                                       {"WesternUnionCardNumber", b.GetStringOrDefault("WesternUnionCardNumber")},
                                                       {"FillingDate",b.GetStringOrDefault("FillingDate")},
                                                       {"FillingTime",b.GetStringOrDefault("FillingTime")},
                                                       {"PromoCoupon", b.GetStringOrDefault("promotions_sender_promo_code")},
                                                       {"DeliveryService", GetDeliveryServiceByCode(b.GetStringOrDefault("DeliveryCode"))},
                                                       {"MessageArea",b.GetStringOrDefault("MessageArea")},
                                                       {"PromotionDescription", b.GetStringOrDefault("promotions_promo_message") }
                                                  }

            });

            return transaction;
        }

        public BillPaymentRequest GetWUBillPayRequest(long customerId, long wuTrxId)
        {
            StoredProcedure getCxnRequest = new StoredProcedure("usp_GetWUBillPayRequestDetails");

            getCxnRequest.WithParameters(InputParameter.Named("CustomerId").WithValue(customerId));
            getCxnRequest.WithParameters(InputParameter.Named("WUBillPayTrxID").WithValue(wuTrxId));

            BillPaymentRequest request = DataConnectionHelper.GetConnectionManager().ExecuteQueryWithResult<BillPaymentRequest>(getCxnRequest, r => new BillPaymentRequest
            {
                FirstName = r.GetStringOrDefault("FirstName"),
                LastName = r.GetStringOrDefault("LastName"),
                Address1 = r.GetStringOrDefault("Address1"),
                Address2 = r.GetStringOrDefault("Address2"),
                City = r.GetStringOrDefault("City"),
                State = r.GetStringOrDefault("State"),
                PostalCode = r.GetStringOrDefault("Zip"),
                ContactPhone = r.GetStringOrDefault("CustomerPhoneNumber"),
                MobilePhone = r.GetStringOrDefault("CustomerMobileNumber"),
                PrimaryIdType = r.GetStringOrDefault("PrimaryIdType"),
                PrimaryIdNumber = r.GetStringOrDefault("PrimaryIdNumber"),
                PrimaryIdPlaceOfIssue = r.GetStringOrDefault("PrimaryIdPlaceOfIssueCode"),
                PrimaryCountryOfIssue = r.GetStringOrDefault("PrimaryCountryOfIssue"),
                PrimaryIdCountryOfIssue = r.GetStringOrDefault("PrimaryIdCountryOfIssueCode"),
                PrimaryIdCountryNameOfIssue = r.GetStringOrDefault("PrimaryIdCountryNameOfIssue"),
                SecondIdNumber = r.GetStringOrDefault("SecondIdNumber"),
                Occupation = r.GetStringOrDefault("Occupation"),
                CountryOfBirth = r.GetStringOrDefault("CountryOfBirth"),
                DateOfBirth = r.GetDateTimeOrDefault("DOB"),
                ForeignRemoteSystemReferenceNo = r.GetStringOrDefault("ForeignRemoteSystemReferenceNo"),
                CardNumber = r.GetStringOrDefault("CardNumber"),
                Email = r.GetStringOrDefault("Email")
            });
            return request;
        }

        public long CreateOrUpdateBillPayWUTransaction(WUTransaction transaction)
        {
            StoredProcedure createWUTransaction = new StoredProcedure("usp_CreateOrUpdateWUBillPayTransaction");

            createWUTransaction.WithParameters(InputParameter.Named("WUBillPayAccountID").WithValue(transaction.WUAccountId));
            createWUTransaction.WithParameters(InputParameter.Named("WUBillPayTrxID").WithValue(transaction.Id));
            createWUTransaction.WithParameters(InputParameter.Named("providerId").WithValue(transaction.ProviderId));
            createWUTransaction.WithParameters(InputParameter.Named("channelType").WithValue(transaction.ChannelType));
            createWUTransaction.WithParameters(InputParameter.Named("channelName").WithValue(transaction.ChannelName));
            createWUTransaction.WithParameters(InputParameter.Named("channelVersion").WithValue(transaction.ChannelVersion));
            createWUTransaction.WithParameters(InputParameter.Named("senderFirstName ").WithValue(transaction.SenderFirstName));
            createWUTransaction.WithParameters(InputParameter.Named("senderLastname").WithValue(transaction.SenderLastname));
            createWUTransaction.WithParameters(InputParameter.Named("senderAddressLine1").WithValue(transaction.SenderAddressLine1));
            createWUTransaction.WithParameters(InputParameter.Named("senderCity").WithValue(transaction.SenderCity));
            createWUTransaction.WithParameters(InputParameter.Named("senderState").WithValue(transaction.SenderState));
            createWUTransaction.WithParameters(InputParameter.Named("senderPostalCode").WithValue(transaction.SenderPostalCode));
            createWUTransaction.WithParameters(InputParameter.Named("senderAddressLine2").WithValue(transaction.SenderAddressLine2));
            createWUTransaction.WithParameters(InputParameter.Named("senderStreet").WithValue(transaction.SenderStreet));
            createWUTransaction.WithParameters(InputParameter.Named("westernUnionCardNumber").WithValue(transaction.WesternUnionCardNumber));
            createWUTransaction.WithParameters(InputParameter.Named("senderDateOfBirth").WithValue(transaction.SenderDateOfBirth));
            createWUTransaction.WithParameters(InputParameter.Named("billerName").WithValue(transaction.BillerName));
            createWUTransaction.WithParameters(InputParameter.Named("customerAccountNumber").WithValue(transaction.CustomerAccountNumber));
            createWUTransaction.WithParameters(InputParameter.Named("foreignRemoteSystemIdentifier").WithValue(transaction.ForeignRemoteSystemIdentifier));
            createWUTransaction.WithParameters(InputParameter.Named("foreignRemoteSystemReference_no").WithValue(transaction.ForeignRemoteSystemReferenceNo));
            createWUTransaction.WithParameters(InputParameter.Named("foreignRemoteSystemCounterId").WithValue(transaction.ForeignRemoteSystemCounterId));
            createWUTransaction.WithParameters(InputParameter.Named("dTTerminalCreate").WithValue(transaction.DTTerminalCreate));
            createWUTransaction.WithParameters(InputParameter.Named("dTServerCreate").WithValue(transaction.DTServerCreate));
            createWUTransaction.WithParameters(OutputParameter.Named("WUTrxID").OfType<long>());

            int trxCount = DataConnectionHelper.GetConnectionManager().ExecuteNonQuery(createWUTransaction);

            return Convert.ToInt64(createWUTransaction.Parameters["WUTrxID"].Value);
        }

        public void UpdateWUBillPayTransaction(WUTransaction transaction)
        {
            try
            {
                StoredProcedure updateWUBillPay = new StoredProcedure("usp_UpdateWUBillPayTransaction");
                updateWUBillPay.WithParameters(InputParameter.Named("WUBillPayTrxID").WithValue(transaction.Id));
                updateWUBillPay.WithParameters(InputParameter.Named("senderCountryCode").WithValue(transaction.SenderCountryCode));
                updateWUBillPay.WithParameters(InputParameter.Named("senderCurrencyCode").WithValue(transaction.SenderCurrencyCode));
                updateWUBillPay.WithParameters(InputParameter.Named("levelCode").WithValue(transaction.LevelCode));
                updateWUBillPay.WithParameters(InputParameter.Named("senderEmail").WithValue(transaction.SenderEmail));
                updateWUBillPay.WithParameters(InputParameter.Named("senderContactPhone").WithValue(transaction.SenderContactPhone));
                updateWUBillPay.WithParameters(InputParameter.Named("billerCityCode").WithValue(transaction.BillerCityCode));
                updateWUBillPay.WithParameters(InputParameter.Named("countryCode").WithValue(transaction.CountryCode));
                updateWUBillPay.WithParameters(InputParameter.Named("currencyCode").WithValue(transaction.CurrencyCode));
                updateWUBillPay.WithParameters(InputParameter.Named("financialsOriginatorsPrincipalAmount").WithValue(transaction.FinancialsOriginatorsPrincipalAmount));
                updateWUBillPay.WithParameters(InputParameter.Named("financialsDestinationPrincipalAmount").WithValue(transaction.FinancialsDestinationPrincipalAmount));
                updateWUBillPay.WithParameters(InputParameter.Named("financialsFee").WithValue(transaction.FinancialsFee));
                updateWUBillPay.WithParameters(InputParameter.Named("financialsGrossTotalAmount").WithValue(transaction.FinancialsGrossTotalAmount));
                updateWUBillPay.WithParameters(InputParameter.Named("financialsTotal").WithValue(transaction.FinancialsTotal));
                updateWUBillPay.WithParameters(InputParameter.Named("financialsUndiscountedCharges").WithValue(transaction.FinancialsUndiscountedCharges));
                updateWUBillPay.WithParameters(InputParameter.Named("financialsDiscountedCharges").WithValue(transaction.FinancialsDiscountedCharges));
                updateWUBillPay.WithParameters(InputParameter.Named("paymentDetailsRecordingCountryCode").WithValue(transaction.PaymentDetailsRecordingCountryCode));
                updateWUBillPay.WithParameters(InputParameter.Named("paymentDetailsRecordingCountryCurrency").WithValue(transaction.PaymentDetailsRecordingCountryCurrency));
                updateWUBillPay.WithParameters(InputParameter.Named("paymentDetailsDestinationCountryCode").WithValue(transaction.PaymentDetailsDestinationCountryCode));
                updateWUBillPay.WithParameters(InputParameter.Named("paymentDetailsDestinationCountryCurrency").WithValue(transaction.PaymentDetailsDestinationCountryCurrency));
                updateWUBillPay.WithParameters(InputParameter.Named("paymentDetailsOriginatingCountryCode").WithValue(transaction.PaymentDetailsOriginatingCountryCode));
                updateWUBillPay.WithParameters(InputParameter.Named("paymentDetailsOriginatingCountryCurrency").WithValue(transaction.PaymentDetailsOriginatingCountryCurrency));
                updateWUBillPay.WithParameters(InputParameter.Named("paymentDetailsOriginatingCity").WithValue(transaction.PaymentDetailsOriginatingCity));
                updateWUBillPay.WithParameters(InputParameter.Named("paymentDetailsOriginatingState").WithValue(transaction.PaymentDetailsOriginatingState));
                updateWUBillPay.WithParameters(InputParameter.Named("paymentDetailsTransactionType").WithValue(transaction.PaymentDetailsTransactionType));
                updateWUBillPay.WithParameters(InputParameter.Named("paymentDetailsPaymentType").WithValue(transaction.PaymentDetailsPaymentType));
                updateWUBillPay.WithParameters(InputParameter.Named("paymentDetailsExchangeRate").WithValue(transaction.PaymentDetailsExchangeRate));
                updateWUBillPay.WithParameters(InputParameter.Named("paymentDetailsFixOnSend").WithValue(transaction.PaymentDetailsFixOnSend));
                updateWUBillPay.WithParameters(InputParameter.Named("paymentDetailsReceiptOptOut").WithValue(transaction.PaymentDetailsReceiptOptOut));
                updateWUBillPay.WithParameters(InputParameter.Named("paymentDetailsAuthStatus").WithValue(transaction.PaymentDetailsAuthStatus));
                updateWUBillPay.WithParameters(InputParameter.Named("fillingDate").WithValue(transaction.FillingDate));
                updateWUBillPay.WithParameters(InputParameter.Named("fillingTime").WithValue(transaction.FillingTime));
                updateWUBillPay.WithParameters(InputParameter.Named("mTCN").WithValue(transaction.Mtcn));
                updateWUBillPay.WithParameters(InputParameter.Named("newMTCN").WithValue(transaction.NewMTCN));
                updateWUBillPay.WithParameters(InputParameter.Named("dfFieldsPDSRequiredFlag").WithValue(transaction.DfFieldsPdsrequiredflag));
                updateWUBillPay.WithParameters(InputParameter.Named("dfFieldsTransactionFlag").WithValue(transaction.DfFieldsTransactionFlag));
                updateWUBillPay.WithParameters(InputParameter.Named("dfFieldsDeliveryServiceName").WithValue(transaction.DfFieldsDeliveryServiceName));
                updateWUBillPay.WithParameters(InputParameter.Named("deliveryCode").WithValue(transaction.DeliveryCode));
                updateWUBillPay.WithParameters(InputParameter.Named("fusionScreen").WithValue(transaction.FusionScreen));
                updateWUBillPay.WithParameters(InputParameter.Named("convSessionCookie").WithValue(transaction.ConvSessionCookie));
                updateWUBillPay.WithParameters(InputParameter.Named("instantNotificationAddlServiceCharges").WithValue(transaction.InstantNotificationAddlServiceCharges));
                updateWUBillPay.WithParameters(InputParameter.Named("instantNotificationAddlServiceLength").WithValue(transaction.InstantNotificationAddlServiceLength));
                updateWUBillPay.WithParameters(InputParameter.Named("promotionscouponspromotions").WithValue(transaction.PromotionsCouponsPromotions));
                updateWUBillPay.WithParameters(InputParameter.Named("promotionspromocodedescription").WithValue(transaction.PromotionsPromoCodeDescription));
                updateWUBillPay.WithParameters(InputParameter.Named("promotionspromosequenceno").WithValue(transaction.PromotionsPromoSequenceNo));
                updateWUBillPay.WithParameters(InputParameter.Named("promotionspromoname").WithValue(transaction.PromotionsPromoName));
                updateWUBillPay.WithParameters(InputParameter.Named("promotionspromomessage").WithValue(transaction.PromotionsPromoMessage));
                updateWUBillPay.WithParameters(InputParameter.Named("promotionspromodiscountamount").WithValue(transaction.PromotionsPromoDiscountAmount));
                updateWUBillPay.WithParameters(InputParameter.Named("promotionspromotionerror").WithValue(transaction.PromotionsPromotionError));
                updateWUBillPay.WithParameters(InputParameter.Named("promotionssenderpromocode").WithValue(transaction.PromotionsSenderPromoCode));
                updateWUBillPay.WithParameters(InputParameter.Named("senderComplianceDetailsTemplateID").WithValue(transaction.SenderComplianceDetailsTemplateID));
                updateWUBillPay.WithParameters(InputParameter.Named("senderComplianceDetailsIdDetailsIdType").WithValue(transaction.SenderComplianceDetailsIdDetailsIdType));
                updateWUBillPay.WithParameters(InputParameter.Named("senderComplianceDetailsIdDetailsIdCountryOfIssue").WithValue(transaction.SenderComplianceDetailsIdDetailsIdCountryOfIssue));
                updateWUBillPay.WithParameters(InputParameter.Named("senderComplianceDetailsIdDetailsIdPlaceOfIssue").WithValue(transaction.SenderComplianceDetailsIdDetailsIdPlaceOfIssue));
                updateWUBillPay.WithParameters(InputParameter.Named("senderComplianceDetailsIdDetailsIdNumber").WithValue(transaction.SenderComplianceDetailsIdDetailsIdNumber));
                updateWUBillPay.WithParameters(InputParameter.Named("senderComplianceDetailsSecondIDIdType").WithValue(transaction.SenderComplianceDetailsSecondIDIdType));
                updateWUBillPay.WithParameters(InputParameter.Named("senderComplianceDetailsSecondIDIdCountryOfIssue").WithValue(transaction.SenderComplianceDetailsSecondIDIdCountryOfIssue));
                updateWUBillPay.WithParameters(InputParameter.Named("senderComplianceDetailsSecondIDIdNumber").WithValue(transaction.SenderComplianceDetailsSecondIDIdNumber));
                updateWUBillPay.WithParameters(InputParameter.Named("senderComplianceDetailsDateOfBirth").WithValue(transaction.SenderComplianceDetailsDateOfBirth));
                updateWUBillPay.WithParameters(InputParameter.Named("senderComplianceDetailsOccupation").WithValue(transaction.SenderComplianceDetailsOccupation));
                updateWUBillPay.WithParameters(InputParameter.Named("senderComplianceDetailsCurrentAddressAddrLine1").WithValue(transaction.SenderComplianceDetailsCurrentAddressAddrLine1));
                updateWUBillPay.WithParameters(InputParameter.Named("senderComplianceDetailsCurrentAddressAddrLine2").WithValue(transaction.SenderComplianceDetailsCurrentAddressAddrLine2));
                updateWUBillPay.WithParameters(InputParameter.Named("senderComplianceDetailsCurrentAddressCity").WithValue(transaction.SenderComplianceDetailsCurrentAddressCity));
                updateWUBillPay.WithParameters(InputParameter.Named("senderComplianceDetailsCurrentAddressStateCode").WithValue(transaction.SenderComplianceDetailsCurrentAddressStateCode));
                updateWUBillPay.WithParameters(InputParameter.Named("senderComplianceDetailsCurrentAddressPostalCode").WithValue(transaction.SenderComplianceDetailsCurrentAddressPostalCode));
                updateWUBillPay.WithParameters(InputParameter.Named("senderComplianceDetailsContactPhone").WithValue(transaction.SenderComplianceDetailsContactPhone));
                updateWUBillPay.WithParameters(InputParameter.Named("senderComplianceDetailsIActOnMyBehalf").WithValue(transaction.SenderComplianceDetailsIActOnMyBehalf));
                updateWUBillPay.WithParameters(InputParameter.Named("senderComplianceDetailsAckFlag").WithValue(transaction.SenderComplianceDetailsAckFlag));
                updateWUBillPay.WithParameters(InputParameter.Named("senderComplianceDetailsComplianceDataBuffer").WithValue(transaction.SenderComplianceDetailsComplianceDataBuffer));
                updateWUBillPay.WithParameters(InputParameter.Named("financialsPlusChargesAmount").WithValue(transaction.FinancialsPlusChargesAmount));
                updateWUBillPay.WithParameters(InputParameter.Named("financialsTotalDiscount").WithValue(transaction.FinancialsTotalDiscount));
                updateWUBillPay.WithParameters(InputParameter.Named("wUCardTotalPointsEarned").WithValue(transaction.WuCardTotalPointsEarned));
                updateWUBillPay.WithParameters(InputParameter.Named("qPCompanyDepartment").WithValue(transaction.QPCompanyDepartment));
                updateWUBillPay.WithParameters(InputParameter.Named("messageArea").WithValue(transaction.MessageArea));
                updateWUBillPay.WithParameters(InputParameter.Named("dTServerDate").WithValue(transaction.DTServerLastModified));
                updateWUBillPay.WithParameters(InputParameter.Named("dTTerminalDate").WithValue(transaction.DTTerminalLastModified));

                DataConnectionHelper.GetConnectionManager().ExecuteNonQuery(updateWUBillPay);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal void UpdateBillPayGoldCardPoints(long transactionId, string totalPointsEarned, int productCode)
        {
            StoredProcedure goldCardPointsProc = new StoredProcedure("usp_UpdateGoldCardPoints");

            goldCardPointsProc.WithParameters(InputParameter.Named("transactionId").WithValue(transactionId));
            goldCardPointsProc.WithParameters(InputParameter.Named("totalPointsEarned").WithValue(totalPointsEarned));

            goldCardPointsProc.WithParameters(InputParameter.Named("productCode").WithValue(productCode));

            DataConnectionHelper.GetConnectionManager().ExecuteNonQuery(goldCardPointsProc);

        }

        public BillPayTransactionRequest GetBillPayTransactionRequest(long wuTrxId)
        {
            StoredProcedure getCxnRequest = new StoredProcedure("usp_GetWUBillPayTransactionRequest");

            getCxnRequest.WithParameters(InputParameter.Named("WUBillPayTrxID").WithValue(wuTrxId));

            BillPayTransactionRequest request = DataConnectionHelper.GetConnectionManager().ExecuteQueryWithResult<BillPayTransactionRequest>(getCxnRequest, r => new BillPayTransactionRequest
            {
                Id = wuTrxId,
                FirstName = r.GetStringOrDefault("FirstName"),
                LastName = r.GetStringOrDefault("LastName"),
                Address1 = r.GetStringOrDefault("Address1"),
                Address2 = r.GetStringOrDefault("Address2"),
                City = r.GetStringOrDefault("City"),
                State = r.GetStringOrDefault("State"),
                PostalCode = r.GetStringOrDefault("Zip"),
                ContactPhone = r.GetStringOrDefault("CustomerPhoneNumber"),
                MobilePhone = r.GetStringOrDefault("CustomerMobileNumber"),
                DateOfBirth = r.GetDateTimeOrDefault("DOB"),
                ForeignRemoteSystemReferenceNo = r.GetStringOrDefault("ForeignRemoteSystemReferenceNo"),
                CardNumber = r.GetStringOrDefault("CardNumber"),
                SenderCity = r.GetStringOrDefault("SenderCity"),
                SenderComplianceDetailsComplianceDataBuffer = r.GetStringOrDefault("SenderComplianceDataBuffer"),
                BillerName = r.GetStringOrDefault("BillerName"),
                BillerCityCode = r.GetStringOrDefault("BillerCityCode"),
                CustomerAccountNumber = r.GetStringOrDefault("CustomerAccountNumber"),
                QPCompanyDepartment = r.GetStringOrDefault("QPCompanyDepartment"),
                FinancialsOriginatorsPrincipalAmount = r.GetDecimalOrDefault("FinancialsOriginatorsPrincipalAmount"),
                FinancialsDestinationPrincipalAmount = r.GetDecimalOrDefault("FinancialsDestinationPrincipalAmount"),
                FinancialsFee = r.GetDecimalOrDefault("FinancialsFee"),
                FinancialsPlusChargesAmount = r.GetDecimalOrDefault("FinancialsPlusChargesAmount"),
                FinancialsUndiscountedCharges = r.GetDecimalOrDefault("FinancialsUndiscountedCharges"),
                FinancialsTotalDiscount = r.GetDecimalOrDefault("FinancialsTotalDiscount"),
                FinancialsDiscountedCharges = r.GetDecimalOrDefault("FinancialsDiscountedCharges"),
                PaymentDetailsExchangeRate = (double)r.GetDecimalOrDefault("PaymentDetailsExchangeRate"),
                PaymentDetailsAuthStatus = r.GetStringOrDefault("PaymentDetailsAuthStatus"),
                PromotionsPromoCodeDescription = r.GetStringOrDefault("PromotionsPromoCodeDescription"),
                PromotionsPromoSequenceNo = r.GetStringOrDefault("PromotionsPromoSequenceNo"),
                PromotionsPromoName = r.GetStringOrDefault("PromotionsPromoName"),
                PromotionsPromoMessage = r.GetStringOrDefault("PromotionsPromoMessage"),
                PromotionsPromoDiscountAmount = r.GetDecimalOrDefault("PromotionsPromoDiscountAmount"),
                PromotionsPromotionError = r.GetStringOrDefault("PromotionsPromotionError"),
                PromotionsSenderPromoCode = r.GetStringOrDefault("PromotionsSenderPromoCode"),
                DeliveryCode = r.GetStringOrDefault("DeliveryCode"),
                ConvSessionCookie = r.GetStringOrDefault("ConvSessionCookie"),
                Mtcn = r.GetStringOrDefault("MTCN"),
                NewMTCN = r.GetStringOrDefault("NewMTCN")
            });

            return request;
        }
        
        internal void CreateOrUpdateImportBillers(List<ImportedBiller> importBillers)
        {
            if (importBillers.Count > 0)
            {
                StoredProcedure importBillerProc = new StoredProcedure("usp_CreateOrUpdateWUImportBillers");

                DataTable table = ConvertToDataTable(importBillers);
                StringWriter writer = new StringWriter();
                table.TableName = "ImportBillers";
                table.WriteXml(writer);
                DataParameter[] dataParameters = new DataParameter[]
                {
                    new DataParameter("ImportBillers", DbType.Xml)
                    {
                        Value = writer.ToString()
                    }
                };

                importBillerProc.WithParameters(dataParameters);

                DataConnectionHelper.GetConnectionManager().ExecuteNonQuery(importBillerProc);
            }
        }

        internal static string GetDeliveryServiceByCode(string deliveryCode)
        {
            string deliveryService = string.Empty;

            switch (deliveryCode)
            {
                case "000":
                    deliveryService = "Urgent";
                    break;
                case "100":
                    deliveryService = "2nd Business Day";
                    break;
                case "200":
                    deliveryService = "3rd Business Day";
                    break;
                case "300":
                    deliveryService = "Next Business Day";
                    break;
                default:
                    break;
            }

            return deliveryService;
        }

        private DataTable ConvertToDataTable(List<ImportedBiller> importBillers)
        {
            DataTable dataTable = new DataTable();
            DataRow row;
            dataTable.Columns.Add("BillerName", typeof(string));
            dataTable.Columns.Add("AccountNumber", typeof(string));
            dataTable.Columns.Add("CardNumber", typeof(string));
            dataTable.Columns.Add("WUIndex", typeof(string));
            dataTable.Columns.Add("AgentSessionId", typeof(long));
            dataTable.Columns.Add("CustomerSessionId", typeof(long));
            dataTable.Columns.Add("DTTerminalDate", typeof(DateTime));
            dataTable.Columns.Add("DTServerDate", typeof(DateTime));

            foreach (var item in importBillers)
        {
                row = dataTable.NewRow();
                row["BillerName"] = item.BillerName;
                row["AccountNumber"] = item.AccountNumber;
                row["CardNumber"] = item.CardNumber;
                row["WUIndex"] = item.WUIndex;
                row["AgentSessionId"] = item.AgentSessionId;
                row["CustomerSessionId"] = item.CustomerSessionId;
                row["DTTerminalDate"] = item.DTTerminalCreate;
                row["DTServerDate"] = item.DTServerCreate;
                dataTable.Rows.Add(row);
            }
            return dataTable;
        }

    }
}
