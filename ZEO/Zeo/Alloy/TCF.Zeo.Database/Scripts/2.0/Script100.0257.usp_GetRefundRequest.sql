--- ===============================================================================
-- Author:		<Nitish Biradar>
-- Create date: <12-15-2016>
-- Description:	Get the Refund Request. 
-- Jira ID:		<AL-8325>

--EXEC usp_GetRefundRequest 1000000001, 1000000000
-- ================================================================================


IF OBJECT_ID(N'usp_GetRefundRequest', N'P') IS NOT NULL
DROP PROC usp_GetRefundRequest
GO

CREATE PROCEDURE usp_GetRefundRequest
(
    @customerSessionId BIGINT,
	@wuTrxId BIGINT
)
AS
BEGIN
	BEGIN TRY
	
		SELECT 
			c.FirstName,  
			c.LastName,
			LastName2,
			c.MothersMaidenName,
			c.DOB,
			c.Address1,
			c.Address2,
			c.City,
			c.Phone1 AS CustomerPhoneNumber,
			c.State,
			c.ZipCode,
			CASE
				WHEN c.Phone1 IS NOT NULL
					AND c.Phone1Type = 'Cell'
				THEN c.Phone1
				WHEN c.Phone2 IS NOT NULL
					AND c.Phone2Type = 'Cell'
				THEN c.Phone2
				ELSE ''
			END AS CustomerMobileNumber,
			wr.SecondLastName AS ReceiverSecondLastName,
			wa.PreferredCustomerAccountNumber,
			wt.GrossTotalAmount,
			wt.AmountToReceiver,
			wt.DestinationPrincipalAmount,
			wt.Charges,
			wt.ExpectedPayoutStateCode,
			wt.ExpectedPayoutCityName,
			wt.DestinationCountryCode,
			wt.DestinationCurrencyCode,
			wt.Sender_ComplianceDetails_ComplianceData_Buffer,
			wt.plus_charges_amount,
			wt.OriginatorsPrincipalAmount,
			wt.MTCN,
			wt.TempMTCN,
			wt.MoneyTransferKey
		FROM 
			tCustomers c
			INNER JOIN tCustomerSessions cs ON c.CustomerId = cs.CustomerId
			INNER JOIN tWUnion_Account wa ON wa.CustomerId = c.CustomerId
			INNER JOIN tWUnion_Trx wt ON wt.WUAccountId = wa.WUAccountId 
			INNER JOIN tWUnion_Receiver wr ON wr.CustomerId = c.CustomerId
		WHERE 
			cs.CustomerSessionId = @customerSessionId AND wt.WUTrxID = @wuTrxId
		
	END TRY
	BEGIN CATCH
		EXECUTE usp_CreateErrorInfo
	END CATCH
END
GO