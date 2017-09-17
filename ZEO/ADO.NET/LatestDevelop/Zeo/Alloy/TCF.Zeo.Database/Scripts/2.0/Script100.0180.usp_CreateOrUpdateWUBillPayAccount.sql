--- ===============================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <18-11-2016>
-- Description:	Create or update WU bill pay account
-- Jira ID:		<AL-8320>
-- ================================================================================

IF OBJECT_ID('usp_CreateOrUpdateWUBillPayAccount') IS NOT NULL
BEGIN
	DROP PROCEDURE dbo.usp_CreateOrUpdateWUBillPayAccount
END
GO

CREATE PROCEDURE usp_CreateOrUpdateWUBillPayAccount 
(																	 
	 @customerSessionId BIGINT,	 																 
	 @cardNumber        VARCHAR(50),
	 @dtTerminalDate    DATETIME,																		 
	 @dtServerDate      DATETIME
)
AS
BEGIN
	BEGIN TRY
		DECLARE @customerId BIGINT = dbo.ufn_GetCustomerIdByCustomerSessionId(@customerSessionId)		  

		IF NOT EXISTS(SELECT 1 FROM tWUnion_BillPay_Account WHERE CustomerId = @customerId)
		BEGIN
			EXEC usp_CreateWUBillPayAccount
					0,
					@customerId,
					@customerSessionId,
					@cardNumber,
					@dtTerminalDate,
					@dtServerDate
		END
		ELSE 

		BEGIN
			UPDATE 
				dbo.tWUnion_BillPay_Account
			SET
				CardNumber = @cardNumber,
				DTTerminalLastModified = @dtTerminalDate,
				DTServerLastModified = @dtServerDate
			WHERE 
				CustomerId = @customerId
		END
	END TRY

	BEGIN CATCH
		EXECUTE dbo.usp_CreateErrorInfo
	END CATCH
END
GO
