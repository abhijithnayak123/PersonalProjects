--- ===============================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <18-11-2016>
-- Description:	 Get bill pay account
-- Jira ID:		<AL-8320>
-- ================================================================================

--  exec usp_GetWUBillPayAccount 1000000011, '2016-11-25 12:48:16.860', '2016-11-25 12:48:16.860'


IF OBJECT_ID('usp_GetWUBillPayAccount') IS NOT NULL
BEGIN
	DROP PROCEDURE dbo.usp_GetWUBillPayAccount
END
GO

CREATE PROCEDURE usp_GetWUBillPayAccount 
(
	 @customerSessionId BIGINT,													  
	 @dtTerminalDate    DATETIME,													  
	 @dtServerDate      DATETIME
)
AS
BEGIN
	BEGIN TRY		  	  
		SET NOCOUNT ON
		DECLARE @customerId BIGINT
		DECLARE @accountId BIGINT 

		SELECT 
			@accountId = ba.WUBillPayAccountId,
			@customerId = cs.CustomerId						 						 
		FROM 
			tWUnion_BillPay_Account ba 
			RIGHT JOIN tCustomerSessions cs ON ba.CustomerId = cs.CustomerId
		WHERE
			cs.CustomerSessionId = @customerSessionId
				
		IF (@accountId IS NULL)	
		BEGIN
			EXECUTE usp_CreateWUBillPayAccount
						@accountId OUTPUT,
						@customerId,
						@customerSessionId,
						'', -- WU gold card number is empty while creating Account, If Enroll the WU gold card, then reusing the same SP with Card Number
						@dtTerminalDate,
						@dtServerDate
		END

		SELECT @accountId as WUBillPayAccountId
	END TRY

	BEGIN CATCH
		EXECUTE usp_CreateErrorInfo
	END CATCH
END
GO

