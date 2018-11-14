--- ===============================================================================
-- Author:		<M.Purna Pushkal>
-- Create date: <02-22-2018>
-- Description:	 SP to delete the Provision details
-- Jira ID:		<B-13218>
-- ================================================================================

IF EXISTS(SELECT 1 FROM sys.objects WHERE name = 'usp_DeleteProvison')
BEGIN
	DROP PROCEDURE usp_DeleteProvison
END
GO

CREATE PROCEDURE usp_DeleteProvison
(
	 @promoProvisionId BIGINT
)
AS 
BEGIN
	BEGIN TRY
	BEGIN TRANSACTION
		DELETE 
		FROM  tPromoProvisions
		WHERE PromoProvisionId = @promoProvisionId	
	COMMIT
	END TRY

BEGIN CATCH
		ROLLBACK
		EXEC usp_CreateErrorInfo  -- Create the error in 'tErrorLog' table and RaiseError
END CATCH
END 