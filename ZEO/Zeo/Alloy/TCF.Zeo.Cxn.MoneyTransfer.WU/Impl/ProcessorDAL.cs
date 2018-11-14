using TCF.Zeo.Common.Data;
using TCF.Zeo.Cxn.MoneyTransfer.Data;
using TCF.Zeo.Cxn.MoneyTransfer.WU.Data;
using P3Net.Data;
using P3Net.Data.Common;
using System;
using System.Data;
using static TCF.Zeo.Common.Util.Helper;
using WUCommonData = TCF.Zeo.Cxn.WU.Common.Data;
using TCF.Zeo.Cxn.Common;

namespace TCF.Zeo.Cxn.MoneyTransfer.WU.Impl
{
    public class ProcessorDAL
    {
        internal long CreateWUTransaction(WUTransaction wuTransaction)
        {
            try
            {
                long wuTransactionId = 0;

                StoredProcedure moneyTransferProcedure = new StoredProcedure("usp_CreateWUTransaction");

                moneyTransferProcedure.WithParameters(InputParameter.Named("originatorsPrincipalAmount").WithValue(wuTransaction.OriginatorsPrincipalAmount));
                moneyTransferProcedure.WithParameters(InputParameter.Named("originatingCountryCode").WithValue(wuTransaction.OriginatingCountryCode));
                moneyTransferProcedure.WithParameters(InputParameter.Named("originatingCurrencyCode").WithValue(wuTransaction.OriginatingCurrencyCode));
                moneyTransferProcedure.WithParameters(InputParameter.Named("tranascationType").WithValue(wuTransaction.TranascationType));
                moneyTransferProcedure.WithParameters(InputParameter.Named("promoCode").WithValue(wuTransaction.PromotionsCode));
                moneyTransferProcedure.WithParameters(InputParameter.Named("exchangeRate").WithValue(wuTransaction.ExchangeRate));
                moneyTransferProcedure.WithParameters(InputParameter.Named("destinationPrincipalAmount").WithValue(wuTransaction.DestinationPrincipalAmount));
                moneyTransferProcedure.WithParameters(InputParameter.Named("grossTotAmt").WithValue(wuTransaction.GrossTotalAmount));
                moneyTransferProcedure.WithParameters(InputParameter.Named("charges").WithValue(wuTransaction.Charges));
                moneyTransferProcedure.WithParameters(InputParameter.Named("taxAmt").WithValue(wuTransaction.TaxAmount));
                moneyTransferProcedure.WithParameters(InputParameter.Named("mtcn").WithValue(wuTransaction.MTCN));
                moneyTransferProcedure.WithParameters(InputParameter.Named("dtTerminalCreate").WithValue(wuTransaction.DTTerminalCreate));
                moneyTransferProcedure.WithParameters(InputParameter.Named("promoDiscount").WithValue(wuTransaction.PromotionDiscount));
                moneyTransferProcedure.WithParameters(InputParameter.Named("otherCharges").WithValue(wuTransaction.OtherCharges));
                moneyTransferProcedure.WithParameters(InputParameter.Named("moneyTranKey").WithValue(wuTransaction.MoneyTransferKey));
                moneyTransferProcedure.WithParameters(InputParameter.Named("additionalCharges").WithValue(wuTransaction.AdditionalCharges));
                moneyTransferProcedure.WithParameters(InputParameter.Named("destinationCountryCode").WithValue(wuTransaction.DestinationCountryCode));
                moneyTransferProcedure.WithParameters(InputParameter.Named("destinationCurrencyCode").WithValue(wuTransaction.DestinationCurrencyCode));
                moneyTransferProcedure.WithParameters(InputParameter.Named("destinationState").WithValue(wuTransaction.DestinationState));
                moneyTransferProcedure.WithParameters(InputParameter.Named("isDomesticTransfer").WithValue(wuTransaction.IsDomesticTransfer));
                moneyTransferProcedure.WithParameters(InputParameter.Named("isFixedOnSend").WithValue(wuTransaction.IsFixedOnSend));
                moneyTransferProcedure.WithParameters(InputParameter.Named("phoneNumber").WithValue(wuTransaction.PhoneNumber));
                moneyTransferProcedure.WithParameters(InputParameter.Named("Url").WithValue(wuTransaction.Url));
                moneyTransferProcedure.WithParameters(InputParameter.Named("agencyName").WithValue(wuTransaction.AgencyName));
                moneyTransferProcedure.WithParameters(InputParameter.Named("channelPartnerId").WithValue(wuTransaction.ChannelPartnerId));
                moneyTransferProcedure.WithParameters(InputParameter.Named("providerId").WithValue(wuTransaction.ProviderId));
                moneyTransferProcedure.WithParameters(InputParameter.Named("testQuestion").WithValue(wuTransaction.TestQuestion));
                moneyTransferProcedure.WithParameters(InputParameter.Named("tempMtcn").WithValue(wuTransaction.TempMTCN));
                moneyTransferProcedure.WithParameters(InputParameter.Named("expectedPayoutStateCode").WithValue(wuTransaction.ExpectedPayoutStateCode));
                moneyTransferProcedure.WithParameters(InputParameter.Named("expectedPayoutCityName").WithValue(wuTransaction.ExpectedPayoutCityName));
                moneyTransferProcedure.WithParameters(InputParameter.Named("testAnswer").WithValue(wuTransaction.TestAnswer));
                moneyTransferProcedure.WithParameters(InputParameter.Named("testQuestionAvaliable").WithValue(wuTransaction.TestQuestionAvaliable));
                moneyTransferProcedure.WithParameters(InputParameter.Named("gcNumber").WithValue(wuTransaction.GCNumber));
                moneyTransferProcedure.WithParameters(InputParameter.Named("senderName").WithValue(wuTransaction.SenderName));
                moneyTransferProcedure.WithParameters(InputParameter.Named("pdsRequiredFlag").WithValue(wuTransaction.PdsRequiredFlag));
                moneyTransferProcedure.WithParameters(InputParameter.Named("DfTransactionFlag").WithValue(wuTransaction.DfTransactionFlag));
                moneyTransferProcedure.WithParameters(InputParameter.Named("deliveryServiceName").WithValue(wuTransaction.DeliveryServiceName));
                moneyTransferProcedure.WithParameters(InputParameter.Named("DtAvailableForPickup").WithValue(wuTransaction.DTAvailableForPickup));
                moneyTransferProcedure.WithParameters(InputParameter.Named("DtServerCreate").WithValue(wuTransaction.DTServerCreate));
                moneyTransferProcedure.WithParameters(InputParameter.Named("receiverFirstName").WithValue(wuTransaction.ReceiverFirstName));
                moneyTransferProcedure.WithParameters(InputParameter.Named("receiverLastName").WithValue(wuTransaction.ReceiverLastName));
                moneyTransferProcedure.WithParameters(InputParameter.Named("receiverSecondLastName").WithValue(wuTransaction.ReceiverSecondLastName));
                moneyTransferProcedure.WithParameters(InputParameter.Named("promoCodeDescription").WithValue(wuTransaction.PromoCodeDescription));
                moneyTransferProcedure.WithParameters(InputParameter.Named("promoName").WithValue(wuTransaction.PromoName));
                moneyTransferProcedure.WithParameters(InputParameter.Named("promoMsg").WithValue(wuTransaction.PromoMessage));
                moneyTransferProcedure.WithParameters(InputParameter.Named("promoError").WithValue(wuTransaction.PromotionError));
                moneyTransferProcedure.WithParameters(InputParameter.Named("sender_ComplianceDetails_ComplianceData_Buffer").WithValue(wuTransaction.SenderComplianceDetailsComplianceDataBuffer));
                moneyTransferProcedure.WithParameters(InputParameter.Named("recordingCountryCode").WithValue(wuTransaction.recordingCountryCode));
                moneyTransferProcedure.WithParameters(InputParameter.Named("recordingCurrencyCode").WithValue(wuTransaction.recordingCurrencyCode));
                moneyTransferProcedure.WithParameters(InputParameter.Named("originating_city").WithValue(wuTransaction.originating_city));
                moneyTransferProcedure.WithParameters(InputParameter.Named("originating_state").WithValue(wuTransaction.originating_state));
                moneyTransferProcedure.WithParameters(InputParameter.Named("municipal_tax").WithValue(wuTransaction.municipal_tax));
                moneyTransferProcedure.WithParameters(InputParameter.Named("state_tax").WithValue(wuTransaction.state_tax));
                moneyTransferProcedure.WithParameters(InputParameter.Named("country_tax").WithValue(wuTransaction.county_tax));
                moneyTransferProcedure.WithParameters(InputParameter.Named("plus_charges_amount").WithValue(wuTransaction.plus_charges_amount));
                moneyTransferProcedure.WithParameters(InputParameter.Named("message_charge").WithValue(wuTransaction.message_charge));
                moneyTransferProcedure.WithParameters(InputParameter.Named("total_undiscounted_charges").WithValue(wuTransaction.total_undiscounted_charges));
                moneyTransferProcedure.WithParameters(InputParameter.Named("total_discount").WithValue(wuTransaction.total_discount));
                moneyTransferProcedure.WithParameters(InputParameter.Named("total_discounted_charges").WithValue(wuTransaction.total_discounted_charges));
                moneyTransferProcedure.WithParameters(InputParameter.Named("instant_notification_addl_service_charges").WithValue(wuTransaction.instant_notification_addl_service_charges));
                moneyTransferProcedure.WithParameters(InputParameter.Named("paySideCharges").WithValue(wuTransaction.PaySideCharges));
                moneyTransferProcedure.WithParameters(InputParameter.Named("paySideTax").WithValue(wuTransaction.PaySideTax));
                moneyTransferProcedure.WithParameters(InputParameter.Named("amountToReceiver").WithValue(wuTransaction.AmountToReceiver));
                moneyTransferProcedure.WithParameters(InputParameter.Named("smsNotificationFlag").WithValue(wuTransaction.SMSNotificationFlag));
                moneyTransferProcedure.WithParameters(InputParameter.Named("personalMsg").WithValue(wuTransaction.PersonalMessage));
                moneyTransferProcedure.WithParameters(InputParameter.Named("deliveryServiceDesc").WithValue(wuTransaction.DeliveryServiceDesc));
                moneyTransferProcedure.WithParameters(InputParameter.Named("referenceNo").WithValue(wuTransaction.ReferenceNo));
                moneyTransferProcedure.WithParameters(InputParameter.Named("pay_or_do_not_pay_indicator").WithValue(wuTransaction.pay_or_do_not_pay_indicator));
                moneyTransferProcedure.WithParameters(InputParameter.Named("originalDestinationCountryCode").WithValue(wuTransaction.OriginalDestinationCountryCode));
                moneyTransferProcedure.WithParameters(InputParameter.Named("originalDestinationCurrencyCode").WithValue(wuTransaction.OriginalDestinationCurrencyCode));
                moneyTransferProcedure.WithParameters(InputParameter.Named("fillingDate").WithValue(wuTransaction.FilingDate));
                moneyTransferProcedure.WithParameters(InputParameter.Named("fillingTime").WithValue(wuTransaction.FilingTime));
                moneyTransferProcedure.WithParameters(InputParameter.Named("paidDateTime").WithValue(wuTransaction.PaidDateTime));
                moneyTransferProcedure.WithParameters(InputParameter.Named("availableForPickup").WithValue(wuTransaction.AvailableForPickup));
                moneyTransferProcedure.WithParameters(InputParameter.Named("delayHrs").WithValue(wuTransaction.DelayHours));
                moneyTransferProcedure.WithParameters(InputParameter.Named("availableForPickupEST").WithValue(wuTransaction.AvailableForPickupEST));
                moneyTransferProcedure.WithParameters(InputParameter.Named("wuCardTotalPointsEarned").WithValue(wuTransaction.WuCardTotalPointsEarned));
                moneyTransferProcedure.WithParameters(InputParameter.Named("originalTransactionID").WithValue(wuTransaction.OriginalTransactionID));
                moneyTransferProcedure.WithParameters(InputParameter.Named("transactionSubType").WithValue(wuTransaction.TransactionSubType));
                moneyTransferProcedure.WithParameters(InputParameter.Named("reasonCode").WithValue(wuTransaction.ReasonCode));
                moneyTransferProcedure.WithParameters(InputParameter.Named("reasonDesc").WithValue(wuTransaction.ReasonDescription));
                moneyTransferProcedure.WithParameters(InputParameter.Named("comments").WithValue(wuTransaction.Comments));
                moneyTransferProcedure.WithParameters(InputParameter.Named("deliveryOption").WithValue(wuTransaction.DeliveryOption));
                moneyTransferProcedure.WithParameters(InputParameter.Named("deliveryOptionDesc").WithValue(wuTransaction.DeliveryServiceDesc));
                moneyTransferProcedure.WithParameters(InputParameter.Named("promoSeqNo").WithValue(wuTransaction.PromotionSequenceNo));
                moneyTransferProcedure.WithParameters(InputParameter.Named("counterId").WithValue(wuTransaction.CounterId));
                moneyTransferProcedure.WithParameters(InputParameter.Named("principalAmt").WithValue(wuTransaction.Principal_Amount));
                moneyTransferProcedure.WithParameters(InputParameter.Named("receiver_unv_Buffer").WithValue(wuTransaction.Receiver_unv_Buffer));
                moneyTransferProcedure.WithParameters(InputParameter.Named("sender_unv_Buffer").WithValue(wuTransaction.Sender_unv_Buffer));
                moneyTransferProcedure.WithParameters(InputParameter.Named("transalatedDeliveryServiceName").WithValue(wuTransaction.TransalatedDeliveryServiceName));
                moneyTransferProcedure.WithParameters(InputParameter.Named("messageArea").WithValue(wuTransaction.MessageArea));
                moneyTransferProcedure.WithParameters(InputParameter.Named("accountId").WithValue(wuTransaction.WUAccountId));
                moneyTransferProcedure.WithParameters(InputParameter.Named("receiverId").WithValue(wuTransaction.ReceiverId));
               // moneyTransferProcedure.WithParameters(InputParameter.Named("wuTransactionId").WithValue(wuTransaction.Id));

                using (IDataReader datareader = DataConnectionHelper.GetConnectionManager().ExecuteReader(moneyTransferProcedure))
                {
                    while (datareader.Read())
                    {
                        wuTransactionId = datareader.GetInt64OrDefault("WUTransactionId");
                    }
                }

                return wuTransactionId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal long UpdateWUTransaction(WUTransaction wuTransaction)
        {
            try
            {
                long wuTransactionId = 0;

                StoredProcedure moneyTransferProcedure = new StoredProcedure("usp_UpdateWUTransaction");

                moneyTransferProcedure.WithParameters(InputParameter.Named("WUTrxId").WithValue(wuTransaction.Id));
                moneyTransferProcedure.WithParameters(InputParameter.Named("mtcn").WithValue(wuTransaction.MTCN));
                moneyTransferProcedure.WithParameters(InputParameter.Named("tempMtcn").WithValue(wuTransaction.TempMTCN));
                moneyTransferProcedure.WithParameters(InputParameter.Named("deliveryServiceDesc").WithValue(wuTransaction.DeliveryServiceDesc));
                moneyTransferProcedure.WithParameters(InputParameter.Named("pdsRequiredFlag").WithValue(wuTransaction.PdsRequiredFlag));
                moneyTransferProcedure.WithParameters(InputParameter.Named("dfTransactionFlag").WithValue(wuTransaction.DfTransactionFlag));
                moneyTransferProcedure.WithParameters(InputParameter.Named("paySideTax").WithValue(wuTransaction.PaySideTax));
                moneyTransferProcedure.WithParameters(InputParameter.Named("paySideCharges").WithValue(wuTransaction.PaySideCharges));
                moneyTransferProcedure.WithParameters(InputParameter.Named("amountToReceiver").WithValue(wuTransaction.AmountToReceiver));
                moneyTransferProcedure.WithParameters(InputParameter.Named("municipal_tax").WithValue(wuTransaction.municipal_tax));
                moneyTransferProcedure.WithParameters(InputParameter.Named("state_tax").WithValue(wuTransaction.state_tax));
                moneyTransferProcedure.WithParameters(InputParameter.Named("country_tax").WithValue(wuTransaction.county_tax));
                moneyTransferProcedure.WithParameters(InputParameter.Named("plus_charges_amount").WithValue(wuTransaction.plus_charges_amount));
                moneyTransferProcedure.WithParameters(InputParameter.Named("message_charge").WithValue(wuTransaction.message_charge));
                moneyTransferProcedure.WithParameters(InputParameter.Named("total_discount").WithValue(wuTransaction.total_discount));
                moneyTransferProcedure.WithParameters(InputParameter.Named("total_discounted_charges").WithValue(wuTransaction.total_discounted_charges));
                moneyTransferProcedure.WithParameters(InputParameter.Named("total_undiscounted_charges").WithValue(wuTransaction.total_undiscounted_charges));
                moneyTransferProcedure.WithParameters(InputParameter.Named("promoCodeDescription").WithValue(wuTransaction.PromoCodeDescription));
                moneyTransferProcedure.WithParameters(InputParameter.Named("promoName").WithValue(wuTransaction.PromoName));
                moneyTransferProcedure.WithParameters(InputParameter.Named("promoMsg").WithValue(wuTransaction.PromoMessage));
                moneyTransferProcedure.WithParameters(InputParameter.Named("promoDiscount").WithValue(wuTransaction.PromotionDiscount));
                moneyTransferProcedure.WithParameters(InputParameter.Named("promoCode").WithValue(wuTransaction.PromotionsCode));
                moneyTransferProcedure.WithParameters(InputParameter.Named("promoSeqNo").WithValue(wuTransaction.PromotionSequenceNo));
                moneyTransferProcedure.WithParameters(InputParameter.Named("grossTotAmt").WithValue(wuTransaction.GrossTotalAmount));
                moneyTransferProcedure.WithParameters(InputParameter.Named("smsNotificationFlag").WithValue(wuTransaction.SMSNotificationFlag));
                moneyTransferProcedure.WithParameters(InputParameter.Named("additionalCharges").WithValue(wuTransaction.AdditionalCharges));
                moneyTransferProcedure.WithParameters(InputParameter.Named("personalMsg").WithValue(wuTransaction.PersonalMessage));
                moneyTransferProcedure.WithParameters(InputParameter.Named("fillingDate").WithValue(wuTransaction.FilingDate));
                moneyTransferProcedure.WithParameters(InputParameter.Named("fillingTime").WithValue(wuTransaction.FilingTime));
                moneyTransferProcedure.WithParameters(InputParameter.Named("instant_notification_addl_service_charges").WithValue(wuTransaction.instant_notification_addl_service_charges));
                moneyTransferProcedure.WithParameters(InputParameter.Named("originating_city").WithValue(wuTransaction.originating_city));
                moneyTransferProcedure.WithParameters(InputParameter.Named("originating_state").WithValue(wuTransaction.originating_state));
                moneyTransferProcedure.WithParameters(InputParameter.Named("expectedPayoutCityName").WithValue(wuTransaction.ExpectedPayoutCityName));
                moneyTransferProcedure.WithParameters(InputParameter.Named("expectedPayoutStateCode").WithValue(wuTransaction.ExpectedPayoutStateCode));
                moneyTransferProcedure.WithParameters(InputParameter.Named("isFixedOnSend").WithValue(wuTransaction.IsFixedOnSend));
                moneyTransferProcedure.WithParameters(InputParameter.Named("deliveryOption").WithValue(wuTransaction.DeliveryOption));
                moneyTransferProcedure.WithParameters(InputParameter.Named("transalatedDeliveryServiceName").WithValue(wuTransaction.TransalatedDeliveryServiceName));
                moneyTransferProcedure.WithParameters(InputParameter.Named("sender_ComplianceDetails_ComplianceData_Buffer").WithValue(wuTransaction.SenderComplianceDetailsComplianceDataBuffer));
                moneyTransferProcedure.WithParameters(InputParameter.Named("taxAmount").WithValue(wuTransaction.TaxAmount));
                moneyTransferProcedure.WithParameters(InputParameter.Named("testQuestion").WithValue(wuTransaction.TestQuestion));
                moneyTransferProcedure.WithParameters(InputParameter.Named("testAnswer").WithValue(wuTransaction.TestAnswer));
                moneyTransferProcedure.WithParameters(InputParameter.Named("paidDateTime").WithValue(wuTransaction.PaidDateTime));
                moneyTransferProcedure.WithParameters(InputParameter.Named("messageArea").WithValue(wuTransaction.MessageArea));
                moneyTransferProcedure.WithParameters(InputParameter.Named("dtTerminalLastModified").WithValue(wuTransaction.DTTerminalLastModified));
                moneyTransferProcedure.WithParameters(InputParameter.Named("dtServerLastModified").WithValue(wuTransaction.DTServerLastModified));
                moneyTransferProcedure.WithParameters(InputParameter.Named("transferType").WithValue(wuTransaction.TranascationType));

                using (IDataReader datareader = DataConnectionHelper.GetConnectionManager().ExecuteReader(moneyTransferProcedure))
                {
                    while (datareader.Read())
                    {
                        wuTransactionId = datareader.GetInt64OrDefault("wuTransactionId");
                    }
                }

                return wuTransactionId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal void UpdateWUTransaction(WUTransaction wuTransaction, UpdateTxType updateTxType, decimal fee)
        {
            try
            {
                StoredProcedure moneyTransferProcedure = new StoredProcedure("usp_UpdateWUTransaction");

                moneyTransferProcedure.WithParameters(InputParameter.Named("WUTrxId").WithValue(wuTransaction.Id));
                moneyTransferProcedure.WithParameters(InputParameter.Named("mtcn").WithValue(wuTransaction.MTCN));
                moneyTransferProcedure.WithParameters(InputParameter.Named("tempMtcn").WithValue(wuTransaction.TempMTCN));
                moneyTransferProcedure.WithParameters(InputParameter.Named("deliveryServiceDesc").WithValue(wuTransaction.DeliveryServiceDesc));
                moneyTransferProcedure.WithParameters(InputParameter.Named("pdsRequiredFlag").WithValue(wuTransaction.PdsRequiredFlag));
                moneyTransferProcedure.WithParameters(InputParameter.Named("dfTransactionFlag").WithValue(wuTransaction.DfTransactionFlag));
                moneyTransferProcedure.WithParameters(InputParameter.Named("paySideTax").WithValue(wuTransaction.PaySideTax));
                moneyTransferProcedure.WithParameters(InputParameter.Named("paySideCharges").WithValue(wuTransaction.PaySideCharges));
                moneyTransferProcedure.WithParameters(InputParameter.Named("amountToReceiver").WithValue(wuTransaction.AmountToReceiver));
                moneyTransferProcedure.WithParameters(InputParameter.Named("municipal_tax").WithValue(wuTransaction.municipal_tax));
                moneyTransferProcedure.WithParameters(InputParameter.Named("state_tax").WithValue(wuTransaction.state_tax));
                moneyTransferProcedure.WithParameters(InputParameter.Named("country_tax").WithValue(wuTransaction.county_tax));
                moneyTransferProcedure.WithParameters(InputParameter.Named("plus_charges_amount").WithValue(wuTransaction.plus_charges_amount));
                moneyTransferProcedure.WithParameters(InputParameter.Named("message_charge").WithValue(wuTransaction.message_charge));
                moneyTransferProcedure.WithParameters(InputParameter.Named("total_discount").WithValue(wuTransaction.total_discount));
                moneyTransferProcedure.WithParameters(InputParameter.Named("total_discounted_charges").WithValue(wuTransaction.total_discounted_charges));
                moneyTransferProcedure.WithParameters(InputParameter.Named("total_undiscounted_charges").WithValue(wuTransaction.total_undiscounted_charges));
                moneyTransferProcedure.WithParameters(InputParameter.Named("promoCodeDescription").WithValue(wuTransaction.PromoCodeDescription));
                moneyTransferProcedure.WithParameters(InputParameter.Named("promoName").WithValue(wuTransaction.PromoName));
                moneyTransferProcedure.WithParameters(InputParameter.Named("promoMsg").WithValue(wuTransaction.PromoMessage));
                moneyTransferProcedure.WithParameters(InputParameter.Named("promoDiscount").WithValue(wuTransaction.PromotionDiscount));
                moneyTransferProcedure.WithParameters(InputParameter.Named("promoSeqNo").WithValue(wuTransaction.PromotionSequenceNo));
                moneyTransferProcedure.WithParameters(InputParameter.Named("grossTotAmt").WithValue(wuTransaction.GrossTotalAmount));
                moneyTransferProcedure.WithParameters(InputParameter.Named("smsNotificationFlag").WithValue(wuTransaction.SMSNotificationFlag));
                moneyTransferProcedure.WithParameters(InputParameter.Named("additionalCharges").WithValue(wuTransaction.AdditionalCharges));
                moneyTransferProcedure.WithParameters(InputParameter.Named("personalMsg").WithValue(wuTransaction.PersonalMessage));
                moneyTransferProcedure.WithParameters(InputParameter.Named("filingDate").WithValue(wuTransaction.FilingDate));
                moneyTransferProcedure.WithParameters(InputParameter.Named("filingTime").WithValue(wuTransaction.FilingTime));
                moneyTransferProcedure.WithParameters(InputParameter.Named("instant_notification_addl_service_charges").WithValue(wuTransaction.instant_notification_addl_service_charges));
                moneyTransferProcedure.WithParameters(InputParameter.Named("originating_city").WithValue(wuTransaction.originating_city));
                moneyTransferProcedure.WithParameters(InputParameter.Named("originating_state").WithValue(wuTransaction.originating_state));
                moneyTransferProcedure.WithParameters(InputParameter.Named("expectedPayoutCityName").WithValue(wuTransaction.ExpectedPayoutCityName));
                moneyTransferProcedure.WithParameters(InputParameter.Named("expectedPayoutStateCode").WithValue(wuTransaction.ExpectedPayoutStateCode));
                moneyTransferProcedure.WithParameters(InputParameter.Named("isFixedOnSend").WithValue(wuTransaction.IsFixedOnSend));
                moneyTransferProcedure.WithParameters(InputParameter.Named("deliveryOption").WithValue(wuTransaction.DeliveryOption));
                moneyTransferProcedure.WithParameters(InputParameter.Named("transalatedDeliveryServiceName").WithValue(wuTransaction.TransalatedDeliveryServiceName));
                moneyTransferProcedure.WithParameters(InputParameter.Named("sender_ComplianceDetails_ComplianceData_Buffer").WithValue(wuTransaction.SenderComplianceDetailsComplianceDataBuffer));
                moneyTransferProcedure.WithParameters(InputParameter.Named("taxAmount").WithValue(wuTransaction.TaxAmount));
                moneyTransferProcedure.WithParameters(InputParameter.Named("testQuestion").WithValue(wuTransaction.TestQuestion));
                moneyTransferProcedure.WithParameters(InputParameter.Named("testAnswer").WithValue(wuTransaction.TestAnswer));
                moneyTransferProcedure.WithParameters(InputParameter.Named("paidDateTime").WithValue(wuTransaction.PaidDateTime));
                moneyTransferProcedure.WithParameters(InputParameter.Named("messageArea").WithValue(wuTransaction.MessageArea));
                moneyTransferProcedure.WithParameters(InputParameter.Named("personalMessage").WithValue(wuTransaction.PersonalMessage));
                moneyTransferProcedure.WithParameters(InputParameter.Named("promotionsCode").WithValue(wuTransaction.PromotionsCode));
                moneyTransferProcedure.WithParameters(InputParameter.Named("gcNumber").WithValue(wuTransaction.GCNumber));
                moneyTransferProcedure.WithParameters(InputParameter.Named("destinationCountryCode").WithValue(wuTransaction.DestinationCountryCode));
                moneyTransferProcedure.WithParameters(InputParameter.Named("destinationCurrencyCode ").WithValue(wuTransaction.DestinationCurrencyCode));
                moneyTransferProcedure.WithParameters(InputParameter.Named("recordingCountryCode").WithValue(wuTransaction.recordingCountryCode));
                moneyTransferProcedure.WithParameters(InputParameter.Named("recordingCurrencyCode").WithValue(wuTransaction.recordingCurrencyCode));
                moneyTransferProcedure.WithParameters(InputParameter.Named("recieverFirstName").WithValue(wuTransaction.ReceiverFirstName));
                moneyTransferProcedure.WithParameters(InputParameter.Named("recieverLastName").WithValue(wuTransaction.ReceiverLastName));
                moneyTransferProcedure.WithParameters(InputParameter.Named("destinationPrincipalAmount").WithValue(wuTransaction.DestinationPrincipalAmount));
                moneyTransferProcedure.WithParameters(InputParameter.Named("originatorsPrincipalAmount ").WithValue(wuTransaction.OriginatorsPrincipalAmount));
                moneyTransferProcedure.WithParameters(InputParameter.Named("testQuestionAvaliable").WithValue(wuTransaction.TestQuestionAvaliable));
                moneyTransferProcedure.WithParameters(InputParameter.Named("destinationState").WithValue(wuTransaction.DestinationState));
                moneyTransferProcedure.WithParameters(InputParameter.Named("exchangeRate").WithValue(wuTransaction.ExchangeRate));
                moneyTransferProcedure.WithParameters(InputParameter.Named("charges").WithValue(wuTransaction.Charges));
                moneyTransferProcedure.WithParameters(InputParameter.Named("availableForPickup ").WithValue(wuTransaction.AvailableForPickup));
                moneyTransferProcedure.WithParameters(InputParameter.Named("dtAvailableForPickup").WithValue(wuTransaction.DTAvailableForPickup));
                moneyTransferProcedure.WithParameters(InputParameter.Named("delayHours").WithValue(wuTransaction.DelayHours));
                moneyTransferProcedure.WithParameters(InputParameter.Named("deliveryServiceName ").WithValue(wuTransaction.DeliveryServiceName));
                moneyTransferProcedure.WithParameters(InputParameter.Named("agencyName").WithValue(wuTransaction.AgencyName));
                moneyTransferProcedure.WithParameters(InputParameter.Named("url").WithValue(wuTransaction.Url));
                moneyTransferProcedure.WithParameters(InputParameter.Named("phoneNumber").WithValue(wuTransaction.PhoneNumber));
                moneyTransferProcedure.WithParameters(InputParameter.Named("wuCardTotalPointsEarned").WithValue(wuTransaction.WuCardTotalPointsEarned));
                moneyTransferProcedure.WithParameters(InputParameter.Named("fee").WithValue(fee));
                moneyTransferProcedure.WithParameters(InputParameter.Named("dtTerminalLastModified ").WithValue(wuTransaction.DTTerminalLastModified));
                moneyTransferProcedure.WithParameters(InputParameter.Named("dtServerLastModified").WithValue(wuTransaction.DTServerLastModified));
                moneyTransferProcedure.WithParameters(InputParameter.Named("requestName").WithValue(Convert.ToString(updateTxType)));
                moneyTransferProcedure.WithParameters(InputParameter.Named("transferType").WithValue(wuTransaction.TranascationType));
                moneyTransferProcedure.WithParameters(InputParameter.Named("payordonotpayindicator").WithValue(wuTransaction.pay_or_do_not_pay_indicator));
                moneyTransferProcedure.WithParameters(InputParameter.Named("ConsumerFraudPromptQuestion").WithValue(wuTransaction.ConsumerFraudPromptQuestion));

                IDataReader datareader = DataConnectionHelper.GetConnectionManager().ExecuteReader(moneyTransferProcedure);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal void GetValidateRequest(long customerSessionId, long wuTransactionId, ref ValidateRequest validateRequest)
        {
            try
            {
                StoredProcedure moneyTransferProcedure = new StoredProcedure("usp_GetValidateRequest");

                moneyTransferProcedure.WithParameters(InputParameter.Named("CustomerSessionId").WithValue(customerSessionId));
                moneyTransferProcedure.WithParameters(InputParameter.Named("WUTransactionId").WithValue(wuTransactionId));

                using (IDataReader datareader = DataConnectionHelper.GetConnectionManager().ExecuteReader(moneyTransferProcedure))
                {
                    while (datareader.Read())
                    {
                        validateRequest.FirstName = datareader.GetStringOrDefault("FirstName");
                        validateRequest.LastName = datareader.GetStringOrDefault("LastName");
                        validateRequest.SecondLastName = datareader.GetStringOrDefault("LastName2");
                        validateRequest.DateOfBirth = datareader.GetDateTimeOrDefault("DOB").ToString("MM/dd/yyyy");
                        validateRequest.Address = datareader.GetStringOrDefault("Address1");
                        validateRequest.City = datareader.GetStringOrDefault("City");
                        validateRequest.State = datareader.GetStringOrDefault("State");
                        validateRequest.Occupation = datareader.GetStringOrDefault("Occupation");
                        validateRequest.PostalCode = datareader.GetStringOrDefault("ZipCode");
                        validateRequest.PhoneNumber = datareader.GetStringOrDefault("CustomerPhoneNumber");
                        validateRequest.ContactPhone = datareader.GetStringOrDefault("CustomerPhoneNumber");
                        validateRequest.CountryOfBirth = datareader.GetStringOrDefault("CountryOfBirth");
                        validateRequest.PreferredCustomerAccountNumber = datareader.GetStringOrDefault("PreferredCustomerAccountNumber");
                        validateRequest.SmsNotificationFlag = datareader.GetStringOrDefault("SmsNotificationFlag");
                        validateRequest.GrossTotalAmount = datareader.GetDecimalOrDefault("GrossTotalAmount");
                        validateRequest.AmountToReceiver = datareader.GetDecimalOrDefault("AmountToReceiver");
                        validateRequest.DestinationPrincipalAmount = datareader.GetDecimalOrDefault("DestinationPrincipalAmount");
                        validateRequest.Charges = datareader.GetDecimalOrDefault("Charges");
                        validateRequest.ExpectedPayoutStateCode = datareader.GetStringOrDefault("ExpectedPayoutStateCode");
                        validateRequest.ExpectedPayoutCityName = datareader.GetStringOrDefault("ExpectedPayoutCityName");
                        validateRequest.DestinationCountryCode = datareader.GetStringOrDefault("DestinationCountryCode");
                        validateRequest.DestinationCurrencyCode = datareader.GetStringOrDefault("DestinationCurrencyCode");
                        validateRequest.ExchangeRate = datareader.GetDecimalOrDefault("ExchangeRate");
                        validateRequest.originating_city = datareader.GetStringOrDefault("originating_city");
                        validateRequest.OriginalDestinationCountryCode = datareader.GetStringOrDefault("OriginalDestinationCountryCode");
                        validateRequest.OriginalDestinationCurrencyCode = datareader.GetStringOrDefault("OriginalDestinationCurrencyCode");
                        validateRequest.SenderComplianceDetailsComplianceDataBuffer = datareader.GetStringOrDefault("Sender_ComplianceDetails_ComplianceData_Buffer");
                        validateRequest.Sender_unv_Buffer = datareader.GetStringOrDefault("Sender_unv_Buffer");
                        validateRequest.Receiver_unv_Buffer = datareader.GetStringOrDefault("Receiver_unv_Buffer");
                        validateRequest.DeliveryOption = datareader.GetStringOrDefault("DeliveryOption");
                        validateRequest.DeliveryServiceName = datareader.GetStringOrDefault("DeliveryServiceName");
                        validateRequest.municipal_tax = datareader.GetDecimalOrDefault("municipal_tax");
                        validateRequest.state_tax = datareader.GetDecimalOrDefault("state_tax");
                        validateRequest.county_tax = datareader.GetDecimalOrDefault("county_tax");
                        validateRequest.plus_charges_amount = datareader.GetDecimalOrDefault("plus_charges_amount");
                        validateRequest.PaySideCharges = datareader.GetDecimalOrDefault("PaySideCharges");
                        validateRequest.PaySideTax = datareader.GetDecimalOrDefault("PaySideTax");
                        validateRequest.DeliveryServiceDesc = datareader.GetStringOrDefault("DeliveryServiceDesc");
                        validateRequest.PdsRequiredFlag = datareader.GetBooleanOrDefault("PdsRequiredFlag");
                        validateRequest.DfTransactionFlag = datareader.GetBooleanOrDefault("DfTransactionFlag");
                        validateRequest.AvailableForPickup = datareader.GetStringOrDefault("AvailableForPickup");
                        validateRequest.AvailableForPickup = datareader.GetStringOrDefault("AvailableForPickupEST");
                        validateRequest.message_charge = datareader.GetDecimalOrDefault("message_charge");
                        validateRequest.total_discount = datareader.GetDecimalOrDefault("total_discount");
                        validateRequest.total_discounted_charges = datareader.GetDecimalOrDefault("total_discounted_charges");
                        validateRequest.total_undiscounted_charges = datareader.GetDecimalOrDefault("total_undiscounted_charges");
                        validateRequest.instant_notification_addl_service_charges = datareader.GetStringOrDefault("instant_notification_addl_service_charges");
                        validateRequest.TestQuestion = datareader.GetStringOrDefault("TestQuestion");
                        validateRequest.TestAnswer = datareader.GetStringOrDefault("TestAnswer");
                        validateRequest.PrimaryIdNumber = datareader.GetStringOrDefault("PrimaryIdNumber");
                        validateRequest.SecondIdNumber = datareader.GetStringOrDefault("SecondIdNumber");
                        validateRequest.CountryOfBirthAbbr2 = datareader.GetStringOrDefault("CountryOfBirthAbbr2");
                        validateRequest.CountryOfBirthAbbr3 = datareader.GetStringOrDefault("CountryOfBirthAbbr3");
                        validateRequest.PrimaryIdType = datareader.GetStringOrDefault("PrimaryIdType");
                        validateRequest.PrimaryIdCountryOfIssue = datareader.GetStringOrDefault("PrimaryIdCountryOfIssue");
                        validateRequest.PrimaryIdPlaceOfIssue = datareader.GetStringOrDefault("PrimaryIdPlaceOfIssueCode");
                        validateRequest.PrimaryCountryOfIssue = datareader.GetStringOrDefault("PrimaryCountryOfIssue");
                        validateRequest.PrimaryCountryCodeOfIssue = datareader.GetStringOrDefault("PrimaryCountryCodeOfIssue");
                        validateRequest.ReferenceNo = datareader.GetStringOrDefault("ReferenceNo");
                        validateRequest.OriginatorsPrincipalAmount = datareader.GetDecimalOrDefault("OriginatorsPrincipalAmount");
                        validateRequest.MoneyTransferKey = datareader.GetStringOrDefault("MoneyTransferKey");
                        validateRequest.MTCN = datareader.GetStringOrDefault("Mtcn");
                        validateRequest.TempMTCN = datareader.GetStringOrDefault("TempMTCN");
                        validateRequest.ReceiverFirstName = datareader.GetStringOrDefault("ReceiverFirstName");
                        validateRequest.ReceiverLastName = datareader.GetStringOrDefault("ReceiverLastName");
                        validateRequest.ReceiverSecondLastName = datareader.GetStringOrDefault("ReceiverSecondLastName");
                        validateRequest.SecondIdType = datareader.GetStringOrDefault("SecondIdType");
                    }
                }

                return;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal CommitRequest GetCommiRequest(long customerSessionId, long wuTransactionId)
        {
            try
            {
                CommitRequest commitRequest = new CommitRequest();

                StoredProcedure moneyTransferProcedure = new StoredProcedure("usp_GetCommitRequest");

                moneyTransferProcedure.WithParameters(InputParameter.Named("customerSessionId").WithValue(customerSessionId));
                moneyTransferProcedure.WithParameters(InputParameter.Named("WUTrxId").WithValue(wuTransactionId));

                using (IDataReader datareader = DataConnectionHelper.GetConnectionManager().ExecuteReader(moneyTransferProcedure))
                {
                    while (datareader.Read())
                    {
                        commitRequest.FirstName = datareader.GetStringOrDefault("FirstName");
                        commitRequest.LastName = datareader.GetStringOrDefault("LastName");
                        commitRequest.SecondLastName = datareader.GetStringOrDefault("LastName2");
                        commitRequest.MiddleName = datareader.GetStringOrDefault("MiddleName");
                        commitRequest.Address = datareader.GetStringOrDefault("Address1");
                        commitRequest.City = datareader.GetStringOrDefault("City");
                        commitRequest.PostalCode = datareader.GetStringOrDefault("ZipCode");
                        commitRequest.State = datareader.GetStringOrDefault("State");
                        commitRequest.ContactPhone = datareader.GetStringOrDefault("CustomerPhoneNumber");
                        commitRequest.MobilePhone = datareader.GetStringOrDefault("CustomerMobileNumber");
                        commitRequest.PhoneNumber = datareader.GetStringOrDefault("CustomerPhoneNumber");
                        commitRequest.ReferenceNo = datareader.GetStringOrDefault("ReferenceNo");
                        commitRequest.TranascationType = datareader.GetStringOrDefault("TranascationType");
                        commitRequest.MTCN = datareader.GetStringOrDefault("MTCN");
                        commitRequest.TempMTCN = datareader.GetStringOrDefault("TempMTCN");
                        commitRequest.MoneyTransferKey = datareader.GetStringOrDefault("MoneyTransferKey");
                        commitRequest.SenderComplianceDetailsComplianceDataBuffer = datareader.GetStringOrDefault("Sender_ComplianceDetails_ComplianceData_Buffer");
                        commitRequest.TestQuestion = datareader.GetStringOrDefault("TestQuestion");
                        commitRequest.TestAnswer = datareader.GetStringOrDefault("TestAnswer");
                        commitRequest.GrossTotalAmount = datareader.GetDecimalOrDefault("GrossTotalAmount");
                        commitRequest.AmountToReceiver = datareader.GetDecimalOrDefault("AmountToReceiver");
                        commitRequest.DestinationPrincipalAmount = datareader.GetDecimalOrDefault("DestinationPrincipalAmount");
                        commitRequest.Charges = datareader.GetDecimalOrDefault("Charges");
                        commitRequest.DestinationCountryCode = datareader.GetStringOrDefault("DestinationCountryCode");
                        commitRequest.DestinationCurrencyCode = datareader.GetStringOrDefault("DestinationCurrencyCode");
                        commitRequest.ExpectedPayoutStateCode = datareader.GetStringOrDefault("ExpectedPayoutStateCode");
                        commitRequest.ExpectedPayoutCityName = datareader.GetStringOrDefault("ExpectedPayoutCityName");
                        commitRequest.ExchangeRate = datareader.GetDecimalOrDefault("ExchangeRate");
                        commitRequest.originating_city = datareader.GetStringOrDefault("originating_city");
                        commitRequest.PromotionSequenceNo = datareader.GetStringOrDefault("PromotionSequenceNo");
                        commitRequest.PromotionsCode = datareader.GetStringOrDefault("PromotionsCode");
                        commitRequest.PromoCodeDescription = datareader.GetStringOrDefault("PromoCodeDescription");
                        commitRequest.PromoName = datareader.GetStringOrDefault("PromoName");
                        commitRequest.PromotionDiscount = datareader.GetDecimalOrDefault("PromotionDiscount");
                        commitRequest.PromoMessage = datareader.GetStringOrDefault("PromoMessage");
                        commitRequest.DeliveryOption = datareader.GetStringOrDefault("DeliveryOption");
                        commitRequest.DeliveryServiceName = datareader.GetStringOrDefault("DeliveryServiceName");
                        commitRequest.municipal_tax = datareader.GetDecimalOrDefault("municipal_tax");
                        commitRequest.state_tax = datareader.GetDecimalOrDefault("state_tax");
                        commitRequest.county_tax = datareader.GetDecimalOrDefault("county_tax");
                        commitRequest.plus_charges_amount = datareader.GetDecimalOrDefault("plus_charges_amount");
                        commitRequest.PaySideCharges = datareader.GetDecimalOrDefault("PaySideCharges");
                        commitRequest.PaySideTax = datareader.GetDecimalOrDefault("PaySideTax");
                        commitRequest.DeliveryServiceDesc = datareader.GetStringOrDefault("DeliveryServiceDesc");
                        commitRequest.PdsRequiredFlag = datareader.GetBooleanOrDefault("PdsRequiredFlag");
                        commitRequest.DfTransactionFlag = datareader.GetBooleanOrDefault("DfTransactionFlag");
                        commitRequest.OriginatorsPrincipalAmount = datareader.GetDecimalOrDefault("OriginatorsPrincipalAmount");
                        commitRequest.PersonalMessage = datareader.GetStringOrDefault("PersonalMessage");
                        commitRequest.message_charge = datareader.GetDecimalOrDefault("message_charge");
                        commitRequest.total_discount = datareader.GetDecimalOrDefault("total_discount");
                        commitRequest.total_discounted_charges = datareader.GetDecimalOrDefault("total_discounted_charges");
                        commitRequest.total_undiscounted_charges = datareader.GetDecimalOrDefault("total_undiscounted_charges");
                        commitRequest.instant_notification_addl_service_charges = datareader.GetStringOrDefault("instant_notification_addl_service_charges");
                        commitRequest.GCNumber = datareader.GetStringOrDefault("GCNumber");
                        commitRequest.PreferredCustomerAccountNumber = datareader.GetStringOrDefault("PreferredCustomerAccountNumber");
                        commitRequest.SmsNotificationFlag = datareader.GetStringOrDefault("SmsNotificationFlag");
                        commitRequest.ReceiverFirstName = datareader.GetStringOrDefault("ReceiverFirstName");
                        commitRequest.ReceiverLastName = datareader.GetStringOrDefault("ReceiverLastName");
                        commitRequest.ReceiverSecondLastName = datareader.GetStringOrDefault("ReceiverSecondLastName");
                        commitRequest.ReceiverContactNumber = datareader.GetStringOrDefault("ReceiverContactNumber");
                        commitRequest.ConsumerFraudPromptQuestion = datareader.GetStringOrDefault("ConsumerFraudPromptQuestion");
                    }

                }

                return commitRequest;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal void GetRefundRequest(RefundRequest refundRequest, long customerSessionId)
        {
            try
            {
                StoredProcedure moneyTransferProcedure = new StoredProcedure("usp_GetCommitRequest");

                moneyTransferProcedure.WithParameters(InputParameter.Named("customerSessionId").WithValue(customerSessionId));
                moneyTransferProcedure.WithParameters(InputParameter.Named("WUTrxId").WithValue(refundRequest.TransactionId));

                using (IDataReader datareader = DataConnectionHelper.GetConnectionManager().ExecuteReader(moneyTransferProcedure))
                {
                    while (datareader.Read())
                    {
                        refundRequest.FirstName = datareader.GetStringOrDefault("FirstName");
                        refundRequest.LastName = datareader.GetStringOrDefault("LastName");
                        refundRequest.SecondLastName = datareader.GetStringOrDefault("LastName2");
                        refundRequest.MiddleName = datareader.GetStringOrDefault("MiddleName");
                        refundRequest.Address = datareader.GetStringOrDefault("Address1");
                        refundRequest.City = datareader.GetStringOrDefault("City");
                        refundRequest.PostalCode = datareader.GetStringOrDefault("ZipCode");
                        refundRequest.State = datareader.GetStringOrDefault("State");
                        refundRequest.ContactPhone = datareader.GetStringOrDefault("CustomerPhoneNumber");
                        refundRequest.MobilePhone = datareader.GetStringOrDefault("CustomerMobileNumber");
                        refundRequest.PhoneNumber = datareader.GetStringOrDefault("CustomerPhoneNumber");
                        refundRequest.PreferredCustomerAccountNumber = datareader.GetStringOrDefault("PreferredCustomerAccountNumber");
                        refundRequest.GrossTotalAmount = datareader.GetDecimalOrDefault("GrossTotalAmount");
                        refundRequest.AmountToReceiver = datareader.GetDecimalOrDefault("AmountToReceiver");
                        refundRequest.DestinationPrincipalAmount = datareader.GetDecimalOrDefault("DestinationPrincipalAmount");
                        refundRequest.Charges = datareader.GetDecimalOrDefault("Charges");
                        refundRequest.ExpectedPayoutStateCode = datareader.GetStringOrDefault("ExpectedPayoutStateCode");
                        refundRequest.ExpectedPayoutCityName = datareader.GetStringOrDefault("ExpectedPayoutCityName");
                        refundRequest.DestinationCountryCode = datareader.GetStringOrDefault("DestinationCountryCode");
                        refundRequest.DestinationCurrencyCode = datareader.GetStringOrDefault("DestinationCurrencyCode");
                        refundRequest.SenderComplianceDetailsComplianceDataBuffer = datareader.GetStringOrDefault("Sender_ComplianceDetails_ComplianceData_Buffer");
                        refundRequest.plus_charges_amount = datareader.GetDecimalOrDefault("plus_charges_amount");
                        refundRequest.OriginatorsPrincipalAmount = datareader.GetDecimalOrDefault("OriginatorsPrincipalAmount");
                        refundRequest.MTCN = datareader.GetStringOrDefault("MTCN");
                        refundRequest.TempMTCN = datareader.GetStringOrDefault("TempMTCN");
                        refundRequest.MoneyTransferKey = datareader.GetStringOrDefault("MoneyTransferKey");
                        refundRequest.ReceiverSecondLastName = datareader.GetStringOrDefault("RecieverSecondLastName");
                        refundRequest.ReceiverFirstName = datareader.GetStringOrDefault("ReceiverFirstName");
                        refundRequest.ReceiverLastName = datareader.GetStringOrDefault("ReceiverLastName");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        internal long CreateReceiveMoneyTransaction(WUTransaction wuTransaction, ZeoContext context)
        {
            try
            {
                StoredProcedure moneyTransferProcedure = new StoredProcedure("usp_CreateReceiveMoneyWUTransaction");

                moneyTransferProcedure.WithParameters(InputParameter.Named("customerSessionId").WithValue(context.CustomerSessionId));
                moneyTransferProcedure.WithParameters(InputParameter.Named("mtcn").WithValue(wuTransaction.MTCN));
                moneyTransferProcedure.WithParameters(InputParameter.Named("channelPartnerId").WithValue(context.ChannelPartnerId));
                moneyTransferProcedure.WithParameters(InputParameter.Named("providerId").WithValue(context.ProviderId));
                moneyTransferProcedure.WithParameters(InputParameter.Named("dtTerminalCreate").WithValue(wuTransaction.DTTerminalCreate));
                moneyTransferProcedure.WithParameters(InputParameter.Named("dtServerCreate").WithValue(wuTransaction.DTServerCreate));
                moneyTransferProcedure.WithParameters(InputParameter.Named("referenceNo").WithValue(wuTransaction.ReferenceNo));
                moneyTransferProcedure.WithParameters(InputParameter.Named("tranascationType").WithValue(wuTransaction.TranascationType));
                moneyTransferProcedure.WithParameters(OutputParameter.Named("WUTrxID").OfType<long>());

                DataConnectionHelper.GetConnectionManager().ExecuteNonQuery(moneyTransferProcedure);

                return Convert.ToInt64(moneyTransferProcedure.Parameters["WUTrxID"].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal void UpdateReceiveMoneyTransaction(WUTransaction wuTransaction)
        {
            try
            {
                StoredProcedure moneyTransferProcedure = new StoredProcedure("usp_UpdateReceiveMoneyWUTransaction");

                moneyTransferProcedure.WithParameters(InputParameter.Named("exchangerate").WithValue(wuTransaction.ExchangeRate));
                moneyTransferProcedure.WithParameters(InputParameter.Named("charges").WithValue(wuTransaction.Charges));
                moneyTransferProcedure.WithParameters(InputParameter.Named("destinationprincipalamount").WithValue(wuTransaction.DestinationPrincipalAmount));
                moneyTransferProcedure.WithParameters(InputParameter.Named("grosstotamt").WithValue(wuTransaction.GrossTotalAmount));
                moneyTransferProcedure.WithParameters(InputParameter.Named("amounttoreceiver").WithValue(wuTransaction.AmountToReceiver));
                moneyTransferProcedure.WithParameters(InputParameter.Named("moneytransferkey").WithValue(wuTransaction.MoneyTransferKey));
                moneyTransferProcedure.WithParameters(InputParameter.Named("dtterminallastmodified").WithValue(wuTransaction.DTTerminalLastModified));
                moneyTransferProcedure.WithParameters(InputParameter.Named("dtserverlastmodified").WithValue(wuTransaction.DTServerLastModified));
                moneyTransferProcedure.WithParameters(InputParameter.Named("tempMtcn").WithValue(wuTransaction.TempMTCN));
                moneyTransferProcedure.WithParameters(InputParameter.Named("mtcn").WithValue(wuTransaction.MTCN));
                moneyTransferProcedure.WithParameters(InputParameter.Named("originatingcurrencycode").WithValue(wuTransaction.OriginatingCurrencyCode));
                moneyTransferProcedure.WithParameters(InputParameter.Named("destinationcurrencycode").WithValue(wuTransaction.DestinationCurrencyCode));
                moneyTransferProcedure.WithParameters(InputParameter.Named("sendername").WithValue(wuTransaction.SenderName));
                moneyTransferProcedure.WithParameters(InputParameter.Named("originatingcountrycode").WithValue(wuTransaction.OriginatingCountryCode));
                moneyTransferProcedure.WithParameters(InputParameter.Named("destinationcountrycode").WithValue(wuTransaction.DestinationCountryCode));
                moneyTransferProcedure.WithParameters(InputParameter.Named("testQuestion").WithValue(wuTransaction.TestQuestion));
                moneyTransferProcedure.WithParameters(InputParameter.Named("testAnswer").WithValue(wuTransaction.TestAnswer));
                moneyTransferProcedure.WithParameters(InputParameter.Named("personalMessage").WithValue(wuTransaction.PersonalMessage));
                moneyTransferProcedure.WithParameters(InputParameter.Named("expectedPayoutCityName").WithValue(wuTransaction.ExpectedPayoutCityName));
                moneyTransferProcedure.WithParameters(InputParameter.Named("expectedPayoutStateCode").WithValue(wuTransaction.ExpectedPayoutStateCode));
                moneyTransferProcedure.WithParameters(InputParameter.Named("originaldestinationcountrycode").WithValue(wuTransaction.OriginalDestinationCountryCode));
                moneyTransferProcedure.WithParameters(InputParameter.Named("originaldestinationcurrencycode").WithValue(wuTransaction.OriginalDestinationCurrencyCode));
                moneyTransferProcedure.WithParameters(InputParameter.Named("originating_city").WithValue(wuTransaction.originating_city));
                moneyTransferProcedure.WithParameters(InputParameter.Named("recieverFirstName").WithValue(wuTransaction.ReceiverFirstName));
                moneyTransferProcedure.WithParameters(InputParameter.Named("recieverLastName").WithValue(wuTransaction.ReceiverLastName));
                moneyTransferProcedure.WithParameters(InputParameter.Named("receiversecondlastname").WithValue(wuTransaction.ReceiverSecondLastName));
                moneyTransferProcedure.WithParameters(InputParameter.Named("WUTrxId").WithValue(wuTransaction.Id));

                IDataReader datareader = DataConnectionHelper.GetConnectionManager().ExecuteReader(moneyTransferProcedure);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal WUCommonData.WUEnrollmentRequest GetEnrollmentRequest(long customerSessionId)
        {
            try
            {
                WUCommonData.WUEnrollmentRequest enrollmentReq = null;

                StoredProcedure customerProcedure = new StoredProcedure("usp_GetEnrollmentRequest");

                customerProcedure.WithParameters(InputParameter.Named("customerSessionId").WithValue(customerSessionId));

                using (IDataReader datareader = DataConnectionHelper.GetConnectionManager().ExecuteReader(customerProcedure))
                {
                    while (datareader.Read())
                    {
                        //initialize the enrollment request.
                        enrollmentReq = new WUCommonData.WUEnrollmentRequest();
                        enrollmentReq.destination_country_currency = new WUCommonData.CountryCurrencyInfo();
                        enrollmentReq.originating_country_currency = new WUCommonData.CountryCurrencyInfo();
                        enrollmentReq.recording_country_currency = new WUCommonData.CountryCurrencyInfo();

                        //Populate the payment details.
                        enrollmentReq.originating_country_currency.country_code = datareader.GetStringOrDefault("OriginatingCountryCode");
                        enrollmentReq.destination_country_currency.country_code = datareader.GetStringOrDefault("DestinationCountryCode");
                        enrollmentReq.originating_country_currency.currency_code = datareader.GetStringOrDefault("DestinationCurrencyCode");
                        enrollmentReq.recording_country_currency.country_code = datareader.GetStringOrDefault("RecordingcountrycurrencyCountryCode");
                        enrollmentReq.recording_country_currency.currency_code = datareader.GetStringOrDefault("RecordingcountrycurrencyCurrencyCode");

                        //Populate the sender details.
                        enrollmentReq.FirstName = datareader.GetStringOrDefault("FirstName");
                        enrollmentReq.LastName = datareader.GetStringOrDefault("LastName");
                        enrollmentReq.AddressAddrLine1 = datareader.GetStringOrDefault("Address");
                        enrollmentReq.AddressCity = datareader.GetStringOrDefault("City");
                        enrollmentReq.AddressState = datareader.GetStringOrDefault("State");
                        enrollmentReq.AddressPostalCode = datareader.GetStringOrDefault("PostalCode");
                        enrollmentReq.ContactPhone = datareader.GetStringOrDefault("ContactPhone");
                        enrollmentReq.Email = datareader.GetStringOrDefault("Email");
                        enrollmentReq.MobilePhone = datareader.GetStringOrDefault("MobilePhone");
                    }
                }

                return enrollmentReq;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal SearchResponse CreateRefundTransaction(SearchRequest searchRefundRequest, ZeoContext context)
        {
            int refundSubType = (int)TransactionSubType.Refund;

            try
            {
                SearchResponse searchResponse = null;

                StoredProcedure moneyTransferProcedure = new StoredProcedure("usp_SendMoneyModifyorRefund");

                moneyTransferProcedure.WithParameters(InputParameter.Named("WUTrxId").WithValue(searchRefundRequest.TransactionId));
                moneyTransferProcedure.WithParameters(InputParameter.Named("customerId").WithValue(context.CustomerId));
                moneyTransferProcedure.WithParameters(InputParameter.Named("reasonCode").WithValue(searchRefundRequest.ReasonCode));
                moneyTransferProcedure.WithParameters(InputParameter.Named("reasonDescription").WithValue(searchRefundRequest.ReasonDesc));
                moneyTransferProcedure.WithParameters(InputParameter.Named("comments").WithValue(searchRefundRequest.Comments));
                moneyTransferProcedure.WithParameters(InputParameter.Named("referenceNo").WithValue(searchRefundRequest.ReferenceNumber));
                moneyTransferProcedure.WithParameters(InputParameter.Named("counterId").WithValue(context.WUCounterId));
                moneyTransferProcedure.WithParameters(InputParameter.Named("transactionSubType").WithValue(refundSubType));
                moneyTransferProcedure.WithParameters(InputParameter.Named("dtTerminalLastModified").WithValue(searchRefundRequest.DTTerminalLastModified));
                moneyTransferProcedure.WithParameters(InputParameter.Named("dtServerLastModified").WithValue(searchRefundRequest.DTServerLastModified));

                using (IDataReader datareader = DataConnectionHelper.GetConnectionManager().ExecuteReader(moneyTransferProcedure))
                {
                    while (datareader.Read())
                    {
                        searchResponse = new SearchResponse();
                        searchResponse.CancelTransactionId = datareader.GetInt64OrDefault("CancelTransactionId");
                        searchResponse.RefundTransactionId = datareader.GetInt64OrDefault("ModifyorRefundTransactionId");
                    }
                }

                return searchResponse;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal void UpdateSendMoneyRefundTransaction(WUTransaction wuTransaction)
        {
            try
            {
                StoredProcedure moneyTransferProcedure = new StoredProcedure("usp_UpdateSendMoneyRefundTransaction");

                moneyTransferProcedure.WithParameters(InputParameter.Named("wuTrxId").WithValue(wuTransaction.Id));
                moneyTransferProcedure.WithParameters(InputParameter.Named("originatorsPrincipalAmount").WithValue(wuTransaction.OriginatorsPrincipalAmount));
                moneyTransferProcedure.WithParameters(InputParameter.Named("destinationPrincipalAmount").WithValue(wuTransaction.DestinationPrincipalAmount));
                moneyTransferProcedure.WithParameters(InputParameter.Named("grossTotalAmount").WithValue(wuTransaction.GrossTotalAmount));
                moneyTransferProcedure.WithParameters(InputParameter.Named("charges").WithValue(wuTransaction.Charges));
                moneyTransferProcedure.WithParameters(InputParameter.Named("tempMTCN ").WithValue(wuTransaction.TempMTCN));
                moneyTransferProcedure.WithParameters(InputParameter.Named("refundType").WithValue(2)); //RefundTransaction.
                moneyTransferProcedure.WithParameters(InputParameter.Named("dtTerminalDate").WithValue(wuTransaction.DTTerminalLastModified));
                moneyTransferProcedure.WithParameters(InputParameter.Named("dtServerDate").WithValue(wuTransaction.DTServerLastModified));

                DataConnectionHelper.GetConnectionManager().ExecuteNonQuery(moneyTransferProcedure);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal void UpdateSendMoneySearchTransaction(long wuTrxId, ModifySendMoneySearch.modifysendmoneysearchreply modifySendMoneySearchResponse, ZeoContext context)
        {
            try
            {
                StoredProcedure trxnmodifyProcedure = new StoredProcedure("usp_UpdateSendMoneyModifyTransaction");

                trxnmodifyProcedure.WithParameters(InputParameter.Named("WUTrxId").WithValue(wuTrxId));
                trxnmodifyProcedure.WithParameters(InputParameter.Named("PrincipalAmount").WithValue(ConvertLongToDecimal(modifySendMoneySearchResponse.payment_transactions.payment_transaction[0].financials.principal_amount)));
                trxnmodifyProcedure.WithParameters(InputParameter.Named("Receiver_unv_Buffer").WithValue(modifySendMoneySearchResponse.payment_transactions.payment_transaction[0].receiver.unv_buffer));
                trxnmodifyProcedure.WithParameters(InputParameter.Named("Sender_unv_Buffer").WithValue(modifySendMoneySearchResponse.payment_transactions.payment_transaction[0].sender.unv_buffer));
                trxnmodifyProcedure.WithParameters(InputParameter.Named("MoneyTransferKey").WithValue(modifySendMoneySearchResponse.payment_transactions.payment_transaction[0].money_transfer_key));
                trxnmodifyProcedure.WithParameters(InputParameter.Named("ModifyType").WithValue(2)); //ModifySearchTransaction.
                trxnmodifyProcedure.WithParameters(InputParameter.Named("DTTerminalDate").WithValue(GetTimeZoneTime(context.TimeZone)));
                trxnmodifyProcedure.WithParameters(InputParameter.Named("DTServerDate").WithValue(DateTime.Now));

                DataConnectionHelper.GetConnectionManager().ExecuteNonQuery(trxnmodifyProcedure);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal long CreateWUAccount(MoneyTransfer.Data.WUAccount wuAccount)
        {
            long wuAccountId = 0;

            StoredProcedure moneyTransferProcedure = new StoredProcedure("usp_CreateMoneyTransferWUAccount");

            moneyTransferProcedure.WithParameters(InputParameter.Named("nameType").WithValue(wuAccount.NameType));
            moneyTransferProcedure.WithParameters(InputParameter.Named("customerSessionId").WithValue(wuAccount.CustomerSessionId));
            moneyTransferProcedure.WithParameters(InputParameter.Named("customerId").WithValue(wuAccount.CustomerId));
            moneyTransferProcedure.WithParameters(InputParameter.Named("preferredCustomerAccountNumber").WithValue(wuAccount.PreferredCustomerAccountNumber));
            moneyTransferProcedure.WithParameters(InputParameter.Named("preferredCustomerLevelCode").WithValue(wuAccount.PreferredCustomerLevelCode));
            moneyTransferProcedure.WithParameters(InputParameter.Named("dTServerCreate").WithValue(wuAccount.DTServerCreate));
            moneyTransferProcedure.WithParameters(InputParameter.Named("dTTerminalCreate").WithValue(wuAccount.DTTerminalCreate));

            using (IDataReader datareader = DataConnectionHelper.GetConnectionManager().ExecuteReader(moneyTransferProcedure))
            {
                while (datareader.Read())
                {
                    wuAccountId = datareader.GetInt64OrDefault("accountId");
                }
            }

            return wuAccountId;
        }

        internal string GetSenderStateCode(long wuTransactionId, string senderStateListCode)
        {
            try
            {
                string senderStateCode = string.Empty;

                StoredProcedure moneyTransferProcedure = new StoredProcedure("usp_GetSenderStateCode");

                moneyTransferProcedure.WithParameters(InputParameter.Named("SenderStateListCode").WithValue(senderStateListCode));
                moneyTransferProcedure.WithParameters(InputParameter.Named("WUTrxId").WithValue(wuTransactionId));

                using (IDataReader datareader = DataConnectionHelper.GetConnectionManager().ExecuteReader(moneyTransferProcedure))
                {
                    while (datareader.Read())
                    {
                        senderStateCode = datareader.GetStringOrDefault("SenderStateCode");
                    }

                }

                return senderStateCode;
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

        internal long ConvertDecimalToLong(decimal amount)
        {
            return Convert.ToInt64(amount * 100);
        }

        internal decimal ConvertLongToDecimal(long amount)
        {
            return Convert.ToDecimal(amount / 100m);
        }
    }
}
