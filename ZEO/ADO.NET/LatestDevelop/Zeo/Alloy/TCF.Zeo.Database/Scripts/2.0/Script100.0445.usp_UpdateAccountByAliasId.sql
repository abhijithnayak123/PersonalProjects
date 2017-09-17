-- ===================================================
-- Author:		Kaushik Sakala		
-- Create date: 11/21/2016	
-- Description:	To Update Account details by AliasId
-- ===================================================

IF OBJECT_ID(N'usp_UpdateAccountByAliasId', N'P') IS NOT NULL
DROP PROC usp_UpdateAccountByAliasId
GO

CREATE PROCEDURE usp_UpdateAccountByAliasId
	@aliasId VARCHAR(50),
	@dTTerminalLastModified DATETIME,
	@dTServerLastModified DATETIME,
	@CardNumber VARCHAR(50)

AS
BEGIN
BEGIN TRY
	UPDATE
		tVisa_Account
	SET
		 DTServerLastModified = @dTTerminalLastModified,
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