--- ===============================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <08-3-2016>
-- Description:	 Get CXN trannsactionId for the Check
-- Jira ID:		<AL-7705>
-- ================================================================================

IF OBJECT_ID(N'usp_GetCheckCxnTransactionId', N'P') IS NOT NULL
DROP PROC usp_GetCheckCxnTransactionId
GO

CREATE PROCEDURE usp_GetCheckCxnTransactionId
(
   @transactionId BIGINT
)
AS
BEGIN	
    BEGIN TRY

		SELECT
		  CXNId as CxnTransactionId
		FROM 
		  tTxn_Check ct WITH (NOLOCK)
		WHERE
		  TransactionId = @transactionId 

	END TRY

	BEGIN CATCH

	  EXECUTE usp_CreateErrorInfo

	END CATCH
END	
GO



