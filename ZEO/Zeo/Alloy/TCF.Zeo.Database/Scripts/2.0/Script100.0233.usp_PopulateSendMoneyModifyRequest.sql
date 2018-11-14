--- ===============================================================================
-- Author:		<Abhijith>
-- Create date: <11-03-2016>
-- Description:	This SP is used to populate the Send Money Modify transaction.
-- Jira ID:		<AL-8324>
-- exec usp_PopulateSendMoneyModifyRequest 1000000005
-- ================================================================================

IF OBJECT_ID(N'usp_PopulateSendMoneyModifyRequest', N'P') IS NOT NULL
DROP PROC usp_PopulateSendMoneyModifyRequest
GO


CREATE PROCEDURE usp_PopulateSendMoneyModifyRequest
(
    @wuTrxId BIGINT
)
AS
BEGIN
BEGIN TRY
	
	SELECT 
      wt.OriginatorsPrincipalAmount
      ,wt.OriginatingCountryCode
      ,wt.OriginatingCurrencyCode
      ,wt.TranascationType
      ,wt.PromotionsCode
      ,wt.ExchangeRate
      ,wt.DestinationPrincipalAmount
      ,wt.GrossTotalAmount
      ,wt.Charges
      ,wt.TaxAmount
      ,wt.Mtcn
      ,wt.DTTerminalCreate
      ,wt.DTTerminalLastModified
      ,wt.PromotionDiscount
      ,wt.OtherCharges
      ,wt.MoneyTransferKey
      ,wt.AdditionalCharges
      ,wt.DestinationCountryCode
      ,wt.DestinationCurrencyCode
      ,wt.DestinationState
      ,wt.IsDomesticTransfer
      ,wt.IsFixedOnSend
      ,wt.PhoneNumber
      ,wt.Url
      ,wt.AgencyName
      ,wt.ChannelPartnerId
      ,wt.ProviderId
      ,wt.TestQuestion
      ,wt.TempMTCN
      ,wt.ExpectedPayoutStateCode
      ,wt.ExpectedPayoutCityName
      ,wt.TestAnswer
      ,wt.TestQuestionAvaliable
      ,wt.GCNumber
      ,wt.SenderName
      ,wt.PdsRequiredFlag
      ,wt.DfTransactionFlag
      ,wt.DeliveryServiceName
      ,wt.DTAvailableForPickup
      ,wt.DTServerCreate
      ,wt.DTServerLastModified
      ,wt.RecieverFirstName
      ,wt.RecieverLastName
      ,wt.RecieverSecondLastName
      ,wt.PromoCodeDescription
      ,wt.PromoName
      ,wt.PromoMessage
      ,wt.PromotionError
      ,wt.Sender_ComplianceDetails_ComplianceData_Buffer
      ,wt.recordingCountryCode
      ,wt.recordingCurrencyCode
      ,wt.originating_city
      ,wt.originating_state
      ,wt.municipal_tax
      ,wt.state_tax
      ,wt.county_tax
      ,wt.plus_charges_amount
      ,wt.message_charge
      ,wt.total_undiscounted_charges
      ,wt.total_discount
      ,wt.total_discounted_charges
      ,wt.instant_notification_addl_service_charges
      ,wt.PaySideCharges
      ,wt.PaySideTax
      ,wt.AmountToReceiver
      ,wt.SMSNotificationFlag
      ,wt.PersonalMessage
      ,wt.DeliveryServiceDesc
      ,wt.ReferenceNo
      ,wt.pay_or_do_not_pay_indicator
      ,wt.OriginalDestinationCountryCode
      ,wt.OriginalDestinationCurrencyCode
      ,wt.FilingDate
      ,wt.FilingTime
      ,wt.PaidDateTime
      ,wt.AvailableForPickup
      ,wt.DelayHours
      ,wt.AvailableForPickupEST
      ,wt.WUCard_TotalPointsEarned
      ,wt.OriginalTransactionID
      ,wt.TransactionSubType
      ,wt.ReasonCode
      ,wt.ReasonDescription
      ,wt.Comments
      ,wt.DeliveryOption
      ,wt.DeliveryOptionDesc
      ,wt.PromotionSequenceNo
      ,wt.CounterId
      ,wt.PrincipalAmount
      ,wt.Receiver_unv_Buffer
      ,wt.Sender_unv_Buffer
      ,wt.TransalatedDeliveryServiceName
      ,wt.MessageArea
      ,wt.WUTrxID AS TransactionId
      ,wt.WUAccountID
      ,wt.WUReceiverID
	  --Fields from Receiver tbl
	  ,wt.RecieverFirstName AS ReceiverFirstName
	  ,wt.RecieverLastName AS ReceiverLastName
	  ,wt.RecieverSecondLastName AS ReceiverSecondLastName
	  ,wr.City AS PickupCity
	  ,wr.PickupCountry AS PickupCountry
	  --Fields from Account tbl
	  ,c.Address1 AS [Address]
	  ,c.City AS City
	  ,c.[State] AS [State]
	  ,c.FirstName AS FirstName
	  ,c.LastName AS LastName
	  ,c.MiddleName AS MiddleName
	  ,c.LastName2 AS SecondLastName
	  ,c.ZipCode AS PostalCode
	  ,c.Phone1 AS ContactPhone
	  , CASE
		 WHEN c.Phone1 IS NOT NULL AND LOWER(c.Phone1Type) = 'cell'
		 THEN c.Phone1
		 WHEN c.Phone2 IS NOT NULL AND LOWER(c.Phone2Type) = 'cell'
		 THEN c.Phone2
		END AS MobilePhone
	FROM dbo.tWUnion_Trx wt
		INNER JOIN tWUnion_Receiver wr ON wt.WUReceiverID = wr.WUReceiverID
		INNER JOIN tWUnion_Account wa ON wt.WUAccountID = wa.WUAccountID
		INNER JOIN tCustomers c ON wa.CustomerId = c.CustomerId
		--INNER JOIN tTrx_MoneyTransfer mt ON wt.WUTrxId = mt.CXNId
	WHERE wt.WUTrxID = @wuTrxId
	

END TRY
BEGIN CATCH
	EXECUTE usp_CreateErrorInfo
END CATCH
END
GO
	