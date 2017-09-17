--- ===============================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <08-04-2016>
-- Description:	 Update IsCheckFranked status on transaction table
-- Jira ID:		<AL-7705>
-- ================================================================================


IF OBJECT_ID(N'usp_UpdateTransactionFranked', N'P') IS NOT NULL
DROP PROC usp_UpdateTransactionFranked
GO


CREATE PROCEDURE usp_UpdateTransactionFranked	
	@transactionId BIGINT,
	@isCheckFranked BIT
AS
BEGIN
	
  BEGIN TRY

		UPDATE 
		  tChxr_Trx
		SET
		  IsCheckFranked = @isCheckFranked 
		WHERE 
		  ChxrTrxID = @transactionId

  END TRY	

  BEGIN CATCH

		EXEC usp_CreateErrorInfo

  END CATCH
END
GO

