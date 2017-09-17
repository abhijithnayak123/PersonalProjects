-- ===================================================
-- Author:		Abhijith		
-- Create date: 03/31/2017	
-- Description:	To Update Account details by AliasId
	-- Update - Added a condition to check the visa account for the customer.
	-- Update - dTTerminalLastModified was updating in place of dTServerLastModified.
-- ===================================================

IF OBJECT_ID(N'usp_UpdateAccountByAliasId', N'P') IS NOT NULL
DROP PROC usp_UpdateAccountByAliasId
GO

CREATE PROCEDURE usp_UpdateAccountByAliasId
	@aliasId VARCHAR(50),
	@dTTerminalLastModified DATETIME,
	@dTServerLastModified DATETIME,
	@CardNumber VARCHAR(50),
	@customerId BIGINT OUTPUT
AS
BEGIN
BEGIN TRY

	SET @customerId = 
	(
		SELECT 
			CustomerId
		FROM 
			tVisa_Account
		WHERE
			CardAliasId = @aliasId
	)

	UPDATE
		tVisa_Account
	SET
		 DTServerLastModified = @dTServerLastModified,
		 DTTerminalLastModified = @dTTerminalLastModified,
		 CardNumber = @CardNumber
	WHERE
		CardAliasId = @aliasId

END TRY

BEGIN CATCH
	EXECUTE usp_CreateErrorInfo
END CATCH
END
GO