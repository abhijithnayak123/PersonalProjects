--- ===============================================================================
-- Author:		<Rizwana Shaik>
-- Create date: <11-18-2016>
-- Description:	Create procedure for Getting Biller Details 
-- Jira ID:		<AL-8421>
-- ================================================================================


IF OBJECT_ID(N'usp_DeleteFavoriteBillerByBillerId', N'P') IS NOT NULL
	DROP PROCEDURE usp_DeleteFavoriteBillerByBillerId   -- Drop the existing Procedure.
GO

CREATE PROCEDURE usp_DeleteFavoriteBillerByBillerId 
(
	 @productId          BIGINT,																	 
	 @customerId         BIGINT,																	 
	 @dtServerModified   DATETIME,																	 
	 @dtTerminalModified DATETIME
)
AS
BEGIN
	 BEGIN TRY

		  EXECUTE usp_UpdateFavouriteBillerStatus
					 @customerId,
					 @productId,
					 0, -- soft delete
					 @dtServerModified,
					 @dtTerminalModified

		  EXECUTE usp_GetFavouriteBillersByCustomerId 
					@customerId
	   
	 END TRY

	 BEGIN CATCH

		EXEC usp_CreateErrorInfo  -- Create the error in 'tErrorLog' table and RaiseError

	 END CATCH
END