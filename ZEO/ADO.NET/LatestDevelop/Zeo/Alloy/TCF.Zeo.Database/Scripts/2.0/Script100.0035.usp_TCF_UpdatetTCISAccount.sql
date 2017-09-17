-- ================================================================================
-- Author:		<Kaushik Sakal>
-- Create date: <07/27/2015>
-- Description:	<As an engineer, I want to implement ADO.Net for Customer module>
-- Jira ID:		<AL-7630>
-- ================================================================================

IF EXISTS(
	SELECT 1 
	FROM SYS.OBJECTS
	WHERE NAME = 'usp_TCF_UpdatetTCISAccount'
)

BEGIN
	DROP PROCEDURE usp_TCF_UpdatetTCISAccount
END
GO

CREATE PROCEDURE usp_TCF_UpdatetTCISAccount
	@TCISAccountID BIGINT
   ,@PartnerAccountNumber NVARCHAR(100)
   ,@RelationshipAccountNumber NVARCHAR(100)
   ,@ProfileStatus SMALLINT
   ,@DTTerminalLastModified DATETIME
   ,@DTServerLastModified DATETIME
   ,@TcfCustInd BIT
AS
BEGIN
	BEGIN TRY
		UPDATE tTCIS_Account
		SET
			PartnerAccountNumber = @PartnerAccountNumber,
			RelationshipAccountNumber = @RelationshipAccountNumber,
			ProfileStatus = @ProfileStatus,
			DTServerLastModified = @DTServerLastModified,
			DTTerminalLastModified = @DTTerminalLastModified,
			TcfCustInd = @TcfCustInd
		WHERE 
			TCISAccountID = @TCISAccountID
	END TRY
	BEGIN CATCH
	-- Execute error retrieval routine.  
		EXECUTE usp_CreateErrorInfo; 
	END CATCH
END
GO
