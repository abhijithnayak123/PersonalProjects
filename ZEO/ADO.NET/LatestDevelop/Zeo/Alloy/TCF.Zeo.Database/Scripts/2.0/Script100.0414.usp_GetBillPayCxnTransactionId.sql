--- ===============================================================================
-- Author:		<Nishad Varghese>
-- Create date: <16-Mar-2017>
-- Description:	 Get CXN trannsactionId for the Bill Pay
-- Jira ID:		<>
-- ================================================================================

IF OBJECT_ID(N'usp_GetBillPayCxnTransactionId', N'P') IS NOT NULL
DROP PROC usp_GetBillPayCxnTransactionId
GO

CREATE PROCEDURE usp_GetBillPayCxnTransactionId
(
   @transactionId BIGINT
)
AS
BEGIN	
    BEGIN TRY

		SELECT
		  CXNId as CxnTransactionId
		FROM 
		  tTxn_BillPay bp WITH (NOLOCK)
		WHERE
		  TransactionId = @transactionId 

	END TRY

	BEGIN CATCH

	  EXECUTE usp_CreateErrorInfo

	END CATCH
END	
GO



