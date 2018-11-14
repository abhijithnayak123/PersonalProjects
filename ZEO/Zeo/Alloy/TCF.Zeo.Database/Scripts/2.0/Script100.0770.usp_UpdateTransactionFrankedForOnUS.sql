--- ===============================================================================
-- Author:		<Abhijith>
-- Create date: <06/22/2018>
-- Description:	 Update IsCheckFranked status on transaction table
-- ================================================================================

IF OBJECT_ID(N'usp_UpdateTransactionFrankedForOnUS', N'P') IS NOT NULL
DROP PROC usp_UpdateTransactionFrankedForOnUS
GO

CREATE PROCEDURE usp_UpdateTransactionFrankedForOnUS	
	@transactionId BIGINT,
	@isCheckFranked BIT
AS
BEGIN
	
  BEGIN TRY

		UPDATE 
		  tTCFOnus_Trx
		SET
		  IsCheckFranked = @isCheckFranked 
		WHERE 
		  TCFOnusTrxID = @transactionId

  END TRY	

  BEGIN CATCH

		EXEC usp_CreateErrorInfo

  END CATCH
END
GO

