--- ===============================================================================
-- Author:		<M.Purna Pushkal>
-- Create date: <02-22-2018>
-- Description:	 SP to delete the Qulaifier details
-- Jira ID:		<B-13218>
-- ================================================================================

IF EXISTS(SELECT 1 FROM sys.objects WHERE name = 'usp_DeleteQualifier')
BEGIN
	DROP PROCEDURE usp_DeleteQualifier
END
GO

CREATE PROCEDURE usp_DeleteQualifier
(
	 @promoQualifierId BIGINT
)
AS 
BEGIN
	BEGIN TRY
	BEGIN TRANSACTION
		DELETE 
		FROM tPromoQualifiers
		WHERE PromoQualifierId = @promoQualifierId	
	COMMIT
	END TRY

BEGIN CATCH
		ROLLBACK
		EXEC usp_CreateErrorInfo  -- Create the error in 'tErrorLog' table and RaiseError
END CATCH
END 