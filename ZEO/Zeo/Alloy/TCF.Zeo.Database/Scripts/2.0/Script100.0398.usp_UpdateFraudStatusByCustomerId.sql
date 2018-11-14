-- =============================================
-- Author:		Kaushik Sakala		
-- Create date: 13/03/2017	
-- Description:	To update customer Fraud Status 
-- =============================================


IF OBJECT_ID(N'usp_UpdateFraudStatusByCustomerId', N'P') IS NOT NULL
DROP PROC usp_UpdateFraudStatusByCustomerId
GO

CREATE PROCEDURE usp_UpdateFraudStatusByCustomerId
	@IsFraud INT,
	@dTAccountClosure DATETIME,
	@dTTerminalLastModified DATETIME,
	@dTServerLastModified DATETIME,
	@customerId BIGINT
AS
BEGIN
BEGIN TRY
	UPDATE
		tVisa_Account
	SET
		 FraudScore = @IsFraud,
		 DTAccountClosed = @dTAccountClosure,
		 DTServerLastModified = @dTServerLastModified,
		 DTTerminalLastModified = @dTTerminalLastModified
	WHERE
		CustomerId =  @customerId
END TRY

BEGIN CATCH
	EXECUTE usp_CreateErrorInfo
END CATCH
END
GO