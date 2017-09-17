--- ===============================================================================
-- Author:		<Abhijith>
-- Create date: <11-04-2016>
-- Description:	This SP is used to update the gold card number in WU account.
-- Jira ID:		<AL-8324>
-- ================================================================================

IF OBJECT_ID(N'usp_UpdateWUAccount', N'P') IS NOT NULL
DROP PROC usp_UpdateWUAccount
GO


CREATE PROCEDURE usp_UpdateWUAccount
(
	@customerSessionId BIGINT
    ,@accountId BIGINT = 0
	,@nameType VARCHAR(200) = NULL
	,@preferredCustomerAccountNumber VARCHAR(250) = NULL
	,@preferredCustomerLevelCode VARCHAR(250) = NULL
	,@smsNotificationFlag VARCHAR(250) = NULL
	,@dtServerLastModified DATETIME = NULL
	,@dtTerminalLastModified DATETIME = NULL
)
AS
BEGIN
	
BEGIN TRY


	UPDATE tWUnion_Account
	SET NameType = ISNULL(@nameType, NameType)
		, PreferredCustomerAccountNumber = ISNULL(@preferredCustomerAccountNumber, PreferredCustomerAccountNumber)
		, PreferredCustomerLevelCode = ISNULL(@preferredCustomerLevelCode, PreferredCustomerLevelCode)
		, SmsNotificationFlag = ISNULL(@smsNotificationFlag, SmsNotificationFlag)
		, DTTerminalLastModified = @dtTerminalLastModified
		, DTServerLastModified = @dtServerLastModified
	WHERE WUAccountID = @accountId
	

END TRY
BEGIN CATCH

	EXECUTE usp_CreateErrorInfo
		
END CATCH
END
GO
