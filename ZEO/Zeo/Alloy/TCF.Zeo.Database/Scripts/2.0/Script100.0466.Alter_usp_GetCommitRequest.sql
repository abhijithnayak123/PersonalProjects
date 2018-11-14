--- ===============================================================================
-- Author:		<Kiranmaie>
-- Create date: <11-28-2016>
-- Description:	Get the Commit Request. 
-- Jira ID:		<AL-8325>

-- exec usp_GetCommitRequest 1000000009,1000000018
---=============================================================================

IF OBJECT_ID(N'usp_GetCommitRequest', N'P') IS NOT NULL
DROP PROC usp_GetCommitRequest
GO



CREATE PROCEDURE [dbo].[usp_GetCommitRequest]
(
    @customerSessionId BIGINT
	,@wuTrxId BIGINT
)
AS
BEGIN
	BEGIN TRY
	
		SELECT 
		    c.FirstName,
			c.LastName,
			c.LastName2,
			c.MiddleName,
			c.Address1,
			c.City,
			c.ZipCode,
			c.Phone1 AS CustomerPhoneNumber,
			c.State,
			CASE
				WHEN c.Phone1 IS NOT NULL
					AND c.Phone1Type = 'Cell'
				THEN c.Phone1
				WHEN c.Phone2 IS NOT NULL
					AND c.Phone2Type = 'Cell'
				THEN c.Phone2
				ELSE ''
			END AS CustomerMobileNumber,
			wt.ReferenceNo,
			wt.TranascationType,
			wt.MTCN,
			wt.TempMTCN,
			wt.MoneyTransferKey,
			wt.Sender_ComplianceDetails_ComplianceData_Buffer,
			wt.TestQuestion,
			wt.TestAnswer,
			wt.GrossTotalAmount,
			wt.AmountToReceiver,
			wt.DestinationPrincipalAmount,
			wt.Charges,
			wt.DestinationCountryCode,
			wt.DestinationCurrencyCode,
			wt.ExpectedPayoutStateCode,
			wt.ExpectedPayoutCityName,
			wt.ExchangeRate,
			wt.originating_city,
			wt.RecieverSecondLastName,
			wt.PromotionSequenceNo,
			wt.PromotionsCode,
			wt.PromoCodeDescription,
			wt.PromoName,
			wt.PromotionDiscount,
			wt.PromoMessage,
			wt.DeliveryOption,
			wt.DeliveryServiceName,
			wt.municipal_tax,
			wt.state_tax,
			wt.county_tax, 
			wt.plus_charges_amount,
			wt.PaySideCharges,
			wt.PaySideTax,
			wt.DeliveryServiceDesc,
			wt.PdsRequiredFlag,
			wt.DfTransactionFlag,
			wt.OriginatorsPrincipalAmount,
			wt.PersonalMessage,
			wt.message_charge,
			wt.total_discount,
			wt.total_discounted_charges,
			wt.total_undiscounted_charges,
			wt.instant_notification_addl_service_charges,
			wt.GCNumber,
			wa.PreferredCustomerAccountNumber,
			wa.SmsNotificationFlag,
			wt.RecieverFirstName AS ReceiverFirstName,
			wt.RecieverLastName AS ReceiverLastName,
			wt.RecieverSecondLastName AS ReceiverSecondLastName,
			--If the receiver having phone notifications delivery options we need to send contact number to western union
			wr.PhoneNumber AS ReceiverContactNumber
		FROM 
		tCustomers c
			INNER JOIN tCustomerSessions cs ON c.CustomerId = cs.CustomerId
			LEFT JOIN tWUnion_Account wa ON wa.CustomerId = c.CustomerId
			LEFT JOIN tWUnion_Trx wt ON wt.WUAccountId = wa.WUAccountId
			LEFT JOIN tWUnion_Receiver wr ON wt.WUReceiverID = wr.WUReceiverID
		WHERE 
			cs.CustomerSessionId = @customerSessionId AND wt.WUTrxID = @wuTrxId
	END TRY
	BEGIN CATCH
		EXECUTE usp_CreateErrorInfo
	END CATCH
END

GO


