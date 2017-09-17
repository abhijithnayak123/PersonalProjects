-- =============================================
-- Author:		Kaushik Sakala		
-- Create date: 11/21/2016	
-- Description:	To Update card number of a Account
-- =============================================


IF OBJECT_ID(N'usp_UpdateCardByAccountID', N'P') IS NOT NULL
DROP PROC usp_UpdateCardByAccountID
GO

CREATE PROCEDURE usp_UpdateCardByAccountID
	@cardNumber VARCHAR(50),
	@dTTerminalLastModified DATETIME,
	@dTServerLastModified DATETIME,
	@accountId BIGINT

AS
BEGIN
BEGIN TRY
	UPDATE
		tVisa_Account
	SET
		 CardNumber = @cardNumber,
		 DTServerLastModified = @dTTerminalLastModified,
		 DTTerminalLastModified = @dTTerminalLastModified
	WHERE
		VisaAccountID = @accountId
END TRY

BEGIN CATCH
	EXECUTE usp_CreateErrorInfo
END CATCH
END
GO