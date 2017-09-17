-- ===========================================================
-- Author:		Kaushik Sakala		
-- Create date: 10/18/2016	
-- Description:	To Update Visa Account for Card closure date
-- ===========================================================


IF OBJECT_ID(N'usp_UpdateClosureDateByCustomerID', N'P') IS NOT NULL
DROP PROC usp_UpdateClosureDateByCustomerID
GO

CREATE PROCEDURE usp_UpdateClosureDateByCustomerID
	@dTTerminalLastModified DATETIME,
	@dTServerLastModified DATETIME,
	@CustomerId BIGINT,
	@dTAccountClosed DATETIME

AS
BEGIN
BEGIN TRY

	UPDATE
		tVisa_Account
	SET
	 DTServerLastModified = @dTTerminalLastModified,
	 DTTerminalLastModified = @dTTerminalLastModified,
	 DTAccountClosed = @dTAccountClosed
	WHERE
		CustomerID = @CustomerId
END TRY

BEGIN CATCH
	EXECUTE usp_CreateErrorInfo
END CATCH
END
GO
