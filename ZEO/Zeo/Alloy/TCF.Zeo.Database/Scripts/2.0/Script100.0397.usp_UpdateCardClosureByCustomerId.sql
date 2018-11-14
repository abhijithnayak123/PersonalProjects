-- =============================================
-- Author:		Kaushik Sakala		
-- Create date: 09/03/2017	
-- Description:	To Update card closure of a Account
-- =============================================


IF OBJECT_ID(N'usp_UpdateCardClosureByCustomerId', N'P') IS NOT NULL
DROP PROC usp_UpdateCardClosureByCustomerId
GO

CREATE PROCEDURE usp_UpdateCardClosureByCustomerId
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